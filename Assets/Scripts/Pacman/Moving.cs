using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moving : MonoBehaviour
{
    private float _speed = 0.5f;
    void Start()
    {
        
    }

    void FixedUpdate()
    {
        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");
        inputX *= Time.deltaTime * _speed;
        inputY *= Time.deltaTime * _speed;

        transform.position = new Vector2(transform.position.x + inputX, transform.position.y + inputY);
        //transform.Translate(inputX, inputY, 0);
    }
}
