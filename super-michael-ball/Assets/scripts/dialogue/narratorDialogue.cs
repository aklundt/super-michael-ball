using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class narratorDialogue : MonoBehaviour
{
    public gameManager gameManager;
    public GameObject dialogueBox;
    public TextMeshProUGUI dialogueBoxTMP;
    public TextMeshProUGUI dialogueCharacterName;
    private dialogueTyper dialogueTyper;
    private bool xboxA;
    public bool dialogueFinished;


    public Coroutine Run(DialogueObject narratorLines)
    {
        dialogueFinished = false;
        dialogueTyper = gameManager.GetComponent<dialogueTyper>();
        openDialogueBox(narratorLines);
        return StartCoroutine(StepThroughDialogue(narratorLines));
    }

    void Update()
    {
        xboxA = Input.GetButtonDown("XboxA");
    }

    private void openDialogueBox(DialogueObject narratorLines) // I will beat Grayson into making pretty little animations. Maybe we should move these to a different script
    {
        gameManager.NPCTalking = true;
        dialogueBox.SetActive(true);
        dialogueCharacterName.text = narratorLines.ActorName;
    }

    private void closeDialogueBox() // ^ last comment
    {
        gameManager.NPCTalking = false;
        dialogueBoxTMP.text = "";
        dialogueBox.SetActive(false);
        dialogueFinished = true;
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
