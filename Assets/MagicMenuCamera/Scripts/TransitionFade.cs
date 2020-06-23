using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// TODO:
/// Fix Color transition, currently disabled completely
/// </summary>
public class TransitionFade : MonoBehaviour
{
    [SerializeField] TransitionTypes.FadeTypes fadeType;

    [SerializeField] Color fadeStartColor = new Color(0.0f, 0.0f, 0.0f, 0.0f);
    [SerializeField] Color fadeEndColor = new Color(0.0f, 0.0f, 0.0f, 1.0f);

    [SerializeField] Image transitionImage;
    [SerializeField] float transitionSpeed = 1.0f;
    [SerializeField] bool transitioning;

    private void Awake()
    {
        transitionImage = GetComponent<Image>();
        InitTransition();
    }

    // Full setup
    public void InitTransition(TransitionTypes.FadeTypes incFadeType, Image.FillMethod incTransitionType, float speed) 
    {
        fadeType = incFadeType;

        transitionImage.fillMethod = incTransitionType;
        transitionSpeed = speed;

        switch (fadeType)
        {
            case TransitionTypes.FadeTypes.fadeIn:
                transitionImage.color = fadeEndColor;
                transitionImage.fillAmount = 1.0f;
                StartCoroutine(SelfDestruct(1 / transitionSpeed));
                break;
            case TransitionTypes.FadeTypes.fadeOut:
                transitionImage.color = fadeStartColor;
                transitionImage.fillAmount = 0.0f;

                break;
        }

        transitioning = true;
    }
    IEnumerator SelfDestruct(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        Destroy(this.transform.parent.gameObject);
    }
    // Default Setup
    public void InitTransition()
    {
        transitioning = true;
    }

    private void Update()
    {
        if(transitioning)
            HandleTransition();
    }
    void HandleTransition()
    {
        switch (fadeType)
        {
            case TransitionTypes.FadeTypes.fadeIn:
                //transitionImage.color = GetColor(transitionImage.color, fadeStartColor);
                transitionImage.fillAmount -= Time.deltaTime * transitionSpeed;
                break;
            case TransitionTypes.FadeTypes.fadeOut:
                //transitionImage.color = GetColor(transitionImage.color, fadeEndColor);
                transitionImage.fillAmount += Time.deltaTime * transitionSpeed;
                break;
        }
    }

    Color GetColor(Color incColor, Color targetColor)
    {
        incColor = Color.Lerp(incColor, targetColor, transitionSpeed * Time.deltaTime);
        return incColor;
    }
}
public static class TransitionTypes
{
    public enum FadeTypes { fadeIn, fadeOut}
}
