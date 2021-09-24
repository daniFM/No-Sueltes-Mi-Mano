using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour
{
    public Transform handPivot;
    public Transform grabPosition;
    public Transform grabSphere;
    public float speed;
    public float tractionForce;
    public LayerMask grabObjectLayer;

    private Rigidbody rb;
    private bool active;
    private bool grabbing;
    private Collider grabbedObject;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }


    void Update()
    {
        if(active)
        {
            //Debug.Log("MouseX: " + Input.GetAxis("Mouse X") + "\nMouseY: " + Input.GetAxis("Mouse Y"));
            Vector3 force = new Vector3(Input.GetAxis("Mouse X"), 0, Input.GetAxis("Mouse Y")) * speed * Time.deltaTime;
            rb.AddForce(force, ForceMode.Acceleration);

            Vector3 rot = transform.eulerAngles;
            transform.LookAt(handPivot.position, -Vector3.up);
            transform.eulerAngles = new Vector3(rot.x, transform.eulerAngles.y, rot.z);
        }

        if(Input.GetMouseButtonDown(0))
        {
            active = true;
            grabbing = true;
            grabbedObject = GrabObject();
        }
        else if(Input.GetMouseButtonUp(0))
        {
            grabbing = false;

            if(grabbedObject != null)
            {
                grabbedObject.attachedRigidbody.isKinematic = false;
                grabbedObject.attachedRigidbody.velocity = rb.velocity;
                grabbedObject.isTrigger = false;
                grabbedObject = null;
            }
        }

        if(Input.GetMouseButton(0))
        {
            if(grabbedObject != null)
            {
                Rigidbody rbObj = grabbedObject.attachedRigidbody;

                if(!rbObj.isKinematic)
                {
                    Vector3 force = (grabPosition.position - rbObj.transform.position) * tractionForce * Time.deltaTime;
                    rbObj.AddForce(force);
                }
                else
                {
                    grabbedObject.transform.position = grabPosition.transform.position;
                }
            }
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Break();
        }
    }

    private Collider GrabObject()
    {
        Collider [] contacts = Physics.OverlapSphere(grabSphere.position, grabSphere.localScale.x / 2, grabObjectLayer);
        Debug.Log("Overlapped with " + contacts.Length + " grabbable objects");
        if(contacts.Length > 0)
        {
            Collider closerObj = contacts[0];
            for(int i = 0; i < contacts.Length; ++i)
            {
                if(contacts[i].tag == "Hand")
                {
                    GameController.instance.GrabHand();
                    active = false;
                    return null;
                }
                else if(Vector3.Distance(contacts[i].ClosestPoint(grabPosition.position), grabPosition.position) <
                        Vector3.Distance(closerObj.ClosestPoint(grabPosition.position), grabPosition.position))
                {
                    closerObj = contacts[i];
                }
            }
            return closerObj;
        }
        return null;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(grabbing)
        {
            if(collision.gameObject.layer == 7)
            {
                collision.rigidbody.isKinematic = true;
                collision.collider.isTrigger = true;
                //collision.transform.position = grabPosition.position; // se hace en el update
            }
        }
    }
}
