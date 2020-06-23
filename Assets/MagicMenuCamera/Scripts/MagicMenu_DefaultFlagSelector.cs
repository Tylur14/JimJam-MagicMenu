using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicMenu_DefaultFlagSelector : MonoBehaviour
{
    [SerializeField] private MagicMenuController mm_Controller;
    [SerializeField] private float requiredHoverTime;
    [SerializeField] private bool hoverRight;
    [SerializeField] private bool hoverLeft;
    [SerializeField] private GameObject defaultFillBar;    
    public float _hoverTimer;

    private void Update()
    {
        hoverRight = Input.mousePosition.x > Screen.width * 0.95f;
        hoverLeft = Input.mousePosition.x < Screen.width * 0.05f;

        if (hoverRight || hoverLeft)
        {
            _hoverTimer += Time.deltaTime;
            if (_hoverTimer >= requiredHoverTime)
            {
                if(hoverRight)
                    mm_Controller.TryChange(1);
                else if(hoverLeft)
                    mm_Controller.TryChange(-1);
                _hoverTimer = 0.0f;
            }
                
                    
        }
        else if(_hoverTimer > 0)
            _hoverTimer -= Time.deltaTime;

        var scale = defaultFillBar.transform.localScale;
        scale.x = _hoverTimer / requiredHoverTime;
        defaultFillBar.transform.localScale = scale;
        // mm_Controller.TryChange(-1);
    }
}
