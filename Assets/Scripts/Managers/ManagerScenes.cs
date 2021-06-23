using UnityEngine;
using UnityEngine.SceneManagement;

public enum Scenes { Menu, Game, Shop }

public class ManagerScenes : MonoBehaviour
{
    public static void LoadScene(Object scene)
    {
        SceneManager.LoadScene(scene.name);
    }

    public static void LoadScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    public static void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
