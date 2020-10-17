using UnityEngine;

public class GhostHome : MonoBehaviour
{

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Enemy"))
        {
            GhostController controller = col.gameObject.GetComponent<GhostController>();
            controller.aiState = GhostAIState.SCATTER;
            col.gameObject.layer = 13;
        }
    }

}
