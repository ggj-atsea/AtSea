using UnityEngine;
using System.Collections;

public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    private static T _inst;
    public static T Instance {
        get {
            if (_inst == null)
                _inst = Object.FindObjectOfType<T>();
            return (T)_inst;
        }
    }
    public static bool HasInstance {
        get {
            return (_inst != null);
        }
    }
	
	void Start() {
        _inst = (T)this;
	}
}
