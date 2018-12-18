using System.Collections.Generic;
using UnityEngine;
using STToolBox.Mathmatics.Geometry;
using STToolBox.Mathmatics.Geometry.Voronoi;

public class VoronoiGenerator : MonoBehaviour
{
    public Vector2 minRange = Vector2.zero;
    public Vector2 maxRange = Vector2.one * 10;
    public int pointCount = 5;
    public int generatorSeed;

    public GameObject pointPrefab;
    public GameObject connectionPrefab;
    public GameObject circlePrefab;

    DelaunayTriangulation triangulation;
    VoronoiDiagram voronoiDiagram;

    GameObject[] pointObjects;
    List<GameObject> connectorObjects;
    List<GameObject> circumcircleObjects;

    void Awake()
    {
        pointObjects = new GameObject[pointCount];
        connectorObjects = new List<GameObject>();
        circumcircleObjects = new List<GameObject>();
    }

    void Start()
    {
        triangulation = new DelaunayTriangulation(new Vector2(maxRange.x - minRange.x, maxRange.y - minRange.y));
        GenerateRandomPoints();
        voronoiDiagram = new VoronoiDiagram(triangulation);

        DisplayDelaunayTriangulation();
        DisplayVoronoiDiagram();
    }

    void GenerateRandomPoints()
    {
        Random.InitState(generatorSeed);

        for (int i = 0; i < pointCount; i++)
        {
            float x = Random.Range(minRange.x, maxRange.x);
            float y = Random.Range(minRange.y, maxRange.y);
            if (!triangulation.AddPoint(new Vector2(x, y)))
            {
                i--;
            }
        }

        triangulation.Verify();
    }

    // ================== \\
    // = User Functions = \\
    // ================== \\

    public Vector2 selectedPoint;
    public void RemoveSelectedPoint()
    {
        voronoiDiagram.RemoveCellAt(selectedPoint);

        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
        DisplayDelaunayTriangulation();
        DisplayVoronoiDiagram();
    }

    // ======================== \\
    // = Display Game Objects = \\
    // ======================== \\

    void DisplayDelaunayTriangulation()
    {
        GameObject parent = new GameObject();
        parent.transform.parent = transform;
        parent.name = "DelaunayTriangulation";
        parent.SetActive(false);

        Triangle[] t = triangulation.GetTriangles();
        for (int i = 0; i < t.Length; i++)
        {
            GameObject triangle = DisplayCircumcircle(t[i], i, parent.transform);

            Vector2[] p = t[i].GetPoints();
            for (int j = 0; j < p.Length; j++)
            {
                DisplayDTPoint(p[j], j, triangle.transform);
            }

            Connector[] c = t[i].GetConnectors();
            for (int j = 0; j < c.Length; j++)
            {
                DisplayDTConnector(c[j], j, triangle.transform);
            }
        }
    }

    bool done = false;
    void DisplayVoronoiDiagram()
    {
        GameObject parent = new GameObject();
        parent.transform.parent = transform;
        parent.name = "VoronoiDiagram";

        Vector2[] p = voronoiDiagram.GetPoints();
        for (int j = 0; j < p.Length; j++)
        {
            DisplayDTPoint(p[j], j, parent.transform);
        }
        if (!done)
        {
            selectedPoint = p[40];
            done = true;
        }

        Connector[] cs = voronoiDiagram.GetEdges();
        for (int i = 0; i < cs.Length; i++)
        {
            DisplayDTConnector(cs[i], i, parent.transform);
        }

        Line[] ls = voronoiDiagram.GetBounds();
        for (int i = 0; i < ls.Length; i++)
        {
            DisplayDTConnector(ls[i], i, parent.transform);
        }
    }

    GameObject DisplayCircumcircle(Triangle t, int i, Transform parent)
    {
        GameObject par = new GameObject();
        par.transform.parent = parent;
        par.name = "Triangle_" + i;

        GameObject cir = Instantiate(circlePrefab, par.transform);
        cir.transform.position = new Vector3(t.center.x, 0, t.center.y);
        cir.transform.localScale = new Vector3(t.radius * 2, 0.01f, t.radius * 2);
        cir.name = "Circumcircle";
        cir.GetComponent<EditorNotes>().notes = "P1: " + t.pointOne + "\nP2: " + t.pointTwo + "\nP3: " + t.pointThree;
        cir.SetActive(false);
        return par;
    }

    void DisplayDTPoint(Vector2 p, int i, Transform parent)
    {
        GameObject point = Instantiate(pointPrefab, parent);
        point.transform.position = new Vector3(p.x, 0, p.y);
        point.GetComponent<EditorNotes>().notes = "Point: " + p;
        point.name = "Point_" + i;
    }

    void DisplayDTConnector(Line c, int i, Transform parent)
    {
        GameObject con = Instantiate(connectionPrefab, parent);
        con.transform.position = new Vector3(c.center.x, 0, c.center.y);
        con.transform.rotation = Quaternion.Euler(90, 90 - c.angle, 0);
        con.transform.localScale = new Vector3(0.03f, c.length / 2, 0.03f);
        con.GetComponent<EditorNotes>().notes = "P1: " + c.pointOne + "\nP2: " + c.pointTwo;
        con.name = "Connector_" + i;
    }

}
