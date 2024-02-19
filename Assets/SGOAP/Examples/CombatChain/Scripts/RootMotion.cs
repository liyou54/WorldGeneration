using UnityEngine;

public class RootMotion : MonoBehaviour
{
    public Transform Target;
    public Animator Animator;

    public void OnAnimatorMove()
    {
        Target.position += Animator.deltaPosition;
    }
}