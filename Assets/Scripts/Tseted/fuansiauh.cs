using UnityEngine;

public class LeverDebugSetMin : MonoBehaviour
{
    [SerializeField] private HingeJoint joint;

    private void Start()
    {
        JointSpring spring = joint.spring;
        spring.targetPosition = joint.limits.min;
        joint.spring = spring;
        joint.useSpring = true;
    }
}