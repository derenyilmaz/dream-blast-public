using System.Collections;
using UnityEngine;

public class Tnt : GameItem
{
    private Coroutine _explodingRoutine;
    
    protected override void TakeDamage()
    {
        if (_explodingRoutine != null)
        {
            return;
        }

        base.TakeDamage();
    }

    public override void Tap()
    {
        EventManager.TntTapped(this);
    }

    public void Explode(bool isCombo)
    {
        if (_explodingRoutine != null)
        {
            return;
        }
        
        _explodingRoutine = StartCoroutine(ExplodeRoutine(isCombo));
    }
    
    private IEnumerator ExplodeRoutine(bool isCombo)
    {
        yield return null;
        
        var size = isCombo ? 5.5f : 4.5f;

        var position = transform.position;
        var raycastHit2Ds = Physics2D.BoxCastAll(
            origin: new Vector2(position.x, position.y), 
            size: Vector2.one * size, 
            angle: 0f, 
            direction:Vector3.forward, 
            distance: 100f,
            layerMask: 1);
        
        foreach (var raycastHit in raycastHit2Ds)
        {
            if (raycastHit.collider != null)
            {
                var affectedItem = raycastHit.collider.GetComponent<GameItem>();

                if (affectedItem != null && affectedItem != this)
                {
                    affectedItem.TakeDamageFromTnt(this);
                }
            }
        }

        _explodingRoutine = null;
        
        Destroy(gameObject);
    }

    public override void Break()
    {
        EventManager.GameItemBroken(this);
        
        Explode(isCombo: false);
    }
}
