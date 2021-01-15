using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshGenerator : MonoBehaviour
{

    public GameObject cube;

    //poligono di controllo
    Vector3[,] controlPoints;
    public int[] controlTriangles;
    public Vector3[] linearControlPoints;
    public Mesh controlMesh;

    //intera superficie
    public Mesh fullBezierSurface;
    public Vector3[] b_vertices;
    public int[] b_triangles;

    //superficie ridotta "taglio"
    public Mesh cutBezierSurface;
    public Vector3[] c_vertices;
    public int[] c_triangles;

    //numero di punti valutati per la superficie
    public int xSize = 100;
    public int zSize = 100;

    //Questi serviranno per stimare i volumi
    public Vector3[] midPoints;

    void Start()
    {
        //creo randomicamente i punti di controllo della superficie di bezier
        CreateControlPoints();
        //creo tutti i vertici e i triangoli che comporranno la mesh
        CreateShape();
        //associo la nuova mesh creata con quella dell'oggetto
        UpdateMesh();
    } 

    void CreateControlPoints()
    {
        //muovere molto sul piano xz, e poco sull'asse y
        int grade = 3;
        controlPoints = new Vector3[grade+1, grade+1];
        linearControlPoints = new Vector3[(grade + 1) * (grade + 1)];
        int y = 0;
        int k = 0;
        for(int i = 0; i <= grade; i++)
        {
            for(int j=0; j<=grade; j++)
            {
                float xNoise = i + Random.Range(-.5f, .2f);
                float zNoise = j + Random.Range(-.5f, .2f);
                float yNoise = y + Random.Range(-.5f, 1f);
                //Sposto di molto l'altezza dei punti di controllo in base alla difficoltà attuale
                if (cube.GetComponent<CubeAdjust>().margin < CubeAdjust.Margin.Easy)
                {
                    yNoise += Random.Range(1f, 1.5f);
                    if (cube.GetComponent<CubeAdjust>().margin == CubeAdjust.Margin.Hard)
                    {
                        yNoise += Random.Range(1f, 1.5f);
                    }
                    bool sign = Random.Range(0, 1000) % 2 == 0 ? true : false;
                    if (sign) yNoise = -yNoise;
                }
                controlPoints[i, j] = new Vector3(xNoise,yNoise,zNoise);
                //Linearizzo i punti di controllo in un vettore unidimensionale
                linearControlPoints[k] = controlPoints[i, j];
                k++;
            }
        }
        //Calcolo i triangoli da visualizzare per disegnare il poligono di controllo
        controlTriangles = calculateTriangles(grade, grade);
        AddControlPointsVisualizer();
    }

    public void AddControlPointsVisualizer()
    {
        //indici dei triangoli necessari per visualizzare un cubo
        int[] triangles = {
            0, 2, 1, 0, 3, 2,//face front
            2, 3, 4, 2, 4, 5, //face top
            1, 2, 5, 1, 5, 6,//face right
            0, 7, 4, 0, 4, 3,//face left
            5, 4, 7, 5, 7, 6,//face back
            0, 6, 7, 0, 1, 6//face bottom
        };

        //combine serve per unire insieme più mesh
        CombineInstance[] combine = new CombineInstance[linearControlPoints.Length + 1];
        //aggiungo l'intera mesh del poligono di controllo
        Mesh m = new Mesh();
        m.vertices = linearControlPoints;
        m.triangles = controlTriangles;
        combine[0].mesh = m;
        combine[0].transform = gameObject.transform.localToWorldMatrix;
        List<Vector3> points = new List<Vector3>();

        float scarto = 0.03f;

        for (int i = 0; i < linearControlPoints.Length; i++)
        {
            //la posizione del controlPoint che stiamo considerando
            float xm =linearControlPoints[i].x;
            float ym = linearControlPoints[i].y;
            float zm = linearControlPoints[i].z;
            //aggiungo gli 8 vertici del cubo
            points.Add(new Vector3(xm - scarto, ym - scarto, zm - scarto));
            points.Add(new Vector3(xm + scarto, ym - scarto, zm - scarto));
            points.Add(new Vector3(xm + scarto, ym + scarto, zm - scarto));
            points.Add(new Vector3(xm - scarto, ym + scarto, zm - scarto));
            points.Add(new Vector3(xm - scarto, ym + scarto, zm + scarto));
            points.Add(new Vector3(xm + scarto, ym + scarto, zm + scarto));
            points.Add(new Vector3(xm + scarto, ym - scarto, zm + scarto));
            points.Add(new Vector3(xm - scarto, ym - scarto, zm + scarto));
            //creo la mesh del cubo
            Mesh n = new Mesh();
            n.vertices = (Vector3[])points.ToArray().Clone();
            n.triangles = (int[])triangles.Clone();
            //la aggiungo alle mesh che dovranno essere unite
            combine[i+1].mesh = n;
            combine[i+1].transform = gameObject.transform.localToWorldMatrix;
            points.Clear();

        }
        //unisco le mesh di tutti gli stick
        controlMesh = new Mesh();
        controlMesh.CombineMeshes(combine);
    }


    Vector3[] calculateVertices(int startX, int startZ, int limX, int limZ)
    {
        Vector3[] support = new Vector3[(limX + 1) * (limZ + 1)];
        for (int i = 0, z = startZ; z <= limZ; z++)
        {
            for (int x = startX; x <= limX; x++)
            {
                float u = ((float)x) / xSize;
                float v = ((float)z) / zSize;
                support[i] = bezier(u, v);
                i++;
            }
        }
        return support;
    }

    int[] calculateTriangles(int limX , int limZ)
    {
        int[] support = new int[limX*limZ*6];
        for (int ti = 0, vi = 0, y = 0; y < limZ; y++, vi++)
        {
            for (int x = 0; x < limX; x++, ti += 6, vi++)
            {
                support[ti] = vi;
                support[ti + 3] = support[ti + 2] = vi + 1;
                support[ti + 4] = support[ti + 1] = vi + limX + 1;
                support[ti + 5] = vi + limX + 2;
            }
        }
        return support;
    }

    Vector3[] calculateMidPoints(Vector3[] vertexs, int[] triangles)
    {
        List<Vector3> support = new List<Vector3>();
        for(int i=0; i < triangles.Length; i += 6)
        {
            support.Add((vertexs[triangles[i]] + vertexs[triangles[i + 2]] + vertexs[triangles[i + 1]] + vertexs[triangles[i + 5]]) / 4.0f);
        }
        return support.ToArray();
    }
    void CreateShape()
    {
        //crea tutti i vertici della superficie di bezier
        b_vertices = new Vector3[(xSize + 1) * (zSize + 1)];
        b_vertices = calculateVertices(0, 0, xSize, zSize);

        //crea tutti i triangoli della superficie di bezier usando i vari vertici
        b_triangles = new int[xSize * zSize * 6];
        b_triangles = calculateTriangles(xSize, zSize);

        //calcolo i punti centrali dei quadrati
        midPoints = new Vector3[xSize * zSize];
        midPoints = calculateMidPoints(b_vertices, b_triangles);
    }

    void UpdateMesh()
    {
        //creo la mesh dell'intera superficie
        fullBezierSurface = new Mesh();
        fullBezierSurface.Clear();
        fullBezierSurface.vertices = b_vertices;
        fullBezierSurface.triangles = b_triangles;
        fullBezierSurface.RecalculateNormals();
    }

    void updateMidPoints(Bounds bc)
    {
        //Funzione che seleziona mi midPoints dopo il "taglio"
        List<Vector3> support = new List<Vector3>();
        for (int i=0; i<midPoints.Length; i++)
        {
            if (bc.Contains(midPoints[i])){
                support.Add(midPoints[i]);
            }
        }
        midPoints = support.ToArray();
    }

    public void CreateCut(Bounds bc)
    {
        //funzione che crea il "taglio" tra la superficie e il cubo
        List<int> support = new List<int>();
        for(int i = 0; i < b_triangles.Length/3; i++)
        {
            int score = 0;
            if (bc.Contains(b_vertices[b_triangles[3 * i + 0]])) {
                score++;
            }
            if (bc.Contains(b_vertices[b_triangles[3 * i + 1]])) {
                score++;
            }
            if (bc.Contains(b_vertices[b_triangles[3 * i + 2]])){
                score++;
            }
            if (score >= 1) { 
                support.Add(b_triangles[3 * i + 0]);
                support.Add(b_triangles[3 * i + 1]);
                support.Add(b_triangles[3 * i + 2]);
            }
        }
        c_vertices = b_vertices;
        c_triangles = support.ToArray();

        //creo la mesh del "taglio"
        cutBezierSurface = new Mesh();
        GetComponent<MeshFilter>().mesh = cutBezierSurface;
        cutBezierSurface.Clear();
        cutBezierSurface.vertices = c_vertices;
        cutBezierSurface.triangles = c_triangles;
        cutBezierSurface.RecalculateNormals();

        updateMidPoints(bc);
        //Solo adesso posso calcolare il volume
        cube.GetComponent<CubeAdjust>().CalculateVolume();
    }

    public Vector3 bezier(float u, float v)
    {
        //assumiamo sempre di utilizzare il grado n=m=3;
        int M = 3, N = 3;
        Vector3 vert = new Vector3(0, 0, 0);
        for (int i=0; i<=N; i++)
        {
            for (int j = 0; j <= M; j++)
            {
                float bernNIS = PolyBernstein3(N, i, u);
                float bernMJT = PolyBernstein3(M, j, v);
                vert.x += controlPoints[i, j].x * bernNIS * bernMJT;
                vert.y += controlPoints[i, j].y * bernNIS * bernMJT;
                vert.z += controlPoints[i, j].z * bernNIS * bernMJT;

            }
        }
        return vert;
    }

    float PolyBernstein3(int n, int i, float u)
    {
        //Questi sono i coefficienti di bernstein per il caso n=3;
        switch (i)
        {
            case 0: return (1 - u) * (1 - u) * (1 - u);
            case 1: return 3 * u * (1 - u) * (1 - u);
            case 2: return 3 * u * u * (1 - u);
            case 3: return u * u * u;
        }
        return 0;
    }
}
