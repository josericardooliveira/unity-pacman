using UnityEngine;

public class GameState : MonoBehaviour
{

    [SerializeField]
    private bool _isInvencible;

    [SerializeField]
    private float invencibleTime = 3.0f;

    public bool isInvencible
    {
        get => _isInvencible;
        set
        {
            if (value)
            {
                CancelInvoke("clearInvencibleStatus");
                Invoke("clearInvencibleStatus", invencibleTime);
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
    }

    private void clearInvencibleStatus()
    {
        isInvencible = false;
    }

}
