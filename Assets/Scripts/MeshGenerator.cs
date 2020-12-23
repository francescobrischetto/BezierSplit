using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshGenerator : MonoBehaviour
{
    public Mesh mesh;

    Vector3[,] controlPoints;
    public Vector3[] vertices;
    int[] triangles;

    //numero di punti valutati per la superficie
    public int xSize = 20;
    public int zSize = 20;


    void Start()
    {
        //creo una nuova mesh e aggiorno la mesh precedente
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        
        //creo randomicamente i punti di controllo della superficie di bezier
        CreateControlPoints();
        //creo tutti i vertici e i triangoli che comporranno la mesh
        CreateShape();
        //aggiorno la mesh
        UpdateMesh();

    } 


    void CreateControlPoints()
    {
        //muovere molto su zx, e poco su y
        int grade = 3;
        controlPoints = new Vector3[grade+1, grade+1];
        //Questa y verrà modificata nel cubo
        int y = 2;
        for(int i = 0; i <= grade; i++)
        {
            for(int j=0; j<=grade; j++)
            {
                float xNoise = i + Random.Range(-.5f, .2f);
                float yNoise = y + Random.Range(-.5f, 1f);
                float zNoise = j + Random.Range(-.5f, .2f);
                xNoise = Normalize(xNoise, -.5f, 3.2f, 0, 3);
                yNoise = Normalize(yNoise, -.5f, 4, 0, 3);
                zNoise = Normalize(zNoise, -.5f, 3.2f, 0, 3);
                controlPoints[i, j] = new Vector3(xNoise,yNoise,zNoise);
            }
        }

    }

    float Normalize(float val, float valmin, float valmax, float min, float max)
    {
        return (((val - valmin) / (valmax - valmin)) * (max - min)) + min;
    }

    void CreateShape()
    {
        //calcola il punto nella curva di bezier
        vertices = new Vector3[(xSize + 1) * (zSize + 1)];
        for (int i = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                //float y = Mathf.PerlinNoise(x * .3f, z * .3f) * 2f;
                float u = ((float)x) / xSize;
                float v = ((float)z) / zSize;
                vertices[i] = bezier(u, v);
                i++;
            }
        }
        //crea i vari triangoli
        triangles = new int[xSize * zSize * 6];

        for (int ti = 0, vi = 0, y = 0; y < zSize; y++, vi++)
        {
            for (int x = 0; x < xSize; x++, ti += 6, vi++)
            {
                triangles[ti] = vi;
                triangles[ti + 3] = triangles[ti + 2] = vi + 1;
                triangles[ti + 4] = triangles[ti + 1] = vi + xSize + 1;
                triangles[ti + 5] = vi + xSize + 2;
            }
        }
    }

    void UpdateMesh()
    {
        //aggiorna la mesh
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();
    }

    Vector3 bezier(float u, float v)
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

    private void OnDrawGizmos()
    {
        if (controlPoints == null)
        {
            return;
        }
        //Questo ci permette di disegnare i punti del poligono di controllo qualora fosse necessario
        for (int col = 0; col < controlPoints.GetLength(0); col++)
        {
            for (int row = 0; row < controlPoints.GetLength(1); row++)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawSphere(controlPoints[col, row], .1f);
            }
        }
        
    }
}
