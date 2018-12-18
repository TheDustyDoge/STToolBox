using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace STToolBox.Mathmatics.Geometry.Voronoi
{
    public class DelaunayTriangulation
    {
        public bool debugLog;
        public Vector2 size { get; private set; }

        Triangle largeTriangle;

        List<Vector2> points;
        List<Triangle> triangles;

        List<Vector2> polygonHole;
        List<Triangle> badTriangles;

        public DelaunayTriangulation() : this(Vector2.one * 10) { }
        public DelaunayTriangulation(Vector2 size)
        {
            this.size = size;

            points = new List<Vector2>();
            triangles = new List<Triangle>();
            GenerateFirstTriangle();

            polygonHole = new List<Vector2>();
            badTriangles = new List<Triangle>();
        }

        public bool AddPoint(Vector2 point)
        {
            if (!points.Contains(point))
            {
                AddPointToDiagram(point);
                return true;
            }
            if (debugLog)
            {
                Debug.Log("Warning: point " + point + "is already in this Voronoi Diagram");
            }
            return false;
        }

        public bool RemovePoint(Vector2 point)
        {
            if (points.Contains(point))
            {
                RemovePointFromDiagram(point);
                return true;
            }
            if (debugLog)
            {
                Debug.Log("Warning: cannot remove point that is not in the diagram.");
            }
            return false;
        }

        public bool Verify()
        {
            bool valid = true;
            for (int i = 0; i < triangles.Count; i++)
            {
                for (int j = 0; j < points.Count; j++)
                {
                    if (triangles[i].CheckForPointInCircumcircle(points[j]) && !triangles[i].PointIsVertex(points[j]))
                    {
                        if (debugLog)
                        {
                            Debug.Log("Calculation Error: point " + j + ": " + points[j] + " is inside of circle " + i + ": " + triangles[i].ToString());
                        }
                        valid = false;
                    }
                }
            }

            if (valid)
            {
                Debug.Log("No Errors Found.");
            }
            return valid;
        }

        // ========================= \\
        // = Delaunay Calculations = \\
        // ========================= \\

        void AddPointToDiagram(Vector2 point)
        {
            polygonHole.Clear();
            badTriangles.Clear();
            points.Add(point);

            FindInvalidatedTriangles((Triangle t) => t.CheckForPointInCircumcircle(point));
            RemoveDuplicatePoints(ref polygonHole);
            RemoveBadTrianglesFromTriangulation();
            FillInPolygonHoleWithPoint(point);
        }

        void RemovePointFromDiagram(Vector2 point)
        {
            badTriangles.Clear();
            polygonHole.Clear();
            points.Remove(point);

            FindInvalidatedTriangles((Triangle t) => t.PointIsVertex(point));
            RemoveDuplicatePoints(ref polygonHole);
            RemoveBadTrianglesFromTriangulation();
            FillInPolygonHoleWithRemovedPoint(point);
        }

        delegate bool CheckPointInvalidatesTriangle(Triangle t);
        void FindInvalidatedTriangles(CheckPointInvalidatesTriangle check)
        {
            for (int i = 0; i < triangles.Count; i++)
            {
                if (check(triangles[i]))
                {
                    badTriangles.Add(triangles[i]);
                    polygonHole.AddRange(triangles[i].GetPoints());
                }
            }
        }

        void RemoveBadTrianglesFromTriangulation()
        {
            for (int i = 0; i < badTriangles.Count; i++)
            {
                triangles.Remove(badTriangles[i]);
            }
            badTriangles.Clear();
        }

        void FillInPolygonHoleWithPoint(Vector2 pivotPoint)
        {
            polygonHole.Sort(new AngleToPointSort(pivotPoint));
            Vector2 lastPoint = polygonHole[polygonHole.Count - 1];
            for (int i = 0; i < polygonHole.Count; i++)
            {
                Triangle t = new Triangle(lastPoint, polygonHole[i], pivotPoint);
                if (!t.CheckValidity())
                {
                    Debug.LogError("Triangle is invalid.");
                }
                triangles.Add(t);
                lastPoint = polygonHole[i];
            }
        }

        void FillInPolygonHoleWithRemovedPoint(Vector2 removedPoint)
        {
            if (polygonHole.Count <= 2)
                return;

            PriorityQueue<Triangle> queue = new PriorityQueue<Triangle>();

            polygonHole.Sort(new AngleToPointSort(removedPoint));

            Vector2 oneBack = polygonHole[polygonHole.Count - 1];
            Vector2 twoBack = polygonHole[polygonHole.Count - 2];
            for (int i = 0; i < polygonHole.Count; i++)
            {
                Triangle ear = new Triangle(twoBack, oneBack, polygonHole[i]);
                if (ear.IsClockwise())
                {
                    queue.Insert(ear, float.MaxValue);
                }
                else
                {
                    queue.Insert(ear, -1 * removedPoint.DistanceTo(ear.center));
                }
                twoBack = oneBack;
                oneBack = polygonHole[i];
            }

            while (queue.Count > 3)
            {
                Triangle next = queue.RetrieveFirstItemInQueue();
                Triangle[] shared = queue.RetrieveItemsWith((Triangle t) => { return t.HasSharedEdge(next); });
                for (int i = 0; i < shared.Length; i++)
                {
                    Connector c = shared[i].GetSharedEdge(next);
                    // what point do i move?
                }
                triangles.Add(next);
            }
        }

        // =========== \\
        // = Helpers = \\
        // =========== \\

        void GenerateFirstTriangle()
        {
            largeTriangle = new Triangle(new Vector2(0, size.y * 2), Vector2.zero, new Vector2(size.x * 2, 0));
            points.AddRange(largeTriangle.GetPoints());
            triangles.Add(largeTriangle);
        }

        void RemoveDuplicatePoints(ref List<Vector2> points)
        {
            points.Sort(new Vector2Comparer());
            points.RemoveDuplicatesFromSortedList();
        }

        void RemoveDuplicateEdgesConnectors(ref List<Connector> connectors)
        {
            connectors.Sort();
            connectors.RemoveDuplicatesFromSortedList(DuplicateEdgeFoundCallback);
        }

        void DuplicateEdgeFoundCallback(ref Connector one, Connector two)
        {
            one.doubleEdged = true;
            one.triangles[1] = two.triangles[0];
        }

        // ===================== \\
        // = Setters / Getters = \\
        // ===================== \\

        public Vector2[] GetPoints()
        {
            return points.ToArray();
        }

        public Connector[] GetEdges()
        {
            List<Connector> c = new List<Connector>();

            for (int i = 0; i < triangles.Count; i++)
            {
                c.AddRange(triangles[i].GetConnectors());
            }
            RemoveDuplicateEdgesConnectors(ref c);

            return c.ToArray();
        }

        public Triangle[] GetTriangles()
        {
            return triangles.ToArray();
        }
    }

    public class AngleToPointSort : IComparer<Vector2>
    {
        Vector2 point;
        public AngleToPointSort(Vector2 point)
        {
            this.point = point;
        }

        public int Compare(Vector2 x, Vector2 y)
        {
            return x.AngleToPoint(point).CompareTo(y.AngleToPoint(point));
        }
    }
}
