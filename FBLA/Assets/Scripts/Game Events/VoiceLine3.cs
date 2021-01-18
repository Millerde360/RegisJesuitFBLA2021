using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoiceLine3 : MonoBehaviour
{
    public AudioClip line;
    bool played = false;
    // Update is called once per frame
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && !played)
        {
            transform.parent.GetComponent<Lvl3>().Player.PlayOneShot(line, .5f);
            played = true;
        }
    }
}
