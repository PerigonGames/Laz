using System.IO;
using Laz;
using UnityEngine;

public static class TextWriter
{
    public static void WriteToFile(LazMovementPropertyScriptableObject file ,string fileName)
    {
        int fileDuplicateIndex = 1;

        while (DoesFileExist(fileName))
        {
            fileName = $"{fileName}_{fileDuplicateIndex}";
            fileDuplicateIndex++;
        }

        string json = JsonUtility.ToJson(file);
        
        File.WriteAllText($"{Application.dataPath}/{fileName}.txt", json);
    }

    private static bool DoesFileExist(string fileName)
    {
        return File.Exists($"{Application.dataPath}/{fileName}.txt");
    }
}
