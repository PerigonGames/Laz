using UnityEngine;
using Object = System.Object;

public static class TextWriter
{
    public static void WriteToFile(Object fileToConvert ,string fileName)
    {
        int fileDuplicateIndex = 1;

        while (DoesFileExist(fileName))
        {
            fileName = $"{fileName}_{fileDuplicateIndex}";
            fileDuplicateIndex++;
        }
        
        Debug.Log($"{fileName}.json Has been Created!");
    }

    private static bool DoesFileExist(string fileName)
    {
        return false;
    }
}
