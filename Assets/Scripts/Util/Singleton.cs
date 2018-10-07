using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    // private const string OBJECT_NAME = "Singleton";
    private static T _instance = null;

    public static T a
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType(typeof(T)) as T;
                //Object not found, we create a temporary one.
                if (_instance == null)
                {
                    GameObject gameObject = new GameObject(typeof(T).Name);
                    _instance = gameObject.AddComponent<T>();

                }

            }
            return _instance;
        }
    }

    protected virtual void Awake()
    {
        _instance = this as T;
    }

    private void OnDestroy()
    {
        if (_instance != null)
        {
            _instance = null;
        }
    }


}