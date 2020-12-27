using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turnable : MonoBehaviour
{
    private Vector3 posLastFrame;
    private Transform objTransform;

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
}
