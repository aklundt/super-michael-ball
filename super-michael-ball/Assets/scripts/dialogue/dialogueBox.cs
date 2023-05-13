using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class dialogueBox : MonoBehaviour
{
    enum axis { X, Y, Z }

    float secondaryHorizontalInput;
    float secondaryVerticalInput;
    public float moveLimit;
    public float moveStabilizationFactor;
    public gameManager gameManager;

    private RectTransform rectTransform;

    // Start is called before the first frame update
    void Start()
    {
        rectTransform = this.gameObject.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        grabInput();
        applyInput();
    }

    void grabInput() {
        secondaryVerticalInput = gameManager.secondaryVerticalInput;
        secondaryHorizontalInput = gameManager.secondaryHorizontalInput;
    }

    void applyInput() {
        if (!gameManager.paused) { 
            stabilizeMovement(rectTransform, axis.X, moveStabilizationFactor);
            Vector2 newPosition = rectTransform.anchoredPosition + (new Vector2(secondaryHorizontalInput, secondaryVerticalInput));
            newPosition = new Vector2(
                Mathf.Clamp(newPosition.x, -moveLimit, moveLimit),
                Mathf.Clamp(newPosition.y, -moveLimit, moveLimit)
                );
            rectTransform.anchoredPosition = newPosition;
        } 
    }

    void stabilizeMovement(RectTransform rectTransform, axis axis, float factor) {
        rectTransform.anchoredPosition = rectTransform.anchoredPosition - (rectTransform.anchoredPosition * factor * Time.deltaTime);
    }
}
