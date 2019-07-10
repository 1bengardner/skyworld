using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

static class StatePersistence
{
    public static void Save(GameState source, string destination)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/" + destination + ".save");
        bf.Serialize(file, source);
        file.Close();
    }

    public static GameState Load(string source)
    {
        if (File.Exists(Application.persistentDataPath + "/" + source + ".save"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/" + source + ".save", FileMode.Open);
            GameState loadedGame = (GameState) bf.Deserialize(file);
            file.Close();

            return loadedGame;
        }
        else
        {
            Debug.LogError("File \"" + Application.persistentDataPath + "/" + source + ".save\"" + " not found!");
        }
        return null;
    }
}