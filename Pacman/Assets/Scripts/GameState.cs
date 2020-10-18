using UnityEngine;

public class GameState : MonoBehaviour
{

    [SerializeField]
    private bool _isInvencible;

    [SerializeField]
    private float invencibleTime = 3.0f;

    [SerializeField]
    private Transform GhostHomeTransform;

    [SerializeField]
    private Transform PlayerStartTransform;


    public bool isInvencible
    {
        get => _isInvencible;
        set
        {
            if (value)
            {
                CancelInvoke("clearInvencibleStatus");
                CancelInvoke("notifyInvencibleFinishing");
                Invoke("clearInvencibleStatus", invencibleTime);
                Invoke("notifyInvencibleFinishing", invencibleTime / 2.0f);
                var ghosts = FindObjectsOfType<GhostController>();
                foreach(GhostController ghost in ghosts)
                {
                    ghost.aiState = GhostAIState.FRIGHTENED;
                }
            }
            _isInvencible = value;
        }
    }

    public int score;

    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        isInvencible = false;
        ChangeGhostAiScatter();
    }

    private void clearInvencibleStatus()
    {
        isInvencible = false;
        var ghosts = FindObjectsOfType<GhostController>();
        foreach (GhostController ghost in ghosts)
        {
            if (ghost.aiState == GhostAIState.FRIGHTENED || ghost.aiState == GhostAIState.FRIGHTENED_FINISHING)
            {
                ghost.aiState = GhostAIState.CHASE;
            }
        }
    }

    private void notifyInvencibleFinishing()
    {
        var ghosts = FindObjectsOfType<GhostController>();
        foreach (GhostController ghost in ghosts)
        {
            if(ghost.aiState == GhostAIState.FRIGHTENED)
            {
                ghost.aiState = GhostAIState.FRIGHTENED_FINISHING;
            }
        }
    }

    public void Restart()
    {
        CancelInvoke("Execute_Restart");
        Invoke("Execute_Restart", 0.1f);
    }

    private void Execute_Restart()
    {
        var ghosts = FindObjectsOfType<GhostController>();
        foreach (GhostController ghost in ghosts)
        {
            ghost.gameObject.transform.position = GhostHomeTransform.position;
            ghost.Reset();
        }

        var players = FindObjectsOfType<PlayerController>();
        foreach (PlayerController player in players)
        {
            player.gameObject.transform.position = PlayerStartTransform.position;
            player.GiveLife();
        }
        ChangeGhostAiScatter();
    }

    private void ChangeGhostAiChase()
    {
        CancelInvoke("ChangeGhostAiScatter");
        Invoke("ChangeGhostAiScatter", 20.0f);
        var ghosts = FindObjectsOfType<GhostController>();
        foreach (GhostController ghost in ghosts)
        {
            if(ghost.aiState == GhostAIState.SCATTER)
            {
                ghost.aiState = GhostAIState.CHASE;
            }
        }
    }

    private void ChangeGhostAiScatter()
    {
        var ghosts = FindObjectsOfType<GhostController>();
        foreach (GhostController ghost in ghosts)
        {
            if(ghost.aiState == GhostAIState.CHASE)
            {
                ghost.aiState = GhostAIState.SCATTER;
            }
        }
        CancelInvoke("ChangeGhostAiChase");
        Invoke("ChangeGhostAiChase", 7.0f);
    }

}
