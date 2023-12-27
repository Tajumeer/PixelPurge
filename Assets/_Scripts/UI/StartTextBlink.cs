using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class StartTextBlink : MonoBehaviour
{
    [SerializeField] private float fadeTime = 1f;
    CanvasGroup canvasGrp;

    private void Awake()
    {
        canvasGrp = GetComponent<CanvasGroup>();
    }

    private void OnEnable()
    {
        canvasGrp.alpha = 0f;

        FadeIn();
    }

    private void FadeIn()
    {
        LeanTween.alphaCanvas(canvasGrp, 0f, fadeTime).setEaseInOutSine().setOnComplete(FadeOut);
    }

    private void FadeOut()
    {
        LeanTween.alphaCanvas(canvasGrp, 1f, fadeTime).setEaseInOutSine().setOnComplete(FadeIn);
    }
}
