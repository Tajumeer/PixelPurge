using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VillageCharacterTrigger : MonoBehaviour
{
    [SerializeField] private Characters m_characterToChoose;

    private bool m_canInteract = false;

    [Header("Text Movement")]
    [SerializeField] private TextMeshProUGUI m_sceneText;
    [SerializeField] private float m_textMoveTime;
    [SerializeField] private float m_textMoveYPosUp;
    private float m_textYPosition;

    private VillageManager m_managerScript;
    private PlayerController m_playerScript;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        // Find VillageManager
        if(m_managerScript == null)
        {
            if (FindObjectOfType<VillageManager>()) m_managerScript = FindObjectOfType<VillageManager>();
            else
            {
                Debug.LogWarning("No VillageManager Script found");
                return;
            }
        }

        m_managerScript.ShowStats(m_characterToChoose);  // Show the Stats of the Character i am standing next to

        m_canInteract = true;

        m_textYPosition = m_sceneText.transform.position.y;
        MoveTextUp();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        m_managerScript.HideStats(m_characterToChoose);

        m_canInteract = false;
    }

    private void MoveTextUp()
    {
        LeanTween.moveLocalY(m_sceneText.gameObject, m_textYPosition + m_textMoveYPosUp, m_textMoveTime).setEaseOutCirc().setOnComplete(MoveTextDown);
    }

    private void MoveTextDown()
    {
        if (m_canInteract)
            LeanTween.moveLocalY(m_sceneText.gameObject, m_textYPosition, m_textMoveTime).setEaseInCirc().setOnComplete(MoveTextUp);
        else
            LeanTween.moveLocalY(m_sceneText.gameObject, m_textYPosition, m_textMoveTime).setEaseInCirc();
    }

    private void Update()
    {
        if (m_canInteract)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                // Find PlayerController
                if (m_playerScript == null)
                {
                    if (FindObjectOfType<PlayerController>()) m_playerScript = FindObjectOfType<PlayerController>();
                    else
                    {
                        Debug.LogWarning("No PlayerController Script found");
                        return;
                    }
                }
                else
                {
                    m_playerScript.SetCharacterVisualsAndData(m_characterToChoose);  // Change Character
                    GameManager.Instance.ClassIndex = (int)m_characterToChoose;
                }
            }
        }
    }
}
