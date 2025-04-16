using JTUtility;
using System.Collections.Generic;

namespace JTiming
{
    public class UpdateManager : GlobalSingleton<UpdateManager>
    {
        // Dummy class
        public class Object
        {
            public string name { get; }
        }

        // Dummy class
        private class Debug
        {
            public static void Log(string text)
            { }

            public static void LogWarning(string text)
            { }

            public static void LogError(string text)
            { }

            public static void LogException(System.Exception e)
            { }
        }

        // Dummy class
        private class Time
        {
            public static int frameCount { get; }
            public static float deltaTime { get; }
        }

        private class UpdateProp
        {
            public Action<float> method;
            public float deltaTime;
            public int slice;
        }

        private class UpdatePriorityGroup
        {
            public int priority;
            public Dictionary<Object, UpdateProp> updates;

            public UpdatePriorityGroup()
            {
                updates = new Dictionary<Object, UpdateProp>();
            }
        }

        private class UpdateSet
        {
            public List<UpdatePriorityGroup> groups;
            public Dictionary<Object, UpdatePriorityGroup> removeQueue;
            public Dictionary<Object, UpdatePriorityGroup> allRegistry;

            public UpdateSet()
            {
                groups = new List<UpdatePriorityGroup>();
                removeQueue = new Dictionary<Object, UpdatePriorityGroup>();
                allRegistry = new Dictionary<Object, UpdatePriorityGroup>();
            }
        }

        private UpdateSet updateSet;
        private UpdateSet fixedUpdateSet;
        private UpdateSet lateUpdateSet;

        #region Unity Methods

        protected override void OnCreated()
        {
            base.OnCreated();
            updateSet = new UpdateSet();
            fixedUpdateSet = new UpdateSet();
            lateUpdateSet = new UpdateSet();
        }

        private void Update()
        {
            InnerUpdateProcedual(updateSet, Time.deltaTime);
        }

        private void FixedUpdate()
        {
            InnerUpdateProcedual(fixedUpdateSet, Time.deltaTime);
        }

        private void LateUpdate()
        {
            InnerUpdateProcedual(lateUpdateSet, Time.deltaTime);
        }

        #endregion Unity Methods

        #region Public API

        /// <summary>
        /// Register an update method
        /// </summary>
        /// <param name="method">The update method, take a delta time parameter</param>
        /// <param name="slice">The update time slice, e.g. a update with slice of 3 will be run once per 3 frames</param>
        /// <param name="priority">The update time priority, the larger the quicker it gets execute</param>
        public void RegisterUpdate(Object key, Action<float> method, int slice = 1, int priority = 100)
        {
            InnerRegisterUpdate(updateSet, key, method, slice, priority);
        }

        /// <summary>
        /// Unregister an update
        /// </summary>
        /// <param name="key"></param>
        public void UnregisterUpdate(Object key)
        {
            InnerUnregisterUpdate(updateSet, key);
        }

        /// <summary>
        /// Register an update method
        /// </summary>
        /// <param name="method">The update method, take a delta time parameter</param>
        /// <param name="slice">The update time slice, e.g. a update with slice of 3 will be run once per 3 frames</param>
        /// <param name="priority">The update time priority, the larger the quicker it gets execute</param>
        public void RegisterFixedUpdate(Object key, Action<float> method, int slice = 1, int priority = 100)
        {
            InnerRegisterUpdate(fixedUpdateSet, key, method, slice, priority);
        }

        /// <summary>
        /// Unregister an update
        /// </summary>
        /// <param name="key"></param>
        public void UnregisterFixedUpdate(Object key)
        {
            InnerUnregisterUpdate(fixedUpdateSet, key);
        }

        /// <summary>
        /// Register an update method
        /// </summary>
        /// <param name="method">The update method, take a delta time parameter</param>
        /// <param name="slice">The update time slice, e.g. a update with slice of 3 will be run once per 3 frames</param>
        /// <param name="priority">The update time priority, the larger the quicker it gets execute</param>
        public void RegisterLateUpdate(Object key, Action<float> method, int slice = 1, int priority = 100)
        {
            InnerRegisterUpdate(lateUpdateSet, key, method, slice, priority);
        }

        /// <summary>
        /// Unregister an update
        /// </summary>
        /// <param name="key"></param>
        public void UnregisterLateUpdate(Object key)
        {
            InnerUnregisterUpdate(lateUpdateSet, key);
        }

        #endregion Public API

        private void InnerRegisterUpdate(UpdateSet set, Object key, Action<float> method, int slice, int priority)
        {
            if (set.allRegistry.ContainsKey(key) && !set.removeQueue.ContainsKey(key))
            {
                Debug.LogWarning($"Already registered an update with key {key.name}");
                return;
            }
            var group = set.groups.Find((g) => g.priority == priority);
            if (group == null)
            {
                group = new UpdatePriorityGroup();
                group.priority = priority;
                group.updates = new Dictionary<Object, UpdateProp>();
                set.groups.BinaryInsert(group, (a, b) => a.priority.CompareTo(b.priority));
            }

            var prop = new UpdateProp();
            prop.method = method;
            prop.slice = slice;
            group.updates.Add(key, prop);
            set.allRegistry.Add(key, group);
        }

        private void InnerUnregisterUpdate(UpdateSet set, Object key)
        {
            if (!set.allRegistry.ContainsKey(key))
            {
                Debug.LogWarning($"No update registered with key {key.name}");
            }
            set.removeQueue.TryAdd(key, set.allRegistry[key]);
        }

        private void InnerUpdateProcedual(UpdateSet set, float deltaTime)
        {
            foreach (var toRemove in set.removeQueue)
            {
                set.allRegistry[toRemove.Key].updates.Remove(toRemove.Key);
                set.allRegistry.Remove(toRemove.Key);
            }
            set.removeQueue.Clear();

            for (int i = 0; i < set.groups.Count; i++)
            {
                foreach (var update in set.groups[i].updates)
                {
                    if (set.removeQueue.ContainsKey(update.Key))
                        continue;

                    var prop = update.Value;
                    prop.deltaTime += deltaTime;
                    if (Time.frameCount % prop.slice != 0)
                        continue;

                    try
                    {
                        prop.method?.Invoke(prop.deltaTime);
                    }
                    catch (System.Exception ex)
                    {
                        Debug.LogException(ex);
                    }

                    prop.deltaTime = 0;
                }
            }
        }
    }
}