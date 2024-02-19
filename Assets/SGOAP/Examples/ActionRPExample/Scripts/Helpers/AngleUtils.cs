using UnityEngine;

namespace SGOAP.Examples
{
    public static class AngleUtils
    {
        public static bool WithinSight(Transform sampleTransform, Transform from, float angle)
        {
            return AngleToObject(sampleTransform, from) <= angle / 2;
        }

        public static float AngleToObject(Transform sampleTransform, Transform from)
        {
            float angleToObject = 0;

            Vector3 tarPosition = sampleTransform.position;
            tarPosition.y = from.position.y;

            Vector3 targetDir = tarPosition - from.position;
            Vector3 forward = from.forward;

            angleToObject = Vector3.SignedAngle(targetDir, forward, Vector3.up);
            angleToObject = Mathf.Abs(angleToObject);

            return angleToObject;
        }

        //Calculate Position
        public static Vector3 GetPosition(float angle, float dist, Vector3 root, Transform from)
        {
            var transformAngle = from.eulerAngles.y;

            var r = Mathf.Deg2Rad * (angle + 90 - transformAngle);
            var x = Mathf.Cos(r);
            var y = Mathf.Sin(r);

            Vector3 displacement = new Vector3(x, 0, y) * dist;
            Vector3 pos = root + displacement;
            return pos;
        }
    }
}