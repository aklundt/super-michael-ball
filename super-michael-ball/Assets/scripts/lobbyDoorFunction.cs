using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lobbyDoorFunction : MonoBehaviour
{

    public gameManager gameManager;

    GameObject cameraOBJ;
    
    public GameObject camera

    // Start is called before the first frame update
    void Start()
    {
        cameraOBJ = gameManager.GetComponent<gameManager>().cameraOBJ;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        StartCoroutine(cameraOBJ.GetComponent<cameraMovement>().moveCameraTo());
    }
}
