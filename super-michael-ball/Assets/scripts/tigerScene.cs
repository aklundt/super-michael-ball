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
    // Start is called before the first frame update
    void Start()
    {
        playerRb = player.GetComponent<Rigidbody>();
        playerRb.AddForce(new Vector3(0, -3, 5) * 5, ForceMode.Impulse);
        gameManager.movementEnabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        StartCoroutine(introduceTiger());
    }

    private void OnTriggerExit(Collider other)
    {
        StartCoroutine(turnCameraAround());
        gameManager.movementEnabled = true;
    }

    private IEnumerator introduceTiger() {
        while (tiger.transform.position.z < -16) {
            tiger.transform.position = new Vector3(tiger.transform.position.x, tiger.transform.position.y, tiger.transform.position.z + 1 * Time.deltaTime);
            yield return null;
        }
        yield return null;
    }

    private IEnumerator turnCameraAround() {
        Debug.Log(UnityEditor.TransformUtils.GetInspectorRotation(gravityController.transform).y);
        Transform cameraHolder = gravityController.transform.GetChild(1);
        while (UnityEditor.TransformUtils.GetInspectorRotation(gravityController.transform).y != 0)
        {
            gravityController.transform.rotation = Quaternion.Slerp(gravityController.transform.rotation, Quaternion.Euler(gravityController.transform.rotation.x, 0, gravityController.transform.rotation.z), 0.05f);
            cameraHolder.position = Vector3.Slerp(cameraHolder.position, player.transform.position + new Vector3(0, 4.5f, -7), 0.05f);
            cameraHolder.rotation = Quaternion.Slerp(cameraHolder.rotation, Quaternion.Euler(30, 0, 0), 0.05f);
            yield return null;
        }
        yield return null;
    }
}
