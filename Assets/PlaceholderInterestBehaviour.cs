using UnityEngine;

namespace Laz
{
    public class PlaceholderInterestBehaviour : MonoBehaviour, IObjectOfInterest
    {
        public Vector3 Position => transform.position;
        
        public void OnLazoActivated()
        {
            Debug.Log("I got wrapped");
        }
    }
}