using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SocialPlatforms.Impl;
using System.Net;
using System.Text;
using System.Linq;
using System.Collections.ObjectModel;
using Unity.VisualScripting;

public class FirestoreRest : MonoBehaviour
{
    [System.Serializable]
    public class FirestoreResponse
    {
        public List<Document> Documents;
        public string NextPageToken;
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
        public ScoreField Score;
        public UserField User;
    }

    [System.Serializable]
    public class UserField
    {
        public string stringValue;
    }

    [System.Serializable]
    public class ScoreField
    {
        public int integerValue;
    }

    [SerializeField] private string m_collection;
    [SerializeField] private string m_collectionTest;
    [SerializeField] private string m_baseURL;

    public List<Document> Leaderboard;
    private string m_nextPageToken;

    [Header("Testing")]
    [SerializeField] private bool m_isTesting;
    [SerializeField] private bool m_isGeneratingDBEntries;
    [SerializeField] private float m_jobDelay;
    [SerializeField] private int m_numberOfEntries;

    private void Start()
    {
        Debug.Log("Started Database Initialization");

        Time.timeScale = 0f;

        if (m_isTesting)
        {
            if (m_isGeneratingDBEntries)
            {
                StartCoroutine(GenerateTestingData(m_numberOfEntries, m_collectionTest, () =>
                {
                    StartCoroutine(GetDataFromFirestore(m_collectionTest, () =>
                    {
                        Debug.Log("Database Operations done for " + Leaderboard.Count + " Entries");
                    }));
                }));
            }
            else
            {
                StartCoroutine(GetDataFromFirestore(m_collectionTest, () =>
                {
                    Debug.Log("Database Operations done for " + Leaderboard.Count + " Entries");
                }));
            }
        }
        else
        {
            if (m_isGeneratingDBEntries)
            {
                StartCoroutine(GenerateTestingData(m_numberOfEntries, m_collection, () =>
                {
                    StartCoroutine(GetDataFromFirestore(m_collection, () =>
                    {
                        Debug.Log("Database Operations done for " + Leaderboard.Count + " Entries");
                    }));
                }));
            }
            else
            {
                StartCoroutine(GetDataFromFirestore(m_collection, () =>
                {
                    Debug.Log("Database Operations done for " + Leaderboard.Count + " Entries");
                }));
            }
        }

        Time.timeScale = 1f;
    }

    public IEnumerator GetDataFromFirestore(string _collection, System.Action _onComplete)
    {
        while (true)
        {
            string url = m_baseURL + "/" + _collection + "?pageSize=50";

            if (!string.IsNullOrEmpty(m_nextPageToken))
            {
                url += $"&pageToken={m_nextPageToken}";
            }

            using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
            {
                yield return webRequest.SendWebRequest();

                if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
                {
                    Debug.LogError("Error: " + webRequest.error);
                }
                else
                {
                    try
                    {
                        string jsonResponse = webRequest.downloadHandler.text;
                        Debug.Log("Response: " + jsonResponse);

                        // Deserialize the JSON response
                        FirestoreResponse response = JsonConvert.DeserializeObject<FirestoreResponse>(jsonResponse);

                        if (response.Documents == null)
                        {
                            Debug.Log("Database is empty");
                        }
                        else if (response != null && response.Documents != null)
                        {
                            Leaderboard.AddRange(response.Documents
                                   .OrderByDescending(doc => doc.Fields.Score.integerValue)
                                   .Select(doc =>
                                   {
                                       string documentName = doc.Name.Substring(doc.Name.LastIndexOf('/') + 1);
                                       doc.Name = documentName;
                                       return doc;
                                   })
                                   .ToList());

                        }

                        m_nextPageToken = response.NextPageToken;

                        if (string.IsNullOrEmpty(m_nextPageToken))
                        {
                            break;
                        }

                    }
                    catch (JsonReaderException ex)
                    {
                        Debug.LogError("JsonReaderException: " + ex.Message);
                        Debug.LogError("Json error occurred. Raw JSON: " + webRequest.downloadHandler.text);
                    }
                }
            }
        }
        _onComplete?.Invoke();
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

    private IEnumerator GenerateTestingData(int _numberOfDBEntries, string _collectionTarget, System.Action _onComplete)
    {
        Debug.Log("Starting Job: Generating " + m_isGeneratingDBEntries + " DataBase Entries");

        for (int i = 0; i < _numberOfDBEntries; i++)
        {
            string randomUsername = GenerateRandomUsername(i);
            int randomScore = Random.Range(100, 100000);

            yield return StartCoroutine(SaveUserData(_collectionTarget, randomUsername, randomScore));
        }

        _onComplete?.Invoke();
        Debug.Log("Job Done");
    }

    private string GenerateRandomUsername(int _userNumber)
    {
        string baseName = "User";
        string randomSuffix = _userNumber.ToString();
        return baseName + randomSuffix;
    }
}