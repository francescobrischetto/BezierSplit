using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    float _xAxisClamp = 0;

    [SerializeField] private Transform _playerTransform;
    [SerializeField] private float _mouseSensitivity = 3f;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void FixedUpdate()
    {
        // Camera rotation input
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        float rotationX = mouseX * _mouseSensitivity;
        float rotationY = mouseY * _mouseSensitivity;

        _xAxisClamp -= rotationY;

        Vector3 playerRotation = _playerTransform.rotation.eulerAngles;

        playerRotation.x -= rotationY;
        playerRotation.y += rotationX;
        playerRotation.z = 0;

        if(_xAxisClamp > 90)
        {
            _xAxisClamp = 90;
            playerRotation.x = 90;
        }
        else if (_xAxisClamp < -90)
        {
            _xAxisClamp = -90;
            playerRotation.x = 270;
        }

        _playerTransform.rotation = Quaternion.Euler(playerRotation);

    }

    public void SetSensitivity(float value)
    {
        _mouseSensitivity = value;
    }
}
