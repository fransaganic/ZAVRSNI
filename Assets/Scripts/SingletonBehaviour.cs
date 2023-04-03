using UnityEngine;

namespace ARudzbenik.General
{
    public abstract class SingletonBehaviour<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance = null;

        public static T Instance
        {
            get
            {
                if (_instance == null) _instance = FindObjectOfType<T>();
                return _instance;
            }
        }

        protected virtual void Awake()
        {
            if (_instance == null) _instance = this as T;
            if (_instance != this) Destroy(gameObject);
        }
    }
}