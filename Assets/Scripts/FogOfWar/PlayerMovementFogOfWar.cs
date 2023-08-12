using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementFogOfWar : MonoBehaviour
{
    public float moveSpeed = 6f;
    Rigidbody rb;
    Camera viewCamera;
    Vector3 velocity;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        viewCamera = Camera.main;
    }

    void Update()
    {
        
        //Vector3 mousePos = viewCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, viewCamera.transform.position.y));
        //Vector3 mousePos = viewCamera.ScreenToWorldPoint(Input.mousePosition);
        //transform.LookAt(mousePos + Vector3.up * transform.position.y);
        velocity = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized * moveSpeed;
    }
    private void FixedUpdate() {
        rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
    }
}
