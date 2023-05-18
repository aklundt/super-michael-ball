using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class trainCutscene : MonoBehaviour
{

    public gameManager gameManager;

    GameObject player;
    GameObject cameraObj;
    VideoPlayer doorTransition;

    public GameObject cameraDestination;
    public GameObject trainObj;
    public GameObject twinklePrefab;
    public DialogueObject footStuckLines;
    bool lookAtPlayer;
    bool cutScenePlayedOnce;

    // Start is called before the first frame update
    void Start()
    {
        player = gameManager.player;
        cameraObj = gameManager.cameraOBJ;
        doorTransition = gameManager.doorTransition;
        //sceneOngoing = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (lookAtPlayer) {
           cameraObj.transform.LookAt(player.transform, Vector3.up);
           if (player.transform.position.x > -174) { player.transform.position = new Vector3(-174, player.transform.position.y, player.transform.position.z); }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!cutScenePlayedOnce) {
            trainObj.SetActive(true);
            cutScenePlayedOnce = true;
            PlayerPrefs.SetInt("gameState", 1);
            gameManager.GetComponent<narratorDialogue>().Run(footStuckLines);
            gameManager.movementEnabled = false;
            gameManager.erasePlayerForces();
            if (player.transform.position.x > -174)
            {
                player.transform.position = new Vector3(-174, player.transform.position.y, player.transform.position.z);

            }
            StartCoroutine(cameraObj.GetComponent<cameraMovement>().moveCameraTo(cameraDestination, 0.05f));
            StartCoroutine(cameraObj.GetComponent<cameraMovement>().rotateTo(player, 0.05f));
            StartCoroutine(startStuffAfterDialogueFinished());
        }
        
    }

    private IEnumerator startStuffAfterDialogueFinished() {
        while (gameManager.textBoxOngoing) {        
            yield return null;
        }
        lookAtPlayer = true;
        gameManager.levelFinished = true;
        yield return new WaitForSeconds(.5f);
        StartCoroutine(trainMoving());
        StartCoroutine(endCutscene());
        yield break;
    }

    private IEnumerator trainMoving() {
        player.GetComponent<Rigidbody>().velocity = Vector3.back * .2f;
        while (lookAtPlayer) {
            trainObj.transform.Translate(0, 0, 50 * Time.deltaTime);
            yield return null;
        }
        yield return null;
    }
    private IEnumerator endCutscene() {
        while (lookAtPlayer)
        {
            yield return new WaitForSeconds(2);
            GameObject twinkleObj = Instantiate(twinklePrefab, player.transform.position, Quaternion.Euler(0, 0, 0));
            player.SetActive(false);
            lookAtPlayer = false;
            yield return new WaitForSeconds(2);
        }
        doorTransition.Play();
        yield return new WaitForSeconds(1);
        while (doorTransition.isPlaying) {
            yield return null;
        }
        SceneManager.LoadScene("DreamLobby");
    }
}
