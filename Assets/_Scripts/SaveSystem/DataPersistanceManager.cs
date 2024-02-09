using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class DataPersistanceManager : MonoBehaviour
{
    private static DataPersistanceManager m_instance;

    public static DataPersistanceManager Instance
    {
        get
        {
            if (m_instance == null)
            {
                GameObject obj = new GameObject("DataPersistanceManager");
                m_instance = obj.AddComponent<DataPersistanceManager>();
            }

            return m_instance;
        }
    }

    [Header("File Storage Config")]
    [SerializeField] private string m_fileName;
    [SerializeField] private bool m_isUsingEncryption;

    private GameData m_gameData;
    private FileDataHandler m_fileHandler;
    private List<IDataPersistence> m_persistenceObjects;

    private void Start()
    {
        m_fileHandler = new FileDataHandler(Application.persistentDataPath, m_fileName, m_isUsingEncryption);
        this.m_persistenceObjects = FindAllPersistenceObjects();
        LoadGame();
    }

    public void NewGame()
    {
        this.m_gameData = new GameData();
    }

    public void LoadGame()
    {
        this.m_gameData = m_fileHandler.Load();

        if(this.m_gameData == null)
        {
            Debug.Log("No Data found. Setting Defaults");
            NewGame();
        }

        foreach(IDataPersistence persistence in m_persistenceObjects)
        {
            persistence.LoadData(m_gameData);
        }

    }

    public void SaveGame()
    {
        foreach (IDataPersistence persistence in m_persistenceObjects)
        {
            persistence.SaveData(ref m_gameData);
        }

        m_fileHandler.Save(m_gameData);
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    private List<IDataPersistence> FindAllPersistenceObjects()
    {
        IEnumerable<IDataPersistence> persistenceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();

        return new List<IDataPersistence>(persistenceObjects);
    }
}
