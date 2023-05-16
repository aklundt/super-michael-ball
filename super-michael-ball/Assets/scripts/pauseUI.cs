using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class pauseUI : MonoBehaviour
{
    public gameManager gameManager;
    public GameObject pauseUIHolder;
    public GameObject confirmationTab;
    public GameObject settingsSubMenu;
    public GameObject checkpointsToggle;
    public Button lobbyButton;
    public GameObject overlay;
    public TextMeshProUGUI globalTimer;

    

    bool resumeMovementAfterResume;
    bool submenuOpen;
    string confirmationFunction;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        confirmationFunction = null;
        StartCoroutine(nextFrameStart());
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
        if (submenuOpen) {
            updatePreferences();
        }
    }

    // while submenu is open, update preferences with settings contents
    // save these preferences in gameManager
    void updatePreferences() {
        PlayerPrefs.SetFloat("moveSensitivity", settingsSubMenu.transform.Find("moveSens").GetComponent<Slider>().value);
        PlayerPrefs.SetFloat("cameraSensitivity", settingsSubMenu.transform.Find("cameraSens").GetComponent<Slider>().value);
        if (checkpointsToggle.GetComponent<Toggle>().isOn)
        {
            PlayerPrefs.SetString("checkpointsToggle", "enabled");
            gameManager.checkpointsEnabled = true;
        }
        else
        {
            PlayerPrefs.SetString("checkpointsToggle", "disabled");
            gameManager.checkpointsEnabled = false;
        }
        gameManager.moveSensitivity = PlayerPrefs.GetFloat("moveSensitivity");
        gameManager.cameraSensitivity = PlayerPrefs.GetFloat("cameraSensitivity");
    }
    IEnumerator nextFrameStart()
    {
        yield return new WaitForEndOfFrame();
        
        
        // allow checkpoints option if tutorial is beaten
        if (gameManager.gameStatus > 1) { 
            checkpointsToggle.GetComponent<Toggle>().interactable = true;
        }

        // update settings menu with saved preferences
        settingsSubMenu.transform.Find("moveSens").GetComponent<Slider>().value = PlayerPrefs.GetFloat("moveSensitivity");
        settingsSubMenu.transform.Find("cameraSens").GetComponent<Slider>().value = PlayerPrefs.GetFloat("cameraSensitivity");
        if (PlayerPrefs.GetString("checkpointsToggle") == "disabled")
        {
            checkpointsToggle.GetComponent<Toggle>().isOn = false;
        }
        else
        {
            checkpointsToggle.GetComponent<Toggle>().isOn = true;
        }
        updatePreferences();
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

        globalTimer.text = "TIME: " + gameManager.toReadableTime(gameManager.globalTimer);
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
        if (!submenuOpen) {
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

    public void openSettings() {
        submenuOpen = true;
        settingsSubMenu.SetActive(true);
    }

    public void closeSettings()
    {
        submenuOpen = false;
        settingsSubMenu.SetActive(false);
    }
    public void openConfirmation(string function) {
        
        submenuOpen = true;
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
            submenuOpen = false;
            resumeGame();
            SceneManager.LoadScene("DreamLobby");
        }
        else if (confirmationFunction == "title")
        {
            StartCoroutine(gameManager.transitionStatic(true, false, false));
            yield return new WaitUntil(() => (gameManager.staticIn.frame == (long)gameManager.staticIn.frameCount - 1));
            submenuOpen = false;
            resumeGame();
            SceneManager.LoadScene("Menu");
            
        }
        else if (confirmationFunction == "exit")
        {
            StartCoroutine(gameManager.transitionStatic(true, false, false));
            yield return new WaitUntil(() => (gameManager.staticIn.frame == (long)gameManager.staticIn.frameCount - 1));
            submenuOpen = false;
            Application.Quit();
        }
        else { noConfirmation(); }
        overlay.GetComponent<RawImage>().color = new Color(1, 1, 1, 0);
        yield return null;
    }
    public void noConfirmation() {
        submenuOpen = false;
        confirmationTab.SetActive(false);
        confirmationFunction = null;
    }

    public void resetScene() {
        closeSettings();
        updatePreferences();
        resumeGame();
        if (gameManager.levelTimer > 0.3f) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        
    }

    
}
