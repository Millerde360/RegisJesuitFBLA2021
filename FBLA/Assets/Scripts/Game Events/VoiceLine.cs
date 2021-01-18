using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoiceLine : MonoBehaviour
{
    public AudioClip line;
    bool initiated = false;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player" && !initiated)
        {
            transform.parent.GetComponent<GameIntro>().player.gameObject.GetComponent<AudioSource>().PlayOneShot(line, .5f);
            initiated = true;
        }
    }
}
