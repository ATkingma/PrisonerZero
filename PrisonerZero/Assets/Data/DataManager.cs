using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance => instance;
    private static DataManager instance;

    private void OnApplicationQuit() {
        SavePlayerData(); }

    private PlayerData currentPlayerData;

    public PlayerSettings PlayerSettings => currentPlayerData.PlayerSettings;
    public Valuta Valuta => currentPlayerData.Valuta;
    public List<SkillNode> upgrades => currentPlayerData.SkillTree.Upgrades;

    private string savePath;
    private const bool dev = true; 
    private string encryptionKey = "7Tc$VX9F&%mY@hnw2*LyZpR@N$bPjG5q";

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        savePath = dev ? Path.Combine(Application.persistentDataPath, "playerdata.json") : Path.Combine(Application.persistentDataPath, "playerdata.dat");

        LoadPlayerData();
    }

    private void LoadPlayerData()
    {
        if (File.Exists(savePath))
        {
            string json = dev ? File.ReadAllText(savePath) : DecryptData(File.ReadAllText(savePath), encryptionKey);
            currentPlayerData = JsonUtility.FromJson<PlayerData>(json);
        }
        else
        {
            currentPlayerData = new PlayerData();
            SavePlayerData();
        }
    }

    public void SavePlayerData()
    {
        string json = JsonUtility.ToJson(currentPlayerData);
        string dataToSave = dev ? json : EncryptData(json, encryptionKey);
        File.WriteAllText(savePath, dataToSave);
    }

    public void SetValuta(Valuta newValuta)
    {
        currentPlayerData.SetValuta(newValuta);
        SavePlayerData();
    }

    private byte[] GetValidKey(string key)
    {
        byte[] keyBytes = Encoding.UTF8.GetBytes(key);
        Array.Resize(ref keyBytes, 32);

        return keyBytes;
    }

    private string EncryptData(string json, string key)
    {
        byte[] plainBytes = Encoding.UTF8.GetBytes(json);
        byte[] keyBytes = GetValidKey(key); 

        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = keyBytes;
            aesAlg.IV = new byte[16];

            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    csEncrypt.Write(plainBytes, 0, plainBytes.Length);
                    csEncrypt.FlushFinalBlock();
                    byte[] encryptedBytes = msEncrypt.ToArray();
                    return Convert.ToBase64String(encryptedBytes);
                }
            }
        }
    }

    private string DecryptData(string encryptedJson, string key)
    {
        byte[] cipherBytes = Convert.FromBase64String(encryptedJson);
        byte[] keyBytes = GetValidKey(key);

        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = keyBytes;
            aesAlg.IV = new byte[16];

            ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
            using (MemoryStream msDecrypt = new MemoryStream(cipherBytes))
            {
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                    {
                        return srDecrypt.ReadToEnd();
                    }
                }
            }
        }
    }
}