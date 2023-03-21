﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Turnable : MonoBehaviour
{
    private Vector3 posLastFrame;

    [SerializeField] private Transform cubeTransform;
    [SerializeField] private Transform surface;

    private void Update()
    {

        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
            posLastFrame = Input.mousePosition;

        if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            Vector3 delta = Input.mousePosition - posLastFrame;
            posLastFrame = Input.mousePosition;

            Vector3 axis = Quaternion.AngleAxis(-90f, Vector3.forward) * delta;
            //rotationPivot.rotation = Quaternion.AngleAxis(delta.magnitude * 0.1f, axis) * rotationPivot.rotation;
            surface.RotateAround(cubeTransform.position, axis, delta.magnitude * 0.1f);
        }
    }

    public void AdjustPosition()
    {
        surface.position = new Vector3(surface.position.x - cubeTransform.position.x,
            surface.position.y - cubeTransform.position.y,
            surface.position.z - cubeTransform.position.z);
    }
}
