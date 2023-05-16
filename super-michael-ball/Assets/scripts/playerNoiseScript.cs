using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerNoiseScript : MonoBehaviour
{

    Rigidbody playerRb;
    public AudioSource playerAudioSource;
    GameObject fridgeObj;
    public AudioClip[] michaelNoises;
    public AudioClip[] michaelHungryNoises;
    // Start is called before the first frame update
    void Start()
    {
        playerRb = gameObject.GetComponent<Rigidbody>();
        playerAudioSource = playerRb.GetComponent<AudioSource>();
        fridgeObj = GameObject.Find("fridge");
        playerAudioSource.clip = pickRandomClip(michaelHungryNoises);
        playerAudioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (fridgeObj != null) {
            if (!playerAudioSource.isPlaying && Vector3.Distance(transform.position, fridgeObj.transform.position) < 20)
            {
                playerAudioSource.clip = pickRandomClip(michaelHungryNoises);
                playerAudioSource.Play();
            }
        }
    }


    private void  OnCollisionEnter(Collision collision)
    {
        if (playerRb.velocity.magnitude > 10) {
            playerAudioSource.volume = playerRb.velocity.magnitude / 140;
            playerAudioSource.clip = pickRandomClip(michaelNoises);
            playerAudioSource.Play();
        }
    }

    public AudioClip pickRandomClip(AudioClip[] list) {
        int x = Random.Range(0, list.Length);
        return list[x];
    }
}
