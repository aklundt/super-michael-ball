using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class narratorDialogue : MonoBehaviour
{
    public gameManager gameManager;

    VideoPlayer textBox;
    VideoClip textBoxOpen;
    VideoClip textBoxClose;

    public TextMeshProUGUI dialogueBoxTMP;
    public RawImage aIcon;
    private dialogueTyper dialogueTyper;
    public bool dialogueFinished;

    void Start() { // grabbing necessary objects from game manager
        textBox = gameManager.textBox;
        textBoxOpen = gameManager.textBoxOpen;
        textBoxClose = gameManager.textBoxClose;
    }
    public Coroutine Run(DialogueObject narratorLines) // open dialogue box and start running through dialogue
    {
        dialogueTyper = gameManager.GetComponent<dialogueTyper>(); // decide the script to write at the start of each run through of a script.
        openDialogueBox();
        return StartCoroutine(StepThroughDialogue(narratorLines));
    }

    private void openDialogueBox() // I beat Grayson into making pretty little animations for opening and closing the dialogue box.
    {
        textBox.clip = textBoxOpen; 
        textBox.Play();
        gameManager.textBoxOngoing = true;
    }

    private void closeDialogueBox() // ^ last comment 
    {
        gameManager.textBoxOngoing = false;
        textBox.clip = textBoxClose;
        textBox.Play();
        dialogueBoxTMP.text = "";
    }


    public IEnumerator StepThroughDialogue(DialogueObject dialogueObject) // step through the dialogue
    {
        yield return new WaitForSeconds(1); // ensures the text doesn't appear until the animation finishes.
       
        foreach (string dialogue in dialogueObject.Dialogue) // run through every string in my dialogueObject and process it.
        {
            yield return dialogueTyper.Run(dialogue, dialogueBoxTMP); // send my dialogue typer the string to write and the text box to write in
            StartCoroutine(fadeAIcon(true)); // "a" button animation on the text box 
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return) || gameManager.xboxADown); // progress through the dialogue on keypresses.
            
        }
        closeDialogueBox(); // close the box after dialogue completes
        yield break;
    }
    private IEnumerator fadeAIcon(bool fadeIn) { // shifts the opacity of the "a" button icon on the text box. This will draw the player's attention to it and let them know they can press "a" to progress through the dialogue
        
        if (fadeIn)
        {
            while (gameManager.textBoxOngoing)
            {
                if (aIcon.color.a < 1) { aIcon.color = aIcon.color + new Color(0, 0, 0, 1f * Time.deltaTime); }
                yield return null;
            }
            StartCoroutine(fadeAIcon(false));
            yield break;
        }
        else
        {
            while (aIcon.color.a > 0) {
                aIcon.color = aIcon.color - new Color(0, 0, 0, 1f * Time.deltaTime);
                yield return null;
            }
            
        }
    }
}
