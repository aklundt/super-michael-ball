using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    public GameObject player;
    public GameObject gravityController;
    public GameObject dialogueBox;
    public TextMeshProUGUI dialogueBoxTMP;
    public TextMeshProUGUI dialogueCharacterName;
    public DialogueObject dialogueLines;
    private dialogueTyper dialogueTyper;
    public gameManager gameManager;
    private bool xboxA; 

    // Start is called before the first frame update
    void Start()
    {
        dialogueTyper = gameManager.GetComponent<dialogueTyper>();
    }

    // Update is called once per frame
    void Update()
    {
        xboxA = Input.GetButtonDown("XboxA");
        if (Input.GetKeyDown(KeyCode.Return) && (player.transform.position - transform.position).magnitude <= 6 && !gameManager.NPCTalking || Input.GetButtonDown("XboxA") && (player.transform.position - transform.position).magnitude <= 6 && !gameManager.NPCTalking)
        {
            openDialogueBox();
            StartCoroutine(StepThroughDialogue(dialogueLines));
        }
    }


    private void openDialogueBox () // I will beat Grayson into making pretty little animations. Maybe we should move these to a different script
    {
        player.GetComponent<Rigidbody>().drag = 50;
        gameManager.NPCTalking = true;
        dialogueBox.SetActive(true);
        dialogueCharacterName.text = dialogueLines.ActorName;
        StartCoroutine(alignCameraToNPC());
    }

    private void closeDialogueBox() // ^ last comment
    {
        gameManager.GetComponent<gameManager>().NPCTalking = false;
        player.GetComponent<Rigidbody>().drag = .5f;
        dialogueBoxTMP.text = "";
        dialogueBox.SetActive(false);
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
            yield return gravityController.transform.rotation = Quaternion.Slerp(gravityController.transform.rotation, Quaternion.Euler(gravityController.transform.rotation.x, rotationToNPC(), gravityController.transform.rotation.y), 0.05f);
        }
        player.GetComponent<Rigidbody>().drag = 0.5f;
        gameManager.movementEnabled = true;
        yield break;
        
    }

    private IEnumerator StepThroughDialogue(DialogueObject dialogueObject)
    {
        foreach (string dialogue in dialogueObject.Dialogue)
        {
            yield return dialogueTyper.Run(dialogue, dialogueBoxTMP);
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return) || xboxA); 
        }
        closeDialogueBox();
        yield break;
    }
}
