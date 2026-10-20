using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelPanelController : MonoBehaviour
{
    [SerializeField] private GameObject losePanel;
    [SerializeField] private GameObject winPanel;
    
    private void Awake()
    {
        EventManager.OnLevelFailed += LevelFailedEventHandler;
        EventManager.OnLevelWon += LevelWonEventHandler;
    }

    private void OnDestroy()
    {
        EventManager.OnLevelFailed -= LevelFailedEventHandler;
        EventManager.OnLevelWon -= LevelWonEventHandler;
    }

    private void LevelFailedEventHandler(object sender, EventArgs args)
    {
        losePanel.SetActive(true);
    }
    
    private void LevelWonEventHandler(object sender, EventArgs args)
    {
        winPanel.SetActive(true);
    }
}
