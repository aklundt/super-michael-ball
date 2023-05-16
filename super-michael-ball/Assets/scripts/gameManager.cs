using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Video;

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
    public bool levelFinished;

    // Start is called before the first frame update
    void Start()
    {

        // get player preferences and save them into scene
        gameStatus = PlayerPrefs.GetInt("gameState");
        globalTimer = PlayerPrefs.GetFloat("globalTimer");
        setPreferenceDefaultsIfNeeded();
        moveSensitivity = PlayerPrefs.GetFloat("moveSensitivity");
        cameraSensitivity = PlayerPrefs.GetFloat("cameraSensitivity");

        // reset Texture Renderers that DON'T RESET AT THE START OF THE GAME FOR SOME REASON >:(
        staticInRenderer.Release();
        staticOutRenderer.Release();
        textBoxRenderer.Release();
        doorTransitionRenderer.Release();

        // transition smoothly into the scene
        StartCoroutine(transitionStatic(false, true, true));
    }

    // Update is called once per frame
    void Update()
    {
        if (player.transform.position.y < -600) {
            Debug.Log(player.transform.position.x);
        }

        checkInput();
        // update timer
        if (!paused && !textBoxOngoing && !levelFinished) {
            levelTimer += Time.deltaTime;
            PlayerPrefs.SetFloat("globalTimer", globalTimer += Time.deltaTime);
            if (timer != null) {
                timer.text = "TIMER: " + levelTimer.ToString("0.00");
            }
            
        }
        
        // debug tool for testing framerates
        Application.targetFrameRate = frameRate;
    }

    //  if mouse or camera sensitivity has never been configured, set to default
    void setPreferenceDefaultsIfNeeded() {
        if (PlayerPrefs.GetFloat("moveSensitivity") == 0) {
            PlayerPrefs.SetFloat("moveSensitivity", 1);
        }
        if (PlayerPrefs.GetFloat("cameraSensitivity") == 0)
        {
            PlayerPrefs.SetFloat("cameraSensitivity", 1);
        }
    }

    // update variables with player input
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

    // teleport player to vector3 position with vector3 rotation
    public void teleportPlayerTo(Vector3 position, Vector3 rotation) { 
        player.transform.position = position;
        player.transform.rotation = Quaternion.Euler(rotation);
    }

    // teleport player to the stored reset position variable with the reset rotationy variable and remove all forces
    void resetPlayer() {
        player.transform.position = resetPosition;
        player.transform.rotation = Quaternion.Euler(new Vector3(0, resetRotationY, 0));

        // could technically call erasePlayerForces here instead but that third line is still needed to reset the y rotation, so i'm leaving this
        player.GetComponent<Rigidbody>().velocity = Vector3.zero;
        player.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        gravityController.transform.rotation = Quaternion.Euler(new Vector3(0, resetRotationY, 0));
        if (!checkpointsEnabled) {
            levelTimer = 0;
        }
    }

    // removes all forces on the player and levels gravityController balance
    public void erasePlayerForces() {
        player.GetComponent<Rigidbody>().velocity = Vector3.zero;
        player.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        gravityController.transform.rotation = Quaternion.Euler(0, gravityController.transform.rotation.eulerAngles.y, 0);
    }

    // fade in that will only finish if xboxA and xboxRightBumper are held down
    public IEnumerator dynamicTransitionFadeIn() {
        staticIn.Prepare();
        while (!staticIn.isPrepared)
        {
            yield return new WaitForFixedUpdate();
        }
        staticIn.Play();
        while (staticIn.frame < 1)
        {
            yield return null;
        }
        while (xboxA && xboxRightBumper && staticIn.isPlaying) {
            yield return null;
        }
        if (xboxA && xboxRightBumper && staticIn.frame == (long)staticIn.frameCount - 1)
        {
            StartCoroutine(transitionStatic(false, true, true));
        }
        else {
            staticIn.Stop();
            staticIn.frame = 0;
            staticInRenderer.Release();
        }
        yield return null;
    }

    // optional fade in, resetting of player, and fade out that occur in that order
    // better than having a fadeIn method and fadeOut method since they need to communicate to know when to start video
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
            while (!staticOut.isPrepared) {
                yield return new WaitForFixedUpdate();
            }
            staticOut.Play();
            staticOut.frame = 0;
            // hide white ball around camera that makes transition seamless after the video starts playing.
            // show timer for same reason
            while (staticOut.frame < 1) {
                yield return null;
            }
            cameraWhite.SetActive(false);
            if (timer != null) {
                timer.gameObject.SetActive(true);
            }
            

            // hide staticIn
            staticIn.frame = 0;
            staticInRenderer.Release();

            while (staticOut.isPlaying)
            {
                yield return null;
            }
            staticOutRenderer.Release();
        }
        yield return null;
    }

    public string toReadableTime(float timeInSeconds)
    {
        float hours = 0;
        float minutes = 0;
        float seconds = 0;
        if (timeInSeconds > 60)
        {
            minutes = (float)System.Math.Truncate(timeInSeconds / 60);
            if (minutes > 60)
            {
                hours = (float)System.Math.Truncate(minutes / 60);
                minutes = (minutes - (60 * hours));
            }
        }
        seconds = (timeInSeconds - (3600 * hours) - (minutes * 60));
        return hours.ToString("00") + ":" + minutes.ToString("00") + ":" + seconds.ToString("00.00");
    }
}
