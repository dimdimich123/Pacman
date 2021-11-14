using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moving : MonoBehaviour
{
    private float _speed = 0.5f;
    private int _horizontalDirection = 1;
    private int _verticalDirection = 0;
    private bool _runing = true;
    [SerializeField]
    private BoxCollider2D _leftCollider, _rightCollider, _topCollider, _downCollider;
    void Start()
    {
        
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.A))
        {
            //if (!_leftCollider.IsTouchingLayers())
            {
                _horizontalDirection = -1;
                _verticalDirection = 0;
                _runing = true;
            }
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            //if (!_rightCollider.IsTouchingLayers())
            {
                _horizontalDirection = 1;
                _verticalDirection = 0;
                _runing = true;
            }
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            //if (!_topCollider.IsTouchingLayers())
            {
                _horizontalDirection = 0;
                _verticalDirection = 1;
                _runing = true;
            }
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            //if (!_downCollider.IsTouchingLayers())
            {
                _horizontalDirection = 0;
                _verticalDirection = -1;
                _runing = true;
            }
        }

        if(_runing)
            transform.position = new Vector2(transform.position.x + _horizontalDirection * Time.deltaTime * _speed, transform.position.y + _verticalDirection * Time.deltaTime * _speed);

    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if(_horizontalDirection == 1 && _verticalDirection == 0 && _rightCollider.IsTouchingLayers())
    //    {
    //        _runing = false;
    //    } else if (_horizontalDirection == -1 && _verticalDirection == 0 && _leftCollider.IsTouchingLayers())
    //    {
    //        _runing = false;
    //    }
    //    else if (_horizontalDirection == 0 && _verticalDirection == 1 && _topCollider.IsTouchingLayers())
    //    {
    //        _runing = false;
    //    }
    //    else if (_horizontalDirection == 0 && _verticalDirection == -1 && _downCollider.IsTouchingLayers())
    //    {
    //        _runing = false;
    //    }
    //}


    private void OnCollisionEnter2D(Collision2D collision)
    {
        _runing = false;
        transform.position = new Vector2(transform.position.x - _horizontalDirection * Time.deltaTime, transform.position.y - _verticalDirection * Time.deltaTime);
    }
}
