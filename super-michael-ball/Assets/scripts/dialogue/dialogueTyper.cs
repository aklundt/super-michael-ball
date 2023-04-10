using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using UnityEngine;
using TMPro;

public class dialogueTyper : MonoBehaviour
{
    private int typeSpeed = 35;
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
            typeSpeed = 35;
            if (Input.GetKey(KeyCode.Space))
            {
                typeSpeed = 90;
            }
            timeWriting += Time.deltaTime * typeSpeed;
            charIndex = Mathf.Clamp(Mathf.FloorToInt(timeWriting), 0, textToType.Length);

            dialogueBoxTMP.text = textToType.Substring(0, charIndex);
            yield return null;
        }

        dialogueBoxTMP.text = textToType;
    }
}
