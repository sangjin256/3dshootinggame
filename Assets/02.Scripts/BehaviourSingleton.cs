using UnityEngine;
using System.Collections;

public abstract class BehaviourSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance = null;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindFirstObjectByType(typeof(T)) as T;
                if (_instance == null)
                {

                }
            }
            return _instance;
        }
        set
        {
            _instance = value;
        }
    }
}