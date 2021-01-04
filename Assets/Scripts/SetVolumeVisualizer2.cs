using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetVolumeVisualizer2 : MonoBehaviour
{
    public GameObject cube;
    private CubeAdjust cubeScript;
    public bool visible = false;
    Mesh m;
    public int[] t;
    public Vector3[] v;

    // Start is called before the first frame update
    void Start()
    {
        cubeScript = cube.GetComponent<CubeAdjust>();
        StartCoroutine(waiter());
    }

    public void SetVisible(bool v)
    {
        visible = v;
    }

    IEnumerator waiter()
    {
        yield return new WaitForSecondsRealtime(0.1f);
        m = cubeScript.volumeMesh1;
        if (visible) GetComponent<MeshFilter>().mesh = m;
        v = m.vertices;
        t = m.triangles;
        //aggiorna la mesh
        /*m.Clear();
        m.vertices = cubeScript.v_vertices;
        m.triangles = cubeScript.v_triangles;
        
        m.RecalculateNormals();*/
    }

    // Update is called once per frame
    void Update()
    {
        if (visible) GetComponent<MeshFilter>().mesh = m;
        else GetComponent<MeshFilter>().mesh = new Mesh();
    }
}
