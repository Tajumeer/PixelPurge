using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropMovement : MonoBehaviour
{
    [SerializeField] private float m_amplitude;
    [SerializeField] private float m_frequency;
    private Vector2 m_startPosition;

    private void Start()
    {
        m_startPosition = transform.position;
    }

    private void Update()
    {
        float VerticalMovement = m_amplitude * Mathf.Sin(m_frequency * Time.deltaTime);

        transform.position = new Vector2(transform.position.x, m_startPosition.y + VerticalMovement);
    }
}
