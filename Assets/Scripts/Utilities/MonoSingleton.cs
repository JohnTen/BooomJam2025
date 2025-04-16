using UnityEngine;

namespace JTUtility
{
    /// <summary>
    /// Mono singleton Class. Extend this class to make singleton component.
    /// Example:
    /// <code>
    /// public class Foo : MonoSingleton<Foo>
    /// </code>. To get the instance of Foo class, use <code>Foo.instance</code>
    /// Override <code>Init()</code> method instead of using <code>Awake()</code>
    /// from this class.
    /// </summary>
    ///
    public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
    {
        protected static T m_Instance = null;
        private static object obj = new object();

        public static bool HasInstance
        {
            get
            {
                if (m_Instance.IsNotNull())
                {
                    return true;
                }

                _isInitialized = false;
                m_Instance = GameObject.FindObjectOfType(typeof(T)) as T;
                return m_Instance.IsNotNull();
            }
        }

        public static T Instance
        {
            get
            {
                lock (obj)
                {
                    // Instance requiered for the first time, we look for it
                    if (m_Instance.IsNull())
                    {
                        _isInitialized = false;
                        m_Instance = GameObject.FindObjectOfType(typeof(T)) as T;

                        // Object not found, we create a temporary one
                        if (m_Instance.IsNull())
                        {
#if UNITY_EDITOR
                            Debug.LogWarning("No instance of " + typeof(T).ToString() + ", a temporary one is created.");
#endif

                            isTemporaryInstance = true;
                            var temp_Instance = new GameObject("Temp Instance of " + typeof(T).ToString(), typeof(T)).GetComponent<T>();

                            // In case the instance here has been replaced by the class initialization
                            if (m_Instance.IsNull())
                                m_Instance = temp_Instance;

                            // Problem during the creation, this should not happen
                            if (m_Instance == null)
                            {
                                Debug.LogError("Problem during the creation of " + typeof(T).ToString());
                            }
                        }
                    }
                    return m_Instance;
                }
            }
        }

        public static bool isTemporaryInstance { protected set; get; }

        private static bool _isInitialized = false;

        // If no other monobehaviour request the instance in an awake function
        // executing before this one, no need to search the object.
        private void Awake()
        {
            if (m_Instance.IsNull())
            {
                m_Instance = this as T;
                _isInitialized = false;
            }
            else if (m_Instance != this)
            {
                Debug.LogError("Another instance of " + GetType() + " is already exist! Destroying self...");
                DestroyImmediate(gameObject);
                return;
            }

            if (!_isInitialized)
            {
                //if (Application.isPlaying)
                //    DontDestroyOnLoad(gameObject);

                OnInit();
                _isInitialized = true;
            }
        }

        /// <summary>
        /// 调用只是为了确保初始化，避免因为场景更新挂载物体丢失的问题
        /// </summary>
        public void Init()
        {
        }

        protected virtual void OnInit()
        {
        }

        /// Make sure the instance isn't referenced anymore when the user quit, just in case.
        private void OnApplicationQuit()
        {
            m_Instance = null;
        }
    }
}