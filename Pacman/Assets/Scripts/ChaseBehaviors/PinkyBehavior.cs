using UnityEngine;

public class PinkyBehavior : MonoBehaviour, TargetProvider
{
    [SerializeField]
    private Transform playerTransform;

    [SerializeField]
    private Transform playerRotation;

    public Vector3 getTargetPosition()
    {
        Vector3 result = Vector3.zero;

        Vector3 rotation = playerRotation.forward;

        if(rotation == Vector3.up)
        {
            result = playerTransform.position + new Vector3(-4, 4);
        }
        else if(rotation == Vector3.down)
        {
            result = playerTransform.position + new Vector3(0, -4);
        }
        else if(rotation == Vector3.left)
        {
            result = playerTransform.position + new Vector3(-4, 0);
        }
        else
        {
            result = playerTransform.position + new Vector3(4, 0);
        }

        return result;
    }
}
