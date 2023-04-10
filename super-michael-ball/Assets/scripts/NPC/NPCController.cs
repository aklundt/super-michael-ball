using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    public GameObject player;
    public GameObject dialogueBox;
    public GameObject cameraObj;
    public gameManager gameManager;
    public TextMeshProUGUI dialogueBoxTMP;
    public DialogueObject testDialogue;
    private dialogueTyper dialogueTyper;
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
            StartCoroutine(StepThroughDialogue(testDialogue));
        }
    }

    private void openDialogueBox () // it will make sense that this is a function after I beat Grayson into making pretty little animations. Maybe he wants to put these into dialogueBox.cs
    {
        gameManager.NPCTalking = true;
        player.GetComponent<Rigidbody>().angularDrag = 300;
        StartCoroutine(alignCameraToNPC());
        gameManager.GetComponent<gameManager>().movementEnabled = false;
        dialogueBox.SetActive(true);
    }

    private void closeDialogueBox() // ^ last comment
    {
        player.GetComponent<Rigidbody>().angularDrag = 0.05f;
        gameManager.GetComponent<gameManager>().movementEnabled = true;
        gameManager.GetComponent<gameManager>().NPCTalking = false;
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

    private IEnumerator alignCameraToNPC () // IDK make this smoothly do it
    {
        while (gameManager.NPCTalking)
        {
            yield return cameraObj.transform.rotation = Quaternion.Euler(cameraObj.transform.rotation.x, rotationToNPC(), cameraObj.transform.rotation.y);
        }
        yield break;
    }

    private IEnumerator StepThroughDialogue(DialogueObject dialogueObject)
    {
        openDialogueBox();
        foreach (string dialogue in dialogueObject.Dialogue)
        {
            yield return dialogueTyper.Run(dialogue, dialogueBoxTMP);
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return) || xboxA); 
        }
        closeDialogueBox();
        yield break;
    }
}
