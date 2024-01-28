using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    [SerializeField] private GameObject m_itemPrefab;
    [Range(0, 1)][SerializeField] private float m_dropChance;

    private Transform m_parent;
    private Transform m_container;


    private void Start()
    {
        m_container = GameObject.FindGameObjectWithTag("MapControl").transform;
        m_parent= GetComponent<Transform>();
    }

    public void DropItem()
    {
        if (UnityEngine.Random.value < m_dropChance)
        {
            Instantiate(m_itemPrefab, m_parent.position, Quaternion.identity, m_container);
        }
    }
}
