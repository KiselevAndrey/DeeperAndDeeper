using UnityEngine;
using UnityEngine.SceneManagement;
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
    }
    #endregion

    #region Scenes
    public static void LoadScene(Object scene)
    {
        SceneManager.LoadScene(scene.name);
    }

    public static void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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
        
    }
}
