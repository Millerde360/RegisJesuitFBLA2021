using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLevel1 : MonoBehaviour
{
    bool initiated = false;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player" && !initiated)
        {
            StartCoroutine(transform.parent.GetComponent<GameIntro>().endLevel(13.8f, 5f, 15f));
            initiated = true;
        }
    }
}
