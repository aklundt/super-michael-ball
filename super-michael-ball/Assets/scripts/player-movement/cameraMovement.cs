using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraMovement : MonoBehaviour
{

    public gameManager gameManager;

    GameObject cameraOBJ;

    private float slerpSpeed = 0.05f;
    // Start is called before the first frame update
    void Start()
    {
        cameraOBJ = gameManager.GetComponent<gameManager>().cameraOBJ;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator moveCameraTo(GameObject destination) {
        while (transform.position != destination.transform.position) {
            transform.position = Vector3.Slerp(transform.position, destination.transform.position, slerpSpeed);
            yield return null;
        }
        yield return null;
    }
}
