using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class FirestoreRETS : MonoBehaviour
{
    [SerializeField] private string baseURL;

    private void Start()
    {
        StartCoroutine(GetFirestoreData("Leaderboard"));
    }

    IEnumerator GetFirestoreData(string _collectionName)
    {
        string url = $"{baseURL}/{_collectionName}";

        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            yield return www.SendWebRequest();

            if(www.isNetworkError || www.isHttpError)
            {
                Debug.Log($"Error: {www.error}");
            }
            else
            {
                Debug.Log($"Response: {www.downloadHandler.text}");
            }
        }
    }

}
