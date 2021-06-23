using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    #region Ball
    static readonly string ballPath = "/ball.st";

    public static void SaveBall(Ball ball)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + ballPath;
        FileStream stream = new FileStream(path, FileMode.Create);

        BallData data = new BallData(ball);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static BallData LoadBall()
    {
        string path = Application.persistentDataPath + ballPath;
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            BallData data = formatter.Deserialize(stream) as BallData;

            stream.Close();

            return data;
        }
        else
        {
            Debug.Log("File is not found. Path: " + path);
            return null;
        }
    }
    #endregion

    #region Player
    static readonly string playerPath = "/player.st";

    public static void SavePlayer(PlayerManager player)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + playerPath;
        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerData data = new PlayerData(player);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static PlayerData LoadPlayer()
    {
        string path = Application.persistentDataPath + playerPath;
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            PlayerData data = formatter.Deserialize(stream) as PlayerData;

            stream.Close();

            return data;
        }
        else
        {
            Debug.Log("File is not found. Path: " + path);
            return null;
        }
    }
    #endregion

    #region ColumnRotate
    static readonly string columnRotatePath = "/columnRotate.st";

    public static void SaveColumnRotate(ColumnRotate rotate)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + columnRotatePath;
        FileStream stream = new FileStream(path, FileMode.Create);

        ColumnRotateData data = new ColumnRotateData(rotate);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static ColumnRotateData LoadColumnRotate()
    {
        string path = Application.persistentDataPath + columnRotatePath;
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            ColumnRotateData data = formatter.Deserialize(stream) as ColumnRotateData;

            stream.Close();

            return data;
        }
        else
        {
            Debug.Log("File is not found. Path: " + path);
            return null;
        }
    }
    #endregion
}
