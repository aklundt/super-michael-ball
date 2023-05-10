using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class lobbyDoorFunction : MonoBehaviour
{

    public gameManager gameManager;

    GameObject cameraOBJ;
    GameObject player;
    VideoPlayer whiteTransition;

    //public String sceneDestination;
    public GameObject cameraEmpty;
    public int doorNumber;

    // Start is called before the first frame update
    void Start()
    {
        cameraOBJ = gameManager.GetComponent<gameManager>().cameraOBJ;
        player = gameManager.GetComponent<gameManager>().player;
        whiteTransition = gameManager.GetComponent<gameManager>().whiteTransition;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (doorNumber >= gameManager.GetComponent<gameManager>().gameStatus) {
            gameManager.GetComponent<gameManager>().movementEnabled = false;
            StartCoroutine(cameraOBJ.GetComponent<cameraMovement>().moveCameraTo(cameraEmpty, 0.03f));
            StartCoroutine(cameraOBJ.GetComponent<cameraMovement>().rotateTo(transform.gameObject, 0.03f));
            StartCoroutine(disablePlayer());
            StartCoroutine(doorEnter());
        }
        
    }

    private IEnumerator disablePlayer() {
        float time = 0;
        while (time < 1) {

            time += Time.deltaTime;
            yield return null;
        }
        player.SetActive(false);
        yield return null;
    }

    private IEnumerator doorEnter() {
        yield return new WaitForSeconds(3);
        Vector3 destination = cameraOBJ.transform.position + cameraOBJ.transform.forward * 10;
        StartCoroutine(transitionVideoAndSceneChange());
        while (cameraOBJ.transform.position != destination) { 
            cameraOBJ.transform.position = Vector3.Slerp(cameraOBJ.transform.position, destination, 0.001f);
            yield return null;
        }
        yield return null;
    }

    private IEnumerator transitionVideoAndSceneChange() {
        yield return new WaitForSeconds(1);
        Debug.Log("should be playing");
        whiteTransition.Play();
        yield return new WaitForSeconds(0.5f);
        while (whiteTransition.isPlaying) {
            yield return null;
        }
        //SceneManager.LoadScene(sceneDestination);
    }
    

}
