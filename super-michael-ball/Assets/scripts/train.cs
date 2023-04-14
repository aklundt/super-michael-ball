using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class train : MonoBehaviour
{

    public gameManager gameManager;
    public GameObject cameraObj;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        gameManager.movementEnabled = false;
        cameraObj.transform.position = Vector3.Slerp(cameraObj.transform.position, new Vector3(-152.5f, -11.4f, 37.41f), 0.05f);
    }
}
