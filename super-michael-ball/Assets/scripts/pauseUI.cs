using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Search;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class pauseUI : MonoBehaviour
{
    public gameManager gameManager;
    public GameObject pauseUIHolder;
    public GameObject confirmationTab;
    public Button lobbyButton;
    public GameObject overlay;

    bool resumeMovementAfterResume;
    bool confirmationOpen;
    string confirmationFunction;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        confirmationFunction = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameManager.paused) {
            if (gameManager.escDown) {
                pauseGame();
            }
            
        }
        else if (gameManager.paused) {
            if (gameManager.escDown)
            {
                resumeGame();
            }
        }
    }

    void pauseGame() {
        gameManager.paused = true;
        if (gameManager.movementEnabled == true) { resumeMovementAfterResume = true;  }
        gameManager.movementEnabled = false;

        gameManager.textBox.playbackSpeed = 0;

        if (gameManager.gameStatus < 1) {
            lobbyButton.interactable = false;
        }
        else { lobbyButton.interactable = true; }

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        pauseUIHolder.SetActive(true);
        StartCoroutine(overlayFadeIn());
        Time.timeScale = 0;
    }

    private IEnumerator overlayFadeIn() {
        while (overlay.GetComponent<RawImage>().color.a < 0.7f && gameManager.paused) {
            overlay.GetComponent<RawImage>().color += new Color(0, 0, 0, 0.005f);
            yield return null;
        }
    }
    private IEnumerator overlayFadeOut() { 
        while (overlay.GetComponent<RawImage>().color.a > 0 && !gameManager.paused)
        {
            overlay.GetComponent<RawImage>().color = new Color(255, 255, 255, overlay.GetComponent<RawImage>().color.a - 0.005f);
            yield return null;
        }
    }

    public void resumeGame() {
        if (!confirmationOpen) {
            gameManager.paused = false;
            if (resumeMovementAfterResume)
            {
                gameManager.movementEnabled = true;
                resumeMovementAfterResume = false;
            }
            gameManager.textBox.playbackSpeed = 1;

            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            pauseUIHolder.SetActive(false);
            StartCoroutine(overlayFadeOut());
            Time.timeScale = 1;
        }
    }
    public void openConfirmation(string function) {
        
        confirmationOpen = true;
        confirmationTab.SetActive(true);
        confirmationFunction = function;
        TextMeshProUGUI query;
        query = confirmationTab.transform.Find("query").GetComponent<TextMeshProUGUI>();
        if (function == "lobby") {
            query.text = "Are you sure you want to return to the lobby?";
        }
        else if (function == "title")
        {
            query.text = "Are you sure you want to return to the title screen?";
        }
        else if (function == "exit")
        {
            
            query.text = "Are you sure you want exit the game?";
        }
    }
    public void yesConfirmationButton() { 
        StartCoroutine(yesConfirmation());
    }
    public IEnumerator yesConfirmation() {
        if (confirmationFunction == "lobby")
        {
            StartCoroutine(gameManager.transitionStatic(true, false, false));
            yield return new WaitUntil(() => (gameManager.staticIn.frame == (long)gameManager.staticIn.frameCount - 1));
            confirmationOpen = false;
            resumeGame();
            SceneManager.LoadScene("DreamLobby");
        }
        else if (confirmationFunction == "title")
        {
            StartCoroutine(gameManager.transitionStatic(true, false, false));
            yield return new WaitUntil(() => (gameManager.staticIn.frame == (long)gameManager.staticIn.frameCount - 1));
            confirmationOpen = false;
            resumeGame();
            SceneManager.LoadScene("Menu");
            
        }
        else if (confirmationFunction == "exit")
        {
            StartCoroutine(gameManager.transitionStatic(true, false, false));
            yield return new WaitUntil(() => (gameManager.staticIn.frame == (long)gameManager.staticIn.frameCount - 1));
            confirmationOpen = false;
            Application.Quit();
        }
        else { noConfirmation(); }
        overlay.GetComponent<RawImage>().color = new Color(1, 1, 1, 0);
        yield return null;
    }
    public void noConfirmation() {
        confirmationOpen = false;
        confirmationTab.SetActive(false);
        confirmationFunction = null;
    }
}
