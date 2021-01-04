using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeAdjust : MonoBehaviour
{
    public Mesh volumeMesh;
    public Mesh volumeMesh1;
    private int[] v_triangles;
    private Vector3[] v_vertices;

    //linking con gli altri script
    public GameObject BezierSurface;
    private MeshGenerator BezierSurfaceScript;
    private Bounds b;
    private Bounds bc;

    //Percentuale di volume riempita
    public int percentage;
    
    //Margin gestirà la difficoltà del gioco!
    public enum Margin
    {
        Easy = 5,
        Medium = 4,
        Hard = 3,
        Impossible = 1
    }
    //TODO: Considera static?
    public Margin margin = Margin.Easy;

    //Numero di scarto del taglio della superficie
    public int cutPrecision = 15;

    //dimensione del parallelepipedo
    private float dimension = 0.8f;

    //min-max Range per aumento di difficoltà
    private float Range = 0.2f;

    //Turnable per centrare la visuale
    public Turnable turnable;
    
    void Start()
    {
        volumeMesh = new Mesh();
        volumeMesh1 = new Mesh();
        BezierSurfaceScript = BezierSurface.GetComponent<MeshGenerator>();
        StartCoroutine(waiter());
    }

    IEnumerator waiter()
    {
        //Attendo che si generi la superficie
        yield return new WaitForSecondsRealtime(0.01f);
        //Il bounding volume della superficie nella sua totalità
        b = BezierSurfaceScript.fullBezierSurface.bounds;
        //Qui avviene il "taglio" rimpicciolendo il cubo sulla superficie
        Vector3 first = BezierSurfaceScript.bezier(((float)cutPrecision)/BezierSurfaceScript.xSize, ((float)cutPrecision)/ BezierSurfaceScript.zSize);
        Vector3 second = BezierSurfaceScript.bezier((BezierSurfaceScript.xSize- ((float)cutPrecision)) / BezierSurfaceScript.xSize, (BezierSurfaceScript.zSize - ((float)cutPrecision)) / BezierSurfaceScript.zSize);

        //Qui randomizzo la Y
        float randomizeY = b.size.y + dimension;
        bc = new Bounds(b.center,new Vector3(System.Math.Abs(first.x- second.x), randomizeY, System.Math.Abs(first.z - second.z)));
        
        //Riposiziono il cubo in base alla difficoltà attuale
        float offset = Random.Range(0, Range);
        if (margin < Margin.Easy)
        {
            offset += Random.Range(0, Range);
            if (margin == Margin.Hard)
            {
                offset += Random.Range(0, Range);
            }
        }
        bool sign = Random.Range(0, 1000) % 2==0 ? true : false;
        if(sign) offset = -offset;
        transform.position = bc.center + new Vector3(0,offset,0);

        transform.localScale = bc.size;
        //Chiamo il metodo della superficie di bezier per mostrare solo il "taglio"
        BezierSurfaceScript.CreateCut(bc);

        //Aggiusto la posizione della visuale
        turnable.AdjustPosition();
    }

    public void CalculateVolume()
    {
        // questi sono i vertici in cui viene valutata la superficie
        Vector3[] midPoints = BezierSurfaceScript.midPoints;
        //le dimensioni del parallelepipedo
        Vector3 boxSize = bc.size;
        //l'altezza della base del parallelepipedo
        float y = this.transform.position.y - this.transform.localScale.y / 2;
        //il volume del parallelepipedo calcolato come il prodotto delle 3 dimensioni
        float totalVolume = boxSize.x * boxSize.y * boxSize.z;
        //l'area dei piccoli cubi della griglia alla base
        boxSize.x /= (BezierSurfaceScript.xSize- cutPrecision * 2);
        boxSize.z /= (BezierSurfaceScript.zSize- cutPrecision * 2);

        float cubeArea = boxSize.x * boxSize.z;
        //il volume della parte sottostante alla superficie calcolata per somme di volumi di piccoli "fiammiferi"
        float volume = 0;
        for (int i = 0; i < midPoints.Length; i++)
            volume += cubeArea * (midPoints[i].y - y);
        //la percentuale di occupazione memorizzata
        percentage = (int) (volume * 100 / totalVolume);
        
        Debug.Log("Total Cube Volume: " + totalVolume);
        Debug.Log("Calculated Volume: " + volume);
        Debug.Log("Percentage: " + percentage + " %");
        onDrawSticks(midPoints, BezierSurfaceScript.xSize, BezierSurfaceScript.zSize);
    }
    
    void onDrawSticks(Vector3[] midPoints, int xSize, int zSize) 
    {
        float x = this.transform.position.x - this.transform.localScale.x / 2;
        float y = this.transform.position.y - this.transform.localScale.y / 2;
        float z = this.transform.position.z - this.transform.localScale.z / 2;
        //triangoli per visualizzare un cubo
        int[] triangles = {
            0, 2, 1, //face front
	        0, 3, 2,
            2, 3, 4, //face top
	        2, 4, 5,
            1, 2, 5, //face right
	        1, 5, 6,
            0, 7, 4, //face left
	        0, 4, 3,
            5, 4, 7, //face back
	        5, 7, 6,
            0, 6, 7, //face bottom
	        0, 1, 6
        };


        /*SOLO GRIGLIE 
        int[] triangles =
        {
            0, 2, 3,
            0, 1, 2
        };*/
        v_triangles = (int [])triangles.Clone();
        float newxSize = xSize - cutPrecision * 2;
        float newzSize = zSize - cutPrecision * 2;
        float xStep = (this.transform.localScale.x / newxSize);
        float zStep = (this.transform.localScale.z / newzSize);

        //***SEPARATED
        /*CombineInstance[] combine = new CombineInstance[2*midPoints.Length];
        List<Vector3> points = new List<Vector3>();
        for (int i = 0; i < midPoints.Length; i++)
        {
            float xm = midPoints[i].x;
            float ym = midPoints[i].y;
            float zm = midPoints[i].z;
            points.Add(new Vector3(xm - xStep / 2, ym, zm - zStep / 2));
            points.Add(new Vector3(xm + xStep / 2, ym, zm - zStep / 2));
            points.Add(new Vector3(xm + xStep / 2, ym, zm + zStep / 2));
            points.Add(new Vector3(xm - xStep / 2, ym, zm + zStep / 2));
            Mesh m = new Mesh();
            m.vertices = (Vector3[])points.ToArray().Clone();
            m.triangles = (int[])v_triangles.Clone();
            combine[i].mesh = m;
            combine[i].transform = BezierSurface.transform.localToWorldMatrix;
            points.Clear();

        }

        for (int i = 0; i < midPoints.Length; i++)
        {
            float xm = midPoints[i].x;
            float zm = midPoints[i].z;
            points.Add(new Vector3(xm - xStep / 2, y, zm - zStep / 2));
            points.Add(new Vector3(xm + xStep / 2, y, zm - zStep / 2));
            points.Add(new Vector3(xm + xStep / 2, y, zm + zStep / 2));
            points.Add(new Vector3(xm - xStep / 2, y, zm + zStep / 2));
            Mesh m = new Mesh();
            m.vertices = (Vector3[])points.ToArray().Clone();
            m.triangles = (int[])v_triangles.Clone();
            combine[i].mesh = m;
            combine[i].transform = BezierSurface.transform.localToWorldMatrix;
            points.Clear();

        }*/


        //***TRIAL UNIFIED
        CombineInstance[] combine = new CombineInstance[midPoints.Length/2+1];
        CombineInstance[] combine1 = new CombineInstance[midPoints.Length/2+1];
        List<Vector3> points = new List<Vector3>();
        int index1=0, index2 = 0;
        for (int i = 0; i < midPoints.Length; i++)
        {
            
            float xm = midPoints[i].x;
            float ym = midPoints[i].y;
            float zm = midPoints[i].z;
            points.Add(new Vector3(xm - xStep / 2, y, zm - zStep / 2));
            points.Add(new Vector3(xm + xStep / 2, y, zm - zStep / 2));
            points.Add(new Vector3(xm + xStep / 2, ym, zm - zStep / 2));
            points.Add(new Vector3(xm - xStep / 2, ym, zm - zStep / 2));
            points.Add(new Vector3(xm - xStep / 2, ym, zm + zStep / 2));
            points.Add(new Vector3(xm + xStep / 2, ym, zm + zStep / 2));
            points.Add(new Vector3(xm + xStep / 2, y, zm + zStep / 2));
            points.Add(new Vector3(xm - xStep / 2, y, zm + zStep / 2));

            Mesh m = new Mesh();
            m.vertices = (Vector3[])points.ToArray().Clone();
            m.triangles = (int[])v_triangles.Clone();
            if (i % 2 == 0)
            {
                combine[index1].mesh = m;
                combine[index1].transform = BezierSurface.transform.localToWorldMatrix;
                index1++;


            }
            else
            {
                combine1[index2].mesh = m;
                combine1[index2].transform = BezierSurface.transform.localToWorldMatrix;
                index2++;
            }
            points.Clear();

        }

        volumeMesh = new Mesh();
        volumeMesh.CombineMeshes(combine);
        volumeMesh1 = new Mesh();
        volumeMesh1.CombineMeshes(combine1);
    }
}
