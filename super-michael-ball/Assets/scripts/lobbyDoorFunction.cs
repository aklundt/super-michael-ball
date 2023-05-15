using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using TMPro;

public class lobbyDoorFunction : MonoBehaviour
{

    public gameManager gameManager;

    GameObject cameraOBJ;
    GameObject player;
    VideoPlayer doorTransition;
    TextMeshPro time;
    GameObject checkpoint;

    public string sceneDestination;
    public GameObject cameraEmpty;
    public int doorNumber;

    // Start is called before the first frame update
    void Start()
    {
        cameraOBJ = gameManager.GetComponent<gameManager>().cameraOBJ;
        player = gameManager.GetComponent<gameManager>().player;
        doorTransition = gameManager.GetComponent<gameManager>().doorTransition;
        time = transform.Find("time").gameObject.GetComponent<TextMeshPro>();
        checkpoint = transform.Find("checkpoint").gameObject;
        time.text = gameManager.toReadableTime(PlayerPrefs.GetFloat("level" + doorNumber + "Time"));
        if (PlayerPrefs.GetInt("level" + doorNumber + "CheckpointsEnabled") == 1) { 
            checkpoint.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // if door should be unlocked...... unlock it
        if (doorNumber <= gameManager.GetComponent<gameManager>().gameStatus)
        {
            gameObject.GetComponent<MeshCollider>().isTrigger = true;
            gameObject.GetComponent<MeshRenderer>().material = gameManager.GetComponent<gameManager>().glowingWhite;
            transform.Find("Point Light").gameObject.SetActive(true);
        }
    }

    // if touched, enter level
    private void OnTriggerEnter(Collider other)
    {
          gameManager.GetComponent<gameManager>().movementEnabled = false;
          StartCoroutine(cameraOBJ.GetComponent<cameraMovement>().moveCameraTo(cameraEmpty, 0.05f));
          StartCoroutine(cameraOBJ.GetComponent<cameraMovement>().rotateTo(transform.gameObject, 0.05f));
          StartCoroutine(disablePlayer());
          StartCoroutine(doorEnter());
        
    }

    // kind of unnecessary now, but makes player disappear even if they are visible to make it feel more like "entering" a level
    private IEnumerator disablePlayer() {
        float time = 0;
        while (time < 1) {
            time += Time.deltaTime;
            yield return null;
        }
        player.SetActive(false);
        yield return null;
    }

    // move camera to the door and start moving camera forward
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

    // transition and load level
    private IEnumerator transitionVideoAndSceneChange() {
        doorTransition.Play();
        yield return new WaitForSeconds(0.5f);
        while (doorTransition.isPlaying) {
            yield return null;
        }
        SceneManager.LoadScene(sceneDestination);
    }
    

}
