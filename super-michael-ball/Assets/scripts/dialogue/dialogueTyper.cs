using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using UnityEngine;
using TMPro;

public class dialogueTyper : MonoBehaviour
{

    public Coroutine Run(string textToType, TextMeshProUGUI dialogueBoxTMP)
    {
        return StartCoroutine(TypeText(textToType, dialogueBoxTMP));
    }

    private IEnumerator TypeText(string textToType, TextMeshProUGUI dialogueBoxTMP)
    {
        float timeWriting = 0;
        int charIndex = 0;

        while (charIndex < textToType.Length)
        {
            timeWriting += Time.deltaTime * 35;
            charIndex = Mathf.Clamp(Mathf.FloorToInt(timeWriting), 0, textToType.Length);

            dialogueBoxTMP.text = textToType.Substring(0, charIndex);
            yield return null;
        }

        dialogueBoxTMP.text = textToType;
    }
}
