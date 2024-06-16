using System;
using System.IO;
using UnityEngine;

public static class Global
{
    public static string _log = "";
    public static int _gold = 0;
    public static int _attackLevel = 1;
    public static int _strengthLevel = 1;
    public static int _woodcutLevel = 1;
    public static int _defenseLevel = 1;
    public static int _miningLevel = 1;
    public static string _inventory = "";
    public static int _nextChallengeLevel = 2;
    public static int temp_willChallenge = 0;
    public static bool temp_hasStarted = false;


    public static void Log(string message) {
        _log = message + "\n" + _log;
    }

    public static void Load()
    {
        _gold = _get("gold");
        _inventory = _getString("inventory");
        _attackLevel = _get("attack");
        _strengthLevel = _get("strength");
        _nextChallengeLevel = _get("_nextChallengeLevel");
        _woodcutLevel = _get("_woodcutLevel");
        _miningLevel = _get("_miningLevel");
        _defenseLevel = _get("_defenseLevel");
    }

    public static void Save()
    {
        _persist("gold", _gold);
        _persist("inventory", _inventory);
        _persist("attack", _attackLevel);
        _persist("strength", _strengthLevel);
        _persist("_nextChallengeLevel", _nextChallengeLevel);
        _persist("_woodcutLevel", _woodcutLevel);
        _persist("_miningLevel", _miningLevel);
        _persist("_defenseLevel", _defenseLevel);
    }

    public static void ResetStats() {
        
    }

    public static String statsAsString() {
        return "Attack: " + _attackLevel.ToString() + " Strength: " +
         _strengthLevel.ToString() + " Challenge: " +
          _nextChallengeLevel.ToString() + " Woodcut: " + _woodcutLevel.ToString()
          + " Mining: " + _miningLevel.ToString() + " Defense: " + _defenseLevel.ToString();
    }

    public static void _persist(String key, string value)
    {
        string path = Application.persistentDataPath + "/" + key + ".txt";

        StreamWriter writer = new StreamWriter(path);
        writer.Write(value);
        writer.Close();
    }

    public static string _getString(String key)
    {
        try
        {
            string path = Application.persistentDataPath + "/" + key + ".txt";
            StreamReader reader = new StreamReader(path);
            string valueAsString = reader.ReadToEnd();
            reader.Close();

            return valueAsString;
        }
        catch (System.Exception e)
        {
            return "";
        }
    }
    public static void _persist(String key, int value)
    {
        string path = Application.persistentDataPath + "/" + key + ".txt";

        StreamWriter writer = new StreamWriter(path);
        writer.Write(value);
        writer.Close();
    }

    public static int _get(String key)
    {
        try
        {
            string path = Application.persistentDataPath + "/" + key + ".txt";
            StreamReader reader = new StreamReader(path);
            string valueAsString = reader.ReadToEnd();
            reader.Close();

            return Int32.Parse(valueAsString);
        }
        catch (System.Exception e)
        {
            return 1;
        }
    }
}

public class DialogMessage {
    public string title = "";
    public string body = "";

    public Texture2D icon;
}
