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

    private void checkProximity() {
        if (Vector3.Distance(player.transform.position, transform.position) < 20) {

            animator.Play("fridgeOpen");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!triggered) {
            triggered = true;
            gameManager.movementEnabled = false;
            gameManager.erasePlayerForces();
            StartCoroutine(cameraOBJ.GetComponent<cameraMovement>().moveCameraTo(destination, 0.03f));
            StartCoroutine(cameraOBJ.GetComponent<cameraMovement>().rotateTo(target, 0.03f));
            StartCoroutine(waitAndReturnToLobby());
        }
    }

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
        
        SceneManager.LoadScene("DreamLobby");
    }
}
