using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Video;

public class NPCController : MonoBehaviour
{
    public TextMeshProUGUI dialogueBoxTMP;
    public TextMeshProUGUI dialogueCharacterName;
    public DialogueObject dialogueLines;
    public gameManager gameManager;
    dialogueTyper dialogueTyper;
    GameObject player;
    GameObject gravityController;
    bool xboxA;
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
        //dialogueBox 
    }

    // Update is called once per frame
    void Update()
    {
        xboxA = Input.GetButtonDown("XboxA");
        if (Input.GetKeyDown(KeyCode.Return) && (player.transform.position - transform.position).magnitude <= 6 && !gameManager.textBoxOngoing || Input.GetButtonDown("XboxA") && (player.transform.position - transform.position).magnitude <= 6 && !gameManager.textBoxOngoing)
        {
            openDialogueBox(); 
            textBox.clip = textBoxOpen;
            textBox.Play();
            dialogueTyper = gameManager.GetComponent<dialogueTyper>();
            StartCoroutine(StepThroughDialogue(dialogueLines));
        }
    }


    private void openDialogueBox () // I will beat Grayson into making pretty little animations. Maybe we should move these to a different script
    {
        player.GetComponent<Rigidbody>().velocity = Vector3.zero;
        player.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        gameManager.textBoxOngoing = true;
        dialogueCharacterName.text = dialogueLines.ActorName;
        StartCoroutine(alignCameraToNPC());
    }

    private void closeDialogueBox() // ^ last comment
    {
        gameManager.textBoxOngoing = false;
        textBox.clip = textBoxClose;
        textBox.Play();
        dialogueBoxTMP.text = "";
        gameManager.GetComponent<gameManager>().textBoxOngoing = false;
        player.GetComponent<Rigidbody>().velocity = Vector3.zero;
        player.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        dialogueBoxTMP.text = "";
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

    private IEnumerator StepThroughDialogue(DialogueObject dialogueObject)
    {
        yield return new WaitForSeconds(1);
        foreach (string dialogue in dialogueObject.Dialogue)
        {
            yield return dialogueTyper.Run(dialogue, dialogueBoxTMP);
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return) || gameManager.xboxADown); 
        }
        closeDialogueBox();
        yield break;
    }
}
