using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeAdjust : MonoBehaviour
{
    public GameObject BezierSurface;
    private MeshGenerator BezierSurfaceScript;
    private Bounds b;

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
        Vector3[] vertices = BezierSurfaceScript.vertices;
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
        for (int i = 0; i < vertices.Length; i++)
            volume += cubeArea * (vertices[i].y - y);
        //la percentuale di occupazione memorizzata
        percentage = (int) (volume * 100 / totalVolume);
        
        Debug.Log("Total Cube Volume: " + totalVolume);
        Debug.Log("Calculated Volume: " + volume);
        Debug.Log("Percentage: " + percentage + " %");
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
