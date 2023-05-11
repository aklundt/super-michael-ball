using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Video;

public class narratorDialogue : MonoBehaviour
{
    public gameManager gameManager;

    RenderTexture textBoxRenderer;
    VideoPlayer textBox;
    VideoClip textBoxOpen;
    VideoClip textBoxClose;
    bool xboxADown;

    public TextMeshProUGUI dialogueBoxTMP;
    private dialogueTyper dialogueTyper;
    public bool dialogueFinished;

    void Start() {
        textBoxRenderer = gameManager.textBoxRenderer;
        textBox = gameManager.textBox;
        textBoxOpen = gameManager.textBoxOpen;
        textBoxClose = gameManager.textBoxClose;
        xboxADown = gameManager.xboxADown;
    }
    public Coroutine Run(DialogueObject narratorLines)
    {
        dialogueFinished = false;
        textBox.clip = textBoxOpen;
        textBox.Play();
        dialogueTyper = gameManager.GetComponent<dialogueTyper>();
        openDialogueBox(narratorLines);
        return StartCoroutine(StepThroughDialogue(narratorLines));
    }

    void Update()
    {
    }

    private void openDialogueBox(DialogueObject narratorLines) // I will beat Grayson into making pretty little animations. Maybe we should move these to a different script
    {
        gameManager.textBoxOngoing = true;
        //dialogueCharacterName.text = narratorLines.ActorName;
    }

    private void closeDialogueBox() // ^ last comment
    {
        gameManager.textBoxOngoing = false;
        textBox.clip = textBoxClose;
        textBox.Play();
        dialogueBoxTMP.text = "";
        dialogueFinished = true;
    }


    private IEnumerator StepThroughDialogue(DialogueObject dialogueObject)
    {
        yield return new WaitForSeconds(1);
        foreach (string dialogue in dialogueObject.Dialogue)
        {
            yield return dialogueTyper.Run(dialogue, dialogueBoxTMP);
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return) || xboxADown);
        }
        closeDialogueBox();
        yield break;
    }
}
