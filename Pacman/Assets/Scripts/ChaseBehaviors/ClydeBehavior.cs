using UnityEngine;

public class ClydeBehavior : MonoBehaviour, TargetProvider
{
    [SerializeField]
    private Transform playerTransform;

    [SerializeField]
    private Transform clydeTransform;

    [SerializeField]
    private Transform cornerTransform;


    public Vector3 getTargetPosition()
    {
        Vector3 result = playerTransform.position;
        float distance = Vector3.Distance(clydeTransform.position, playerTransform.position);

        if(distance < 8)
        {
            result = cornerTransform.position;
        }

        return result;
    }

}

