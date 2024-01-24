using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SocialPlatforms.Impl;
using System.Net;
using System.Text;

public class FirestoreExample : MonoBehaviour
{
    [System.Serializable]
    public class FirestoreResponse
    {
        public List<Document> Documents;
    }

    [System.Serializable]
    public class Document
    {
        public string Name;
        public Fields Fields;
    }

    [System.Serializable]
    public class Fields
    {
        public FirestoreValue Score;
        public FirestoreValue User;
    }

    [System.Serializable]
    public class FirestoreValue
    {
        public string IntegerValue;
        public string StringValue;
    }

    [SerializeField] private string m_collection;
    [SerializeField] private string m_baseURL;

    void Start()
    {
        StartCoroutine(GetDataFromFirestore(m_collection));
        StartCoroutine(SaveUserData(m_collection, "Tajumeer", 20000));
    }

    public IEnumerator GetDataFromFirestore(string _collection)
    {
        string url = m_baseURL + "/" + _collection;

        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error: " + webRequest.error);
            }
            else
            {
                string jsonResponse = webRequest.downloadHandler.text;
                Debug.Log("Response: " + jsonResponse);

                // Deserialize the JSON response
                FirestoreResponse response = JsonConvert.DeserializeObject<FirestoreResponse>(jsonResponse);

                if (response.Documents != null)
                {
                    foreach (var document in response.Documents)
                    {
                        string user = document.Fields.User?.StringValue ?? "";
                        string score = document.Fields.Score?.IntegerValue ?? "";
                        //TODO: Display Leaderboard
                        Debug.Log("User: " + user + ", Score: " + score);
                    }
                }
                else
                {
                    Debug.Log("Database is empty");
                }
            }
        }
    }

    public IEnumerator SaveUserData(string _collection, string _userName, int _score)
    {
        string url = m_baseURL + "/" + _collection;

        string jsonPayload = "{" +
          "\"fields\": {" +
              "\"User\": {\"stringValue\": \"" + _userName + "\"}," +
              "\"Score\": {\"integerValue\": \"" + _score.ToString() + "\"}" +
          "}" +
        "}";

        using (UnityWebRequest webRequest = new UnityWebRequest(url, "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonPayload);
            webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            webRequest.SetRequestHeader("Content-Type", "application/json");

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log("Error: " + webRequest.error);
            }
            else
            {
                string jsonResponse = webRequest.downloadHandler.text;
                Debug.Log("Response: " + jsonResponse);
            }
        }
    }
}
