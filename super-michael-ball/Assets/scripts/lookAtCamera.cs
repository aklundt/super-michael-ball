using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lookAtCamera : MonoBehaviour
{
    public GameObject cameraOBJ;
    public float xRotateOffset;
    public float yRotateOffset;
    public float zRotateOffset;

    // Start is called before the first frame update
    void Start()
    {
        if (cameraOBJ == null) {
            cameraOBJ = GameObject.Find("main-camera");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        transform.LookAt(cameraOBJ.transform);
        transform.Rotate(Vector3.right, xRotateOffset, Space.Self);
        transform.Rotate(Vector3.up, yRotateOffset, Space.Self);
        transform.Rotate(Vector3.forward, zRotateOffset, Space.Self);
    }
}
