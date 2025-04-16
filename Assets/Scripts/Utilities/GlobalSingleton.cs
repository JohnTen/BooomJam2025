using UnityEngine;

namespace JTUtility
{
    public abstract class GlobalSingleton<T> : MonoBehaviour where T : GlobalSingleton<T>
    {
        protected static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance) return _instance;

                _instance = GlobalObject.Instance.GetOrAddComponent<T>();
                return _instance;
            }
        }

        private void Awake()
        {
            if (Instance != this)
            {
                Destroy(this.gameObject);
                return;
            }

            if (!Instance.transform.GetComponentInParent<GlobalObject>())
            {
                Debug.LogWarning(typeof(T).Name + " isn't attached on GlobalObject!");
            }

            OnCreated();
        }

        private void OnDestroy()
        {
            if (_instance != this)
                return;

            OnDestroying();
            _instance = null;

            Debug.LogWarning("The instance of a GlobalSingleton has been destroied!");
        }

        protected virtual void OnCreated()
        { }

        protected virtual void OnDestroying()
        { }
    }
}