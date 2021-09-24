using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour
{
    public Transform grabPosition;
    public float speed = 100;
    public float tractionForce;

    private Rigidbody rb;
    private bool active;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }


    void Update()
    {
        if(active)
        {
            Debug.Log("MouseX: " + Input.GetAxis("Mouse X") + "\nMouseY: " + Input.GetAxis("Mouse Y"));
            Vector3 force = new Vector3(Input.GetAxis("Mouse X"), 0, Input.GetAxis("Mouse Y")) * speed * Time.deltaTime;
            rb.AddForce(force);
        }

        if(Input.GetMouseButtonDown(0))
        {
            active = !active;
        }
    }
}
