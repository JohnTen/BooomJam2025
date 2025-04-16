using UnityEngine.Events;

namespace JTUtility.Event
{
    public class EventProptyGetEvent<T> : UnityEvent<T>
    { }

    public class EventProptySetEvent<T> : UnityEvent<T>
    { }

    public class EventProperty<T>
    {
        /// <summary>
        /// 获取事件
        /// </summary>
        private EventProptyGetEvent<T> get = new EventProptyGetEvent<T>();

        /// <summary>
        /// 改变事件
        /// </summary>
        private EventProptySetEvent<T> set = new EventProptySetEvent<T>();

        /// <summary>
        /// 获取器
        /// </summary>
        private Func<T> getter = null;

        /// <summary>
        /// 设置器
        /// </summary>
        private Action<T> setter = null;

        private Func<T, T, bool> equaler = null;

        private T _value;

        public void Invoke()
        {
            set?.Invoke(_value);
        }

        /// <summary>
        /// 值
        /// </summary>
        public T propValue
        {
            get
            {
                if (getter == null)
                {
                    if (get != null)
                        get.Invoke(_value);
                    return _value;
                }
                else
                {
                    if (get != null)
                        get.Invoke(getter());
                    return getter();
                }
            }
            set
            {
                if (setter == null)
                {
                    if (value == null)
                    {
                        _value = default(T);
                    }
                    else if (equaler(value, _value))
                    {
                        return;
                    }
                    else
                    {
                        _value = value;
                    }

                    if (set != null)
                        set.Invoke(_value);
                }
                else
                {
                    if (value == null)
                    {
                        setter(default(T));
                    }
                    else if (getter != null && equaler(value, getter()))
                    {
                        return;
                    }
                    else if (getter == null && equaler(value, _value))
                    {
                        return;
                    }
                    else
                    {
                        _value = value;
                        setter(value);
                    }
                    if (set != null)
                        set.Invoke(value);
                }
            }
        }

        /// <summary>
        /// 初始化值
        /// </summary>
        public void Init()
        {
            if (set != null)
            {
                if (getter != null && setter != null)
                {
                    set.Invoke(getter());
                }
                else
                {
                    set.Invoke(_value);
                }
            }
        }

        public EventProperty()
        {
            equaler = (T1, T2) => { return T1.Equals(T2); };
        }

        public EventProperty(T value)
        {
            this._value = value;
            equaler = (T1, T2) => { return T1.Equals(T2); };
        }

        public EventProperty(T value, Func<T, T, bool> equaler)
        {
            this._value = value;
            this.equaler = equaler;
        }

        public EventProperty(Func<T> getter, Action<T> setter)
        {
            this.getter = getter;
            this.setter = setter;
            equaler = (T1, T2) => { return T1.Equals(T2); };
        }

        public void AddSetListener(UnityAction<T> call)
        {
            set.AddListener(call);
        }

        public void RemoveSetListener(UnityAction<T> call)
        {
            set.RemoveListener(call);
        }

        public void RemoveAllSetListener()
        {
            set.RemoveAllListeners();
        }

        public void AddGetListener(UnityAction<T> call)
        {
            get.AddListener(call);
        }

        public void RemoveGetListener(UnityAction<T> call)
        {
            get.RemoveListener(call);
        }

        public void RemoveAllGetListener()
        {
            get.RemoveAllListeners();
        }
    }
}