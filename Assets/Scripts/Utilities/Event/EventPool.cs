using System;
using System.Collections.Generic;
using UnityEngine;

namespace JTUtility.Event
{
    public class EventPool
    {
        public static Dictionary<int, EventActions> DelegatePool = new Dictionary<int, EventActions>();

        public class EventActions
        {
            public List<Action> Actions = new List<Action>();
            public List<Action> rtActions = new List<Action>();

            public void Invoke()
            {
                if (rtActions.Count > Actions.Count)
                    rtActions.RemoveRange(Actions.Count, rtActions.Count - Actions.Count);

                for (int i = 0; i < Actions.Count; i++)
                {
                    if (rtActions.Count <= i)
                        rtActions.Add(Actions[i]);
                    else if (rtActions[i] != Actions[i])
                        rtActions[i] = Actions[i];
                }

                foreach (Action a in rtActions)
                {
                    try
                    {
                        //Debug.Log($"Invoking Action : {a.Target}.{a.Method}");
                        a();
                    }
                    catch(System.Exception e)
                    {
                        UnityEngine.Debug.LogException(e);
                    }
                }
            }
        }
    }

    public class EventPool<T1>
    {
        public static Dictionary<int, EventActions> DelegatePool = new Dictionary<int, EventActions>();

        public class EventActions
        {
            public List<Action<T1>> Actions = new List<Action<T1>>();
            public List<Action<T1>> rtActions = new List<Action<T1>>();

            public void Invoke(T1 value1)
            {
                if (rtActions.Count > Actions.Count)
                    rtActions.RemoveRange(Actions.Count, rtActions.Count - Actions.Count);

                for (int i = 0; i < Actions.Count; i++)
                {
                    if (rtActions.Count <= i)
                        rtActions.Add(Actions[i]);
                    else if (rtActions[i] != Actions[i])
                        rtActions[i] = Actions[i];
                }

                foreach (Action<T1> a in rtActions)
                {
                    try
                    {
                        //Debug.Log($"Invoking Action<{typeof(T1)}> : {a.Target}.{a.Method}");
                        a(value1);
                    }
                    catch (System.Exception e)
                    {
                        UnityEngine.Debug.LogException(e);
                    }
                }
            }
        }
    }

    public class EventPool<T1, T2>
    {
        public static Dictionary<int, EventActions> DelegatePool = new Dictionary<int, EventActions>();

        public class EventActions
        {
            public List<Action<T1, T2>> Actions = new List<Action<T1, T2>>();
            public List<Action<T1, T2>> rtActions = new List<Action<T1, T2>>();

            public void Invoke(T1 value1, T2 value2)
            {
                if (rtActions.Count > Actions.Count)
                    rtActions.RemoveRange(Actions.Count, rtActions.Count - Actions.Count);

                for (int i = 0; i < Actions.Count; i++)
                {
                    if (rtActions.Count <= i)
                        rtActions.Add(Actions[i]);
                    else if (rtActions[i] != Actions[i])
                        rtActions[i] = Actions[i];
                }

                try
                {
                    foreach (Action<T1, T2> a in rtActions)
                    {
                        try
                        {
                            //Debug.Log($"Invoking Action<{typeof(T1)},{typeof(T2)}> : {a.Target}.{a.Method}");
                            a(value1, value2);
                        }
                        catch (System.Exception e)
                        {
                            UnityEngine.Debug.LogException(e);
                        }
                    }
                }
                catch (System.Exception e)
                {
                    UnityEngine.Debug.LogException(e);
                }
            }
        }
    }

    public class EventPool<T1, T2, T3>
    {
        public static Dictionary<int, EventActions> DelegatePool = new Dictionary<int, EventActions>();

        public class EventActions
        {
            public List<Action<T1, T2, T3>> Actions = new List<Action<T1, T2, T3>>();
            public List<Action<T1, T2, T3>> rtActions = new List<Action<T1, T2, T3>>();

            public void Invoke(T1 value1, T2 value2, T3 value3)
            {
                if (rtActions.Count > Actions.Count)
                    rtActions.RemoveRange(Actions.Count, rtActions.Count - Actions.Count);

                for (int i = 0; i < Actions.Count; i++)
                {
                    if (rtActions.Count <= i)
                        rtActions.Add(Actions[i]);
                    else if (rtActions[i] != Actions[i])
                        rtActions[i] = Actions[i];
                }

                foreach (Action<T1, T2, T3> a in rtActions)
                {
                    try
                    {
                        //Debug.Log($"Invoking Action<{typeof(T1)},{typeof(T2)},{typeof(T3)}> : {a.Target}.{a.Method}");
                        a(value1, value2, value3);
                    }
                    catch (System.Exception e)
                    {
                        UnityEngine.Debug.LogException(e);
                    }
                }
            }
        }
    }

    public class EventPool<T1, T2, T3, T4>
    {
        public static Dictionary<int, EventActions> DelegatePool = new Dictionary<int, EventActions>();

        public class EventActions
        {
            public List<Action<T1, T2, T3, T4>> Actions = new List<Action<T1, T2, T3, T4>>();
            public List<Action<T1, T2, T3, T4>> rtActions = new List<Action<T1, T2, T3, T4>>();

            public void Invoke(T1 value1, T2 value2, T3 value3, T4 value4)
            {
                if (rtActions.Count > Actions.Count)
                    rtActions.RemoveRange(Actions.Count, rtActions.Count - Actions.Count);

                for (int i = 0; i < Actions.Count; i++)
                {
                    if (rtActions.Count <= i)
                        rtActions.Add(Actions[i]);
                    else if (rtActions[i] != Actions[i])
                        rtActions[i] = Actions[i];
                }

                foreach (Action<T1, T2, T3, T4> a in rtActions)
                {
                    try
                    {
                        //Debug.Log($"Invoking Action<{typeof(T1)},{typeof(T2)},{typeof(T3)},{typeof(T4)}> : {a.Target}.{a.Method}");
                        a(value1, value2, value3, value4);
                    }
                    catch (System.Exception e)
                    {
                        UnityEngine.Debug.LogException(e);
                    }
                }
            }
        }
    }
}