using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class FileHelper : MonoBehaviour
{
    public static bool IsFileExist(string path)
    {
        if (File.Exists(path)) return true;

        return false;
    }

    public static bool IsDirecExist(string path)
    {
        if (Directory.Exists(path)) return true;

        return false;
    }

    public static bool WriteToFile(string path, string fileName, string info, FileMode mode = FileMode.OpenOrCreate)
    {
        string filePath = path + "/" + fileName;
        FileStream stream = new FileStream(filePath, mode);
        byte[] bytes = Encoding.Default.GetBytes(info);
        stream.Write(bytes, 0, bytes.Length);
        stream.Close();
        return true;
    }

    public static string ReadFromFile(string path, string fileName)
    {
        string filePath = path + "/" + fileName;
        if (!IsFileExist(filePath)) return string.Empty;
        byte[] data = File.ReadAllBytes(filePath);
        string str = Encoding.Default.GetString(data);
        return str;
    }
}