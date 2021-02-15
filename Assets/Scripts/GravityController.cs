using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityController : MonoBehaviour
{
    public ShipController ship;
    public float gravityScale = 3f;

    void Start()
    {
        Physics2D.gravity = Vector2.zero;
    }

    void Update()
    {
        Physics2D.gravity = Vector2.down * ship.throttle * gravityScale;
    }
}
