using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    public GameObject player;
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
            gameManager.GetComponent<gameManager>().movementEnabled = false;
            player.GetComponent<Rigidbody>().angularDrag = 300;
            StartCoroutine(StepThroughDialogue(testDialogue));
        }
    }

    private IEnumerator StepThroughDialogue(DialogueObject dialogueObject)
    {
        gameManager.NPCTalking = true;
        foreach (string dialogue in dialogueObject.Dialogue)
        {
            yield return dialogueTyper.Run(dialogue, dialogueBoxTMP);
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        }
        dialogueBoxTMP.text = "";
        gameManager.GetComponent<gameManager>().NPCTalking = false;
        gameManager.GetComponent<gameManager>().movementEnabled = true;
        yield break;
    }
}
