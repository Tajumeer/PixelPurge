using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.IO;
using UnityEngine;

public class FileDataHandler
{
    private string m_dataDirPath = "";
    private string m_dataFileName = "";

    private bool m_isUsingEncryption = false;
    private readonly string encryptionCodeWord = "Purge";
    public FileDataHandler(string _dataDirPath, string _dataFileName, bool _useEncryption)
    {
        this.m_dataDirPath = _dataDirPath;
        this.m_dataFileName = _dataFileName;
        this.m_isUsingEncryption = _useEncryption;
    }

    public GameData Load()
    {
        string fullPath = Path.Combine(m_dataDirPath, m_dataFileName);

        GameData loadedData = null;
        if (File.Exists(fullPath))
        {
            try
            {
                string dataToLoad = "";

                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                if (m_isUsingEncryption)
                {
                    dataToLoad = EncryptDecrypt(dataToLoad);
                }

                loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
            }
            catch (Exception e)
            {
                Debug.LogError("Error occured when trying to load data from file: " + fullPath + "\n" + e);
            }
        }
        return loadedData;
    }

    public void Save(GameData _data)
    {
        string fullPath = Path.Combine(m_dataDirPath, m_dataFileName);

        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            string dataToStore = JsonUtility.ToJson(_data, true);

            if(m_isUsingEncryption)
            {
                dataToStore = EncryptDecrypt(dataToStore);
            }

            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }
        }
        catch (Exception e)
        {
            Debug.Log("Error when trying to save data to file: " + fullPath + "\n" + e);
        }
    }
    

    //XOR Encryption
    private string EncryptDecrypt(string _data)
    {
        string modifiedData = "";
        for (int i = 0; i < _data.Length; i++)
        {
            modifiedData += (char)(_data[i] ^ encryptionCodeWord[i % encryptionCodeWord.Length]);
        }

        return modifiedData;
    }
}
