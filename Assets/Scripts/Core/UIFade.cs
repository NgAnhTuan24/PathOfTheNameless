using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFade : Singleton<UIFade>
{
    [SerializeField] private Image fadeScreen;
    [SerializeField] private float fadeSpeed = 1f;

    private IEnumerator fadeRoutine;

    [SerializeField] private GameObject[] uiElements;

    public void FadeToBlack()
    {
        if (fadeRoutine != null)
        {
            StopCoroutine(fadeRoutine);
        }

        SetUIVisible(false);
        fadeRoutine = FadeRoutine(1);
        StartCoroutine(fadeRoutine);
    }

    public void FadeToClear()
    {
        if (fadeRoutine != null)
        {
            StopCoroutine(fadeRoutine);
        }

        SetUIVisible(true);
        fadeRoutine = FadeRoutine(0);
        StartCoroutine(fadeRoutine);
    }

    public IEnumerator FadeRoutine(float targetAlpha)
    {
        while (!Mathf.Approximately(fadeScreen.color.a, targetAlpha))
        {
            float alpha = Mathf.MoveTowards(fadeScreen.color.a, targetAlpha, fadeSpeed * Time.deltaTime);
            fadeScreen.color = new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b, alpha);
            yield return null;
        }
    }

    private void SetUIVisible(bool visible)
    {
        foreach (var ui in uiElements)
        {
            if (ui != null)
                ui.SetActive(visible);
        }
    }
}
