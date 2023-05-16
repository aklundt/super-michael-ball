using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dialogueBox : MonoBehaviour
{

    // This script moves the dialogue box with camera input just for funsies
    enum axis { X, Y, Z }

    float secondaryHorizontalInput;
    float secondaryVerticalInput;
    public float moveLimit;
    public float moveStabilizationFactor;
    public gameManager gameManager;

    private RectTransform rectTransform;

    // Start is called before the first frame update
    void Start() // grabs the rectTransform of the dialogue box
    {
        rectTransform = this.gameObject.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        grabInput(); 
        applyInput();
    }

    void grabInput() { // grabs controller input
        secondaryVerticalInput = gameManager.secondaryVerticalInput;
        secondaryHorizontalInput = gameManager.secondaryHorizontalInput;
    }

    void applyInput() { // uses the player input to wiggle the text box
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
