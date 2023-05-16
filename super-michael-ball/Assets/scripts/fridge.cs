using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class fridge : MonoBehaviour
{
    public gameManager gameManager;

    Animator animator;
    GameObject cameraOBJ;
    GameObject player;
    VideoPlayer doorTransition;

    bool triggered;

    public GameObject destination;
    public GameObject target;
    public GameObject bodyPart;
    public int doorNumberToUnlock;

    // Start is called before the first frame update
    void Start()
    {
        player = gameManager.player;
        animator = this.GetComponent<Animator>();
        cameraOBJ = gameManager.cameraOBJ;
        doorTransition = gameManager.GetComponent<gameManager>().doorTransition;
    }

    // Update is called once per frame
    void Update()
    {
        checkProximity();
    }

    // if player approaches fridge, play opening animation
    private void checkProximity() {
        if (Vector3.Distance(player.transform.position, transform.position) < 20) {

            animator.Play("fridgeOpen");
        }
    }

    // on collision, zoom in on freezer and wait to return to lobby
    private void OnCollisionEnter(Collision collision)
    {
        if (!triggered) {
            gameManager.levelFinished = true;
            triggered = true;
            gameManager.movementEnabled = false;
            gameManager.erasePlayerForces();
            StartCoroutine(speedUpBodyPartSpin());
            StartCoroutine(cameraOBJ.GetComponent<cameraMovement>().moveCameraTo(destination, 0.05f));
            StartCoroutine(cameraOBJ.GetComponent<cameraMovement>().rotateTo(target, 0.05f));
            StartCoroutine(waitAndReturnToLobby());
        }
    }
    IEnumerator speedUpBodyPartSpin() {
        while (true) {
            bodyPart.GetComponent<spin>().spinSpeed *= (1 + Time.deltaTime);
            yield return null;
        }
        
    }

    

    // update game info and return to lobby
    IEnumerator waitAndReturnToLobby() {
        
        yield return new WaitForSeconds(4);
        doorTransition.Play();
        yield return new WaitForSeconds(0.5f);
        while (doorTransition.isPlaying) { 
            yield return null;
        }
        if (PlayerPrefs.GetInt("gameState") < doorNumberToUnlock) {
            PlayerPrefs.SetInt("gameState", doorNumberToUnlock);
        }
        // if time is the first time
        // or greater than last time
        // or checkpoints were enabled last time and disabled this time
        // update time
        if (PlayerPrefs.GetFloat("level" + (doorNumberToUnlock - 1) + "Time") == 0 || 
            gameManager.levelTimer < PlayerPrefs.GetFloat("level" + (doorNumberToUnlock - 1) + "Time") || 
            PlayerPrefs.GetInt("level" + (doorNumberToUnlock - 1) + "CheckpointsEnabled") == 1 && gameManager.checkpointsEnabled == false) {
            PlayerPrefs.SetFloat("level" + (doorNumberToUnlock - 1) + "Time", gameManager.levelTimer);
        }
        PlayerPrefs.SetInt("level" + (doorNumberToUnlock - 1) + "CheckpointsEnabled", (gameManager.checkpointsEnabled ? 1 : 0));
        if (doorNumberToUnlock == 2) { PlayerPrefs.SetString("checkpointsToggle", "disabled"); }

        SceneManager.LoadScene("DreamLobby");
    }
}
