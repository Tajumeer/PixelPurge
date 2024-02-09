using UnityEngine;
using UnityEngine.UI;

// Maya

/// <summary>
/// Moves the Text of a Button when Button is clicked
/// </summary>
public class ButtonTextMovement : MonoBehaviour
{
    /// <summary>
    /// Rect Transform of the text
    /// </summary>
    private RectTransform m_transf;

    [SerializeField] private float m_clickedPosition = 0f;
    [SerializeField] private float m_normalPosition = 10f;

    private void Awake()
    {
        m_transf = GetComponent<RectTransform>();
    }

    /// <summary>
    /// move text down along with the button
    /// </summary>
    public void OnButtonPressed()
    {
        if (!GetComponentInParent<Button>().interactable) return;

        m_transf.anchoredPosition = new Vector2(0f, m_clickedPosition);
    }

    /// <summary>
    /// move text up with the button
    /// </summary>
    public void OnButtonReleased()
    {
        if (!GetComponentInParent<Button>().interactable) return;

        m_transf.anchoredPosition = new Vector2(0f, m_normalPosition);
    }
}
