using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTUtility
{
    public static class ExtensionMethods
    {
        /// <summary>
        /// Checks whether a Unity object exists; i.e. it hasn't been destroyed
        /// </summary>
        /// <remarks>
        /// Unlike the normal null check, this will return <c>true</c> for objects
        /// that have been merely disabled.
        /// </remarks>
        /// <param name="obj"></param>
        /// <returns><c>true</c> if the object exists, otherwise <c>false</c></returns>
        public static bool IsExists(this object obj)
        {
            if (obj == null)
                return false;
            bool exists = false;
            var o = obj as UnityEngine.Object;
            if (((object)o) != null && o != null)
                try
                {
                    //o.hideFlags.Equals(null);
                    exists = true;
                }
                catch { }
            return exists;
        }

        // Force a Unity object collaps to null if it's "null"
        public static T NullCheck<T>(this T obj) where T : UnityEngine.Object
        {
            if (obj == null || obj.Equals(null))
                obj = null;

            return obj;
        }

        //public static bool IsTaggedDestroy(this Component obj)
        //{
        //	return obj.gameObject.tag == TagConstant.Destroying;
        //}

        //public static bool IsTaggedDestroy(this GameObject obj)
        //{
        //	return obj.tag == TagConstant.Destroying;
        //}

        public static bool IsNull(this object obj)
        {
            return obj == null || obj.Equals(null);
        }

        public static bool IsNotNull(this object obj)
        {
            return obj != null && !obj.Equals(null);
        }

        public static bool IsValidIndex<T>(this T[] array, int index)
        {
            return index >= 0 && index < array.Length;
        }

        public static bool IsValidIndex<T>(this ICollection<T> collection, int index)
        {
            return index >= 0 && index < collection.Count;
        }

        public static int ClampIndex<T>(this T[] array, int index)
        {
            return index < 0 ? 0 : index >= array.Length ? array.Length - 1 : index;
        }

        public static int ClampIndex<T>(this ICollection<T> collection, int index)
        {
            return index < 0 ? 0 : index >= collection.Count ? collection.Count - 1 : index;
        }

        public static bool IsNullOrEmpty(this IEnumerable enumerable)
        {
            return enumerable.IsNull() || enumerable.GetEnumerator().MoveNext() == false;
        }

        public static bool IsNullOrEmpty<T>(this IEnumerable<T> enumerable)
        {
            return enumerable.IsNull() || enumerable.GetEnumerator().MoveNext() == false;
        }

        public static bool IsNullOrEmpty<T>(this ICollection<T> collection)
        {
            return collection.IsNull() || collection.Count <= 0;
        }

        public static bool IsNullOrEmpty<T>(this T[] array)
        {
            return array.IsNull() || array.Length <= 0;
        }

        public static bool HasNullItem<T>(this IEnumerable<T> collection)
        {
            foreach (T item in collection)
                if (item.IsNull())
                    return true;
            return false;
        }

        /// <summary>
        /// Gets component from another component if it has one, and adds one if it doesn't
        /// </summary>
        /// <param name="obj"></param>
        /// <returns><c>true</c> if the object exists, otherwise <c>false</c></returns>
        public static T GetOrAddComponent<T>(this Component c) where T : Component
        {
            if (c == null) throw new ArgumentNullException("The operational component is null!");
            var part = c.GetComponent<T>();
            if (part == null)
                part = c.gameObject.AddComponent<T>();

            return part;
        }

        /// <summary>
        /// Returns the square of the given number
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static int Square(this int num)
        {
            return num * num;
        }

        /// <summary>
        /// Returns the square of the given number
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static float Square(this float num)
        {
            return num * num;
        }

        /// <summary>
        /// Clamps an angle between -180 and +180
        /// </summary>
        /// <param name="angle"></param>
        /// <returns></returns>
        public static float Clamp180(this float angle)
        {
            angle %= 360;
            if (angle > 180f)
                angle -= 360f;
            else if (angle < -180f)
                angle += 360f;
            return angle;
        }

        /// <summary>
        /// Clamps an euler rotation between -180 and +180
        /// </summary>
        /// <param name="angle"></param>
        /// <returns></returns>
        public static Vector3 Clamp180(this Vector3 angle)
        {
            return new Vector3(angle.x.Clamp180(),
                angle.y.Clamp180(),
                angle.z.Clamp180());
        }

        /// <summary>
        /// Clamps an euler rotation between -180 and +180
        /// </summary>
        /// <param name="angle"></param>
        /// <returns></returns>
        public static Vector2 Clamp180(this Vector2 angle)
        {
            return new Vector2(angle.x.Clamp180(),
                angle.y.Clamp180());
        }

        public static float RandomBetween(this Vector2 vec)
        {
            return UnityEngine.Random.Range(vec.x, vec.y);
        }

        public static int RandomBetween(this Vector2Int vec)
        {
            return UnityEngine.Random.Range(vec.x, vec.y);
        }

        public static int RandomBetweenIncluded(this Vector2Int vec)
        {
            return UnityEngine.Random.Range(vec.x, vec.y + 1);
        }

        public static T PickRandom<T>(this T[] array)
        {
            return array[UnityEngine.Random.Range(0, array.Length)];
        }

        public static T PickRandom<T>(this ICollection<T> collection)
        {
            if (collection.IsNullOrEmpty())
                return default;

            int random = UnityEngine.Random.Range(0, collection.Count);
            foreach (var item in collection)
            {
                if (random == 0)
                    return item;
                random--;
            }
            throw new Exception("This shouldn't happen");
        }

        public static void Randomize<T>(this T[] array)
        {
            for (int i = 0; i < array.Length * 2; i++)
            {
                int index1 = UnityEngine.Random.Range(0, array.Length);
                int index2 = UnityEngine.Random.Range(0, array.Length);
                if (index1 == index2)
                    continue;

                var temp = array[index1];
                array[index1] = array[index2];
                array[index2] = temp;
            }
        }

        public static void Randomize<T>(this List<T> list)
        {
            for (int i = 0; i < list.Count * 2; i++)
            {
                int index1 = UnityEngine.Random.Range(0, list.Count);
                int index2 = UnityEngine.Random.Range(0, list.Count);
                if (index1 == index2)
                    continue;

                var temp = list[index1];
                list[index1] = list[index2];
                list[index2] = temp;
            }
        }

        public static void PickRandomRange<T>(this T[] array, T[] output)
        {
            if (output.IsNullOrEmpty())
                return;

            for (int i = 0; i < output.Length; i++)
            {
                output[i] = array.PickRandom();
            }
        }

        public static float LerpBetween(this Vector2 vec, float t)
        {
            return Mathf.Lerp(vec.x, vec.y, t);
        }

        public static bool IsNaN(this Vector3 vec)
        {
            return float.IsNaN(vec.x) || float.IsNaN(vec.y) || float.IsNaN(vec.z);
        }

        public static bool IsInfinity(this Vector3 vec)
        {
            return float.IsInfinity(vec.x) || float.IsInfinity(vec.y) || float.IsInfinity(vec.z);
        }

        /// <summary>
        /// 0,1 => x,y
        /// </summary>
        /// <param name="vec"></param>
        /// <param name="index"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Vector2 AlterComponent(this Vector2 vec, int index, float value)
        {
            if (index == 0)
                vec.x = value;
            else if (index == 1)
                vec.y = value;
            return vec;
        }

        /// <summary>
        /// 0,1 => x,y
        /// </summary>
        /// <param name="vec"></param>
        /// <param name="index"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Vector2Int AlterComponent(this Vector2Int vec, int index, int value)
        {
            if (index == 0)
                vec.x = value;
            else if (index == 1)
                vec.y = value;
            return vec;
        }

        /// <summary>
        /// 0,1,2 => x,y,z
        /// </summary>
        /// <param name="vec"></param>
        /// <param name="index"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Vector3 AlterComponent(this Vector3 vec, int index, float value)
        {
            if (index == 0)
                vec.x = value;
            else if (index == 1)
                vec.y = value;
            else if (index == 2)
                vec.z = value;
            return vec;
        }

        public static Vector3 AlterX(this Vector3 vector, float x)
        {
            vector.x = x;
            return vector;
        }

        public static Vector3 AlterY(this Vector3 vector, float y)
        {
            vector.y = y;
            return vector;
        }

        public static Vector3 AlterZ(this Vector3 vector, float z)
        {
            vector.z = z;
            return vector;
        }

        /// <summary>
        /// 0,1,2 => x,y,z
        /// </summary>
        /// <param name="vec"></param>
        /// <param name="index"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Vector3Int AlterComponent(this Vector3Int vec, int index, int value)
        {
            if (index == 0)
                vec.x = value;
            else if (index == 1)
                vec.y = value;
            else if (index == 2)
                vec.z = value;
            return vec;
        }

        public static Vector3Int AlterZ(this Vector3Int vector, int z)
        {
            vector.z = z;
            return vector;
        }

        /// <summary>
        /// 0,1,2,3 => x,y,z,w
        /// </summary>
        /// <param name="vec"></param>
        /// <param name="index"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Vector4 AlterComponent(this Vector4 vec, int index, float value)
        {
            if (index == 0)
                vec.x = value;
            else if (index == 1)
                vec.y = value;
            else if (index == 2)
                vec.z = value;
            else if (index == 3)
                vec.w = value;
            return vec;
        }

        public static Vector4 AlterW(this Vector4 vector, float w)
        {
            vector.w = w;
            return vector;
        }

        /// <summary>
        /// 0,1,2,3 => r,g,b,a
        /// </summary>
        /// <param name="vec"></param>
        /// <param name="index"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Color AlterComponent(this Color col, int index, float value)
        {
            if (index == 0)
                col.r = value;
            else if (index == 1)
                col.g = value;
            else if (index == 2)
                col.b = value;
            else if (index == 3)
                col.a = value;
            return col;
        }

        public static void AlterAlpha(this Color color, Action<Color> setter, float alpha)
        {
            color.a = alpha;
            setter(color);
        }

        public static Color AlterAlpha(this Color color, float alpha)
        {
            color.a = alpha;
            return color;
        }

        public static Vector3 Divide(this int num, Vector3 vec2)
        {
            return new Vector3(num / vec2.x, num / vec2.y, num / vec2.z);
        }

        public static Vector3 Divide(this float num, Vector3 vec2)
        {
            return new Vector3(num / vec2.x, num / vec2.y, num / vec2.z);
        }

        public static Vector3 Divide(this Vector3 vec1, Vector3 vec2)
        {
            return new Vector3(vec1.x / vec2.x, vec1.y / vec2.y, vec1.z / vec2.z);
        }

        public static Vector3 Abs(this Vector3 vec)
        {
            vec.x = Mathf.Abs(vec.x);
            vec.y = Mathf.Abs(vec.y);
            vec.z = Mathf.Abs(vec.z);
            return vec;
        }

        public static Vector3Int Abs(this Vector3Int vec)
        {
            vec.x = Mathf.Abs(vec.x);
            vec.y = Mathf.Abs(vec.y);
            vec.z = Mathf.Abs(vec.z);
            return vec;
        }

        public static float[] ToArray(this Vector3 vec)
        {
            return new float[] { vec.x, vec.y, vec.z };
        }

        public static int[] ToArray(this Vector3Int vec)
        {
            return new int[] { vec.x, vec.y, vec.z };
        }

        public static Vector3Int ToVector3Int(this Vector3 vec)
        {
            return new Vector3Int((int)vec.x, (int)vec.y, (int)vec.z);
        }

        public static Vector3Int RoundToVector3Int(this Vector3 vec)
        {
            return new Vector3Int(
                Mathf.RoundToInt(vec.x),
                Mathf.RoundToInt(vec.y),
                Mathf.RoundToInt(vec.z));
        }

        public static Vector2Int ToVector2Int(this Vector2 vec)
        {
            return new Vector2Int((int)vec.x, (int)vec.y);
        }

        public static Vector2Int RoundToVector2Int(this Vector2 vec)
        {
            return new Vector2Int(
                Mathf.RoundToInt(vec.x),
                Mathf.RoundToInt(vec.y));
        }

        public static Vector3 SetMagnitude(this ref Vector3 vector, float newMag)
        {
            vector = vector.normalized * newMag;
            return vector;
        }

        public static Vector2 SetMagnitude(this ref Vector2 vector, float newMag)
        {
            vector = vector.normalized * newMag;
            return vector;
        }

        public static Vector3 Rotate(this Vector3 vector, Vector3 angles)
        {
            return Quaternion.Euler(angles) * vector;
        }

        public static Vector3 Rotate(this Vector3 vector, float x, float y, float z)
        {
            return Quaternion.Euler(x, y, z) * vector;
        }

        public static Vector2 Rotate(this Vector2 vector, float angles)
        {
            var cosA = Mathf.Cos(angles * Mathf.Deg2Rad);
            var sinA = Mathf.Sin(angles * Mathf.Deg2Rad);
            var rotated = vector;

            rotated.x = cosA * vector.x - sinA * vector.y;
            rotated.y = sinA * vector.x + cosA * vector.y;

            return rotated;
        }

        public static bool IsIncluded(this Vector2 bound, float value)
        {
            return value >= bound.x && value <= bound.y;
        }

        public static bool IsIncluded(this Vector2Int bound, float value)
        {
            return value >= bound.x && value <= bound.y;
        }

        public static bool IsWithInBound(this Bounds self, Bounds other)
        {
            return other.Contains(self.max) && other.Contains(self.min);
        }

        public static Bounds GetIntersection(this Bounds self, Bounds other)
        {
            var result = self;

            var min = new Vector3(
                other.min.x < self.min.x ? self.min.x : other.min.x,
                other.min.y < self.min.y ? self.min.y : other.min.y,
                other.min.z < self.min.z ? self.min.z : other.min.z
                );

            var max = new Vector3(
                other.max.x > self.max.x ? self.max.x : other.max.x,
                other.max.y > self.max.y ? self.max.y : other.max.y,
                other.max.z > self.max.z ? self.max.z : other.max.z
                );

            if (min.x > max.x)
                min.x = max.x = (min.x + max.x) * 0.5f;
            if (min.y > max.y)
                min.y = max.y = (min.y + max.y) * 0.5f;
            if (min.z > max.z)
                min.z = max.z = (min.z + max.z) * 0.5f;

            result.SetMinMax(min, max);
            return result;
        }

        /// <summary>
        /// Attempts to add a value to a dictionary, not replacing an existing value
        /// </summary>
        /// <returns><c>true</c>, if the value was not in this collection, <c>false</c> otherwise.</returns>
        /// <param name="dict">The dictionary</param>
        /// <param name="key">The key of the item to add</param>
        /// <param name="addValue">The value of the item to add</param>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        public static bool TryAdd<TKey, TValue>(this IDictionary<TKey, TValue> dict,
            TKey key, TValue addValue)
        {
            if (dict.ContainsKey(key))
            {
                return false;
            }
            else
            {
                dict.Add(key, addValue);
                return true;
            }
        }

        /// <summary>
        /// Attempts to set a value to a dictionary, add if key value pair not exist
        /// </summary>
        /// <returns><c>true</c>, if the value was not in this collection, <c>false</c> otherwise.</returns>
        /// <param name="dict">The dictionary</param>
        /// <param name="key">The key of the item to add</param>
        /// <param name="setValue">The value of the item to add</param>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        public static bool TrySet<TKey, TValue>(this IDictionary<TKey, TValue> dict,
            TKey key, TValue setValue)
        {
            if (dict.ContainsKey(key))
            {
                dict[key] = setValue;
                return false;
            }
            else
            {
                dict.Add(key, setValue);
                return true;
            }
        }

        public static bool TryRemove<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key)
        {
            if (dict.ContainsKey(key))
            {
                dict.Remove(key);
                return true;
            }

            return false;
        }

        public static void BinaryInsert<V>(this IList<V> list, V item, System.Comparison<V> comparer)
        {
            if (list.Count == 0)
            {
                list.Add(item);
                return;
            }

            if (list.Count == 1)
            {
                if (comparer(list[0], item) < 0)
                {
                    list.Add(item);
                }
                else
                {
                    list.Insert(0, item);
                }
                return;
            }

            int head = 0;
            int tail = list.Count - 1;
            int mid = tail / 2;
            int compVal = comparer(list[mid], item);

            while (tail != head)
            {
                if (compVal == 0)
                {
                    list.Insert(mid, item);
                    return;
                }

                if (compVal < 0)
                {
                    head = mid == head ? mid + 1 : mid;
                }
                else
                {
                    tail = mid;
                }
                mid = (tail - head) / 2 + head;
                compVal = comparer(list[mid], item);
            }

            if (compVal < 0)
            {
                list.Add(item);
            }
            else
            {
                list.Insert(tail, item);
            }
        }

        public static int RemoveNullKeys<TKey, TValue>(this IDictionary<TKey, TValue> dict) where TKey : class
        {
            if (dict == null) return -1;

            UnityEngine.Profiling.Profiler.BeginSample("ExtensionMethods.RemoveNullKeys");
            var enumerator = dict.GetEnumerator();
            bool hasNull = false;

            while (enumerator.MoveNext())
            {
                if (enumerator.Current.Key.IsNotNull()) continue;

                hasNull = true;
                break;
            }

            if (hasNull == false)
            {
                UnityEngine.Profiling.Profiler.EndSample();
                return 0;
            }

            enumerator.Reset();
            var backingPairs = new List<KeyValuePair<TKey, TValue>>();
            while (enumerator.MoveNext())
            {
                if (enumerator.Current.Key.IsNotNull())
                    backingPairs.Add(enumerator.Current);
            }

            var removed = dict.Count - backingPairs.Count;
            dict.Clear();
            for (int i = 0; i < backingPairs.Count; i++)
            {
                dict.Add(backingPairs[i]);
            }

            UnityEngine.Profiling.Profiler.EndSample();
            return removed;
        }

        public static void Add<K, V>(this IDictionary<K, V> dictionary, KeyValuePair<K, V> pair)
        {
            dictionary.Add(pair.Key, pair.Value);
        }

        public static void AddRange<K, V>(this IDictionary<K, V> dictionary, IEnumerable<KeyValuePair<K, V>> pairedValues)
        {
            foreach (var pair in pairedValues)
            {
                dictionary.Add(pair.Key, pair.Value);
            }
        }

        public static void Add<K, V>(this IDictionary<K, V> dictionary, PairedValue<K, V> pair)
        {
            dictionary.Add(pair.Key, pair.Value);
        }

        public static void AddRange<K, V>(this IDictionary<K, V> dictionary, IEnumerable<PairedValue<K, V>> pairedValues)
        {
            foreach (var pair in pairedValues)
            {
                dictionary.Add(pair.Key, pair.Value);
            }
        }

        /// <summary>
        /// {minute:D2}:{seconds:D2}:{milliseconds:D2}
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToMSMTime(this float value)
        {
            int minute = Mathf.FloorToInt(value / 60);
            int seconds = Mathf.FloorToInt(value % 60);
            int milliseconds = Mathf.FloorToInt((value % 1) * 100);

            return $"{minute:D2}:{seconds:D2}:{milliseconds:D2}";
        }

        public static void ChangeClipSeamlessly(this AudioSource source, AudioClip clip, double offset)
        {
            if (source == null || clip == null)
                return;

            if (!source.isPlaying)
            {
                source.clip = clip;
                return;
            }

            var currentClipTime = (double)source.timeSamples / source.clip.frequency;
            var targetTimeSamples = (int)((currentClipTime + offset) * clip.frequency);

            source.clip = clip;
            source.timeSamples = targetTimeSamples;
        }

        private static Vector3[] corners = new Vector3[4];

        public static Rect WorldRect(this RectTransform rt)
        {
            //  Get World corners, take top left
            rt.GetWorldCorners(corners);
            Vector3 bottomLeft = corners[0];
            Vector3 topRight = corners[2];
            // Rect Size ... I'm not sure if this is working correctly?
            Vector2 size = new Vector2(topRight.x - bottomLeft.x, topRight.y - bottomLeft.y);
            return new Rect(bottomLeft, size);
        }
    }
}