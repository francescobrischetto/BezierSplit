using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turnable : MonoBehaviour
{
    private Vector3 posLastFrame;
    private Transform objTransform;

    public GameObject cube;

    [SerializeField] private Transform bezierTransform;
    [SerializeField] private Transform cubeTransform;

    private void Awake()
    {
        objTransform = GetComponent<Transform>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            posLastFrame = Input.mousePosition;

        if (Input.GetMouseButton(0))
        {
            Vector3 delta = Input.mousePosition - posLastFrame;
            posLastFrame = Input.mousePosition;

            Vector3 axis = Quaternion.AngleAxis(-90f, Vector3.forward) * delta;
            objTransform.rotation = Quaternion.AngleAxis(delta.magnitude * 0.1f, axis) * objTransform.rotation;
        }
    }

    public void AdjustPosition()
    {
        //TODO: Fixare il centro della scena
        bezierTransform.position = new Vector3(bezierTransform.position.x - cubeTransform.localScale.x/2 , 
             bezierTransform.position.y - cubeTransform.localScale.y/2 ,
             bezierTransform.position.z - cubeTransform.localScale.z/2 );
        //(Possibile Hint che non funziona però) : bezierTransform.position = cube.GetComponent<MeshFilter>().mesh.bounds.center;
    }
}
