using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MovePanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI moveCountText;

    private int _moveCountValue;

    private int MoveCount
    {
        get => _moveCountValue;
        set
        {
            _moveCountValue = value;
            moveCountText.text = _moveCountValue.ToString();
        }
    }


    private void Awake()
    {
        EventManager.OnLevelStarted += LevelStartedEventHandler;
        EventManager.OnMoveSpent += MoveSpentEventHandler;
    }

    private void MoveSpentEventHandler(object sender, EventArgs args)
    {
        MoveCount--;
    }

    private void LevelStartedEventHandler(object sender, EventManager.LevelStartedEventArgs args)
    {
        MoveCount = args.LevelRepresentation.move_count;
    }

    private void OnDestroy()
    {
        EventManager.OnLevelStarted -= LevelStartedEventHandler;
        EventManager.OnMoveSpent -= MoveSpentEventHandler;
    }
}
