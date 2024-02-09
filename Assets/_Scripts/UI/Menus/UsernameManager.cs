using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

// Maya

public class UsernameManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI m_usernameInput;
    [SerializeField] Button m_confirmButton;
    [SerializeField] string m_defaultName;

    private void Update()
    {
        if (m_usernameInput.text.Length > 3)
            m_confirmButton.interactable = true;
        else 
            m_confirmButton.interactable = false;
    }

    public void ConfirmName()
    {
        //if (m_usernameInput.text.Length < 3) 
        //    GameManager.Instance.UserName = m_defaultName;
        //else 
            GameManager.Instance.UserName = m_usernameInput.text;

        MenuManager.Instance.LoadVillage();
    }
}
