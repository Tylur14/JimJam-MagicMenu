using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MagicMenu_RenderFader : MonoBehaviour
{
    public enum MaterialTypes { TextMeshPro, Text_3D, MeshRenderer}

        [Header("Object Properties")]
    [SerializeField] MaterialTypes renderType = MaterialTypes.TextMeshPro;
    public bool active;

        [Header("Fade Settings")]
    [SerializeField] float fadeInSpeed = 1; 
    [SerializeField] float fadeOutSpeed = 2; 

        [Header("Fade Status")]
    public bool fading;  // False - alpha 0 -> 1, True - alpha 1 -> 0
    
        // Supported Renderers - ? Needs heavy compatibility testing with blank projects
    TextMeshPro tMPrend;
    TextMesh text3DRend;
    MeshRenderer meshrend;

    private void Awake()
    {
        GetRenderer();
        fading = true;
        Fade(true);
    }


    private void Update()
    {
        Fade();
    }

    public void InitObj()
    {
        fading = false;
    }

    public void Sleep()
    {
        fading = true;
    }

    void GetRenderer()
    {
        switch (renderType)
        {
            case MaterialTypes.TextMeshPro:
                tMPrend = GetComponent<TextMeshPro>();
                break;
            case MaterialTypes.Text_3D:
                text3DRend = GetComponent<TextMesh>(); // Not recommended, simply because I like TMPro so much :P
                break;
            case MaterialTypes.MeshRenderer:
                meshrend = GetComponent<MeshRenderer>(); // TESTING
                break;
        }
    }

    public void Fade(bool passThroughInstant = false)
    {
        switch (renderType)
        {
            case MaterialTypes.TextMeshPro:
                var newColor = tMPrend.color;
                tMPrend.color = GetColor(newColor, passThroughInstant);
                break;
            case MaterialTypes.Text_3D:
                var newColor1 = text3DRend.color;
                text3DRend.color = GetColor(newColor1, passThroughInstant);
                break;
            case MaterialTypes.MeshRenderer:
                var newColor2 = meshrend.material.color;
                meshrend.material.color = GetColor(newColor2, passThroughInstant);
                break;
        }
    }

    Color GetColor(Color incColor, bool instant = false)
    {
        if (instant)
        {
            if (fading)
            {
                incColor.a = 0.0f;
            }
            else if (!fading)
            {
                incColor.a = 1.0f;
            }
            incColor.a = Mathf.Clamp(incColor.a, 0.0f, 1.0f);
        }
        else if (!instant)
        {
            if (fading)
            {
                incColor.a -= fadeOutSpeed * Time.deltaTime;
            }
            else if (!fading)
            {
                incColor.a += fadeInSpeed * Time.deltaTime;
            }
            incColor.a = Mathf.Clamp(incColor.a, 0.0f, 1.0f);
        }
        
        return incColor;
    }
}
