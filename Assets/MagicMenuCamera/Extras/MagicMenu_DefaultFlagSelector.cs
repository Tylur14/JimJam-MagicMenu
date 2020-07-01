using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Extras feature to change between cameras
/// </summary>
public class MagicMenu_DefaultFlagSelector : MonoBehaviour
{
    private MagicMenuController mm_Controller;
    [Header("Navigator Settings")]
    [SerializeField] private float requiredHoverTime;
    private bool hoverRight;
    private bool hoverLeft;
    [SerializeField] private GameObject defaultFillBar;    
    float _hoverTimer;

    [Header("Testing Values")] 
    public int targetFlag;

    private void Start()
    {
        mm_Controller = FindObjectOfType<MagicMenuController>();
    }

    private void Update()
    {
        // START TESTING
        if(Input.GetKeyDown(KeyCode.P))
            mm_Controller.TargetSpecificFlag(targetFlag);
        // END TESTING
        
        if (mm_Controller)
        {
            hoverRight = Input.mousePosition.x > Screen.width * 0.95f;
            hoverLeft = Input.mousePosition.x < Screen.width * 0.05f;

            if (((( hoverRight && mm_Controller.CheckIfValidChangeDir(1) ) || ( hoverLeft && mm_Controller.CheckIfValidChangeDir(-1) )) || ((hoverRight || hoverLeft) && mm_Controller.doesLoop)) && mm_Controller.CheckIfRequirementsMet())
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
        }
        else Debug.LogError("You are missing a MagicMenuController!");
        

        var scale = defaultFillBar.transform.localScale;
        scale.x = _hoverTimer / requiredHoverTime;
        defaultFillBar.transform.localScale = scale;
        // mm_Controller.TryChange(-1);
    }
}
