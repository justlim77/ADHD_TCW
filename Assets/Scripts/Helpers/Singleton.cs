﻿using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;

    public static T Instance {
        get {
            if (applicationIsQuitting) {
                //Debug.LogWarning("[Singleton] Instance '" + typeof(T) +
                //    "' is already destroyed on application quit." +
                //    " Returning null.");

                return null;
            }

            if (_instance == null)
            {
                GameObject singleton = new GameObject();
                _instance = singleton.AddComponent<T>();
                singleton.name = typeof(T).ToString();

                DontDestroyOnLoad(singleton);

                //Debug.Log("[Singleton] An instance of " + typeof(T) + "is needed in the scene, so '"
                //    + singleton + "' was created with DontDestroyOnLoad().");
            }

            else {
                //Debug.Log("[Singleton] Using instance already created: " +
                //    _instance.gameObject.name);
            }

            return _instance;
        }
    }

    /// <summary>
    /// /// When Unity quits, it destroys objects in a random order.
	/// In principle, a Singleton is only destroyed when application quits.
	/// If any script calls Instance after it have been destroyed, 
	/// it will create a buggy ghost object that will stay on the Editor scene
	/// even after stopping playing the Application. Really bad!
	/// So, this was made to be sure we're not creating that buggy ghost object.
    /// </summary>
    private static bool applicationIsQuitting = false;
    
    public void OnDestroy() {
        applicationIsQuitting = true;
    }
}
