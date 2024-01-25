using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Maya

[RequireComponent(typeof(CanvasGroup))]
public class LeanTweenFading : MonoBehaviour
{
    Image m_image;
    CanvasGroup m_canvasGrp;

    [Header("Position in Y")]
    [SerializeField] float m_posStart;
    [SerializeField] float m_posEnd;
    [SerializeField] float m_posChangeSpeed;

    [Header("Rotation around Z")]
    [SerializeField] float m_rotStart;
    [SerializeField] float m_rotAdd;
    [SerializeField] float m_rotChangeSpeed;

    [Header("Alpha from Image")]
    [SerializeField] float m_alphaStart;
    [SerializeField] float m_alphaEnd;
    [Tooltip("After which Animation duration should the Object fade? Cant be greater than the posChangeSpeed!")]
    [SerializeField] float m_alphaChangeTimer;
    float m_alphaChangeDuration;

    private void Awake()
    {
        m_image = GetComponent<Image>();
        m_canvasGrp = GetComponent<CanvasGroup>();
    }

    private void OnEnable()
    {
        MoveInY();
        RotateInZ();

        if (m_alphaChangeTimer > m_posChangeSpeed) m_alphaChangeTimer = m_posChangeSpeed;
        m_alphaChangeDuration = m_posChangeSpeed - m_alphaChangeTimer;
        StartCoroutine(FadeAlphaDelay());
    }

    /// <summary>
    /// Move in Y in a given amount of time
    /// </summary>
    private void MoveInY()
    {
        // teleport up
        transform.localPosition = new Vector3(transform.localPosition.x, m_posStart, transform.localPosition.z);
        // slide down
        LeanTween.moveLocalY(this.gameObject, m_posEnd, m_posChangeSpeed).setOnComplete(MoveInY);
    }

    /// <summary>
    /// Rotate in Z with a given speed
    /// </summary>
    private void RotateInZ()
    {
        // set start Rotation
        transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, m_rotStart);
        // rotate 
        LeanTween.rotateAround(gameObject, Vector3.back, m_rotAdd, m_rotChangeSpeed).setOnComplete(RotateInZ);
    }

    /// <summary>
    ///  Set Alpha to Start Value and wait for the timer
    /// </summary>
    /// <returns></returns>
    private IEnumerator FadeAlphaDelay()
    {
        // set start Color
        m_canvasGrp.alpha = m_alphaStart;
        yield return new WaitForSeconds(m_alphaChangeTimer);
        FadeAlpha();
    }

    /// <summary>
    /// Fade Alpha from start to end
    /// </summary>
    private void FadeAlpha()
    {
        // change alpha
        LeanTween.alphaCanvas(m_canvasGrp, m_alphaEnd, m_alphaChangeDuration).setOnComplete(() => StartCoroutine(FadeAlphaDelay()));
    }
}
