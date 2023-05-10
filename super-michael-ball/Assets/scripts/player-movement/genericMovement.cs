using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;

public class genericMovement : MonoBehaviour
{

    enum axis { X,Y,Z }

    float primaryHorizontalInput;
    float primaryVerticalInput;
    float secondaryHorizontalInput;
    float secondaryVerticalInput;
    bool xboxA;
    bool xboxRightBumper;
    public float gravityTiltLimit;
    public float cameraTiltLimit;
    public float gravStabilizationFactor;
    public float cameraStabilizationFactor;

    public GameObject player;
    public gameManager gameManager;

    private Transform cameraObj;
    private Transform gravityTarget;

    // start is called before the first frame update
    void Start()
    {
        gravityTarget = transform.GetChild(0);
        cameraObj = transform.GetChild(1).GetChild(0);
        
    }

    // update is called once per frame
    void Update()
    {
        grabInput();
        applyInput();
    }


    // reads horizontal and vertical inputs and update values.
    void grabInput() {
        primaryHorizontalInput = gameManager.primaryHorizontalInput;
        primaryVerticalInput = gameManager.primaryVerticalInput;
        secondaryHorizontalInput = gameManager.secondaryHorizontalInput;
        secondaryVerticalInput = gameManager.secondaryVerticalInput;
        xboxA = Input.GetButtonDown("XboxA");
        xboxRightBumper = Input.GetButton("XboxRightBumper");
    }
    
    // updates gravity-controller and camera with input
    // restricts and stabilizes rotation
    // updates physics gravity to correspond with gravity-controller
    void applyInput() {
        stabilizeRotation(this.gameObject, axis.X, gravStabilizationFactor);
        stabilizeRotation(this.gameObject, axis.Z, gravStabilizationFactor);
        

        transform.position = player.transform.position + new Vector3(0, 2, 0);
        // if movement is enabled update gameobjects
        if (gameManager.movementEnabled)
        {
            stabilizeRotation(cameraObj.gameObject, axis.X, cameraStabilizationFactor);

            // update gravity-controller
            Vector3 gravControllerRotation = new Vector3(
                -primaryVerticalInput * Time.deltaTime * 700, 
                secondaryHorizontalInput, 
                primaryHorizontalInput * Time.deltaTime * 700
                );
            gravControllerRotation = transform.localEulerAngles + gravControllerRotation;
            gravControllerRotation = new Vector3(
                Mathf.Clamp(toNegativeDegrees(gravControllerRotation.x),-gravityTiltLimit, gravityTiltLimit), 
                gravControllerRotation.y, 
                Mathf.Clamp(toNegativeDegrees(gravControllerRotation.z), -gravityTiltLimit, gravityTiltLimit)
                );
            transform.localEulerAngles = gravControllerRotation;

            // update camera
            Vector3 cameraRotation = new Vector3(
                secondaryVerticalInput, 
                0, 
                0
                );
            cameraRotation = cameraObj.localEulerAngles + cameraRotation;
            cameraRotation = new Vector3(
                Mathf.Clamp(toNegativeDegrees(cameraRotation.x), -cameraTiltLimit, cameraTiltLimit),
                cameraRotation.y,
                cameraRotation.z
                );
            cameraObj.localEulerAngles = cameraRotation;
            
        }
        // update physics
        Physics.gravity = gravityTarget.position - transform.position;

        // reset player position when shortcut is pressed
        if (xboxRightBumper && xboxA)
        {
            player.transform.position = new Vector3 (0, 1, 0);
            player.transform.rotation = new Quaternion(0, 0, 0, 1);
            player.GetComponent<Rigidbody>().velocity = Vector3.zero;
            player.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            transform.rotation = new Quaternion(0, 0, 0, 1);
        }

    }
    // uses a factor to gradually rotate an object back towards 0.
    void stabilizeRotation(GameObject obj, axis axis, float factor) {
        if (axis == axis.X) 
        {
            float x = toNegativeDegrees(obj.transform.localEulerAngles.x);
            x = x - (x * factor * Time.deltaTime);
            obj.transform.localEulerAngles = new Vector3(
                x, 
                obj.transform.localEulerAngles.y, 
                obj.transform.localEulerAngles.z
                );
        }
        else if (axis == axis.Y)
        {
            float y = toNegativeDegrees(obj.transform.localEulerAngles.y);
            y = y - (y * factor * Time.deltaTime);
            obj.transform.localEulerAngles = new Vector3(
                obj.transform.localEulerAngles.x,
                y,
                obj.transform.localEulerAngles.z
                );
        }
        else if (axis == axis.Z)
        {
            float z = toNegativeDegrees(obj.transform.localEulerAngles.z);
            z = z - (z * factor * Time.deltaTime);
            obj.transform.localEulerAngles = new Vector3(
                obj.transform.localEulerAngles.x,
                obj.transform.localEulerAngles.y,
                z
                );
        }
    }

    // unity likes to use degrees in the range 0 to 360. this function will try and return a range of -180 to 180.
    // there is a flaw in unity however that makes the x axis, and ONLY the x axis not cooperate past 90 degrees. it is very weird.
    // if it MUST be fixed, then we can start storing the x axis rotation in this script rather than using requesting unitys.
    float toNegativeDegrees(float degree) {
        degree /= 360;
        if (degree > 0.5) { degree -= 1; }
        degree *= 360;
        return degree;
    }
}
