using System.Collections.Generic;
using UnityEngine;

namespace Laz
{
    public partial class Lazo
    {
        #region Debug

        private List<GameObject> _listOfDebugCubes = new List<GameObject>();

        public bool IsDebugging { get; set; }

        private void DebugCreateCubeAt(Vector3 position)
        {
            if (!IsDebugging) return;
            Debug.Log("Number of Cube: " + _listOfDebugCubes.Count);
            var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.position = position;
            cube.GetComponent<BoxCollider>().enabled = false;
            _listOfDebugCubes.Add(cube);
        }

        private void DebugDestroyLastCubeOnList()
        {
            if (!IsDebugging) return;
            if (_listOfDebugCubes.Count > 0)
            {
                var cube = _listOfDebugCubes[0];
                _listOfDebugCubes.RemoveAt(0);
                GameObject.Destroy(cube);
            }
        }

        private void DebugClearListOfCubes()
        {
            if (!IsDebugging) return;
            if (_listOfDebugCubes.Count > 0)
            {
                foreach (var cube in _listOfDebugCubes)
                {
                    GameObject.Destroy(cube);
                }

                _listOfDebugCubes.Clear();
            }
        }

        #endregion
    }
}