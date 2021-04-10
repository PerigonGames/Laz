using System;
using UnityEngine;

namespace Laz
{
    public class PlaceholderInterestBehaviour : MonoBehaviour, IObjectOfInterest
    {
        public Vector3 Position => transform.position;
        public event Action OnActivated;

        public void OnLazoActivated()
        {
            Debug.Log("I got wrapped");
            if (OnActivated != null)
            {
                OnActivated();
            }
        }
    }
}