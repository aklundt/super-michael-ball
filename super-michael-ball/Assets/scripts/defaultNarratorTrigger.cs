using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class defaultNarratorTrigger : MonoBehaviour
{

    public gameManager gameManager;
    public DialogueObject dialogueLines;
    public GameObject destination;
    public GameObject target;
    public bool oneTimeTrigger;
    public string triggerName;
    cameraMovement cameraMovement;

    bool triggered;
    // Start is called before the first frame update
    void Start()
    {
        cameraMovement = gameManager.cameraOBJ.GetComponent<cameraMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // if triggered move camera and start dialogue
    private void OnTriggerEnter(Collider other)
    {
        if (!triggered) {
            triggered = true;
            if (oneTimeTrigger && PlayerPrefs.GetInt(triggerName + "Triggered") == 1) {
                return;
            }
            PlayerPrefs.SetInt(triggerName + "Triggered", 1);
            gameManager.movementEnabled = false;
            gameManager.erasePlayerForces();
            gameManager.player.GetComponent<Rigidbody>().drag = 5;
            StartCoroutine(cameraMovement.moveCameraTo(destination, 0.05f));
            StartCoroutine(cameraMovement.rotateTo(target, 0.05f));
            gameManager.GetComponent<narratorDialogue>().Run(dialogueLines);
            StartCoroutine(waitForDialogueEnd());
        }
        
    }
    // after dialogue ends, move camera back and resume game
    private IEnumerator waitForDialogueEnd() {
        while (gameManager.textBoxOngoing)
        {
            yield return null;
        }
        StartCoroutine(cameraMovement.resetCamera(0.05f));
        gameManager.player.GetComponent<Rigidbody>().drag = 0.5f;
        gameManager.movementEnabled = true;
    }
}
