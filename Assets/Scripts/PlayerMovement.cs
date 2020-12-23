using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _speed = 1f;
    private Vector3 _moveDirection;
    private Transform _playerTransform;

    private void Start()
    {
        _playerTransform = GetComponent<Transform>();
        _moveDirection = new Vector3(0, 0, 0);
    }

    private void Update()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        _moveDirection = new Vector3(moveX, 0, moveZ);
        _moveDirection = transform.TransformDirection(_moveDirection);
        _moveDirection *= _speed;
    }

    private void FixedUpdate()
    {
        _playerTransform.position += _moveDirection * Time.deltaTime;
    }
}
