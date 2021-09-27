using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HandController : MonoBehaviour
{
    public Transform handPivot;
    public Transform grabPosition;
    public Transform grabSphere;
    public Transform extension;
    public float extensionMultiplier;
    public float speed;
    public float tractionForce;
    public float shake;
    public LayerMask grabObjectLayer;
    public Animator anim;
    public HandSound sound;

    private Rigidbody rb;
    private bool active = true;
    private bool canMove;
    private bool grabbing;
    private Collider grabbedObject;

    IEnumerator Start()
    {
        rb = GetComponent<Rigidbody>();

        yield return new WaitForSeconds(1);

        canMove = true;
    }

    private void OnEnable()
    {
        Cursor.visible = false;
    }

    private void OnDisable()
    {
        Cursor.visible = true;
    }

    void Update()
    {
        if(!active)
            return;

        if(canMove)
        {
            //Debug.Log("MouseX: " + Input.GetAxis("Mouse X") + "\nMouseY: " + Input.GetAxis("Mouse Y"));
            Vector3 shakeForce = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)) * shake;
            Vector3 force = new Vector3(Input.GetAxis("Mouse X"), 0, Input.GetAxis("Mouse Y")) * speed * Time.deltaTime;
            force = transform.InverseTransformDirection(-force);
            force += shakeForce;
            rb.AddForce(force, ForceMode.Acceleration);

            Vector3 rot = transform.eulerAngles;
            transform.LookAt(handPivot.position, -Vector3.up);
            transform.eulerAngles = new Vector3(rot.x, transform.eulerAngles.y, rot.z);

            //extension.position = handPivot.position;
            extension.localScale = new Vector3(extension.localScale.x, Vector3.Distance(transform.position, handPivot.position) * extensionMultiplier, extension.localScale.z);
        }

        if(Input.GetMouseButtonDown(0))
        {
            if(GameManager.instance != null && !GameManager.instance.pauseMenu.paused)
                Cursor.lockState = CursorLockMode.Locked;

            canMove = true;
            grabbing = true;
            grabbedObject = GrabObject();
            anim.SetTrigger("SwitchGrip");
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
            anim.SetTrigger("SwitchGrip");
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
                    Debug.Log("Grab hand");
                    StartCoroutine(GrabHandRoutine());
                    return null;
                }
                else if (contacts[i].tag == "Phone")
                {
                    StartCoroutine(GrabPhoneRoutine());
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

    private IEnumerator GrabHandRoutine()
    {
        canMove = false;
        active = false;
        transform.DOMoveY(transform.position.y - 0.6f, 1);
        yield return new WaitForSeconds(1);
        sound.source.spatialBlend = 0;
        sound.source.volume = 0.6f;
        sound.PlaySound(HandSound.Sound.hand);
        GameController.instance.GrabHand();
    }

    private IEnumerator GrabPhoneRoutine()
    {
        canMove = false;
        active = false;
        transform.DOMoveY(0.75f, 1);
        yield return new WaitForSeconds(1);
        GameController.instance.GrabPhone();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(grabbing)
        {
            if(collision.gameObject.layer == 7)
            {
                // GRAB OBJECT
                collision.rigidbody.isKinematic = true;
                collision.collider.isTrigger = true;
                //collision.transform.position = grabPosition.position; // se hace en el update
            }
        }

        sound.source.spatialBlend = 1;
        sound.source.volume = 1;
        sound.PlaySound(HandSound.Sound.contact);
    }
}
