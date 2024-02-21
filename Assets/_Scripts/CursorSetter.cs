using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorSetter : MonoBehaviour
{

    private void Update()
    {
        if (Cursor.visible)
        {
            Cursor.visible = false;
        }
        Vector2 cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector2 (cursorPosition.x + (transform.localScale.x * .5f), cursorPosition.y - (transform.localScale.y * .5f));
    }
}
