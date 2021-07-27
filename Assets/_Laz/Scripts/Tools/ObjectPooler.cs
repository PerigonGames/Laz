//-----------------------------------------------------------------------
// <copyright file="PowerUpsObjectPooler.cs" company="PERIGON GAMES">
//     Copyright (c) PERIGON GAMES. 2019 All Rights Reserved
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using UnityEngine;

namespace PerigonGames
{
    /// <summary>
    /// Object Pool
    /// </summary>
    /// <typeparam name="T">Generic that is Type Component</typeparam>
    public abstract class ObjectPool<T> : MonoBehaviour where T : Component
    {
        protected Dictionary<int, Queue<T>> m_poolDictionary = null;
        private Transform m_parent = null;

        #region Singleton
        public static ObjectPool<T> Instance { get; private set; }

        protected virtual void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }

        #endregion

        /// <summary>
        /// Initialize the pooled of objects
        /// </summary>
        /// <param name="differentObjectsToSpawn"> List of different objects to spawn</param>
        /// <param name="parent">parent object to place in</param>
        protected void Initialize(List<PoolingObject<T>> differentObjectsToSpawn, Transform parent = null)
        {
            Clear();
            m_poolDictionary = new Dictionary<int, Queue<T>>();
            if (parent == null)
            {
                parent = transform;
            }
            
            m_parent = parent;

           foreach (var pooled in differentObjectsToSpawn)
           {
               Queue<T> pooledQueue = new Queue<T>();
               for (int i = 0; i < pooled.size; i++)
               {
                   GameObject obj = Instantiate(pooled.pooledObject.gameObject, m_parent, true);
                   obj.SetActive(false);
                   pooledQueue.Enqueue(obj.GetComponent<T>());
               }

               try
               {
                   m_poolDictionary.Add(pooled.key, pooledQueue);
               }
               catch (Exception e)
               {
                   print($"Need to have unique keys in object pooling dictionary: {e}");
               }
           }
        }
        
        /// <summary>
        /// Gets an object from with a stored key
        /// </summary>
        /// <param name="poolKey">key</param>
        /// <returns>Pooled Object</returns>
        public T PopPooledObject(int poolKey)
        {
            Queue<T> poolQueue = new Queue<T>();
            if (!m_poolDictionary.TryGetValue(poolKey, out poolQueue))
            {
                Debug.LogWarning(message: $"{poolKey} does not exist");
                return null;
            }
            
            T pooledObj = poolQueue.Dequeue();
            poolQueue.Enqueue(pooledObj);
            
            return pooledObj;
        }

        /// <summary>
        /// Tries to pop an inactive object from queue, if it's still active, it will instantiate a new object
        /// </summary>
        /// <param name="poolKey">key</param>
        /// <returns>inactive component</returns>
        public T PopInActivePooledObject(int poolKey)
        {
            Queue<T> poolQueue = new Queue<T>();
            if (!m_poolDictionary.TryGetValue(poolKey, out poolQueue))
            {
                Debug.LogWarning(message: $"{poolKey} does not exist");
                return null;
            }
            
            T pooledObj = poolQueue.Peek();
            if (pooledObj.gameObject.activeSelf)
            {
                GameObject obj = Instantiate(pooledObj.gameObject, m_parent, true);
                obj.SetActive(false);
                T newlyCreatedComponent = obj.GetComponent<T>();
                poolQueue.Enqueue(newlyCreatedComponent);
                pooledObj = newlyCreatedComponent;
            }
            else
            {
                poolQueue.Dequeue();
                poolQueue.Enqueue(pooledObj);
            }
            
            
            return pooledObj;
        }

        /// <summary>
        /// Gets an object from the stored key and place in specific position and rotation
        /// </summary>
        /// <param name="poolKey">key</param>
        /// <param name="position">spawn position</param>
        /// <param name="rotation">spawn rotation</param>
        /// <returns>Pooled Object</returns>
        public T SpawnAt(int poolKey, Vector3 position = new Vector3(), Vector3 rotation = new Vector3())
        {
            T spawnedItem = PopPooledObject(poolKey);
            spawnedItem.transform.rotation = Quaternion.Euler(rotation);
            spawnedItem.transform.position = position;
            spawnedItem.gameObject.SetActive(true);

            return spawnedItem;
        }

        /// <summary>
        /// Creates a PoolingObject
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="poolingObject">object to pool</param>
        /// <param name="size">amount to spawn</param>
        /// <returns></returns>
        protected static PoolingObject<T> CreatePoolingObject(int key, T poolingObject, int size)
        {
            return new PoolingObject<T>(key, poolingObject, size);
        }
        
        private void Clear()
        {
            if (m_parent == null)
            {
                return;
            }

            foreach (var pool in m_poolDictionary)
            {
                foreach (var queue in pool.Value)
                {
                    Destroy(queue.gameObject);
                }
            }
        }
    }

    /// <summary>
    /// Object for Pooling
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public struct PoolingObject<T> where T : Component
    {
        // Tried to use string for this one, and then hashing an int
        // but there were still collisions which causes issues (e.g. same keys for different things)
        public int key;
        public T pooledObject;
        public int size;

        /// <summary>
        /// Constructor for Pooler Object
        /// </summary>
        /// <param name="key">key for object</param>
        /// <param name="pooledObject">object to pool</param>
        /// <param name="size">size to instantiate</param>
        public PoolingObject(int key, T pooledObject, int size)
        {
            this.key = key;
            this.pooledObject = pooledObject;
            this.size = size;
        }
    }
}