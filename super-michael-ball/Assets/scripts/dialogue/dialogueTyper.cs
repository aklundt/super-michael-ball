using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using UnityEngine;
using TMPro;


public class dialogueTyper : MonoBehaviour
{
    private int typeSpeed = 35; 
    public Coroutine Run(string textToType, TextMeshProUGUI dialogueBoxTMP) // accessible coroutine to put a string in a text box over time with a typewriter effect
    {
        return StartCoroutine(TypeText(textToType, dialogueBoxTMP));
    }

    private IEnumerator TypeText(string textToType, TextMeshProUGUI dialogueBoxTMP) // will put a string in a text box a few characters at a time, rather than instantaneously. it looks cooler
    {
        float timeWriting = 0; // the time since the string has started writing MODIFIED by the typespeed
        int charIndex = 0; // what index of the string the code will write up to

        while (charIndex < textToType.Length) // loop until the string is fully written
        {
            typeSpeed = 35; // reset type speed if it gets shifted by the next two lines. This will ensure the text is only written faster as the buttons are held
            if (Input.GetKey(KeyCode.Space) || Input.GetButton("XboxA")) // if the a button is held or the space bar, the speed of the text is faster to skip through dialogue.
            {
                typeSpeed = 90;
            }
            timeWriting += Time.deltaTime * typeSpeed; // this will make timeWriting increase with the time at a scale decided by the type speed
            charIndex = Mathf.Clamp(Mathf.FloorToInt(timeWriting), 0, textToType.Length); // sets the index to write to at a floored and clamped timeWriting within the length of the script

            dialogueBoxTMP.text = textToType.Substring(0, charIndex); // puts the text in the box
            yield return null;
        }

        dialogueBoxTMP.text = textToType;
    }
}
