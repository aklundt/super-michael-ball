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
    private Transform cameraObj;
    private Transform gravityTarget;
    public float gravityTiltLimit;
    public float cameraTiltLimit;
    public float gravityStabilizationFactor;
    public float cameraStabilizationFactor;

    // start is called before the first frame update
    void Start()
    {
        gravityTarget = transform.GetChild(0);
        cameraObj = transform.GetChild(1).GetChild(0);
    }

    // update is called once per frame
    void Update()
    {
        checkInput();
        updateGravityController();
        updateCamera();
    }

    private void LateUpdate()
    {
        // clean up camera movement
        restrictRotation(cameraObj.gameObject, cameraTiltLimit, axis.X);
        stabilizeRotation(cameraObj.gameObject, cameraStabilizationFactor, axis.X);
    }

    // reads horizontal and vertical inputs and update values.
    void checkInput() {
        primaryHorizontalInput = Input.GetAxis("Horizontal");
        primaryVerticalInput = Input.GetAxis("Vertical");
        secondaryHorizontalInput = Input.GetAxis("Secondary Horizontal");
        secondaryVerticalInput = Input.GetAxis("Secondary Vertical");
    }

    // rotates gravity-controller (or this.) object in response to primaryAxisInputs and secondaryHorizontalInput.
    // restricts gravity-controller's axis rotations, updates gravity, and stabilizes the object.
    void updateGravityController()
    {
        transform.position = player.transform.position + new Vector3(0, 2, 0);
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x - primaryVerticalInput, transform.localEulerAngles.y + secondaryHorizontalInput * 1.5f, transform.localEulerAngles.z + primaryHorizontalInput);
        restrictRotation(this.gameObject, gravityTiltLimit, axis.X);
        restrictRotation(this.gameObject, gravityTiltLimit, axis.Z);
        Physics.gravity = gravityTarget.position - transform.position;
        stabilizeRotation(this.gameObject, gravityStabilizationFactor, axis.X);
        stabilizeRotation(this.gameObject, gravityStabilizationFactor, axis.Z);
    }
    // rotates camera in response to secondaryVerticalInput.
    // method is unnecessary because it's one line of code but it looks nicer.
    void updateCamera() {
        cameraObj.localEulerAngles = new Vector3(cameraObj.localEulerAngles.x + secondaryVerticalInput, cameraObj.localEulerAngles.y, cameraObj.localEulerAngles.z);
    }
    
    // reusable function to restrict an objects rotation in one axis by changing the rotation to the desired limit when the rotation surpasses it.
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

    // reusable function to stabilize an objects rotation in one axis by mulitplying the rotation by a decimal factor (cameraStabilizationFactor).
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
