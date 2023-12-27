using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class LeanTweenFading : MonoBehaviour
{
    Image image;
    CanvasGroup canvasGrp;

    [Header("Position in Y")]
    [SerializeField] float posStart;
    [SerializeField] float posEnd;
    [SerializeField] float posChangeSpeed;

    [Header("Rotation around Z")]
    [SerializeField] float rotStart;
    [SerializeField] float rotAdd;
    [SerializeField] float rotChangeSpeed;

    [Header("Alpha from Image")]
    [SerializeField] float alphaStart;
    [SerializeField] float alphaEnd;
    [Tooltip("After which Animation duration should the Object fade? Cant be greater than the posChangeSpeed!")]
    [SerializeField] float alphaChangeTimer;
    float alphaChangeDuration;

    private void Awake()
    {
        image = GetComponent<Image>();
        canvasGrp = GetComponent<CanvasGroup>();
    }

    private void OnEnable()
    {
        MoveInY();
        RotateInZ();

        if (alphaChangeTimer > posChangeSpeed) alphaChangeTimer = posChangeSpeed;
        alphaChangeDuration = posChangeSpeed - alphaChangeTimer;
        StartCoroutine(FadeAlphaDelay());
    }

    /// <summary>
    /// Move in Y in a given amount of time
    /// </summary>
    private void MoveInY()
    {
        // teleport up
        transform.localPosition = new Vector3(transform.localPosition.x, posStart, transform.localPosition.z);
        // slide down
        LeanTween.moveLocalY(this.gameObject, posEnd, posChangeSpeed).setOnComplete(MoveInY);
    }

    /// <summary>
    /// Rotate in Z with a given speed
    /// </summary>
    private void RotateInZ()
    {
        // set start Rotation
        transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, rotStart);
        // rotate 
        LeanTween.rotateAround(gameObject, Vector3.back, rotAdd, rotChangeSpeed).setOnComplete(RotateInZ);
    }

    /// <summary>
    ///  Set Alpha to Start Value and wait for the timer
    /// </summary>
    /// <returns></returns>
    private IEnumerator FadeAlphaDelay()
    {
        // set start Color
        canvasGrp.alpha = alphaStart;
        yield return new WaitForSeconds(alphaChangeTimer);
        FadeAlpha();
    }

    /// <summary>
    /// Fade Alpha from start to end
    /// </summary>
    private void FadeAlpha()
    {
        // change alpha
        LeanTween.alphaCanvas(canvasGrp, alphaEnd, alphaChangeDuration).setOnComplete(() => StartCoroutine(FadeAlphaDelay()));
    }
}
