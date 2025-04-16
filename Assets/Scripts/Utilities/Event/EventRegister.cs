namespace JTUtility.Event
{
    public class EventRegister
    {
        public static void Register(int s, Action action)
        {
            EventPool.EventActions actions = GetEventActionsWithName(s);
            if (actions.Actions.Contains(action) == false)
                actions.Actions.Add(action);
        }

        public static void UnRegister(int s, Action action)
        {
            if (EventPool.DelegatePool.ContainsKey(s))
            {
                EventPool.EventActions actions = GetEventActionsWithName(s);
                if (actions.Actions.Contains(action))
                    actions.Actions.Remove(action);
            }
        }

        private static EventPool.EventActions GetEventActionsWithName(int s)
        {
            if (!EventPool.DelegatePool.ContainsKey(s))
                EventPool.DelegatePool.Add(s, new EventPool.EventActions());

            return EventPool.DelegatePool[s];
        }
    }

    public class EventRegister<T>
    {
        public static void Register(int s, Action<T> action)
        {
            EventPool<T>.EventActions actions = GetEventActionsWithName(s);
            if (actions.Actions.Contains(action) == false)
                actions.Actions.Add(action);
        }

        public static void UnRegister(int s, Action<T> action)
        {
            if (EventPool<T>.DelegatePool.ContainsKey(s))
            {
                EventPool<T>.EventActions actions = GetEventActionsWithName(s);
                if (actions.Actions.Contains(action))
                    actions.Actions.Remove(action);
            }
        }

        private static EventPool<T>.EventActions GetEventActionsWithName(int s)
        {
            if (!EventPool<T>.DelegatePool.ContainsKey(s))
                EventPool<T>.DelegatePool.Add(s, new EventPool<T>.EventActions());

            return EventPool<T>.DelegatePool[s];
        }
    }

    public class EventRegister<T1, T2>
    {
        public static void Register(int s, Action<T1, T2> action)
        {
            EventPool<T1, T2>.EventActions actions = GetEventActionsWithName(s);
            if (actions.Actions.Contains(action) == false)
                actions.Actions.Add(action);
        }

        public static void UnRegister(int s, Action<T1, T2> action)
        {
            if (EventPool<T1, T2>.DelegatePool.ContainsKey(s))
            {
                EventPool<T1, T2>.EventActions actions = GetEventActionsWithName(s);
                if (actions.Actions.Contains(action))
                    actions.Actions.Remove(action);
            }
        }

        private static EventPool<T1, T2>.EventActions GetEventActionsWithName(int s)
        {
            if (!EventPool<T1, T2>.DelegatePool.ContainsKey(s))
                EventPool<T1, T2>.DelegatePool.Add(s, new EventPool<T1, T2>.EventActions());

            return EventPool<T1, T2>.DelegatePool[s];
        }
    }

    public class EventRegister<T1, T2, T3>
    {
        public static void Register(int s, Action<T1, T2, T3> action)
        {
            EventPool<T1, T2, T3>.EventActions actions = GetEventActionsWithName(s);
            if (actions.Actions.Contains(action) == false)
                actions.Actions.Add(action);
        }

        public static void UnRegister(int s, Action<T1, T2, T3> action)
        {
            if (EventPool<T1, T2, T3>.DelegatePool.ContainsKey(s))
            {
                EventPool<T1, T2, T3>.EventActions actions = GetEventActionsWithName(s);
                if (actions.Actions.Contains(action))
                    actions.Actions.Remove(action);
            }
        }

        private static EventPool<T1, T2, T3>.EventActions GetEventActionsWithName(int s)
        {
            if (!EventPool<T1, T2, T3>.DelegatePool.ContainsKey(s))
                EventPool<T1, T2, T3>.DelegatePool.Add(s, new EventPool<T1, T2, T3>.EventActions());

            return EventPool<T1, T2, T3>.DelegatePool[s];
        }
    }

    public class EventRegister<T1, T2, T3, T4>
    {
        public static void Register(int s, Action<T1, T2, T3, T4> action)
        {
            EventPool<T1, T2, T3, T4>.EventActions actions = GetEventActionsWithName(s);
            if (actions.Actions.Contains(action) == false)
                actions.Actions.Add(action);
        }

        public static void UnRegister(int s, Action<T1, T2, T3, T4> action)
        {
            if (EventPool<T1, T2, T3, T4>.DelegatePool.ContainsKey(s))
            {
                EventPool<T1, T2, T3, T4>.EventActions actions = GetEventActionsWithName(s);
                if (actions.Actions.Contains(action))
                    actions.Actions.Remove(action);
            }
        }

        private static EventPool<T1, T2, T3, T4>.EventActions GetEventActionsWithName(int s)
        {
            if (!EventPool<T1, T2, T3, T4>.DelegatePool.ContainsKey(s))
                EventPool<T1, T2, T3, T4>.DelegatePool.Add(s, new EventPool<T1, T2, T3, T4>.EventActions());

            return EventPool<T1, T2, T3, T4>.DelegatePool[s];
        }
    }
}