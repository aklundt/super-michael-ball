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

    void Start() {
        textBox = gameManager.textBox;
        textBoxOpen = gameManager.textBoxOpen;
        textBoxClose = gameManager.textBoxClose;
    }
    public Coroutine Run(DialogueObject narratorLines)
    {
        dialogueTyper = gameManager.GetComponent<dialogueTyper>();
        openDialogueBox();
        return StartCoroutine(StepThroughDialogue(narratorLines));
    }

    void Update()
    {
    }

    private void openDialogueBox() // I will beat Grayson into making pretty little animations. Maybe we should move these to a different script
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


    public IEnumerator StepThroughDialogue(DialogueObject dialogueObject)
    {
        yield return new WaitForSeconds(1);
       
        foreach (string dialogue in dialogueObject.Dialogue)
        {
            yield return dialogueTyper.Run(dialogue, dialogueBoxTMP);
            StartCoroutine(fadeAIcon(true));
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return) || gameManager.xboxADown);
            
        }
        closeDialogueBox();
        yield break;
    }
    private IEnumerator fadeAIcon(bool fadeIn) {
        
        if (fadeIn)
        {
            while (gameManager.textBoxOngoing)
            {
                if (aIcon.color.a < 1) { aIcon.color = aIcon.color + new Color(0, 0, 0, 0.003f); }
                yield return null;
            }
            StartCoroutine(fadeAIcon(false));
            yield break;
        }
        else
        {
            while (aIcon.color.a > 0) {
                aIcon.color = aIcon.color - new Color(0, 0, 0, 0.003f);
                yield return null;
            }
            
        }
    }
}
