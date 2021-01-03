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

    //Boolean per disegnare gli sticks
    public bool drawSticks = false;

    //Percentuale di volume riempita
    public int percentage;

    //Numero di scarto del taglio della superficie
    public int cutPrecision = 15;

    //dimensione del parallelepipedo
    public float dimension = 1.2f;

    //min-max Range per aumento di difficoltà
    public float minRange = 0.2f;
    public float maxRange = 0.2f;

    //Turnable per centrare la visuale
    public Turnable turnable;
    
    void Start()
    {
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
        //Riposiziono il cubo 
        //TODO: modificare questo in una scelta di difficoltà di livello
        transform.position = bc.center + new Vector3(0,Random.Range(-minRange,maxRange),0);
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
        //TODO:FIXARLO
        if(drawSticks)  onDrawSticks(midPoints, BezierSurfaceScript.xSize, BezierSurfaceScript.zSize);
    }
    
    //TODO: FIXARLO
    void onDrawSticks(Vector3[] midPoints, int xSize, int zSize) 
    {
        float x = this.transform.position.x - this.transform.localScale.x / 2;
        float y = this.transform.position.y - this.transform.localScale.y / 2;
        float z = this.transform.position.z - this.transform.localScale.z / 2;
        LineRenderer lr = gameObject.AddComponent<LineRenderer>();
        lr.material = new Material(Shader.Find("Sprites/Default"));
        Vector3[] positions = new Vector3[2*5*(xSize)*(zSize)];

        float xStep = transform.localScale.x / xSize;
        float zStep = transform.localScale.z / zSize;
        int index = 0;
        //griglia di base
        for ( int j = 1; j <= xSize; j++)
        {
            for(int i=1; i<= zSize; i++)
            {
                float newx = x + i * xStep;
                float newz = z + j * zStep;
                positions[index] = new Vector3(newx-xStep,y, newz - zStep);
                index++;
                positions[index] = new Vector3(newx, y, newz - zStep);
                index++;
                positions[index] = new Vector3(newx, y, newz);
                index++;
                positions[index] = new Vector3(newx - xStep, y, newz);
                index++;
                positions[index] = new Vector3(newx - xStep, y, newz - zStep);
                index++;
            }
        }
        //griglia di piano
        int altroindex = 0;
        for (int j = 1; j <= xSize; j++)
        {
            for (int i = 1; i <= zSize; i++)
            {
                float newx = x + i * xStep;
                float newz = z + j * zStep;
                positions[index] = new Vector3(newx - xStep, midPoints[altroindex].y, newz - zStep);
                index++;
                positions[index] = new Vector3(newx, midPoints[altroindex].y, newz - zStep);
                index++;
                positions[index] = new Vector3(newx, midPoints[altroindex].y, newz);
                index++;
                positions[index] = new Vector3(newx - xStep, midPoints[altroindex].y, newz);
                index++;
                positions[index] = new Vector3(newx - xStep, midPoints[altroindex].y, newz - zStep);
                index++;
                altroindex++;
            }
        }
        lr.positionCount = positions.Length;
        lr.SetPositions(positions);
        lr.startWidth = 0.01f;
    }
}
