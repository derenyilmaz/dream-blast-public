using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class GameItem : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private List<Sprite> stageSprites;
    
    public Constants.GameItemType gameItemType;
    public bool canFallDown;
    
    public int health;
    public int xIndex;
    public int yIndex;


    private TntDamageSource _tntDamageSource;
    private BoundaryDamageSource _boundaryDamageSource;
    
    
    private void Awake()
    {
        _tntDamageSource = GetComponent<TntDamageSource>();
        _boundaryDamageSource = GetComponent<BoundaryDamageSource>();
    }

    public virtual void Tap()
    {
        
    }

    public virtual void TakeDamageFromTnt(Tnt tnt)
    {
        if (_tntDamageSource != null)
        {
            TakeDamage();
        }
    }

    public virtual void TakeDamageFromBoundary()
    {
        if (_boundaryDamageSource != null)
        {
            TakeDamage();
        }
    }

    protected virtual void TakeDamage()
    {
        health--;

        if (health == 0)
        {
            Break();
            return;
        }

        spriteRenderer.sprite = stageSprites[health - 1];
    }

    public virtual void Break()
    {
        EventManager.GameItemBroken(this);
        Destroy(gameObject);
    }

    public void SetSortingOrder(int sortingOrder)
    {
        spriteRenderer.sortingOrder = sortingOrder;
    }

    public Sprite GetSprite()
    {
        return spriteRenderer.sprite;
    }
}
