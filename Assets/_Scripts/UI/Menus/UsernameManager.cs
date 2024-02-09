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
    [SerializeField] string m_defaultName;

    public void ConfirmName()
    {
        string username;

        if (m_usernameInput == null) username = m_defaultName;
        else username = m_usernameInput.text;

        // username in networking

        MenuManager.Instance.LoadVillage();
    }
}
