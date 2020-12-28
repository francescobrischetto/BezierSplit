using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetBackBezier : MonoBehaviour
{
    public GameObject BezierSurface;
    private MeshGenerator BezierSurfaceScript;

    // Start is called before the first frame update
    void Start()
    {
        BezierSurfaceScript = BezierSurface.GetComponent<MeshGenerator>();
        StartCoroutine(waiter());
    }

    IEnumerator waiter()
    {
        yield return new WaitForSecondsRealtime(0.01f);
        Mesh mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        //aggiorna la mesh
        mesh.Clear();
        Vector3[] vertices = (Vector3[]) BezierSurfaceScript.mesh.vertices.Clone();
        for(int i=0; i < vertices.Length; i++)
        {
            vertices[i] = vertices[i] + new Vector3(0, 0.013f, 0);
        }
        mesh.vertices = vertices;
        mesh.triangles = BezierSurfaceScript.mesh.triangles;

        //mesh.RecalculateNormals();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
