using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetVolumeVisualizer : MonoBehaviour
{
    public GameObject cube;
    private CubeAdjust cubeScript;

    public bool visible = false;
    private Mesh m;
    public int[] triangles;
    public Vector3[] vertices;

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
        yield return new WaitForSecondsRealtime(0.03f);
        m =cubeScript.volumeMesh;
        if (visible) GetComponent<MeshFilter>().mesh = m;
        vertices = m.vertices;
        triangles = m.triangles;
    }

    void Update()
    {
        if (visible) GetComponent<MeshFilter>().mesh = m;
        else GetComponent<MeshFilter>().mesh = new Mesh();
    }
}
