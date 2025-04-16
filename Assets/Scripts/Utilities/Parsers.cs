using UnityEngine;

namespace JTUtility
{
    public static class Parsers
    {
        public static bool TryParseVector2(string str, out Vector2 result)
        {
            result = Vector2.zero;
            var components = str.Split(',');
            if (components.Length != 2) return false;

            float x, y;
            if (!float.TryParse(components[0], out x) ||
                !float.TryParse(components[1], out y))
            {
                return false;
            }

            result = new Vector2(x, y);
            return true;
        }

        public static bool TryParseVector3(string str, out Vector3 result)
        {
            result = Vector3.zero;
            var components = str.Split(',');
            if (components.Length != 3) return false;

            float x, y, z;
            if (!float.TryParse(components[0], out x) ||
                !float.TryParse(components[1], out y) ||
                !float.TryParse(components[2], out z))
            {
                return false;
            }

            result = new Vector3(x, y, z);
            return true;
        }

        public static bool TryParseVector4(string str, out Vector4 result)
        {
            result = Vector4.zero;
            var components = str.Split(',');
            if (components.Length != 4) return false;

            float x, y, z, w;
            if (!float.TryParse(components[0], out x) ||
                !float.TryParse(components[1], out y) ||
                !float.TryParse(components[2], out z) ||
                !float.TryParse(components[3], out w))
            {
                return false;
            }

            result = new Vector4(x, y, z, w);
            return true;
        }

        /// <summary>
        /// Parse a string into <see cref="Color"/>, supports three following formats:
        /// <para>"RGBA(1.0, 0.5, 0.6, 1.0)"</para>
        /// <para>"#ff7f85" or "#ff7f85ff"</para>
        /// <para>"255,127,135" or "255,127,135,255"</para>
        /// </summary>
        /// <param name="str"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool TryParseColor(string str, out Color result)
        {
            if (str.StartsWith("#"))
                return TryParseColorHex(str, out result);
            else if (str.StartsWith("RGBA"))
                return TryParseColorRGBA(str, out result);
            else
                return TryParseColorDec(str, out result);
        }

        /// <summary>
        ///  <para>Parse a string into <see cref="Color"/> which following the format below.</para>
        ///  <para>"RGBA([Red], [Green], [Blue], [Alpha])" with range (0.0, 1.0)</para>
        ///  <para>E.g. "RGBA(0.0, 0.216, 1.0, 0.5)"</para>
        /// </summary>
        /// <param name="str"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool TryParseColorRGBA(string str, out Color result)
        {
            result = Color.clear;
            if (!str.StartsWith("RGBA(") || !str.EndsWith(")"))
                return false;

            str = str.Replace("RGBA(", "");
            str = str.Replace(")", "");

            var segments = str.Split(',');
            if (segments.Length != 4) return false;

            float r, g, b, a;
            if (!float.TryParse(segments[0], out r) ||
                !float.TryParse(segments[1], out g) ||
                !float.TryParse(segments[2], out b) ||
                !float.TryParse(segments[3], out a))
            {
                return false;
            }

            r = Mathf.Clamp01(r);
            g = Mathf.Clamp01(g);
            b = Mathf.Clamp01(b);
            a = Mathf.Clamp01(a);

            result = new Color(r, g, b, a);

            return true;
        }

        /// <summary>
        ///  <para>Parse a string into <see cref="Color"/> which following the format below, [Alpha] is optional.</para>
        ///  <para>"#[0xRed][0xGreen][0xBlue][0xAlpha]" with range (00, ff)</para>
        ///  <para>E.g. "#ff00ff" => RGBA(1.0, 0.0, 1.0, 1.0); "#77ffff77" => RGBA(0.467, 1.0, 1.0, 0.467)</para>
        /// </summary>
        /// <param name="str"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool TryParseColorHex(string str, out Color result)
        {
            result = Color.clear;

            str.Trim();
            if (!str.StartsWith("#")) return false;
            if (str.Length != 7 && str.Length != 9) return false;
            string[] segments = new string[4];
            segments[0] = new string(str.ToCharArray(), 1, 2);
            segments[1] = new string(str.ToCharArray(), 3, 2);
            segments[2] = new string(str.ToCharArray(), 5, 2);
            segments[3] = str.Length == 9 ? new string(str.ToCharArray(), 7, 2) : "FF";

            float r, g, b, a;
            try
            {
                r = int.Parse(segments[0], System.Globalization.NumberStyles.HexNumber) / 255.0f;
                g = int.Parse(segments[1], System.Globalization.NumberStyles.HexNumber) / 255.0f;
                b = int.Parse(segments[2], System.Globalization.NumberStyles.HexNumber) / 255.0f;
                a = int.Parse(segments[3], System.Globalization.NumberStyles.HexNumber) / 255.0f;

                result = new Color(r, g, b, a);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// <para>Parse a string into <see cref="Color"/> which following the format below, [Alpha] is optional.</para>
        /// <para>"[Red], [Green], [Blue], [Alpha]" with range (0, 255)</para>
        /// <para>E.g. "255,255,255" => RGBA(1.0, 1.0, 1.0, 1.0); "255,50,255,50" => RGBA(1.0, 0.196, 1.0, 0.196)</para>
        /// </summary>
        /// <param name="str"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool TryParseColorDec(string str, out Color result)
        {
            result = Color.clear;
            var components = str.Split(',');
            if (components.Length != 4 && components.Length != 3) return false;

            uint r = 0;
            uint g = 0;
            uint b = 0;
            uint a = 255;

            if (components.Length == 4 &&
                !uint.TryParse(components[3], out a))
            {
                return false;
            }

            if (!uint.TryParse(components[0], out r) ||
                !uint.TryParse(components[1], out g) ||
                !uint.TryParse(components[2], out b))
            {
                return false;
            }

            result = new Color(
                Mathf.Clamp01(r / 255.0f),
                Mathf.Clamp01(g / 255.0f),
                Mathf.Clamp01(b / 255.0f),
                Mathf.Clamp01(a / 255.0f)
                );

            return true;
        }
    }
}