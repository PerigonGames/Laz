using UnityEngine;

namespace Laz
{
    public class DebugLog
    {
        private readonly string _uniqueName;
        private bool _enablePrint = false;

        public DebugLog(string name, bool enablePrint)
        {
            _uniqueName = name;
            _enablePrint = enablePrint;
        }

        public void Log(string message)
        {
            if (_enablePrint)
            {
                Debug.Log($"{_uniqueName}: {message}");
            }
        }
    }
}

