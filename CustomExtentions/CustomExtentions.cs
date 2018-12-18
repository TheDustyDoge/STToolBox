using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class CustomExtentions
{
    // ========= \\
    // = float = \\
    // ========= \\

    public static float Clamp(this float number, float min, float max)
    {
        if (number < min)
            number = min;
        if (max < number)
            number = max;
        return number;
    }

    public static float CircularClamp(this float number, float min, float max)
    {
        float range = max - min;
        if (number < min)
            return CircularClamp(number + range, min, max);
        if (number >= max)
            return CircularClamp(number - range, min, max);
        return number;
    }

    public static float CircularClamp360(this float number)
    {
        return CircularClamp(number, 0, 360);
    }

    public static bool NumberIsInBounds(this float number, float min, float max)
    {
        return number.DetailedNumberIsInBounds(min, max) == 0;
    }

    public static int DetailedNumberIsInBounds(this float number, float min, float max)
    {
        if (number < min)
            return -1;
        if (max < number)
            return 1;
        return 0;
    }

    // =========== \\
    // = Vector2 = \\
    // =========== \\

    public static Vector2 RotateAroundPoint(this Vector2 movePoint, Vector2 axisPoint, float degrees)
    {
        Vector2 p = new Vector2(movePoint.x, movePoint.y);

        float s = Mathf.Sin(degrees * Mathf.Deg2Rad);
        float c = Mathf.Cos(degrees * Mathf.Deg2Rad);

        p.x -= axisPoint.x;
        p.y -= axisPoint.y;

        float xnew = p.x * c - p.y * s;
        float ynew = p.x * s + p.y * c;

        p.x = xnew + axisPoint.x;
        p.y = ynew + axisPoint.y;

        p = p.RoundValues(7); // Accuracy of a float
        return p;
    }

    public static Vector2 RoundValues(this Vector2 v, int decimalPoints)
    {
        v.x = v.x - (v.x % Mathf.Pow(10, -1 * decimalPoints));
        v.y = v.y - (v.y % Mathf.Pow(10, -1 * decimalPoints));
        return v;
    }

    public static int Compare(this Vector2 x, Vector2 y)
    {
        int ret = x.x.CompareTo(y.x);
        if (ret != 0)
            return ret;
        return x.y.CompareTo(y.y);
    }

    public static float AngleToPoint(this Vector2 x, Vector2 y)
    {
        return Mathf.Rad2Deg * Mathf.Atan2((x.y - y.y), (x.x - y.x));
    }

    public static float DistanceTo(this Vector2 x, Vector2 y)
    {
        return Vector2.Distance(x, y);
    }

    public static bool PointIsInBounds(this Vector2 point, Vector2 min, Vector2 max)
    {
        return point.DetailedPointIsInBounds(min, max) == Vector2.zero;
    }

    public static Vector2 DetailedPointIsInBounds(this Vector2 point, Vector2 min, Vector2 max)
    {
        return new Vector2(
            point.x.DetailedNumberIsInBounds(min.x, max.x),
            point.y.DetailedNumberIsInBounds(min.y, max.y)
            );
    }

    public static Vector2 CalculateAverage(this Vector2 [] vectors)
    {
        Vector2 ret = Vector2.zero;
        for (int i = 0; i < vectors.Length; i++)
        {
            ret += vectors[i];
        }
        return ret / vectors.Length;
    }

    // =========== \\
    // = List<T> = \\
    // =========== \\

    public delegate void DuplicateFoundCallback<T>(ref T original, T duplicate);
    public static void RemoveDuplicatesFromSortedList<T>(this List<T> list, DuplicateFoundCallback<T> callback = null)
    {
        for (int i = 1; i < list.Count;)
        {
            bool equal;

            if (list[i] is IComparable)
            {
                equal = (list[i] as IComparable).CompareTo(list[i - 1]) == 0;
            }
            else
            {
                equal = list[i].Equals(list[i - 1]);
            }

            if (equal)
            {
                if (callback != null)
                {
                    T one = list[i - 1];
                    callback(ref one, list[i]);
                    list[i - 1] = one;

                }
                list.RemoveAt(i);
            }
            else
            {
                i++;
            }
        }
    }

}

public class Vector2Comparer : IComparer<Vector2>
{
    public int Compare(Vector2 x, Vector2 y)
    {
        return x.Compare(y);
    }
}
