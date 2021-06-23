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
        PlayerManager.PlayerDie += PlayerDie;
    }

    private void OnDestroy()
    {
        PlayerManager.PlayerDie -= PlayerDie;
    }

    private void Start()
    {
        gameUI.SetActive(true);
        afterGameUI.SetActive(false);
        Time.timeScale = 1f;

        PlayerManager.singleton.Load();
        
        Cursor.visible = false;
    }
    #endregion

    void PlayerDie()
    {
        PlayerManager.singleton.Save();

        gameUI.SetActive(false);
        afterGameUI.SetActive(true);

        string score = "Current score: " + PlayerManager.singleton.currentScore;
        score += "\nBest score: " + PlayerManager.singleton.bestScore;
        scoreText.text = score;
        Cursor.visible = true;
    }
}
