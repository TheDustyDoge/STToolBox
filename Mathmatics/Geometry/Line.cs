using UnityEngine;
using System;

namespace STToolBox.Mathmatics.Geometry
{
    public class Line : IComparable
    {
        public Vector2 pointOne { get; private set; }
        public Vector2 pointTwo { get; private set; }
        public bool isVertical { get; private set; }

        public Vector2 center { get; private set; }
        public float angle { get; private set; }
        public float length { get; private set; }

        public float slope { get; private set; }
        public float intercept { get; private set; }

        public Line(Vector2 startPoint, Vector2 endPoint)
        {
            pointOne = startPoint;
            pointTwo = endPoint;
            CalculateValues();
        }

        void CalculateValues()
        {
            isVertical = pointOne.x == pointTwo.x;

            if (pointTwo.x < pointOne.x)
            {
                Vector2 temp = pointOne;
                pointOne = pointTwo;
                pointTwo = temp;
            }

            if (pointOne.x != pointTwo.x)
            {
                slope = (pointOne.y - pointTwo.y) / (pointOne.x - pointTwo.x);
                intercept = pointOne.y - (pointOne.x * slope);
            }
            else
            {
                intercept = pointOne.x;
            }

            center = (pointOne - pointTwo) / 2 + pointTwo;
            angle = pointOne.AngleToPoint(pointTwo);
            length = pointOne.DistanceTo(pointTwo);
        }

        // ===================== \\
        // = Line Calculations = \\
        // ===================== \\

        public float GetXFromY(float y)
        {
            if (slope == 0)
            {
                Debug.LogError("ERROR: Cannot get X value from horizontal line.");
                return 0;
            }

            return (y - intercept) / slope;
        }

        public float GetYFromX(float x)
        {
            if (isVertical)
            {
                Debug.LogError("ERROR: Cannot get Y value from vertical line.");
                return 0;
            }

            return ((x * slope) + intercept);
        }

        public Line GetPerpendicularLine()
        {
            Vector2 p1 = pointOne.RotateAroundPoint(center, 90);
            Vector2 p2 = pointTwo.RotateAroundPoint(center, 90);

            return new Line(p1, p2);
        }

        public static Vector2 GetIntersectionPoint(Line one, Line two)
        {
            if (one.angle == two.angle)
            {
                Debug.LogError("ERROR: Parallel lines have no intersection point.");
                return Vector2.zero;
            }

            if (one.isVertical)
            {
                return new Vector2(one.intercept, two.GetYFromX(one.intercept));
            }
            else if (two.isVertical)
            {
                return new Vector2(two.intercept, one.GetYFromX(two.intercept));
            }

            float newx = (two.intercept - one.intercept) / (one.slope - two.slope);
            return new Vector2(newx, one.GetYFromX(newx));
        }

        public Vector2 GetIntersectionPoint(Line other)
        {
            return GetIntersectionPoint(this, other);
        }

        public bool ClampLineWithinBounds(Vector2 min, Vector2 max)
        {
            if (!LineSegmentEntersBounds(min, max))
            {
                // Debug.Log("Warning: cannot conform line to bounds if line doesn't enter bounds.");
                return false;
            }

            Vector2 det1 = pointOne.DetailedPointIsInBounds(min, max);
            Vector2 det2 = pointTwo.DetailedPointIsInBounds(min, max);
            Vector2 newOne = pointOne;
            Vector2 newTwo = pointTwo;

            if (isVertical)
            {
                // It's vertical so the x value is fine.
                newOne.y = pointOne.y.Clamp(min.y, max.y);
                newTwo.y = pointTwo.y.Clamp(min.y, max.y);
            }
            else if (slope == 0) // Is Horizontal
            {
                // It's horizontal so the x value is fine.
                newOne.x = pointOne.x.Clamp(min.x, max.x);
                newTwo.x = pointTwo.x.Clamp(min.x, max.x);
            }
            else // Not Vertical / Horizontal
            {
                if (det1 != Vector2.zero) // p1 is out of bounds
                {
                    if (det1.x != 0) // x out of bounds
                    {
                        newOne.x = pointOne.x.Clamp(min.x, max.x);
                        newOne.y = GetYFromX(newOne.x);
                    }
                    if (det1.y != 0) // y out of bounds
                    {
                        newOne.y = pointOne.y.Clamp(min.y, max.y);
                        newOne.x = GetXFromY(newOne.y);
                    }
                }

                if (det2 != Vector2.zero) // p2 is out of bounds
                {
                    if (det2.x != 0) // x out of bounds
                    {
                        newTwo.x = pointTwo.x.Clamp(min.x, max.x);
                        newTwo.y = GetYFromX(newTwo.x);
                    }
                    if (det2.y != 0) // y out of bounds
                    {
                        newTwo.y = pointTwo.y.Clamp(min.y, max.y);
                        newTwo.x = GetXFromY(newTwo.y);
                    }
                }
            }

            pointOne = newOne;
            pointTwo = newTwo;

            CalculateValues();
            return true;
        }

        // ================== \\
        // = Boolean Checks = \\
        // ================== \\

        public bool PointIsAbove(Vector2 point)
        {
            // If line isVertical, returns true for line to the right of the line
            if (isVertical)
            {
                return intercept < point.x;
            }
            return GetYFromX(point.x) < point.y;
        }

        public bool LineSegmentEntersBounds(Vector2 min, Vector2 max)
        {
            return LineEntersBounds(min, max) && // Crosses through bounds
                (pointOne.x < max.x && min.x < pointTwo.x) && // p1.x <= p2.x ALWAYS
                ((pointOne.y < max.y && min.y < pointTwo.y) || (pointTwo.y < max.y && min.y < pointOne.y)); // p1.y & p2.y have no correlation
        }

        public bool LineEntersBounds(Vector2 min, Vector2 max)
        {
            return PointIsAbove(min) != PointIsAbove(max) || PointIsAbove(new Vector2(min.x, max.y)) != PointIsAbove(new Vector2(max.x, min.y));
        }

        public bool PointIsOnLine(Vector2 point)
        {
            // TODO
            return false;
        }

        public bool SameSlope(Line line)
        {
            return line.slope == slope && line.isVertical == isVertical;
        }

        // ===================== \\
        // = Setters / Getters = \\
        // ===================== \\

        public void SetPointOne(Vector2 point)
        {
            pointOne = point;
            CalculateValues();
        }

        public void SetPointTwo(Vector2 point)
        {
            pointTwo = point;
            CalculateValues();
        }

        public void SetPoints(Vector2 pointOne, Vector2 pointTwo)
        {
            this.pointOne = pointOne;
            this.pointTwo = pointTwo;
            CalculateValues();
        }

        // ============= \\
        // = Overrides = \\
        // ============= \\

        public override string ToString()
        {
            return "(" + pointOne.ToString() + " : " + pointTwo.ToString() + ")";
        }

        public int CompareTo(object obj)
        {
            if (!(obj is Line))
                return -1;

            int ret = (obj as Line).pointOne.Compare(pointOne);
            if (ret != 0)
                return ret;
            return (obj as Line).pointTwo.Compare(pointTwo);
        }
    }

}
