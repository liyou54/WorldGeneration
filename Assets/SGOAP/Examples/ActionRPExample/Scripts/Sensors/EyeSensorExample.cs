using System.Collections.Generic;
using UnityEngine;

namespace SGOAP.Examples
{
    /// <summary>
    /// Just an example of how to make a sensor that can be used by another class
    /// Note the implementation here isn't very optimized and doesn't take into concepts of forgetting.
    /// </summary>
    public class EyeSensorExample : MonoBehaviour
    {
        public AgentRuntimeActionData RuntimeData;

        [Header("Settings")]
        public LayerMask Layer;

        [Range(0, 360)]
        public float Angle = 60;
        public float Radius = 20;
        public float DetectRange = 15;
        public float HeightRange = 100;

        [Header("Info")]
        public Collider[] FoundObjects;
        public List<Transform> SeenObjects;

        private void Update()
        {
            SeenObjects.Clear();
            FoundObjects = Physics.OverlapSphere(transform.position, Radius, Layer);

            foreach (var col in FoundObjects)
            {
                var distance = Vector3.Distance(transform.position, col.transform.position);
                var withinDistance = distance <= DetectRange;
                var withinSight = AngleUtils.WithinSight(col.transform, transform, Angle);
                var match = withinSight || withinDistance;
                var withinHeight = Mathf.Abs(col.transform.position.y - transform.position.y) <= HeightRange;

                if (!withinHeight)
                    match = false;

                if (!match) 
                    continue;

                if(!SeenObjects.Contains(col.transform))
                    SeenObjects.Add(col.transform);
            }

            // No concept of forgetting or picking up a new target.
            if (RuntimeData.TargetCharacter != null)
            {
                // When this target character is dead, we'll forget it.
                if (RuntimeData.TargetCharacter.IsDead())
                    RuntimeData.SetActionTarget(null);
            }
            else
            {
                if (SeenObjects.Count > 0)
                    RuntimeData.SetActionTarget(SeenObjects[0]);
            }
        }

        private void OnDrawGizmosSelected()
        {
            Vector3 posL = GetPosition(-(Angle / 2), Radius, transform.position);
            Vector3 posR = GetPosition(Angle / 2, Radius, transform.position);

            Debug.DrawRay(transform.position, transform.forward * Radius, Color.red);
            Debug.DrawLine(transform.position, posL, Color.blue);
            Debug.DrawLine(transform.position, posR, Color.blue);

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, Radius);

            Gizmos.DrawLine(transform.position - Vector3.up * (HeightRange * 0.5f), transform.position + Vector3.up * (HeightRange * 0.5f));
        }

        public Vector3 GetPosition(float angle, float dist, Vector3 root)
        {
            return AngleUtils.GetPosition(angle, dist, root, transform);
        }
    }
}