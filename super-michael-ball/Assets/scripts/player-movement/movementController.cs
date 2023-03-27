using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class movementController : MonoBehaviour
{

    float horizontalInput;
    float verticalInput;
    public GameObject gravityController;
    private Transform gravityTarget;
    public float tiltDegreeLimit;
    public float stabilizationFactor;

    // start is called before the first frame update
    void Start()
    {
        gravityTarget = gravityController.transform.GetChild(0);
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
        Physics.gravity = gravityTarget.position - gravityController.transform.position;
        stabilizeGravityController();
    }
    void updateGravityController() {
        gravityController.transform.position = transform.position + new Vector3(0, 2, 0);
        gravityController.transform.localEulerAngles = new Vector3(gravityController.transform.localEulerAngles.x - verticalInput, gravityController.transform.localEulerAngles.y, gravityController.transform.localEulerAngles.z + horizontalInput);
    }

    void restrictGravityController() {
        float coterminalTiltLimit = 360 - tiltDegreeLimit;

        if (gravityController.transform.localEulerAngles.x > tiltDegreeLimit && gravityController.transform.localEulerAngles.x < 180) { 
            gravityController.transform.localEulerAngles = new Vector3(tiltDegreeLimit, gravityController.transform.localEulerAngles.y, gravityController.transform.localEulerAngles.z); 
        }
        if (gravityController.transform.localEulerAngles.x < coterminalTiltLimit && gravityController.transform.localEulerAngles.x > 180) { 
            gravityController.transform.localEulerAngles = new Vector3(coterminalTiltLimit, gravityController.transform.localEulerAngles.y, gravityController.transform.localEulerAngles.z); 
        }
        if (gravityController.transform.localEulerAngles.z > tiltDegreeLimit && gravityController.transform.localEulerAngles.z < 180) { 
            gravityController.transform.localEulerAngles = new Vector3(gravityController.transform.localEulerAngles.x, gravityController.transform.localEulerAngles.y, tiltDegreeLimit); 
        }
        if (gravityController.transform.localEulerAngles.z < coterminalTiltLimit && gravityController.transform.localEulerAngles.z > 180) { 
            gravityController.transform.localEulerAngles = new Vector3(gravityController.transform.localEulerAngles.x, gravityController.transform.localEulerAngles.y, coterminalTiltLimit); 
        }
    }

    void stabilizeGravityController() {
        float x = gravityController.transform.localEulerAngles.x;
        x = x / 360;
        if (x > 0.5) { x -= 1; }
        float y = gravityController.transform.localEulerAngles.y;
        y = y / 360;
        if (y > 0.5) { y -= 1; }
        float z = gravityController.transform.localEulerAngles.z;
        z = z / 360;
        if (z > 0.5) { z -= 1; }
        gravityController.transform.localEulerAngles = stabilizationFactor * new Vector3(x, y, z) * 360;
    }
}
