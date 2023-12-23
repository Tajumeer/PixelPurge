using UnityEngine;
using UnityEngine.UI;

public class ButtonTextMovement : MonoBehaviour
{
    /// <summary>
    /// Rect Transform of the text
    /// </summary>
    private RectTransform transf;

    private float clickedPosition = 0f;
    private float normalPosition = 10f;

    private void Awake()
    {
        transf = GetComponent<RectTransform>();
    }

    /// <summary>
    /// move text down along with the button
    /// </summary>
    public void OnButtonPressed()
    {
        transf.anchoredPosition = new Vector2(0f, clickedPosition);
    }

    /// <summary>
    /// move text up with the button
    /// </summary>
    public void OnButtonReleased()
    {
        transf.anchoredPosition = new Vector2(0f, normalPosition);
    }
}
