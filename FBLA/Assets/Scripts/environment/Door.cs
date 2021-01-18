using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public AudioClip open;

    public void Open()
    {
        Debug.Log("Open");
        if(!GetComponent<Animator>().GetBool("Door Open"))
            GameObject.FindGameObjectWithTag("Player").GetComponent<AudioSource>().PlayOneShot(open, .4f);
        GetComponent<Animator>().SetBool("Door Open", true);
    }
}
