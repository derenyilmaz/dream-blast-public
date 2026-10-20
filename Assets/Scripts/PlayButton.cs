using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI levelText; 
    
    private void Start()
    {
        var playerLevel = PlayerPrefs.GetInt(Constants.PLayerPrefsLevelKey);

        if (playerLevel == 0)
        {
            playerLevel = 1;
        }

        levelText.text = "Level " + playerLevel;
    }

    public void PlayLevel()
    {
        SceneManager.LoadScene(Constants.LevelSceneName);
    }
}
