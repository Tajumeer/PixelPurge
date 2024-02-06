using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

// Maya

public class UsernameManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI usernameInput;
    [SerializeField] string defaultName;

    public void ConfirmName()
    {
        string username;

        if (usernameInput == null) username = defaultName;
        else username = usernameInput.text;

        // username in networking

        MenuManager.Instance.LoadVillage();
    }
}
