using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeAdjust : MonoBehaviour
{
    public GameObject BezierSurface;
    private MeshGenerator BezierSurfaceScript;
    private Bounds b;
    public bool drawSticks = false;
    public int percentage;
    // Start is called before the first frame update
    void Start()
    {
        BezierSurfaceScript = BezierSurface.GetComponent<MeshGenerator>();
        StartCoroutine(waiter());
    }

    IEnumerator waiter()
    {
        yield return new WaitForSecondsRealtime(0.01f);
        b = BezierSurfaceScript.mesh.bounds;
        transform.position = b.center;
        transform.localScale = b.size;
        CalculateVolume();

    }

    void CalculateVolume()
    {
        // questi sono i vertici in cui viene valutata la superficie
        Vector3[] midPoints = BezierSurfaceScript.midPoints;
        //le dimensioni del parallelepipedo
        Vector3 boxSize = GetComponent<Renderer>().bounds.size;
        //l'altezza della base del parallelepipedo
        float y = this.transform.position.y - this.transform.localScale.y / 2;
        //il volume del parallelepipedo calcolato come il prodotto delle 3 dimensioni
        float totalVolume = boxSize.x * boxSize.y * boxSize.z;
        //l'area dei piccoli cubi della griglia alla base
        boxSize.x /= BezierSurfaceScript.xSize;
        boxSize.z /= BezierSurfaceScript.zSize;
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
        if(drawSticks)  onDrawSticks(midPoints, BezierSurfaceScript.xSize, BezierSurfaceScript.zSize);
    }
    
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

    // Update is called once per frame
    void Update()
    {
        /*
        b = BezierSurfaceScript.mesh.bounds;
        transform.position = b.center;
        transform.localScale = b.size;
        Debug.Log(b);*/
    }
}
