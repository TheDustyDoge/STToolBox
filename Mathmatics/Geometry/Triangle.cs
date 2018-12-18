using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace STToolBox.Mathmatics.Geometry
{
    public class Triangle
    {
        public Vector2 pointOne { get; private set; }
        public Vector2 pointTwo { get; private set; }
        public Vector2 pointThree { get; private set; }

        public Connector connectorOne { get; private set; }
        public Connector connectorTwo { get; private set; }
        public Connector connectorThree { get; private set; }

        public Vector2 center { get; private set; }
        public float radius { get; private set; }

        public Triangle(Vector2 one, Vector2 two, Vector2 three)
        {
            pointOne = one;
            pointTwo = two;
            pointThree = three;

            connectorOne = new Connector(pointTwo, pointThree);
            connectorTwo = new Connector(pointThree, pointOne);
            connectorThree = new Connector(pointOne, pointTwo);

            connectorOne.triangles[0] = connectorTwo.triangles[0] = connectorThree.triangles[0] = this;

            InitCircumcircle();
        }

        private void InitCircumcircle()
        {
            Line lineOne = connectorOne.GetPerpendicularLine();
            Line lineTwo = connectorTwo.GetPerpendicularLine();

            center = Line.GetIntersectionPoint(lineOne, lineTwo);
            radius = Vector2.Distance(pointOne, center);
        }

        private void ReinitCircumcircle()
        {
            connectorOne.SetPoints(pointTwo, pointThree);
            connectorTwo.SetPoints(pointThree, pointOne);
            connectorThree.SetPoints(pointOne, pointTwo);
            InitCircumcircle();
        }

        // ========================= \\
        // = Triangle Calculations = \\
        // ========================= \\

        public Vector2[] GetPoints()
        {
            return new Vector2[] { pointOne, pointTwo, pointThree };
        }

        public Vector2[] GetPointsWithout(Vector2 point)
        {
            if (!PointIsVertex(point))
                return GetPoints();
            return new Vector2[] { pointOne == point ? pointTwo : pointOne, pointTwo == point ? pointThree : pointTwo };
        }

        public Connector[] GetConnectors()
        {
            //connectorOne.doubleEdged = connectorTwo.doubleEdged = connectorThree.doubleEdged = false;
            return new Connector[] { connectorOne, connectorTwo, connectorThree };
        }

        public int CheckForLineOfSight(Vector2 point)
        {
            // Is the point on the same side of the triangle as each vertex?
            bool[] sameSide = new bool[] { connectorOne.PointIsAbove(pointOne) == connectorOne.PointIsAbove(point), connectorTwo.PointIsAbove(pointTwo) == connectorTwo.PointIsAbove(point), connectorThree.PointIsAbove(pointThree) == connectorThree.PointIsAbove(point) };

            if (!sameSide[0] && sameSide[1] && sameSide[2]) // Can't see point one
            {
                return 0;
            }
            if (sameSide[0] && !sameSide[1] && sameSide[2]) // Can't see point two
            {
                return 1;
            }
            if (sameSide[0] && sameSide[1] && !sameSide[2]) // Can't see point three
            {
                return 2;
            }

            return -1; // Can see all points
        }

        public void DoIt(Triangle one, Triangle two)
        {

        }

        // ================== \\
        // = Boolean Checks = \\
        // ================== \\

        public bool PointIsVertex(Vector2 point)
        {
            return point.Equals(pointOne) || point.Equals(pointTwo) || point.Equals(pointThree);
        }

        public bool IsEdgeInTriangle(Connector edge)
        {
            return connectorOne.CompareTo(edge) == 0 || connectorTwo.CompareTo(edge) == 0 || connectorThree.CompareTo(edge) == 0;
        }

        public bool CheckValidity()
        {
            return !(connectorOne.SameSlope(connectorTwo) || connectorTwo.SameSlope(connectorThree) || connectorThree.SameSlope(connectorOne));
        }

        public bool CheckForPointInCircumcircle(Vector2 point)
        {
            return center.DistanceTo(point) < radius;
        }

        public bool IsClockwise()
        {
            float angleOne = center.AngleToPoint(pointOne).CircularClamp360();
            float angleTwo = center.AngleToPoint(pointTwo).CircularClamp360();
            float angleThree = center.AngleToPoint(pointThree).CircularClamp360();

            if (angleOne > angleThree)
            {
                return angleOne > angleTwo && angleTwo > angleThree;
            }
            else
            {
                return angleOne < angleTwo && angleTwo < angleThree;
            }
        }

        public bool HasSharedEdge(Triangle t)
        {
            return GetSharedEdge(t) != null;
        }

        public Connector GetSharedEdge(Triangle t)
        {
            Connector[] e = t.GetConnectors();
            for (int i = 0; i < e.Length; i++)
            {
                if (IsEdgeInTriangle(e[i]))
                {
                    return e[i];
                }
            }
            return null;
        }

        // ===================== \\
        // = Setters / Getters = \\
        // ===================== \\

        public void ModifyPoint(Vector2 initialPoint, Vector2 newPoint)
        {
            if (pointOne.Equals(initialPoint))
                pointOne = newPoint;
            else if (pointTwo.Equals(initialPoint))
                pointTwo = newPoint;
            else if (pointThree.Equals(initialPoint))
                pointThree = newPoint;
            else
            {
                Debug.Log("Warning: triangle " + ToString() + "does not contain point " + initialPoint + " so it cannot be modified.");
                return;
            }

            SetPoints(pointOne, pointTwo, pointThree);
        }

        public void SetPoints(Vector2 pointOne, Vector2 pointTwo, Vector2 pointThree)
        {
            this.pointOne = pointOne;
            this.pointTwo = pointTwo;
            this.pointThree = pointThree;
            ReinitCircumcircle();
        }

        // ============= \\
        // = Overrides = \\
        // ============= \\

        public override string ToString()
        {
            return "( " + pointOne.ToString() + " : " + pointTwo.ToString() + " : " + pointThree.ToString() + ")";
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Triangle))
                return false;

            return (obj as Triangle).pointOne.Equals(pointOne) && (obj as Triangle).pointTwo.Equals(pointTwo) && (obj as Triangle).pointThree.Equals(pointThree);
        }
    }
}
