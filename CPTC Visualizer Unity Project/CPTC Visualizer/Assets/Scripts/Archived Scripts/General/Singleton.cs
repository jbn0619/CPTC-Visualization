using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Author: The code used here was gathered from this online tutorial: http://www.unitygeek.com/unity_c_singleton/
/// Function: Allows any script that inherits from this one to be referenced anywhere within the scene without actually holding a refernece. Also allows singleton objects to persist between scenes.
/// </summary>
/// <typeparam name="T">The type this instance of the singleton is made from.</typeparam>
public class Singleton<T>: MonoBehaviour where T : Component
{
    #region Fields

    protected static T instance;

    [SerializeField]
    private bool stayInScene;
    
    #endregion Fields
    
    #region Properties

    /// <summary>
    /// The instance of this singleton in the scene.
    /// </summary>
    public static T Instance
    {
        get
        {
            // Check if there is no active instance.
            if (instance == null)
            {
                // Try and find an instance.
                instance = FindObjectOfType<T>();
                if (instance == null)
                {
                    // If no such instance exists, then make one.
                    GameObject obj = new GameObject();
                    obj.name = typeof(T).Name;
                    instance = obj.AddComponent<T>();
                }
            }

            return instance;
        }
    }
    
    #endregion Properties

    /// <summary>
    /// Checks if there is already an instance. If so, then destroy this occurance of the singleton.
    /// </summary>
    public virtual void Awake()
    {
        if (stayInScene)
        {
            if (instance == null)
            {
                instance = this as T;
            }
            else
            {
                Destroy(gameObject);
            }
        }
        else
        {
            if (instance == null)
            {
                instance = this as T;
                DontDestroyOnLoad(this.gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
