using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lobbyDoorFunction : MonoBehaviour
{

    public gameManager gameManager;

    GameObject cameraOBJ;

    public GameObject cameraEmpty;

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
        gameManager.GetComponent<gameManager>().movementEnabled = false;
        StartCoroutine(cameraOBJ.GetComponent<cameraMovement>().moveCameraTo(cameraEmpty, 0.03f));
        StartCoroutine(cameraOBJ.GetComponent<cameraMovement>().rotateTo(transform.gameObject, 0.03f));
        StartCoroutine(doorEnter());
    }

    private IEnumerator doorEnter() {
        yield return new WaitForSeconds(3);
        while (true) { 
            cameraOBJ.transform.Translate(Vector3.forward * 10 * Time.deltaTime, Space.Self);
            yield return null;
        }
        yield return null;
    }

}
