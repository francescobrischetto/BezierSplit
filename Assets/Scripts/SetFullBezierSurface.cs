using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetFullBezierSurface : MonoBehaviour
{
    public GameObject BezierSurface;
    private MeshGenerator BezierSurfaceScript;
    public bool visible = false;
    Mesh m;
    // Start is called before the first frame update
    void Start()
    {
        BezierSurfaceScript = BezierSurface.GetComponent<MeshGenerator>();
        StartCoroutine(waiter());
    }
    public void SetVisible(bool v)
    {
        visible = v;
    }
    IEnumerator waiter()
    {
        yield return new WaitForSecondsRealtime(0.01f);
        m  = new Mesh();
        if (visible) GetComponent<MeshFilter>().mesh = m;
        //aggiorna la mesh
        m.Clear();
        m.vertices = BezierSurfaceScript.b_vertices;
        m.triangles = BezierSurfaceScript.b_triangles;
        m.RecalculateNormals();
    }

        // Update is called once per frame
        void Update()
    {
        if (visible) GetComponent<MeshFilter>().mesh = m;
        else GetComponent<MeshFilter>().mesh = new Mesh();
    }
}
