using UnityEngine;
using UnityEngine.UI;

public class ScoreController : MonoBehaviour
{
    [SerializeField]
    private GameState gameState;

    [SerializeField]
    private Text scoreText;

    // Update is called once per frame
    void FixedUpdate()
    {
        scoreText.text = gameState.score.ToString();
    }
}
