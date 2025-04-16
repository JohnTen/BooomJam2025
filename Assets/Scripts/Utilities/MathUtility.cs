using System;
using UnityEngine;

namespace JTUtility
{
    public class MathUtility
    {
        public static float VerticalFov2HorizontalFov(float verticalFov)
        {
            float horizontalFov = 0;

            var temp = Mathf.Tan(verticalFov * Mathf.Deg2Rad / 2) * Screen.width * 1.0f / Screen.height;

            horizontalFov = Mathf.Atan(temp) * Mathf.Rad2Deg * 2;

            return horizontalFov;
        }

        //(0, 360]
        public static float makeAngleInRange2(float angle)
        {
            while (angle > 360)
            {
                angle -= 360;
            }
            while (angle <= 0)
            {
                angle += 360;
            }
            return angle;
        }

        public static Vector3 RotateAroundPoint(Vector3 self, Vector3 target, float a)
        {
            float rx0 = target.x;
            float ry0 = target.y;
            float x = self.x;
            float y = self.y;

            float x0 = (x - rx0) * Mathf.Cos(Mathf.Deg2Rad * a) - (y - ry0) * Mathf.Sin(Mathf.Deg2Rad * a) + rx0;
            float y0 = (x - rx0) * Mathf.Sin(Mathf.Deg2Rad * a) + (y - ry0) * Mathf.Cos(Mathf.Deg2Rad * a) + ry0;

            return new Vector3(x0, y0, 0);
        }

        public static float GetDesireAngleWithHeightAndDistance(float curRange, float speed, float H)
        {
            float riseTime = curRange / (2 * speed);
            float g = 2 * H / (riseTime * riseTime);                                                     //g = H /t
            float verticalSpeed = g * riseTime;
            float angle = (float)(180 / Math.PI * Mathf.Atan(verticalSpeed / speed));                     //得到角度
            return angle;
        }

        public static float CalculateAngleBySpeed(float speed, float range, float altitude, float gravityMultiplier = 1f)
        {
            float gravity = Physics.gravity.magnitude * gravityMultiplier;
            float _2PowerOfRange = Mathf.Pow(range, 2);
            float _2PowerOfSpeed = Mathf.Pow(speed, 2);
            float _4PowerOfSpeed = Mathf.Pow(_2PowerOfSpeed, 2);

            float equation = gravity * _2PowerOfRange + 2 * altitude * _2PowerOfSpeed;
            equation = _4PowerOfSpeed - gravity * equation;

            if (equation < 0)
            {
                return Mathf.Infinity;
            }
            equation = Mathf.Sqrt(equation);

            // Take the shorter one(flatter trajectory)
            var angle = Mathf.Atan((_2PowerOfSpeed - equation) / (gravity * range));

            return angle * Mathf.Rad2Deg;
        }

        public static void CalculateSpeedAndAngleByHeight(float height, float range, float altitude, out float angle, out float speed, float gravityMultiplier = 1f)
        {
            float gravity = Physics.gravity.magnitude * gravityMultiplier;
            float t1 = Mathf.Sqrt(2 * height / gravity);
            float t2 = Mathf.Sqrt(2 * (height - altitude) / gravity);
            float vx = range / (t1 + t2);
            float vy = gravity * t1;
            Vector2 launch = new Vector2(vx, vy);
            angle = Vector2.Angle(Vector2.right, launch);
            //if (Vector2.Angle(Vector2.down, launch) < 90) angle = -angle;
            speed = launch.magnitude;
        }

        public static void CalculateSpeedAndAngleByTime(float time, float range, float altitude, out float speed, out float angle, float gravityMultiplier = 1f)
        {
            float gravity = Physics.gravity.magnitude * gravityMultiplier;
            float vx = range / time;
            float vy = (altitude + 0.5f * gravity * time * time) / time;
            Vector2 velocity = new Vector2(vx, vy);
            angle = Vector2.Angle(Vector2.right, velocity);
            if (Vector2.Angle(Vector2.down, velocity) < 90) angle = -angle;
            speed = velocity.magnitude;
        }

        public static Vector3 FirstOrderIntercept(
            Vector3 shooterPosition,
            Vector3 shooterVelocity,
            float shotSpeed,
            Vector3 targetPosition,
            Vector3 targetVelocity)
        {
            Vector3 targetRelativePosition = targetPosition - shooterPosition;
            Vector3 targetRelativeVelocity = targetVelocity - shooterVelocity;
            float t = FirstOrderInterceptTime
            (
                shotSpeed,
                targetRelativePosition,
                targetRelativeVelocity
            );
            return targetPosition + t * (targetRelativeVelocity);
        }

        //first-order intercept using relative target position
        public static float FirstOrderInterceptTime
        (
            float shotSpeed,
            Vector3 targetRelativePosition,
            Vector3 targetRelativeVelocity
        )
        {
            float velocitySquared = targetRelativeVelocity.sqrMagnitude;
            if (velocitySquared < 0.001f)
                return 0f;

            float a = velocitySquared - shotSpeed * shotSpeed;

            //handle similar velocities
            if (Mathf.Abs(a) < 0.001f)
            {
                float t = -targetRelativePosition.sqrMagnitude /
                (
                    2f * Vector3.Dot
                    (
                        targetRelativeVelocity,
                        targetRelativePosition
                    )
                );
                return Mathf.Max(t, 0f); //don't shoot back in time
            }

            float b = 2f * Vector3.Dot(targetRelativeVelocity, targetRelativePosition);
            float c = targetRelativePosition.sqrMagnitude;
            float determinant = b * b - 4f * a * c;

            if (determinant > 0f)
            { //determinant > 0; two intercept paths (most common)
                float t1 = (-b + Mathf.Sqrt(determinant)) / (2f * a),
                        t2 = (-b - Mathf.Sqrt(determinant)) / (2f * a);
                if (t1 > 0f)
                {
                    if (t2 > 0f)
                        return Mathf.Min(t1, t2); //both are positive
                    else
                        return t1; //only t1 is positive
                }
                else
                    return Mathf.Max(t2, 0f); //don't shoot back in time
            }
            else if (determinant < 0f) //determinant < 0; no intercept path
                return 0f;
            else //determinant = 0; one intercept path, pretty much never happens
                return Mathf.Max(-b / (2f * a), 0f); //don't shoot back in time
        }

        public static float GetDesireGravity(float range, float speed, float H)
        {
            float riseTime = range / (2 * speed);
            float g = 2 * H / (riseTime * riseTime);

            return g;
        }

        // make angles in the vectors in the range (-180, 180]
        public static float makeAngleInRange(float angle)
        {
            while (angle > 180)
            {
                angle -= 360;
            }
            while (angle <= -180)
            {
                angle += 360;
            }
            return angle;
        }

        public static double makeAngleInRange(double angle)
        {
            while (angle > 180)
            {
                angle -= 360;
            }
            while (angle <= -180)
            {
                angle += 360;
            }
            return angle;
        }

        public static float getSignAngle(Vector3 dir)
        {
            return Vector3.Angle(Vector3.up, dir) * Mathf.Sign(Vector3.Cross(Vector3.up, dir).z);
        }

        public static float getSignAngle(Vector3 a, Vector3 b)
        {
            return Vector3.Angle(a, b) * Mathf.Sign(Vector3.Dot(Vector3.Cross(a, b), Vector3.up));
        }

        public static float getSignAngle(Vector3 a, Vector3 b, Vector3 up)
        {
            return Vector3.Angle(a, b) * Mathf.Sign(Vector3.Dot(Vector3.Cross(a, b), up));
        }

        public static bool EqualFloat(float a, float b)
        {
            if (Mathf.Abs(a - b) < 0.01f)
            {
                return true;
            }

            return false;
        }

        public static bool EqualVector2(Vector2 a, Vector2 b)
        {
            if (EqualFloat(a.x, b.x) && EqualFloat(a.y, b.y))
                return true;
            return false;
        }

        public static bool GreaterFloat(float a, float b)
        {
            if (a - b > 0.01f)
            {
                return true;
            }

            return false;
        }

        public static bool EqualVector3(Vector3 a, Vector3 b)
        {
            if (Mathf.Abs(a.x - b.x) < 0.01f && Mathf.Abs(a.y - b.y) < 0.01f && Mathf.Abs(a.z - b.z) < 0.01f)
            {
                return true;
            }

            return false;
        }

        public static Vector2 Project(Vector2 src, Vector2 dst)
        {
            var temp = dst.normalized;

            return Vector2.Dot(temp, src) * dst.normalized;
        }

        public static Vector3 Project(Vector3 src, Vector3 dst)
        {
            var temp = dst.normalized;
            return Vector3.Dot(temp, src) * temp;
        }

        public static bool isInSight(Vector2 a_egde, Vector2 b_edge, Vector2 dir)
        {
            float angle = Vector2.Angle(a_egde, b_edge);

            float partA = Vector2.Angle(a_egde, dir);
            float partB = Vector2.Angle(b_edge, dir);

            if (Mathf.Abs(partA + partB - angle) < 1)
            {
                return true;
            }

            return false;
        }

        public static float getDistanceToLine(Vector3 lineStartPosition, Vector3 lineDirection, Vector3 targetPoint)
        {
            var vec = targetPoint - lineStartPosition;
            var vecMagnitute = vec.magnitude;
            var dotValue = Mathf.Abs(Vector3.Dot(vec, lineDirection));

            var cosTheta = dotValue / vecMagnitute;
            var sinTheta = Mathf.Sqrt(1 - cosTheta * cosTheta);
            var distance2Line = vecMagnitute * sinTheta;
            return distance2Line;
        }

        public static Vector2 getRandomCircle(Vector2 originCircleCenter, float originRadius, float newOriginRadius)
        {
            if (newOriginRadius > originRadius)
                return Vector2.zero;

            Vector2 result = Vector2.zero;
            float r = originRadius - newOriginRadius;
            float angle = UnityEngine.Random.Range(0, 360) * Mathf.Deg2Rad;
            float x = r * Mathf.Cos(angle);
            float y = r * Mathf.Sin(angle);
            result.x = x + originCircleCenter.x;
            result.y = y + originCircleCenter.y;

            return result;
        }

        //public static Vector2 getRandomCircle2(Vector2 originCircleCenter, float originRadius, float percent,int seed)
        //{
        //    if (percent > 1)
        //        return Vector2.zero;

        //    Vector2 result = Vector2.zero;
        //    float r = originRadius - originRadius*percent;

        //    float angle = UniformRandom.randomInt(seed, 0, 360) * Mathf.Deg2Rad;
        //    float x = r * Mathf.Cos(angle);
        //    float y = r * Mathf.Sin(angle);
        //    result.x = x+ originCircleCenter.x;
        //    result.y = y+ originCircleCenter.y;

        //    return result;
        //}

        public static float Remap(float t1, float t2, float value, float T1, float T2)
        {
            float m = (value - t1) / (t2 - t1);

            return m * (T2 - T1) + T1;
        }

        public static float RemapWithClamp(float t1, float t2, float value, float T1, float T2)
        {
            float m = Mathf.Clamp01((value - t1) / (t2 - t1));

            return m * (T2 - T1) + T1;
        }

        public static Vector2 Remap(Vector3 t1, Vector3 t2, Vector3 value, Vector2 T1, Vector2 T2)
        {
            float mX = (value.x - t1.x) / (t2.x - t1.x);
            float nX = mX * (T2.x - T1.x) + T1.x;

            float mZ = (value.z - t1.z) / (t2.z - t1.z);
            float nZ = mZ * (T2.y - T1.y) + T1.y;

            return new Vector2(nX, nZ);
        }

        /// <summary>
        /// https://www.pianshen.com/article/4827574623/
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public static bool IsPointInPolygon(Vector2 pos, Vector2[] polygon)
        {
            //统计目标点向右画射线与多边形的相交次数
            int crossNum = 0;

            for (int i = 0; i < polygon.Length; i++)
            {
                ///两个顶点的连线是多边形的一条边
                Vector2 v1 = polygon[i];

                Vector2 v2 = polygon[(i + 1) % polygon.Length];

                ///先从y轴开始判断

                ///如果线是水平的,目标点低于这个线段，目标点高于这个线段，continue
                if (Math.Abs(v1.y - v2.y) < 0.01f || pos.y < Mathf.Min(v1.y, v2.y) || pos.y > Mathf.Max(v1.y, v2.y))
                {
                    continue;
                }

                ///现在的情况是过p1画水平线，过p2画水平线，目标点在这两条线中间

                ///这段确实看不懂...：
                ///过目标点，画一条水平线，x是这条线与多边形当前边的交点x坐标
                float x = (pos.y - v1.y) * (v2.x - v1.x) / (v2.y - v1.y) + v1.x;

                ///如果交点在右边，统计加一。这等于从目标点向右发一条射线（ray），与多边形各边的相交（crossing）次数
                if (x > pos.x)
                {
                    crossNum++;
                }
            }

            //如果是奇数，说明在多边形里(有一个只进不出或者只出不进)
            return crossNum % 2 == 1;
        }
    }
}