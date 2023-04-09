using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    public GameObject player;
    public GameObject dialogueBox;
    public gameManager gameManager;
    public TextMeshProUGUI dialogueBoxTMP;
    public DialogueObject testDialogue;
    private dialogueTyper dialogueTyper; 

    // Start is called before the first frame update
    void Start()
    {
        dialogueTyper = gameManager.GetComponent<dialogueTyper>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && (player.transform.position - transform.position).magnitude <= 6 && !gameManager.NPCTalking)
        {
            StartCoroutine(StepThroughDialogue(testDialogue));
        }
    }

    private void openDialogueBox () // it will make sense that this is a function after I beat Grayson into making pretty little animations. Maybe he wants to put these into dialogueBox.cs
    {
        player.GetComponent<Rigidbody>().angularDrag = 300;
        gameManager.GetComponent<gameManager>().movementEnabled = false;
        gameManager.NPCTalking = true;
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

    private IEnumerator StepThroughDialogue(DialogueObject dialogueObject)
    {
        openDialogueBox();
        foreach (string dialogue in dialogueObject.Dialogue)
        {
            yield return dialogueTyper.Run(dialogue, dialogueBoxTMP);
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        }
        closeDialogueBox();
        yield break;
    }
}
