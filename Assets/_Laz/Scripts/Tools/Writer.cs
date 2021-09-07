using System.IO;
using System.Text;
using UnityEngine;

public static class Writer
{
    public static void WriteToFile(string fileName, object data)
    {
        string originalFileName = fileName;
        int fileDuplicateIndex = 1;

        while (DoesFileExist(fileName))
        {
            fileName = $"{originalFileName}_{fileDuplicateIndex}";
            fileDuplicateIndex++;
        }

        string dataToSave = JsonUtility.ToJson(data, true);
        File.WriteAllText($"{Application.dataPath}/{fileName}.json", dataToSave, Encoding.UTF8);
        
#if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh();
#endif
    }

    private static bool DoesFileExist(string fileName)
    {
        return File.Exists($"{Application.dataPath}/{fileName}.json");
    }
}
