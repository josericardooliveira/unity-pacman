using UnityEngine;

public class GhostDoor : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Enemy"))
        {
            GhostController controller = col.gameObject.GetComponent<GhostController>();
            if(controller.aiState != GhostAIState.EATEN) col.gameObject.layer = 10;
        }
    }

}
