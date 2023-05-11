using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Unity.VisualScripting;
using UnityEditor.EventSystems;
using UnityEngine;

public class boxTrigger : MonoBehaviour
{
    public gameManager gameManager;

    GameObject cameraOBJ;
    cameraMovement cameraMovement;
    deathPlane deathPlane;

    public GameObject box;
    public GameObject[] distractions;
    public GameObject cameraDestination;
    public GameObject cameraTarget;

    public GameObject cameraTeleportDestination;
    public GameObject cameraTeleportTarget;

    int initialDeathCount;

    public bool triggered;
    // Start is called before the first frame update
    void Start()
    {
        cameraOBJ = gameManager.cameraOBJ;
        cameraMovement = cameraOBJ.GetComponent<cameraMovement>();
        deathPlane = gameManager.deathPlane.GetComponent<deathPlane>();
    }

    // Update is called once per frame
    void Update()
    {
        // if the trigger was triggered, check how many times player dies until they die twice
        // then start second part of cutscene and move player
        if (triggered) {
            if (deathPlane.deaths > initialDeathCount + 1) {
                if (gameManager.gravityController.transform.position.y > -240) {
                    gameManager.movementEnabled = false;
                    gameManager.erasePlayerForces();
                    StartCoroutine(cameraMovement.moveCameraTo(cameraDestination, 0.005f));
                    StartCoroutine(cameraMovement.rotateTo(cameraTarget, 0.005f));
                    StartCoroutine(teleportToEnd());
                    triggered = false;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // if triggered, freeze player, reveal horrific level, and start counting deaths in Update()
        if (!triggered)
        {
            gameManager.resetPosition = new Vector3(-527, -223, 189);
            gameManager.resetRotationY = -90;
            initialDeathCount = deathPlane.deaths;
            triggered = true;
            gameManager.movementEnabled = false;
            gameManager.erasePlayerForces();
            foreach (GameObject distraction in distractions)
            {
                distraction.GetComponent<spin>().active = true;
            }
            StartCoroutine(moveBox());
        }
    }

    // lifts box off of level
    IEnumerator moveBox() {
        StartCoroutine(cameraMovement.moveCameraTo(cameraDestination, 0.005f));
        StartCoroutine(cameraMovement.rotateTo(cameraTarget, 0.005f));
        while (box.transform.position.y < 100)
        {
            box.transform.Translate(Vector3.up * 100 * Time.deltaTime, Space.World);
            yield return null;
        }

        // I WANT TO PUT A TEXT BOX HERE SAYING "GOOD LUCK! :)" THAT THE GAME DOESNT CONTINUE UNTIL ITS CLOSED

        StartCoroutine(cameraMovement.resetCamera(0.05f));
        gameManager.movementEnabled = true;
    }

    // teleports player to the end
    IEnumerator teleportToEnd() { 
        yield return new WaitForSeconds(3);
        StartCoroutine(cameraMovement.moveCameraTo(cameraTeleportDestination, 0.01f));
        StartCoroutine(cameraMovement.rotateTo(cameraTeleportTarget, 0.01f));

        // I WANT TO REPLACE THIS WAIT HERE WITH TEXT SAYING "YEAH OKAY YOU'RE NOT CUT OUT FOR THIS" THAT THE GAME DOESNT CONTINUE UNTIL ITS CLOSED
        yield return new WaitForSeconds(3);


        gameManager.teleportPlayerTo(cameraTeleportTarget.transform.position + new Vector3(0, 2, 0), new Vector3(0, -90, 0));
        gameManager.resetPosition = cameraTeleportTarget.transform.position + new Vector3(0, 2, 0);
        yield return new WaitForSeconds(2);
        gameManager.gravityController.transform.position = gameManager.player.transform.position + new Vector3(0, 2, 0);
        gameManager.gravityController.transform.rotation = Quaternion.Euler(0, -90, 0);
        cameraOBJ.transform.position = cameraTeleportDestination.transform.position;
        gameManager.movementEnabled = true;
        StartCoroutine(cameraMovement.resetCamera(0.05f));
        
        
        
        

    }
}
