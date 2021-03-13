using System;
using UnityEngine;

namespace Laz
{
    public static class GeometryUtilities
    {
        private static readonly float ArbitraryLargeFloat = 999999f;

        /*
         * https://www.geeksforgeeks.org/how-to-check-if-a-given-point-lies-inside-a-polygon/
         */
        public static bool IsIntersecting(Vector3 p1, Vector3 q1, Vector3 p2, Vector3 q2)
        {

            var o1 = Orientation(p1, q1, p2);
            var o2 = Orientation(p1, q1, q2);
            var o3 = Orientation(p2, q2, p1);
            var o4 = Orientation(p2, q2, q1);

            if (o1 != o2 && o3 != o4)
            {
                return true;
            }

            if (o1 == 0 && OnSegment(p1, p2, q1))
            {
                return true;
            }

            if (o2 == 0 && OnSegment(p1, q2, q1))
            {
                return true;
            }

            if (o3 == 0 && OnSegment(p2, p1, q2))
            {
                return true;
            }

            if (o4 == 0 && OnSegment(p2, p1, q2))
            {
                return true;
            }

            return false;

        }

        public static bool IsInside(Vector3[] points, Vector3 point)
        {
            var length = points.Length;
            if (length < 3)
            {
                return false;
            }

            // For some reason - float.infinite does not work, probably calculations issues
            // If this some how bugs - you just need to find some arbitrary large random point
            Vector3 extreme = new Vector3(999999f, 0, point.z);
            int count = 0;
            int i = 0;
            do
            {
                int next = (i + 1) % length;
                if (IsIntersecting(points[i], points[next], point, extreme))
                {
                    if (Orientation(points[i], point, points[next]) == 0)
                    {
                        return OnSegment(points[i], point, points[next]);
                    }

                    count++;
                }

                i = next;
            } while (i != 0);

            return (count % 2 == 1);
        }

        private static bool OnSegment(Vector3 p, Vector3 q, Vector3 r)
        {
            return (q.x <= Math.Max(p.x, r.x) && q.x >= Math.Min(p.x, r.x) &&
                    q.z <= Math.Max(p.z, r.z) && q.z >= Math.Min(p.z, r.z));
        }

        private static int Orientation(Vector3 p, Vector3 q, Vector3 r)
        {
            var val = (q.z - p.z) * (r.x - q.x) -
                      (q.x - p.x) * (r.z - q.z);

            return (val > 0) ? 1 : 2;
        }
    }
}

