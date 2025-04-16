using System;
using System.Collections.Generic;
using UnityEngine;

namespace JTUtility
{
    // 所有枚举必须预赋唯一值, 赋值后不能修改
    public enum UDStt : short
    {
        Example = 9999,
        IsReplacing = 1,

        /// <summary>
        /// Avoid = -1, Indifferent = 0, Aggressive = 1,
        /// </summary>
        Aggressive = 2,

        /// <summary>
        /// 用于判断执行步骤，通常0=>未开始，999=>完全结束
        /// </summary>
        Stage = 3,

        /// <summary>
        /// 事件的结果, 通常0为false，非0为true，特殊情况自定
        /// </summary>
        Result = 4,

        /// <summary>
        /// 时间段，0为任何时候，1为在手牌中（Card）或在场上（Brick）
        /// </summary>
        Timeframe = 5,

        /// <summary>
        /// 修改分数的行为，0为默认，1为不会减少分数
        /// </summary>
        KeepScore = 6,

        /// <summary>
        /// 在一些特殊条件的applybuff中，用特殊条件的计数作为buff的stack
        /// </summary>
        CountAsStack = 7,

        /// <summary>
        /// 默认为0，1时Buff/Aoe执行的阵营将以施放者的阵营为准
        /// </summary>
        UseCasterSide = 8,

        /// <summary>
        /// 默认为0，1时Buff/Aoe执行的阵营将以回合的阵营为准
        /// </summary>
        UseTurnSide = 9,

        /// <summary>
        /// 设置一些Buff的tag为Consistent
        /// </summary>
        Consistent = 10,

        VFXBlockControl = 11,

        IncludeFlippedBrick = 12,

        BuffIconSetting = 13,

        ForceRemove = 14,

        UseAoeShape = 15,

        /// <summary>
        /// 默认为0，0为不包括任何，1为正面，2为负面，3为全部包括
        /// </summary>
        FlipState = 16,

        TriggerOnce = 17,

        /// 0 => undefined
        /// 1 => self(turn or stack became 0)
        /// 2 => other(other than self)
        RemovedBy = 18,

        // 0 => default enable auto vfx in applybuff
        // 1 => disable auto vfx
        NoAutoVFX = 19,
    }

    public enum UDVc3 : short
    {
        Example = 9999,
        Position = 1,
        Size = 2,
    }

    public enum UDV2I : short
    {
        Example = 9999,
        Position = 1,
    }

    public enum UDInt : short
    {
        Example = 9999,
        Count = 1,
        Side = 2,
        LastSide = 3,
        Stack = 4,
        AIScoreAffector_Add = 5,
        AIScoreAffector_Mul = 6,
        Score = 7,
        Turns = 8,
        DestroiedBrickCount = 9,
        DestroiedPaintCount = 10,
        Cost = 11,
        BuffIconSide = 12,
        TriggerOnceCounter = 13,

        // SpecValues
        OverloadedAltarOccupied = 10001,
        OverloadedAltarUnoccupied = 10002,
        LeftEyeScore = 10003,
        RightEyeScore = 10004,

        // EnumValues
        TargetSide = 20001,
        TargetType = 20002,
        TargetSide2 = 20003,
        TargetType2 = 20004,
    }

    public enum UDFlt : short
    {
        Example = 9999,
        StackMultiplier = 1,
    }

    public enum UDStr : short
    {
        Example = 9999,
        BrickID = 1,
        UnitID = 2,
        CardID = 3,
        MultiCardID = 9,
        AoeID = 4,
        BuffID = 5,
        GridID = 6,
        OrigGridID = 7,
        VFXID = 8,
        MultiGridID = 10,
        NewBuffID = 11,
        AssembleBrickID = 12,
        Keywords = 13,
        MultiBuffID = 14,
        TargetTag = 15,
        MultiBrickID = 16,
        VFXQueueID = 17,
        BrickTags = 18,
        TargetBuffID = 19,
        BuffIcon = 20,
    }

    public enum UDRef : short
    {
        Example = 9999,
        AddBuffInfo = 1,
        AddAoeInfo = 2,
        BuffCarrier = 3,
        V2iPositions = 4,
        GridRotation = 5,
        GridModel = 6,
        OrigGridModel = 7,
        GridShape = 8,
        LastPlacedBrick = 9,
        BrickList = 10,
        BrickRecord = 11,
        V3iPlacements = 12,
        MapV2iPositions = 13,
        MapV3iPlacements = 14,
        VFXClip1 = 15,
        VFXClip2 = 16,
        VFXClipList = 17,
        SpawnPlacements = 18,
        ActiveTurns = 19,
        QuestAreas = 20,
        RandomStacks = 21,
        V2iVfxPairs = 22,
        QuestAreaInstance = 23,
        AppliedBuffs = 24,
        CopyCatCardRef = 25,
        RelicCard = 26,
    }

    [Serializable]
    public class SerializableUData
    {
        [Serializable] private class SttPair : PairedValue<UDStt, int> { }

        [SerializeField] private List<SttPair> stts;// State, used as bool

        [Serializable] private class Vc3Pair : PairedValue<UDVc3, Vector3> { }

        [SerializeField] private List<Vc3Pair> vc3s;

        [Serializable] private class V2iPair : PairedValue<UDV2I, Vector2Int> { }

        [SerializeField] private List<V2iPair> v2is;

        [Serializable] private class IntPair : PairedValue<UDInt, int> { }

        [SerializeField] private List<IntPair> ints;

        [Serializable] private class FltPair : PairedValue<UDFlt, float> { }

        [SerializeField] private List<FltPair> flts;

        [Serializable] private class StrPair : PairedValue<UDStr, string> { }

        [SerializeField] private List<StrPair> strs;

        [Serializable] private class RefPair : PairedValue<UDRef, UnityEngine.Object> { }

        [SerializeField] private List<RefPair> refs;

        public UData Deserialize()
        {
            UData uData = new UData();
            if (stts != null)
            {
                uData.stts = new Dictionary<UDStt, int>();
                uData.stts.AddRange(stts);
            }

            if (vc3s != null)
            {
                uData.vc3s = new Dictionary<UDVc3, Vector3>();
                uData.vc3s.AddRange(vc3s);
            }

            if (v2is != null)
            {
                uData.v2is = new Dictionary<UDV2I, Vector2Int>();
                uData.v2is.AddRange(v2is);
            }

            if (ints != null)
            {
                uData.ints = new Dictionary<UDInt, int>();
                uData.ints.AddRange(ints);
            }

            if (flts != null)
            {
                uData.flts = new Dictionary<UDFlt, float>();
                uData.flts.AddRange(flts);
            }

            if (strs != null)
            {
                uData.strs = new Dictionary<UDStr, string>();
                uData.strs.AddRange(strs);
            }

            if (refs != null)
            {
                uData.refs = new Dictionary<UDRef, object>();
                for (int i = 0; i < refs.Count; i++)
                {
                    uData.refs.Add(refs[i].Key, refs[i].Value);
                }
            }

            return uData;
        }
    }

    /// <summary>
    /// Universal Data.
    /// </summary>
    public class UData
    {
        public Dictionary<UDStt, int> stts = new Dictionary<UDStt, int>();// State, used as bool
        public Dictionary<UDVc3, Vector3> vc3s = new Dictionary<UDVc3, Vector3>();
        public Dictionary<UDV2I, Vector2Int> v2is = new Dictionary<UDV2I, Vector2Int>();
        public Dictionary<UDInt, int> ints = new Dictionary<UDInt, int>();
        public Dictionary<UDFlt, float> flts = new Dictionary<UDFlt, float>();
        public Dictionary<UDStr, string> strs = new Dictionary<UDStr, string>();
        public Dictionary<UDRef, object> refs = new Dictionary<UDRef, object>();

        #region index[]

        public int? this[UDStt idx]
        {
            get { if (stts != null && stts.ContainsKey(idx)) return stts[idx]; return null; }
            set
            {
                if (stts == null) stts = new Dictionary<UDStt, int>();
                if (value == null)
                    stts.TryRemove(idx);
                else
                    stts.TrySet(idx, value.Value);
            }
        }

        public Vector3? this[UDVc3 idx]
        {
            get { if (vc3s != null && vc3s.ContainsKey(idx)) return vc3s[idx]; return null; }
            set
            {
                if (vc3s == null) vc3s = new Dictionary<UDVc3, Vector3>();
                if (value == null)
                    vc3s.TryRemove(idx);
                else
                    vc3s.TrySet(idx, value.Value);
            }
        }

        public Vector2Int? this[UDV2I idx]
        {
            get { if (v2is != null && v2is.ContainsKey(idx)) return v2is[idx]; return null; }
            set
            {
                if (v2is == null) v2is = new Dictionary<UDV2I, Vector2Int>();
                if (value == null)
                    v2is.TryRemove(idx);
                else
                    v2is.TrySet(idx, value.Value);
            }
        }

        public int? this[UDInt idx]
        {
            get { if (ints != null && ints.ContainsKey(idx)) return ints[idx]; return null; }
            set
            {
                if (ints == null) ints = new Dictionary<UDInt, int>();
                if (value == null)
                    ints.TryRemove(idx);
                else
                    ints.TrySet(idx, value.Value);
            }
        }

        public float? this[UDFlt idx]
        {
            get { if (flts != null && flts.ContainsKey(idx)) return flts[idx]; return null; }
            set
            {
                if (flts == null) flts = new Dictionary<UDFlt, float>();
                if (value == null)
                    flts.TryRemove(idx);
                else
                    flts.TrySet(idx, value.Value);
            }
        }

        public string this[UDStr idx]
        {
            get { if (strs != null && strs.ContainsKey(idx)) return strs[idx]; return null; }
            set
            {
                if (strs == null) strs = new Dictionary<UDStr, string>();
                if (string.IsNullOrWhiteSpace(value))
                    strs.TryRemove(idx);
                else
                    strs.TrySet(idx, value);
            }
        }

        public object this[UDRef idx]
        {
            get { if (refs != null && refs.ContainsKey(idx)) return refs[idx]; return null; }
            set
            {
                if (refs == null) refs = new Dictionary<UDRef, object>();
                if (value.IsNull())
                    refs.TryRemove(idx);
                else
                    refs.TrySet(idx, value);
            }
        }

        #endregion index[]

        #region Set(Sequence)

        public UData Set(UDStt key, int value)
        {
            this[key] = value;
            return this;
        }

        public UData Set(UDVc3 key, Vector3 value)
        {
            this[key] = value;
            return this;
        }

        public UData Set(UDV2I key, Vector2Int value)
        {
            this[key] = value;
            return this;
        }

        public UData Set(UDInt key, int value)
        {
            this[key] = value;
            return this;
        }

        public UData Set(UDFlt key, float value)
        {
            this[key] = value;
            return this;
        }

        public UData Set(UDStr key, string value)
        {
            this[key] = value;
            return this;
        }

        public UData Set(UDRef key, object value)
        {
            this[key] = value;
            return this;
        }

        #endregion Set(Sequence)

        #region TryGet

        public int TryGet(UDStt key, int defaultValue = default)
        {
            if (stts.ContainsKey(key))
                return stts[key];
            return defaultValue;
        }

        public Vector3 TryGet(UDVc3 key, Vector3 defaultValue = default)
        {
            if (vc3s.ContainsKey(key))
                return vc3s[key];
            return defaultValue;
        }

        public Vector2Int TryGet(UDV2I key, Vector2Int defaultValue = default)
        {
            if (v2is.ContainsKey(key))
                return v2is[key];
            return defaultValue;
        }

        public int TryGet(UDInt key, int defaultValue = default)
        {
            if (ints.ContainsKey(key))
                return ints[key];
            return defaultValue;
        }

        public float TryGet(UDFlt key, float defaultValue = default)
        {
            if (flts.ContainsKey(key))
                return flts[key];
            return defaultValue;
        }

        public string TryGet(UDStr key, string defaultValue = "")
        {
            if (strs.ContainsKey(key))
                return strs[key];
            return defaultValue;
        }

        public object TryGet(UDRef key, object defaultValue = null)
        {
            if (refs.ContainsKey(key))
                return refs[key];
            return defaultValue;
        }

        #endregion TryGet

        public UData Merge(UData data, bool keepSelf = false)
        {
            if (data.stts != null)
            {
                foreach (var item in data.stts)
                {
                    if (this[item.Key].HasValue && keepSelf) continue;
                    this[item.Key] = item.Value;
                }
            }

            if (data.vc3s != null)
            {
                foreach (var item in data.vc3s)
                {
                    if (this[item.Key].HasValue && keepSelf) continue;
                    this[item.Key] = item.Value;
                }
            }

            if (data.v2is != null)
            {
                foreach (var item in data.v2is)
                {
                    if (this[item.Key].HasValue && keepSelf) continue;
                    this[item.Key] = item.Value;
                }
            }

            if (data.ints != null)
            {
                foreach (var item in data.ints)
                {
                    if (this[item.Key].HasValue && keepSelf) continue;
                    this[item.Key] = item.Value;
                }
            }

            if (data.flts != null)
            {
                foreach (var item in data.flts)
                {
                    if (this[item.Key].HasValue && keepSelf) continue;
                    this[item.Key] = item.Value;
                }
            }

            if (data.strs != null)
            {
                foreach (var item in data.strs)
                {
                    if (!string.IsNullOrWhiteSpace(this[item.Key]) && keepSelf) continue;
                    this[item.Key] = item.Value;
                }
            }

            if (data.refs != null)
            {
                foreach (var item in data.refs)
                {
                    if (this[item.Key].IsNotNull() && keepSelf) continue;
                    this[item.Key] = item.Value;
                }
            }

            return this;
        }

        public bool Contains(UDStt key)
        {
            return stts?.ContainsKey(key) == true;
        }

        public bool Contains(UDVc3 key)
        {
            return vc3s?.ContainsKey(key) == true;
        }

        public bool Contains(UDV2I key)
        {
            return v2is?.ContainsKey(key) == true;
        }

        public bool Contains(UDInt key)
        {
            return ints?.ContainsKey(key) == true;
        }

        public bool Contains(UDFlt key)
        {
            return flts?.ContainsKey(key) == true;
        }

        public bool Contains(UDStr key)
        {
            return strs?.ContainsKey(key) == true;
        }

        public bool Contains(UDRef key)
        {
            return refs.ContainsKey(key) == true;
        }

        //public UData Clone()
        //{
        //	if (this == null)
        //		return new UData();

        //	var clone = new UData();
        //	if (stts != null)
        //	{
        //		clone.stts = new Dictionary<UDStt, short>();
        //		clone.stts.Add(stts);
        //	}

        //	if (vc3s != null)
        //	{
        //		clone.vc3s = new Dictionary<UDVc3, Vector3>();
        //		clone.vc3s.Add(vc3s);
        //	}

        //	if (v2is != null)
        //	{
        //		clone.v2is = new Dictionary<UDV2I, Vector2Int>();
        //		clone.v2is.Add(v2is);
        //	}

        //	if (ints != null)
        //	{
        //		clone.ints = new Dictionary<UDInt, int>();
        //		clone.ints.Add(ints);
        //	}

        //	if (flts != null)
        //	{
        //		clone.flts = new Dictionary<UDFlt, float>();
        //		clone.flts.Add(flts);
        //	}

        //	if (strs != null)
        //	{
        //		clone.strs = new Dictionary<UDStr, string>();
        //		clone.strs.Add(strs);
        //	}

        //	if (refs != null)
        //	{
        //		clone.refs = new Dictionary<UDRef, object>();
        //		clone.refs.Add(refs);
        //	}

        //	return clone;
        //}

        public UData()
        {
        }

        public UData(UData data) : base()
        {
            if (data == null)
                return;

            if (data.stts != null)
            {
                stts = new Dictionary<UDStt, int>();
                stts.AddRange(data.stts);
            }

            if (data.vc3s != null)
            {
                vc3s = new Dictionary<UDVc3, Vector3>();
                vc3s.AddRange(data.vc3s);
            }

            if (data.v2is != null)
            {
                v2is = new Dictionary<UDV2I, Vector2Int>();
                v2is.AddRange(data.v2is);
            }

            if (data.ints != null)
            {
                ints = new Dictionary<UDInt, int>();
                ints.AddRange(data.ints);
            }

            if (data.flts != null)
            {
                flts = new Dictionary<UDFlt, float>();
                flts.AddRange(data.flts);
            }

            if (data.strs != null)
            {
                strs = new Dictionary<UDStr, string>();
                strs.AddRange(data.strs);
            }

            if (data.refs != null)
            {
                refs = new Dictionary<UDRef, object>();
                refs.AddRange(data.refs);
            }
        }
    }
}