using JTUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace GameData
{
    public abstract class NewDatabase<DB, T, DT> : ScriptableObject where DB : NewDatabase<DB, T, DT> where DT : class where T : class, ICloneable
    {
        [Serializable] protected class ItemPair : PairedValue<string, DT> { }

        [SerializeField] protected string resourcesPath = "Prefabs/";

        [SerializeField] protected ScriptableObject dataTable;

        protected static DB _instance;

        protected Dictionary<string, T> itemDict;

        public Dictionary<string, T> ItemDict => itemDict;

        public static DB Instance
        {
            get
            {
                if (_instance != null)
                    return _instance;

#if UNITY_EDITOR
                var typeName = typeof(DB).Name;
                _instance = Resources.Load($"Database/DB_{typeName}") as DB;
                if (_instance != null)
                {
                    _instance.InitDatabase();
                    return _instance;
                }

                var path = $"Assets/Resources/Database/DB_{typeName}.prefab";
                _instance = CreateInstance(typeName) as DB;
                AssetDatabase.CreateAsset(_instance, path);
#else
            var typeName = typeof(DB).Name;
            _instance = Resources.Load($"Database/DB_{typeName}") as DB;
#endif
                if (_instance == null)
                    Debug.LogError($"Cannot find {typeName} Database!");

                return _instance;
            }
        }
        

        public T GetDefaultItem()
        {
            if (itemDict.Count > 0)
                return (T)itemDict.Values.First().Clone();

            return null;
        }

        public bool ContainsID(string id)
        {
            return itemDict.ContainsKey(id);
        }

        public virtual T GetItem(string id)
        {
            if (!itemDict.ContainsKey(id))
            {
                Debug.LogWarning("No item with id " + id);
                return null;
            }

            return (T)itemDict[id].Clone();
        }

        public virtual bool TryGetItem(string id, out T item)
        {
            if (!itemDict.ContainsKey(id))
            {
                item = null;
                return false;
            }

            item = (T)itemDict[id].Clone();
            return true;
        }

        private void OnEnable()
        {
            itemDict = null;
            InitDatabase();
        }

        public void ParseIntoUParam(UData param, string item)
        {
            var index = item.IndexOf(',');
            var key = item.Remove(index);
            var value = item.Substring(index + 1);

            if (key.ToLower() == "aggressive")
            {
                try
                {
                    param[UDStt.Aggressive] = int.Parse(value);
                }
                catch { }
                return;
            }
            else if (key.ToLower() == "timeframe")
            {
                try
                {
                    param[UDStt.Timeframe] = int.Parse(value);
                }
                catch { }
                return;
            }
            else if (key.ToLower() == "keepscore")
            {
                try
                {
                    param[UDStt.KeepScore] = int.Parse(value);
                }
                catch { }
                return;
            }
            else if (key.ToLower() == "countasstack")
            {
                try
                {
                    param[UDStt.CountAsStack] = int.Parse(value);
                }
                catch { }
                return;
            }
            else if (key.ToLower() == "usecasterside")
            {
                try
                {
                    param[UDStt.UseCasterSide] = int.Parse(value);
                }
                catch { }
                return;
            }
            else if (key.ToLower() == "useturnside")
            {
                try
                {
                    param[UDStt.UseTurnSide] = int.Parse(value);
                }
                catch { }
                return;
            }
            else if (key.ToLower() == "consistent")
            {
                try
                {
                    param[UDStt.Consistent] = int.Parse(value);
                }
                catch { }
                return;
            }
            else if (key.ToLower() == "vfxblockcontrol")
            {
                try
                {
                    param[UDStt.VFXBlockControl] = int.Parse(value);
                }
                catch { }
                return;
            }
            else if (key.ToLower() == "includeflippedbrick")
            {
                try
                {
                    param[UDStt.IncludeFlippedBrick] = int.Parse(value);
                }
                catch { }
                return;
            }
            else if (key.ToLower() == "bufficonsetting")
            {
                try
                {
                    param[UDStt.BuffIconSetting] = int.Parse(value);
                }
                catch { }
                return;
            }
            else if (key.ToLower() == "forceremove")
            {
                try
                {
                    param[UDStt.ForceRemove] = int.Parse(value);
                }
                catch { }
                return;
            }
            else if (key.ToLower() == "useaoeshape")
            {
                try
                {
                    param[UDStt.UseAoeShape] = int.Parse(value);
                }
                catch { }
                return;
            }
            else if (key.ToLower() == "flipstate")
            {
                try
                {
                    param[UDStt.FlipState] = int.Parse(value);
                }
                catch { }
                return;
            }
            else if (key.ToLower() == "triggeronce")
            {
                try
                {
                    param[UDStt.TriggerOnce] = int.Parse(value);
                }
                catch { }
                return;
            }
            else if (key.ToLower() == "noautovfx")
            {
                try
                {
                    param[UDStt.NoAutoVFX] = int.Parse(value);
                }
                catch { }
                return;
            }
            else if (key.ToLower() == "side")
            {
                try
                {
                    param[UDInt.Side] = int.Parse(value);
                }
                catch { }
                return;
            }
            else if (key.ToLower() == "count")
            {
                try
                {
                    param[UDInt.Count] = int.Parse(value);
                }
                catch { }
                return;
            }
            else if (key.ToLower() == "score")
            {
                try
                {
                    param[UDInt.Score] = int.Parse(value);
                }
                catch { }
                return;
            }
            else if (key.ToLower() == "stack")
            {
                try
                {
                    param[UDInt.Stack] = int.Parse(value);
                }
                catch { }
                return;
            }
            else if (key.ToLower() == "turns")
            {
                try
                {
                    param[UDInt.Turns] = int.Parse(value);
                }
                catch { }
                return;
            }
            else if (key.ToLower() == "cost")
            {
                try
                {
                    param[UDInt.Cost] = int.Parse(value);
                }
                catch { }
                return;
            }
            else if (key.ToLower() == "bufficonside")
            {
                try
                {
                    param[UDInt.BuffIconSide] = int.Parse(value);
                }
                catch { }
                return;
            }
            else if (key.ToLower() == "overloadedaltaroccupied")
            {
                try
                {
                    param[UDInt.OverloadedAltarOccupied] = int.Parse(value);
                }
                catch { }
                return;
            }
            else if (key.ToLower() == "overloadedaltarunoccupied")
            {
                try
                {
                    param[UDInt.OverloadedAltarUnoccupied] = int.Parse(value);
                }
                catch { }
                return;
            }
            else if (key.ToLower() == "aiscoreaffector_add")
            {
                try
                {
                    param[UDInt.AIScoreAffector_Add] = int.Parse(value);
                }
                catch { }
                return;
            }
            else if (key.ToLower() == "aiscoreaffector_mul")
            {
                try
                {
                    param[UDInt.AIScoreAffector_Mul] = int.Parse(value);
                }
                catch { }
                return;
            }
            else if (key.ToLower() == "stackmultiplier")
            {
                try
                {
                    param[UDFlt.StackMultiplier] = float.Parse(value);
                }
                catch { }
                return;
            }
            else if (key.ToLower() == "brickid")
            {
                param[UDStr.BrickID] = value;
                return;
            }
            else if (key.ToLower() == "cardid")
            {
                param[UDStr.CardID] = value;
                return;
            }
            else if (key.ToLower() == "unitid")
            {
                param[UDStr.UnitID] = value;
                return;
            }
            else if (key.ToLower() == "buffid")
            {
                param[UDStr.BuffID] = value;
                return;
            }
            else if (key.ToLower() == "aoeid")
            {
                param[UDStr.AoeID] = value;
                return;
            }
            else if (key.ToLower() == "gridid")
            {
                param[UDStr.GridID] = value;
                return;
            }
            else if (key.ToLower() == "vfxid")
            {
                param[UDStr.VFXID] = value;
                return;
            }
            else if (key.ToLower() == "newbuffid")
            {
                param[UDStr.NewBuffID] = value;
                return;
            }
            else if (key.ToLower() == "multicardid")
            {
                param[UDStr.MultiCardID] = value;
                return;
            }
            else if (key.ToLower() == "multigridid")
            {
                param[UDStr.MultiGridID] = value;
                return;
            }
            else if (key.ToLower() == "multibuffid")
            {
                param[UDStr.MultiBuffID] = value;
                return;
            }
            else if (key.ToLower() == "multibrickid")
            {
                param[UDStr.MultiBrickID] = value;
                return;
            }
            else if (key.ToLower() == "keywords")
            {
                param[UDStr.Keywords] = value;
                return;
            }
            else if (key.ToLower() == "targettag")
            {
                param[UDStr.TargetTag] = value;
                return;
            }
            else if (key.ToLower() == "bricktags")
            {
                param[UDStr.BrickTags] = value;
                return;
            }
            else if (key.ToLower() == "targetbuffid")
            {
                param[UDStr.TargetBuffID] = value;
                return;
            }
            else if (key.ToLower() == "bufficon")
            {
                param[UDStr.BuffIcon] = value;
                return;
            }
            else if (key.ToLower() == "area" || key.ToLower() == "positions")
            {
                var grids = value.Split("]");
                var poses = new List<Vector2Int>();
                for (int i = 0; i < grids.Length; i++)
                {
                    if (string.IsNullOrEmpty(grids[i]))
                        continue;
                    var axis = grids[i].Substring(1).Split(",");
                    try
                    {
                        poses.Add(new Vector2Int(int.Parse(axis[0]), int.Parse(axis[1])));
                    }
                    catch
                    {
                        Debug.LogWarning("Failed to parse " + grids[i].Substring(1));
                    }
                }
                param[UDRef.V2iPositions] = poses;
                return;
            }
            else if (key.ToLower() == "placements")
            {
                var grids = value.Split("]");
                var poses = new List<Vector3Int>();
                for (int i = 0; i < grids.Length; i++)
                {
                    if (string.IsNullOrEmpty(grids[i]))
                        continue;
                    var axis = grids[i].Substring(1).Split(",");
                    try
                    {
                        poses.Add(new Vector3Int(int.Parse(axis[0]), int.Parse(axis[1]), int.Parse(axis[2])));
                    }
                    catch
                    {
                        Debug.LogWarning("Failed to parse " + grids[i].Substring(1));
                    }
                }
                param[UDRef.V3iPlacements] = poses;
                return;
            }

            else if (key.ToLower() == "maparea")
            {
                var grids = value.Split("]");
                var poses = new List<Vector2Int>();
                for (int i = 0; i < grids.Length; i++)
                {
                    if (string.IsNullOrEmpty(grids[i]))
                        continue;
                    var axis = grids[i].Substring(1).Split(",");
                    try
                    {
                        poses.Add(new Vector2Int(int.Parse(axis[0]), int.Parse(axis[1])));
                    }
                    catch
                    {
                        Debug.LogWarning("Failed to parse " + grids[i].Substring(1));
                    }
                }
                param[UDRef.MapV2iPositions] = poses;
                return;
            }
            else if (key.ToLower() == "mapplacements")
            {
                var grids = value.Split("]");
                var poses = new List<Vector3Int>();
                for (int i = 0; i < grids.Length; i++)
                {
                    if (string.IsNullOrEmpty(grids[i]))
                        continue;
                    var axis = grids[i].Substring(1).Split(",");
                    try
                    {
                        poses.Add(new Vector3Int(int.Parse(axis[0]), int.Parse(axis[1]), int.Parse(axis[2])));
                    }
                    catch
                    {
                        Debug.LogWarning("Failed to parse " + grids[i].Substring(1));
                    }
                }
                param[UDRef.MapV3iPlacements] = poses;
                return;
            }
            else if (key.ToLower() == "activeturns")
            {
                var activeTurns = new List<int>();
                var strValue = value.Split(",");
                for (int i = 0; i < strValue.Length; i++)
                {
                    if (string.IsNullOrEmpty(strValue[i]))
                    {
                        continue;
                    }

                    try
                    {
                        activeTurns.Add(int.Parse(strValue[i]));
                    }
                    catch
                    {
                        Debug.LogWarning("Failed to parse " + strValue[i]);
                    }
                }

                param[UDRef.ActiveTurns] = activeTurns;
            }
            else if (key.ToLower() == "randomstacks")
            {
                var stacks = new List<int>();
                var strValue = value.Split(",");
                for (int i = 0; i < strValue.Length; i++)
                {
                    if (string.IsNullOrEmpty(strValue[i]))
                    {
                        continue;
                    }

                    try
                    {
                        stacks.Add(int.Parse(strValue[i]));
                    }
                    catch
                    {
                        Debug.LogWarning("Failed to parse " + strValue[i]);
                    }
                }

                param[UDRef.RandomStacks] = stacks;
            }
        }

        public abstract void InitDatabase();

        public abstract T ToItem(DT data);
    }
}