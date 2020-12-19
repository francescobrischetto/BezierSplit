using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeAdjust : MonoBehaviour
{
    public GameObject BezierSurface;
    private MeshGenerator BezierSurfaceScript;
    private Bounds b;
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
