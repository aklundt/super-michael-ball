using OpenCover.Framework.Model;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Video;
using static UnityEditor.IMGUI.Controls.PrimitiveBoundsHandle;

public class gameManager : MonoBehaviour
{
    public GameObject player;
    public GameObject cameraOBJ;
    public GameObject cameraWhite;
    public GameObject gravityController;
    public GameObject deathPlane;
    public RenderTexture staticInRenderer;
    public VideoPlayer staticIn;
    public RenderTexture staticOutRenderer;
    public VideoPlayer staticOut;
    public RenderTexture doorTransitionRenderer;
    public VideoPlayer doorTransition;
    public RenderTexture textBoxRenderer;
    public VideoPlayer textBox;
    public VideoClip textBoxOpen;
    public VideoClip textBoxClose;
    public Material glowingWhite;
    public TextMeshProUGUI timer;

    public float primaryHorizontalInput;
    public float primaryVerticalInput;
    public float secondaryHorizontalInput;
    public float secondaryVerticalInput;
    public bool escDown;
    public bool xboxA;
    public bool xboxADown;
    public bool xboxRightBumper;
    public bool xboxRightBumperDown;

    public int gameStatus;
    public float levelTimer;
    public float globalTimer;
    public bool checkpointsEnabled;
    public float moveSensitivity;
    public float cameraSensitivity;
    public int frameRate;

    public Vector3 resetPosition;
    public float resetRotationY;
    public bool movementEnabled;


    public bool textBoxOngoing;
    public bool paused;

    // Start is called before the first frame update
    void Start()
    {
        gameStatus = PlayerPrefs.GetInt("gameState");
        globalTimer = PlayerPrefs.GetFloat("globalTimer");
        setPreferenceDefaultsIfNeeded();
        moveSensitivity = PlayerPrefs.GetFloat("moveSensitivity");
        cameraSensitivity = PlayerPrefs.GetFloat("cameraSensitivity");
        staticInRenderer.Release();
        staticOutRenderer.Release();
        textBoxRenderer.Release();
        doorTransitionRenderer.Release();
        StartCoroutine(transitionStatic(false, true, true));
    }

    // Update is called once per frame
    void Update()
    {
        checkInput();
        if (!paused && !textBoxOngoing) {
            levelTimer += Time.deltaTime;
            PlayerPrefs.SetFloat("globalTimer", globalTimer += Time.deltaTime);
            if (timer != null) {
                timer.text = "TIMER: " + System.Math.Round(levelTimer, 2);
            }
            
        }
        
        Application.targetFrameRate = frameRate;
    }

    void setPreferenceDefaultsIfNeeded() {
        if (PlayerPrefs.GetFloat("moveSensitivity") == 0) {
            PlayerPrefs.SetFloat("moveSensitivity", 1);
        }
        if (PlayerPrefs.GetFloat("cameraSensitivity") == 0)
        {
            PlayerPrefs.SetFloat("cameraSensitivity", 1);
        }
    }

    void checkInput()
    {
        primaryHorizontalInput = Input.GetAxis("Horizontal") * moveSensitivity;
        primaryVerticalInput = Input.GetAxis("Vertical") * moveSensitivity;
        secondaryHorizontalInput = Input.GetAxis("Secondary Horizontal") * cameraSensitivity;
        secondaryVerticalInput = Input.GetAxis("Secondary Vertical") * cameraSensitivity;
        escDown = Input.GetKeyDown(KeyCode.Escape);
        xboxA = Input.GetButton("XboxA");
        xboxADown = Input.GetButtonDown("XboxA");
        xboxRightBumper = Input.GetButton("XboxRightBumper");
        xboxRightBumperDown = Input.GetButtonDown("XboxRightBumper");
    }

    public void teleportPlayerTo(Vector3 position, Vector3 rotation) { 
        player.transform.position = position;
        player.transform.rotation = Quaternion.Euler(rotation);
    }
    void resetPlayer() {
        player.transform.position = resetPosition;
        player.transform.rotation = Quaternion.Euler(new Vector3(0, resetRotationY, 0));
        player.GetComponent<Rigidbody>().velocity = Vector3.zero;
        player.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        gravityController.transform.rotation = Quaternion.Euler(new Vector3(0, resetRotationY, 0));
    }

    public void erasePlayerForces() {
        player.GetComponent<Rigidbody>().velocity = Vector3.zero;
        player.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        gravityController.transform.rotation = Quaternion.Euler(0, gravityController.transform.rotation.eulerAngles.y, 0);
    }
    public IEnumerator dynamicTransitionFadeIn() {
        staticIn.Play();
        yield return new WaitForSeconds(0.3f);
        while (xboxA && xboxRightBumper && staticIn.isPlaying) {
            yield return null;
        }
        if (xboxA && xboxRightBumper && staticIn.frame == (long)staticIn.frameCount - 1)
        {
            StartCoroutine(transitionStatic(false, true, true));
            Debug.Log("stayed held donw");
        }
        else {
            staticIn.Stop();
            staticIn.frame = 0;
            staticInRenderer.Release();
            Debug.Log("not held donw");
        }
        yield return null;
    }

    public IEnumerator transitionStatic(bool fadeIn, bool reset, bool fadeOut) {
        if (fadeIn) {
            staticIn.Play();
            yield return new WaitForSeconds(0.5f);
            while (staticIn.isPlaying)
            {
                yield return null;
            }
        }
        if (reset) { resetPlayer(); }
        if (fadeOut) {
            staticOut.Prepare();
            Debug.Log(staticOut.isPrepared);
            while (!staticOut.isPrepared) {
                yield return new WaitForFixedUpdate();
            }
            staticOut.Play();
            staticOut.frame = 0;
            while (staticOut.frame < 1) {
                yield return null;
                
            }
            cameraWhite.SetActive(false);


            staticIn.frame = 0;
            staticInRenderer.Release();
            yield return new WaitForSeconds(0.3f);
            while (staticOut.isPlaying)
            {
                yield return null;
            }
            staticOutRenderer.Release();
        }
        yield return null;
    }

}
