using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorSetter : MonoBehaviour
{
    [SerializeField] private Texture2D m_cursorTexture;
    private Vector2 m_cursorPosition;

    //private void Update()
    //{
    //    if (Cursor.visible)
    //    {
    //        Cursor.visible = false;
    //    }
    //    Vector2 cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    //    transform.position = new Vector2 (cursorPosition.x + (transform.localScale.x * .5f), cursorPosition.y - (transform.localScale.y * .5f));
    //}

    private void Start()
    {
        m_cursorPosition = new Vector2(m_cursorTexture.width / 2, m_cursorTexture.height / 2);
    }

    private void OnEnable()
    {
        Cursor.SetCursor(m_cursorTexture, m_cursorPosition, CursorMode.Auto);
    }
}
