using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] GameObject gameUI;
    [SerializeField] GameObject afterGameUI;
    [SerializeField] Text scoreText;

    #region Awake OnDestroy Start
    private void Awake()
    {
        Ball.PlayerDie += PlayerDie;
    }

    private void OnDestroy()
    {
        Ball.PlayerDie -= PlayerDie;
    }

    private void Start()
    {
        gameUI.SetActive(true);
        afterGameUI.SetActive(false);
        Time.timeScale = 1f;

        Ball.singleton.Load();
        //ColumnRotate.singleton.Load();

        Cursor.visible = false;
    }
    #endregion

    void PlayerDie()
    {
        Ball.singleton.Save();

        gameUI.SetActive(false);
        afterGameUI.SetActive(true);

        string score = "Current score: " + Ball.singleton.currentScore;
        score += "\nBest score: " + Ball.singleton.bestScore;
        scoreText.text = score;
        Cursor.visible = true;
    }
}
