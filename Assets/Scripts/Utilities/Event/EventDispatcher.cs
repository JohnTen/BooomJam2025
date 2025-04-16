namespace JTUtility.Event
{
    public class EventDispatcher
    {
        public static void Dispatch(int name)
        {
            if (Contains(name))
            {
                EventPool.DelegatePool[name].Invoke();
            }
        }

        private static bool Contains(int s)
        {
            return EventPool.DelegatePool.ContainsKey(s);
        }
    }

    public class EventDispatcher<T>
    {
        public static void Dispatch(int name, T value)
        {
            if (Contains(name))
            {
                EventPool<T>.DelegatePool[name].Invoke(value);
            }
        }

        private static bool Contains(int s)
        {
            return EventPool<T>.DelegatePool.ContainsKey(s);
        }
    }

    public class EventDispatcher<T1, T2>
    {
        public static void Dispatch(int name, T1 value1, T2 value2)
        {
            if (Contains(name))
            {
                EventPool<T1, T2>.DelegatePool[name].Invoke(value1, value2);
            }
        }

        private static bool Contains(int s)
        {
            return EventPool<T1, T2>.DelegatePool.ContainsKey(s);
        }
    }

    public class EventDispatcher<T1, T2, T3>
    {
        public static void Dispatch(int name, T1 value1, T2 value2, T3 value3)
        {
            if (Contains(name))
            {
                EventPool<T1, T2, T3>.DelegatePool[name].Invoke(value1, value2, value3);
            }
        }

        private static bool Contains(int s)
        {
            return EventPool<T1, T2, T3>.DelegatePool.ContainsKey(s);
        }
    }

    public class EventDispatcher<T1, T2, T3, T4>
    {
        public static void Dispatch(int name, T1 value1, T2 value2, T3 value3, T4 value4)
        {
            if (Contains(name))
            {
                EventPool<T1, T2, T3, T4>.DelegatePool[name].Invoke(value1, value2, value3, value4);
            }
        }

        private static bool Contains(int s)
        {
            return EventPool<T1, T2, T3, T4>.DelegatePool.ContainsKey(s);
        }
    }
}