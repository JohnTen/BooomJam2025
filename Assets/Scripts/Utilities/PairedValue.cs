using System;
using UnityEngine;

namespace JTUtility
{
    [Serializable]
    public abstract class PairedValue
    { }

    [Serializable]
    public class PairedValue<K, V> : PairedValue
    {
        [SerializeField] private K key;
        [SerializeField] private V value;

        public K Key
        {
            get { return key; }
            set { key = value; }
        }

        public V Value
        {
            get { return value; }
            set { this.value = value; }
        }

        public PairedValue()
        {
            key = default;
            value = default;
        }

        public PairedValue(K key, V value)
        {
            this.key = key;
            this.value = value;
        }

        public PairedValue(PairedValue<K, V> other)
        {
            this.key = other.key;
            this.value = other.value;
        }
    }

    [Serializable]
    public class IntIntPair : PairedValue<int, int>
    {
        public IntIntPair() : base()
        {
        }

        public IntIntPair(int key, int value) : base(key, value)
        {
        }

        public IntIntPair(IntIntPair other) : base(other)
        {
        }
    }

    [Serializable]
    public class StrIntPair : PairedValue<string, int>
    {
        public StrIntPair() : base()
        {
        }

        public StrIntPair(string key, int value) : base(key, value)
        {
        }

        public StrIntPair(StrIntPair other) : base(other)
        {
        }
    }

    [Serializable]
    public class StrBoolPair : PairedValue<string, bool>
    {
        public StrBoolPair() : base()
        {
        }

        public StrBoolPair(string key, bool value) : base(key, value)
        {
        }

        public StrBoolPair(StrBoolPair other) : base(other)
        {
        }
    }

    [Serializable]
    public class StrFloatPair : PairedValue<string, float>
    {
        public StrFloatPair() : base()
        {
        }

        public StrFloatPair(string key, float value) : base(key, value)
        {
        }

        public StrFloatPair(StrFloatPair other) : base(other)
        {
        }
    }

    [Serializable]
    public class StrStrPair : PairedValue<string, string>
    {
        public StrStrPair() : base()
        {
        }

        public StrStrPair(string key, string value) : base(key, value)
        {
        }

        public StrStrPair(StrStrPair other) : base(other)
        {
        }
    }

    [Serializable]
    public class StrVecPair : PairedValue<string, Vector3>
    {
        public StrVecPair() : base()
        {
        }

        public StrVecPair(string key, Vector3 value) : base(key, value)
        {
        }

        public StrVecPair(StrVecPair other) : base(other)
        {
        }
    }

    [Serializable]
    public class V2IntStrPair : PairedValue<Vector2Int, string>
    {
        public V2IntStrPair() : base()
        {
        }

        public V2IntStrPair(Vector2Int key, string value) : base(key, value)
        {
        }

        public V2IntStrPair(V2IntStrPair other) : base(other)
        {
        }
    }
}