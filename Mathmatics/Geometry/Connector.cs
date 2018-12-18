using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace STToolBox.Mathmatics.Geometry
{
    public class Connector : Line
    {
        public bool doubleEdged = false;
        public Triangle[] triangles = new Triangle[2];

        public Connector(Vector2 startPoint, Vector2 endPoint) : base(startPoint, endPoint) { }

        public Vector2 GetPointConnectedTo(Vector2 point)
        {
            return point == pointOne ? pointTwo : (point == pointTwo ? pointOne : Vector2.one * -1);
        }

        public Connector GetTrianglularSplit()
        {
            if (!doubleEdged)
            {
                Debug.Log("Calculation Error: This edge is not double edged. Cannot get triangular split.");
                return null;
            }

            return new Connector(triangles[0].center, triangles[1].center);
        }

        // ============= \\
        // = Overrides = \\
        // ============= \\

        public override string ToString()
        {
            return base.ToString() + "DoubleEdge?: " + doubleEdged;
        }
    }
}
