using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance => instance;
    private static DataManager instance;

    private void OnApplicationQuit() { SavePlayerData(); }

    private PlayerData currentPlayerData;

    public PlayerSettings PlayerSettings => currentPlayerData.PlayerSettings;
    public Valuta Valuta => currentPlayerData.Valuta;
    public NoviceTree NoviceTree => currentPlayerData.SkillTree.NoviceTree;

    private string savePath;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance);
        }

        instance = this;

        DontDestroyOnLoad(gameObject);

        savePath = Application.persistentDataPath + "/playerdata.dat";

        LoadPlayerData();
    }

    private void LoadPlayerData()
    {
        if (File.Exists(savePath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(savePath, FileMode.Open);

            currentPlayerData = formatter.Deserialize(stream) as PlayerData;

            stream.Close();
        }
        else
        {
            currentPlayerData = new PlayerData();
        }
    }

    public void SavePlayerData()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(savePath, FileMode.Create);

        formatter.Serialize(stream, currentPlayerData);

        stream.Close();
    }


    public void SetValuta(Valuta newValuta)
    {
        currentPlayerData.SetValuta(newValuta);
        SavePlayerData();
    }
}