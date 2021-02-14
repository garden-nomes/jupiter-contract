using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityController : MonoBehaviour
{
    void Start()
    {
        Physics2D.gravity = Vector2.zero;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.G))
        {
            Physics2D.gravity = Vector2.down * 2f;
        }
        else
        {
            Physics2D.gravity = Vector2.zero;
        }
    }
}
