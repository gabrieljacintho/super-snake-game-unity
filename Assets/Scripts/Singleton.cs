using UnityEngine;

namespace GabrielBertasso
{
    public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T s_instance;

        public static T Instance
        {
            get
            {
                if (s_instance == null)
                {
                    s_instance = FindFirstObjectByType(typeof(T)) as T;
                }

                return s_instance;
            }
            private set => s_instance = value;
        }

        public virtual bool IsPersistent => false;


        protected virtual void Awake()
        {
            if (s_instance != null && s_instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this as T;

                if (IsPersistent)
                {
                    DontDestroyOnLoad(gameObject);
                }
            }
        }
    }
}
