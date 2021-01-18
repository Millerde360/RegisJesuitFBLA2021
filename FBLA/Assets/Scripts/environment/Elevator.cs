using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{

    public GameObject elevatorMove;
    public BoxCollider col;
    public float speed;
    public AudioClip voiceLine;
    public AudioClip voiceLine2;
    public bool isVC2Played;
    public bool isEndLevel;
    public float timeToEnd;

    void Update()
    {
        //SPECIFIC ONLY TO LVL 3 LARGE ELEVATOR
        if(speed == -2)
        {
            col.enabled = false;
            if (!isVC2Played)
            {
                GameObject.FindObjectOfType<AudioSource>().PlayOneShot(voiceLine2, .5f);
                isVC2Played = true;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (voiceLine != null)
                GameObject.FindObjectOfType<AudioSource>().PlayOneShot(voiceLine, .5f);
            if (isEndLevel)
            {
                StartCoroutine(GameObject.Find("Game Manager").GetComponent<Lvl2>().endLevel(1, 1, timeToEnd));
                isEndLevel = false;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player")
        {
            elevatorMove.transform.position += elevatorMove.transform.up * speed * Time.deltaTime;
        }
    }
}
