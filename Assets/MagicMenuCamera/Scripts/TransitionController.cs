using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;

public class TransitionController : MonoBehaviour
{
    [Header("General Settings")]
    [SerializeField] GameObject transitionObj;

    // Fade settings, may eventually support color change as well
    [Header("Fade In Settings")]
    [SerializeField] float fadeInSpeed;
    [SerializeField] Image.FillMethod fadeInTrasnitionType;

    [Header("Fade Out Settings")]
    [SerializeField] float fadeOutSpeed;
    [SerializeField] Image.FillMethod fadeOutTrasnitionType;

    private void Awake()
    {
        FadeIn();
    }

    void FadeIn()
    {
        var fadeInObj = Instantiate(transitionObj);
        fadeInObj.GetComponentInChildren<TransitionFade>().InitTransition(TransitionTypes.FadeTypes.fadeIn, fadeInTrasnitionType, fadeInSpeed);
    }

    #region Scene-to-Scene Fadeout

    public void FadeOut(string gotoSceneName)
    {
        var fadeInObj = Instantiate(transitionObj);
        fadeInObj.GetComponentInChildren<TransitionFade>().InitTransition(TransitionTypes.FadeTypes.fadeOut, fadeOutTrasnitionType, fadeOutSpeed);
        StartCoroutine(GotoScene(gotoSceneName));
    }

    IEnumerator GotoScene(string newScene)
    {
        yield return new WaitForSeconds(1 / fadeOutSpeed);
        SceneManager.LoadScene(newScene);
    }

    #endregion

    #region Quit App Fadeout

    public void FadeOutQuitGame()
    {
        var fadeInObj = Instantiate(transitionObj);
        fadeInObj.GetComponentInChildren<TransitionFade>().InitTransition(TransitionTypes.FadeTypes.fadeOut, fadeOutTrasnitionType, fadeOutSpeed);
        StartCoroutine(QuitGame());
    }

    IEnumerator QuitGame()
    {
        yield return new WaitForSeconds(1 / fadeOutSpeed);
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#endif
            Application.Quit();
    }

    #endregion
}
