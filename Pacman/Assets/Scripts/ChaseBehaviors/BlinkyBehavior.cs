using UnityEngine;

public class BlinkyBehavior : MonoBehaviour, TargetProvider
{
    [SerializeField]
    private Transform playerTransform;


    public Vector3 getTargetPosition()
    {
        return playerTransform.position;
    }

}
