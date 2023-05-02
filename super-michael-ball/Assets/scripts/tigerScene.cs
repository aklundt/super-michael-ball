using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class tigerScene : MonoBehaviour
{
    public GameObject player;
    public GameObject gravityController;
    private Rigidbody playerRb;
    public gameManager gameManager;
    public GameObject tiger;
    private float tigerEndCutsceneZPos = -26;
    // Start is called before the first frame update
    void Start()
    {
        playerRb = player.GetComponent<Rigidbody>();
        playerRb.AddForce(new Vector3(0, -3, 2) * 15, ForceMode.Impulse);
        playerRb.AddTorque(new Vector3(10, 0, 0), ForceMode.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        StartCoroutine(introduceTiger());
        StartCoroutine(turnCameraSlowlyToTiger());
    }

    private void OnTriggerExit(Collider other)
    {
        StartCoroutine(turnCameraAround());
        StartCoroutine(resetCameraPosition());
        StartCoroutine(resetCameraXRotation());
        //StopCoroutine(turnCameraAround());
        //StopCoroutine(resetCameraPosition());
        //StopCoroutine(resetCameraXRotation());
        StartCoroutine(clearAllCoroutines(1.5f));
        
    }

    private IEnumerator introduceTiger() {
        while (tiger.transform.position.z < tigerEndCutsceneZPos) {
            tiger.transform.position = new Vector3(tiger.transform.position.x, tiger.transform.position.y, tiger.transform.position.z + 1 * Time.deltaTime);
            yield return null;
        }
        yield return null;
    }

    private IEnumerator turnCameraAround() {
        
        while (UnityEditor.TransformUtils.GetInspectorRotation(gravityController.transform).y != 0)
        {
            gravityController.transform.rotation = Quaternion.Slerp(gravityController.transform.rotation, Quaternion.Euler(gravityController.transform.rotation.x, 0, gravityController.transform.rotation.z), 0.005f);

            
            yield return null;
        }
        yield return null;
    }

    private IEnumerator resetCameraPosition() {
        Transform cameraHolder = gravityController.transform.GetChild(1);
        Vector3 newPos = player.transform.position + new Vector3(0, 10, -7);
        while (cameraHolder.position != newPos) {
            //Debug.Log("onging position");
            newPos = player.transform.position + new Vector3(0, 10, -7);
            cameraHolder.position = Vector3.Slerp(cameraHolder.position, newPos, 0.005f);
            yield return null;
        }
        yield return null;
    }
    private IEnumerator resetCameraXRotation()
    {
        Transform cameraHolder = gravityController.transform.GetChild(1);
        Quaternion newRot = Quaternion.Euler(41, 0, 0);
        while (cameraHolder.rotation != newRot)
        {
            //Debug.Log("onging X rotation");
            cameraHolder.rotation = Quaternion.Slerp(cameraHolder.rotation, newRot, 0.005f);
            yield return null;
        }
        yield return null;
    }

    private IEnumerator turnCameraSlowlyToTiger() {
        
        Transform cameraHolder = gravityController.transform.GetChild(1);
        while (tiger.transform.position.z < tigerEndCutsceneZPos) {
            cameraHolder.transform.LookAt(tiger.transform);
            yield return null;
        }
        
            yield return null;
    }

    private IEnumerator clearAllCoroutines(float time) { 
        yield return new WaitForSeconds(time);
        forceCameraSettings();
        gameManager.movementEnabled = true;

        StopAllCoroutines();
    }

    private void forceCameraSettings() {
        Transform cameraHolder = gravityController.transform.GetChild(1);
        cameraHolder.rotation = Quaternion.Euler(41, 0, 0);
        cameraHolder.position = player.transform.position + new Vector3(0, 10, -7);
    }
}
