using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeAdjust : MonoBehaviour
{

    //linking con gli altri script
    public GameObject BezierSurface;
    private MeshGenerator BezierSurfaceScript;
    private Bounds b;
    private Bounds bc;

    //Percentuale di volume riempita
    public int percentage;

    //La mesh del volume usata dal visualizzatore del volume
    public Mesh volumeMesh;

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
        //turnable.AdjustPosition();
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
        CalculateSticks(midPoints, BezierSurfaceScript.xSize, BezierSurfaceScript.zSize);
        //BezierSurface.GetComponent<MeshGenerator>().CalculateSticks();
    }
    
    void CalculateSticks(Vector3[] midPoints, int xSize, int zSize) 
    {
        //Y della base del cubo
        float y = this.transform.position.y - this.transform.localScale.y / 2;
        //indici dei triangoli necessari per visualizzare un singolo stick
        int[] triangles = {
            0, 2, 1, 0, 3, 2,//face front
            2, 3, 4, 2, 4, 5, //face top
            1, 2, 5, 1, 5, 6,//face right
            0, 7, 4, 0, 4, 3,//face left
            5, 4, 7, 5, 7, 6,//face back
            0, 6, 7, 0, 1, 6//face bottom
        };
        //le dimensioni del taglio
        float newxSize = xSize - cutPrecision * 2;
        float newzSize = zSize - cutPrecision * 2;
        //le dimensioni orizzontali dello stick
        float xStep = (this.transform.localScale.x / newxSize);
        float zStep = (this.transform.localScale.z / newzSize);
        //combine serve per unire insieme più mesh
        CombineInstance[] combine = new CombineInstance[midPoints.Length];
        List<Vector3> points = new List<Vector3>();

        for (int i = 0; i < midPoints.Length; i++)
        {
                //la posizione del midpoint che stiamo considerando
                float xm = midPoints[i].x;
                float ym = midPoints[i].y;
                float zm = midPoints[i].z;
                //aggiungo gli 8 vertici dello stick
                points.Add(new Vector3(xm - xStep / 2, y, zm - zStep / 2));
                points.Add(new Vector3(xm + xStep / 2, y, zm - zStep / 2));
                points.Add(new Vector3(xm + xStep / 2, ym, zm - zStep / 2));
                points.Add(new Vector3(xm - xStep / 2, ym, zm - zStep / 2));
                points.Add(new Vector3(xm - xStep / 2, ym, zm + zStep / 2));
                points.Add(new Vector3(xm + xStep / 2, ym, zm + zStep / 2));
                points.Add(new Vector3(xm + xStep / 2, y, zm + zStep / 2));
                points.Add(new Vector3(xm - xStep / 2, y, zm + zStep / 2));
                //creo la mesh dello stick 
                Mesh m = new Mesh();
                m.vertices = (Vector3[])points.ToArray().Clone();
                m.triangles = (int[])triangles.Clone();
                //la aggiungo alle mesh che dovranno essere unite
                combine[i].mesh = m;
                combine[i].transform = BezierSurface.transform.localToWorldMatrix;
                points.Clear();

        }
        //unisco le mesh di tutti gli stick
        volumeMesh = new Mesh();
        volumeMesh.CombineMeshes(combine);
    }

    public void SetEasy()
    {
        margin = Margin.Easy;
    }

    public void SetMedium()
    {
        margin = Margin.Medium;
    }

    public void SetHard()
    {
        margin = Margin.Hard;
    }

    public void SetImpossible()
    {
        margin = Margin.Impossible;
    }
}
