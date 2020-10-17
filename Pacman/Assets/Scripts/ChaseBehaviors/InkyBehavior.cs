using UnityEngine;

public class InkyBehavior : MonoBehaviour, TargetProvider
{
    [SerializeField]
    private Transform playerTransform;

    [SerializeField]
    private Transform blinkyTransform;

    [SerializeField]
    private Transform playerDirectionTransform;

    public Vector3 getTargetPosition()
    {
        Vector3 result = Vector3.zero;

        Vector3 rotation = playerDirectionTransform.forward;

        if (rotation == Vector3.up)
        {
            result = playerDirectionTransform.position + new Vector3(-2, 2);
        }
        else if (rotation == Vector3.down)
        {
            result = playerDirectionTransform.position + new Vector3(0, -2);
        }
        else if (rotation == Vector3.left)
        {
            result = playerDirectionTransform.position + new Vector3(-2, 0);
        }
        else
        {
            result = playerDirectionTransform.position + new Vector3(2, 0);
        }

        result = RotatePointAroundPivot(blinkyTransform.position, result, new Vector3(0, 0, 180));

        return result;
    }

    private Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles) {
        Vector3 dir  = point - pivot; // get point direction relative to pivot
        dir = Quaternion.Euler(angles) * dir; // rotate it
        point = dir + pivot; // calculate rotated point
        return point; // return it
    }
}
