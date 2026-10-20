using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TouchController : MonoBehaviour
{
    private bool _touchEnabled = true;
    
    private void Start()
    {
        EventManager.OnLevelFailed += LevelFailedEventHandler;
        EventManager.OnLevelWon += LevelWonEventHandler;
    }
    
    private void OnDestroy()
    {
        EventManager.OnLevelFailed -= LevelFailedEventHandler;
        EventManager.OnLevelWon -= LevelWonEventHandler;
    }

    private void LevelFailedEventHandler(object sender, EventArgs e)
    {
        _touchEnabled = false;
    }
    
    private void LevelWonEventHandler(object sender, EventArgs e)
    {
        _touchEnabled = false;
    }

    private void Update()
    {
        if (!_touchEnabled)
        {
            return;
        }
        
        if (Input.GetMouseButtonDown(0))
        {
            var raycastHit = Physics2D.Raycast(
                origin: Camera.main.ScreenToWorldPoint(Input.mousePosition),
                direction: Vector3.forward,
                distance: 100f, 
                layerMask: 2);

            if (raycastHit.collider != null)
            {
                var tappedTile = raycastHit.collider.GetComponent<Tile>();

                if (tappedTile != null)
                {
                    tappedTile.Tap();
                }
            }
        }
        
        else if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
