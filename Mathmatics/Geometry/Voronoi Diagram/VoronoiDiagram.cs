using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace STToolBox.Mathmatics.Geometry.Voronoi
{
    public class VoronoiDiagram
    {
        DelaunayTriangulation triangulation;
        List<Connector> edges;

        public VoronoiDiagram(Vector2 size) : this(new DelaunayTriangulation(size)) { }
        public VoronoiDiagram(DelaunayTriangulation triangulation)
        {
            this.triangulation = triangulation;

            edges = new List<Connector>();
            ConvertToVoronoi();
        }

        public bool AddCellAt(Vector2 point)
        {
            return triangulation.AddPoint(point);
        }

        public bool RemoveCellAt(Vector2 point)
        {
            return triangulation.RemovePoint(point);
        }

        public bool VerifyTriangulation()
        {
            return triangulation.Verify();
        }

        // ======================== \\
        // = Voronoi Calculations = \\
        // ======================== \\

        void ConvertToVoronoi()
        {
            edges.Clear();
            Connector[] dtEdges = triangulation.GetEdges();

            for (int i = 0; i < dtEdges.Length; i++)
            {
                if (dtEdges[i].doubleEdged)
                {
                    Connector edge = dtEdges[i].GetTrianglularSplit();
                    if (edge.ClampLineWithinBounds(Vector2.zero, triangulation.size))
                    {
                        edges.Add(edge);
                    }
                }
            }
        }

        // ===================== \\
        // = Setters / Getters = \\
        // ===================== \\

        public Vector2[] GetPoints()
        {
            return triangulation.GetPoints();
        }

        public Connector[] GetEdges()
        {
            return edges.ToArray();
        }

        public Line[] GetBounds()
        {
            return new Line[]
            {
                new Line(Vector2.zero, new Vector2(triangulation.size.x, 0)),
                new Line(new Vector2(triangulation.size.x, 0), new Vector2(triangulation.size.x, triangulation.size.y)),
                new Line(new Vector2(triangulation.size.x, triangulation.size.y), new Vector2(0, triangulation.size.y)),
                new Line(new Vector2(0, triangulation.size.y), Vector2.zero)
            };
        }
    }
}