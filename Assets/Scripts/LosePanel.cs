using UnityEngine;
using UnityEngine.SceneManagement;

public class LosePanel : MonoBehaviour
{
    public void PlayAgain()
    {
        SceneManager.LoadScene(Constants.LevelSceneName);
    }

    public void ClosePanel()
    {
        SceneManager.LoadScene(Constants.MainSceneName);
    }
}
