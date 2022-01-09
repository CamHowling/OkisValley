using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundScript : MonoBehaviour
{
    public AudioClip canPlaceClip;
    public AudioClip cantPlaceClip;
    public AudioClip edgeOfMapClip;
    public AudioClip itemSwapClip;
    private Vector3 CameraPos;
    private AudioSource src;

    // Start is called before the first frame update
    void Start()
    {
        CameraPos = Camera.main.transform.position;
        src = this.GetComponent<AudioSource>();
    }

    public void playCantPlace()
    {
        //Play sound can't place
        src.PlayOneShot(cantPlaceClip);

    }

    public void playCanPlace()
    {
        src.PlayOneShot(canPlaceClip);
    }

    public void playEdgeOfMap()
    {
        src.PlayOneShot(edgeOfMapClip);
    }

    public void playItemSwap()
    {
        src.PlayOneShot(itemSwapClip);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
