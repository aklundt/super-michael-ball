using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class genericMovement : MonoBehaviour
{

    enum axis { X,Y,Z }

    float primaryHorizontalInput;
    float primaryVerticalInput;
    float secondaryHorizontalInput;
    float secondaryVerticalInput;
    public GameObject player;
    private Transform camera;
    private Transform gravityTarget;
    public float gravityTiltLimit;
    public float cameraTiltLimit;
    public float gravityStabilizationFactor;
    public float cameraStabilizationFactor;

    // start is called before the first frame update
    void Start()
    {
        gravityTarget = transform.GetChild(0);
        camera = transform.GetChild(1).GetChild(0);
    }

    // update is called once per frame
    void Update()
    {
        checkInput();
        updateGravity();
        updateCamera();
    }

    private void LateUpdate()
    {
        restrictRotation(camera.gameObject, cameraTiltLimit, axis.X);
        stabilizeRotation(camera.gameObject, cameraStabilizationFactor, axis.X);
    }

    // reads horizontal and vertical inputs and update values.
    void checkInput() {
        primaryHorizontalInput = Input.GetAxis("Primary Horizontal");
        primaryVerticalInput = Input.GetAxis("Primary Vertical");
        secondaryHorizontalInput = Input.GetAxis("Secondary Horizontal");
        secondaryVerticalInput = Input.GetAxis("Secondary Vertical");
    }
    void updateGravity() {
        updateGravityController();
        restrictRotation(this.gameObject, gravityTiltLimit, axis.X);
        restrictRotation(this.gameObject, gravityTiltLimit, axis.Z);
        Physics.gravity = gravityTarget.position - transform.position;
        stabilizeGravityController();
        stabilizeRotation(this.gameObject, gravityStabilizationFactor, axis.X);
        stabilizeRotation(this.gameObject, gravityStabilizationFactor, axis.Z);
    }

    void updateCamera() {
        camera.localEulerAngles = new Vector3(camera.localEulerAngles.x + secondaryVerticalInput, camera.localEulerAngles.y, camera.localEulerAngles.z);

    }
    void updateGravityController() {
        transform.position = player.transform.position + new Vector3(0, 2, 0);
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x - primaryVerticalInput, transform.localEulerAngles.y + secondaryHorizontalInput * 1.5f, transform.localEulerAngles.z + primaryHorizontalInput);
    }

    void restrictRotation(GameObject restrictedObject, float degreeLimit, axis axis) {
        float coterminalDegreeLimit = 360 - degreeLimit;
        if (axis == axis.X)
        {
            if (restrictedObject.transform.localEulerAngles.x > degreeLimit && restrictedObject.transform.localEulerAngles.x < 180)
            {
                restrictedObject.transform.localEulerAngles = new Vector3(degreeLimit, restrictedObject.transform.localEulerAngles.y, restrictedObject.transform.localEulerAngles.z);
            }
            if (restrictedObject.transform.localEulerAngles.x < coterminalDegreeLimit && restrictedObject.transform.localEulerAngles.x > 180)
            {
                restrictedObject.transform.localEulerAngles = new Vector3(coterminalDegreeLimit, restrictedObject.transform.localEulerAngles.y, restrictedObject.transform.localEulerAngles.z);
            }
        }
        else if (axis == axis.Y)
        {
            if (restrictedObject.transform.localEulerAngles.y > degreeLimit && restrictedObject.transform.localEulerAngles.y < 180)
            {
                restrictedObject.transform.localEulerAngles = new Vector3(restrictedObject.transform.localEulerAngles.x, degreeLimit, restrictedObject.transform.localEulerAngles.z);
            }
            if (restrictedObject.transform.localEulerAngles.y < coterminalDegreeLimit && restrictedObject.transform.localEulerAngles.y > 180)
            {
                restrictedObject.transform.localEulerAngles = new Vector3(restrictedObject.transform.localEulerAngles.x, coterminalDegreeLimit, restrictedObject.transform.localEulerAngles.z);
            }
        }
        else if (axis == axis.Z) {
            if (restrictedObject.transform.localEulerAngles.z > degreeLimit && restrictedObject.transform.localEulerAngles.z < 180)
            {
                restrictedObject.transform.localEulerAngles = new Vector3(restrictedObject.transform.localEulerAngles.x, restrictedObject.transform.localEulerAngles.y, degreeLimit);
            }
            if (restrictedObject.transform.localEulerAngles.z < coterminalDegreeLimit && restrictedObject.transform.localEulerAngles.z > 180)
            {
                restrictedObject.transform.localEulerAngles = new Vector3(restrictedObject.transform.localEulerAngles.x, restrictedObject.transform.localEulerAngles.y, coterminalDegreeLimit);
            }
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
        transform.localEulerAngles = gravityStabilizationFactor * new Vector3(x, 0, z) * 360;
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, y, transform.localEulerAngles.z);
    }

    void stabilizeRotation(GameObject stabilizedObject, float degreeLimit, axis axis) {
        float x = stabilizedObject.transform.localEulerAngles.x;
        float y = stabilizedObject.transform.localEulerAngles.y;
        float z = stabilizedObject.transform.localEulerAngles.z;
        if (axis == axis.X)
        {
            x = x / 360;
            if (x > 0.5) { x -= 1; }
            stabilizedObject.transform.localEulerAngles = gravityStabilizationFactor * new Vector3(x, 0, 0) * 360;
            stabilizedObject.transform.localEulerAngles = new Vector3(stabilizedObject.transform.localEulerAngles.x, y, z);
        }
        else if (axis == axis.Y)
        {
            y = y / 360;
            if (y > 0.5) { y -= 1; }
            stabilizedObject.transform.localEulerAngles = gravityStabilizationFactor * new Vector3(0, y, 0) * 360;
            stabilizedObject.transform.localEulerAngles = new Vector3(x, stabilizedObject.transform.localEulerAngles.y, z);
        }
        else if (axis == axis.Z) {
            z = z / 360;
            if (z > 0.5) { z -= 1; }
            stabilizedObject.transform.localEulerAngles = gravityStabilizationFactor * new Vector3(0, 0, z) * 360;
            stabilizedObject.transform.localEulerAngles = new Vector3(x, y, stabilizedObject.transform.localEulerAngles.z);
        }
    }

}
