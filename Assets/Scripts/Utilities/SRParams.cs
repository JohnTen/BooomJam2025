using System.Collections.Generic;
using UnityEngine;

namespace JTUtility
{
    /// <summary>
    /// String Referenced Parameters -- included value types to avoid (un)boxing
    /// </summary>
    public class SRParams
    {
        public Dictionary<string, int> ints = new Dictionary<string, int>();
        public Dictionary<string, float> floats = new Dictionary<string, float>();
        public Dictionary<string, string> strs = new Dictionary<string, string>();
        public Dictionary<string, Vector3> vectors = new Dictionary<string, Vector3>();
        public Dictionary<string, object> refs = new Dictionary<string, object>();
    }
}