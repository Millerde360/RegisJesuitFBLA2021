using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Voiceline1_1 : MonoBehaviour
{
    private void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.tag == "Player"){
            transform.parent.GetComponent<GameIntro>().oneOneStart = true;
        }
    }
}
