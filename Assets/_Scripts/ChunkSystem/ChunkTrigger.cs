using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkTrigger : MonoBehaviour
{
    private MapController m_mapController;
    public GameObject TargetChunk;

    void Start()
    {
        m_mapController = FindObjectOfType<MapController>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            m_mapController.CurrentChunk = TargetChunk;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (m_mapController.CurrentChunk == TargetChunk)
            {
                m_mapController.CurrentChunk = null;
            }
        }
    }

}
