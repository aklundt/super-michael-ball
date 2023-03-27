using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class movementController : MonoBehaviour
{

    float horizontalInput;
    float verticalInput;
    public GameObject player;
    public GameObject gravityController;
    private Transform gravityTarget;
    public float tiltDegreeLimit;
    public float stabilizationFactor;

    // start is called before the first frame update
    void Start()
    {
        gravityTarget = transform.GetChild(0);
    }

    // update is called once per frame
    void Update()
    {
        checkInput();
        updateGravity();
    }

    // reads horizontal and vertical inputs and update values.
    void checkInput() {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
    }
    void updateGravity() {
        updateGravityController();
        restrictGravityController();
        Physics.gravity = gravityTarget.position - transform.position;
        stabilizeGravityController();
    }
    void updateGravityController() {
        transform.position = player.transform.position + new Vector3(0, 2, 0);
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x - verticalInput, transform.localEulerAngles.y, transform.localEulerAngles.z + horizontalInput);
    }

    void restrictGravityController() {
        float coterminalTiltLimit = 360 - tiltDegreeLimit;

        if (transform.localEulerAngles.x > tiltDegreeLimit && transform.localEulerAngles.x < 180) { 
            transform.localEulerAngles = new Vector3(tiltDegreeLimit, transform.localEulerAngles.y, transform.localEulerAngles.z); 
        }
        if (transform.localEulerAngles.x < coterminalTiltLimit && transform.localEulerAngles.x > 180) { 
            transform.localEulerAngles = new Vector3(coterminalTiltLimit, transform.localEulerAngles.y, transform.localEulerAngles.z); 
        }
        if (transform.localEulerAngles.z > tiltDegreeLimit && transform.localEulerAngles.z < 180) { 
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, tiltDegreeLimit); 
        }
        if (transform.localEulerAngles.z < coterminalTiltLimit && transform.localEulerAngles.z > 180) { 
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, coterminalTiltLimit); 
        }
    }

    void stabilizeGravityController() {
        float x = transform.localEulerAngles.x;
        x = x / 360;
        if (x > 0.5) { x -= 1; }
        float y = transform.localEulerAngles.y;
        //y = y / 360;
        //if (y > 0.5) { y -= 1; }
        float z = transform.localEulerAngles.z;
        z = z / 360;
        if (z > 0.5) { z -= 1; }
        transform.localEulerAngles = stabilizationFactor * new Vector3(x, 0, z) * 360;
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, y, transform.localEulerAngles.z);
    }
}
