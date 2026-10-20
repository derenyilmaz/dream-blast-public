using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class GoalItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI countText;
    [SerializeField] private GameObject taskCompletionTick;
    [SerializeField] private Image itemImage;
        
    
    private int _count;
    
    public int Count
    {
        get => _count;
        set
        {
            _count = value;

            if (_count == 0)
            {
                countText.gameObject.SetActive(false);
                taskCompletionTick.SetActive(true);
            }
            else
            {
                countText.text = _count.ToString();
            }
        }
    }


    public void Configure(int count, Sprite sprite)
    {
        Count = count;
        itemImage.sprite = sprite;
    }
}
