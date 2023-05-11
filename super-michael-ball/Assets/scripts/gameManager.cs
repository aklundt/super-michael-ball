using OpenCover.Framework.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Video;

public class gameManager : MonoBehaviour
{
    public GameObject player;
    public GameObject cameraOBJ;
    public GameObject gravityController;
    public RenderTexture transitionRenderer;
    public VideoPlayer whiteTransition;
    public RenderTexture staticInRenderer;
    public VideoPlayer staticIn;
    public RenderTexture staticOutRenderer;
    public VideoPlayer staticOut;

    public float primaryHorizontalInput;
    public float primaryVerticalInput;
    public float secondaryHorizontalInput;
    public float secondaryVerticalInput;
    public bool xboxA;
    public bool xboxADown;
    public bool xboxRightBumper;
    public bool xboxRightBumperDown;

    public int gameStatus;
    public int frameRate;

    public Vector3 resetPosition;
    public bool movementEnabled;


    public bool NPCTalking;
  

    // Start is called before the first frame update
    void Start()
    {
        NPCTalking = false;
        //transitionRenderer.Release();
        staticInRenderer.Release();
        staticOutRenderer.Release();
        StartCoroutine(transitionStatic(false, true, true));
    }

    // Update is called once per frame
    void Update()
    {
        checkInput();
        Application.targetFrameRate = frameRate;
    }

    void checkInput()
    {
        primaryHorizontalInput = Input.GetAxis("Horizontal");
        primaryVerticalInput = Input.GetAxis("Vertical");
        secondaryHorizontalInput = Input.GetAxis("Secondary Horizontal");
        secondaryVerticalInput = Input.GetAxis("Secondary Vertical");
        xboxA = Input.GetButton("XboxA");
        xboxADown = Input.GetButtonDown("XboxA");
        xboxRightBumper = Input.GetButton("XboxRightBumper");
        xboxRightBumperDown = Input.GetButtonDown("XboxRightBumper");
    }

    void resetPlayer() {
        player.transform.position = resetPosition;
        player.transform.rotation = Quaternion.Euler(Vector3.zero);
        player.GetComponent<Rigidbody>().velocity = Vector3.zero;
        player.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        gravityController.transform.rotation = Quaternion.Euler(Vector3.zero);
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
            staticOut.Play();
            staticOut.frame = 0;


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
