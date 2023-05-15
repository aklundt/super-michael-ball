using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class NPCController : MonoBehaviour
{
    //public TextMeshProUGUI dialogueBoxTMP;
    //public TextMeshProUGUI dialogueCharacterName;
    public DialogueObject dialogueLines;
    public gameManager gameManager;
    narratorDialogue narratorDialogue;
    public float talkDistance;
    RawImage aIcon;
    //dialogueTyper dialogueTyper;
    GameObject player;
    GameObject gravityController;
    VideoPlayer textBox;
    VideoClip textBoxOpen;
    VideoClip textBoxClose;
    public GameObject dialogueBox;

    // Start is called before the first frame update
    void Start()
    {
        player = gameManager.player;
        gravityController = gameManager.gravityController;
        textBox = gameManager.textBox;
        textBoxOpen = gameManager.textBoxOpen;
        textBoxClose = gameManager.textBoxClose;
        narratorDialogue = gameManager.gameObject.GetComponent<narratorDialogue>();
        aIcon = narratorDialogue.aIcon;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && (player.transform.position - transform.position).magnitude <= talkDistance && !gameManager.textBoxOngoing || 
            Input.GetButtonDown("XboxA") && (player.transform.position - transform.position).magnitude <= talkDistance && !gameManager.textBoxOngoing)
        {
            Debug.Log("presseed");
            gameManager.GetComponent<narratorDialogue>().Run(dialogueLines);
            //dialogueTyper = gameManager.GetComponent<dialogueTyper>();
            pauseMovementAndLookAtNPC(); 
            //StartCoroutine(narratorDialogue.StepThroughDialogue(dialogueLines));
            StartCoroutine(waitToEnableMovement());
        }
    }


    private void pauseMovementAndLookAtNPC() // I will beat Grayson into making pretty little animations. Maybe we should move these to a different script
    {
        player.GetComponent<Rigidbody>().velocity = Vector3.zero;
        player.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        StartCoroutine(alignCameraToNPC());
    }

    private IEnumerator waitToEnableMovement() {
        while (gameManager.textBoxOngoing) { 
            yield return null;
        }
        player.GetComponent<Rigidbody>().velocity = Vector3.zero;
        player.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
    }

    private float rotationToNPC ()
    {
        float rotationToNPC = (float)Math.Round(Mathf.Atan((transform.position.x - player.transform.position.x) / (transform.position.z - player.transform.position.z)), 7) * (180 / Mathf.PI);
        if (transform.position.z < player.transform.position.z)
        {
            rotationToNPC -= 180;
        }
        if (rotationToNPC < -180)
        {
            rotationToNPC += 360;
        }
        return rotationToNPC;
    }

    private IEnumerator alignCameraToNPC ()
    {
        while (Math.Round(rotationToNPC()) != Math.Round(UnityEditor.TransformUtils.GetInspectorRotation(gravityController.transform).y))
        {
            gravityController.transform.rotation = Quaternion.Slerp(gravityController.transform.rotation, Quaternion.Euler(gravityController.transform.rotation.x, rotationToNPC(), gravityController.transform.rotation.y), 0.05f); // LOOK AT THE NUMBER
            yield return new WaitForFixedUpdate();
        }
        gameManager.movementEnabled = true;
        yield break;
        
    }

}
