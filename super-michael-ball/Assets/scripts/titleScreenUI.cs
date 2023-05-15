using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class titleScreenUI : MonoBehaviour
{

    //bool submenuOpen;
    public gameManager gameManager;
    public GameObject confirmationTab;
    public GameObject overlay;
    public VideoPlayer background;
    public VideoClip startScreenStart;
    string confirmationFunction;

    public GameObject[] buttons;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void startButton()
    {
        foreach (GameObject button in buttons) {
            button.SetActive(false);
        }
        background.clip = startScreenStart;
        background.Play();
        background.isLooping = false;
        StartCoroutine(waitForAnimationEnd());
        

    }


    public void openConfirmation(string function)
    {
        //submenuOpen = true;
        StartCoroutine(overlayFadeIn());
        confirmationTab.SetActive(true);
        confirmationFunction = function;
        TextMeshProUGUI query;
        query = confirmationTab.transform.Find("query").GetComponent<TextMeshProUGUI>();
        if (function == "reset")
        {
            query.text = "Are you sure you want to reset ALL of your data?";
        }
        else if (function == "exit")
        {

            query.text = "Are you sure you want exit the game?";
        }
    }
    public void yesConfirmationButton() {
        StartCoroutine(yesConfirmation());
    }

    IEnumerator waitForAnimationEnd() {
        yield return new WaitForSeconds(0.2f);
        if (PlayerPrefs.GetInt("gameState") > 0)
        {
            while (background.frame < 100)
            {
                yield return null;
            }
        }
        else {
            while(background.isPlaying) {
                yield return null;
            }
        }
        
        if (PlayerPrefs.GetInt("gameState") == 0)
        {
            SceneManager.LoadScene("RealWorldHouse");
        }
        else
        {
            SceneManager.LoadScene("DreamLobby");
        }
    }
    IEnumerator yesConfirmation() {
        if (confirmationFunction == "reset")
        {
            StartCoroutine(gameManager.transitionStatic(true, false, false));
            yield return new WaitUntil(() => (gameManager.staticIn.frame == (long)gameManager.staticIn.frameCount - 1));
            PlayerPrefs.DeleteAll();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        else if (confirmationFunction == "exit")
        {
            StartCoroutine(gameManager.transitionStatic(true, false, false));
            yield return new WaitUntil(() => (gameManager.staticIn.frame == (long)gameManager.staticIn.frameCount - 1));
            Application.Quit();
        }
    }

    public void noConfirmation() {
        StartCoroutine(overlayFadeOut());
        confirmationTab.SetActive(false);
    }

    private IEnumerator overlayFadeIn()
    {
        while (overlay.GetComponent<RawImage>().color.a < 0.7f)
        {
            overlay.GetComponent<RawImage>().color += new Color(0, 0, 0, 0.005f);
            yield return null;
        }
    }
    private IEnumerator overlayFadeOut()
    {
        while (overlay.GetComponent<RawImage>().color.a > 0)
        {
            overlay.GetComponent<RawImage>().color = new Color(255, 255, 255, overlay.GetComponent<RawImage>().color.a - 0.010f);
            yield return null;
        }
    }
}