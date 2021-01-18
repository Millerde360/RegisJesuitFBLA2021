using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AcidWater : MonoBehaviour
{

    #region Variables
    public GameObject respawn;
    public float fadeTime;
    public float fadeTimeCur = 0;
    public bool whitingOut;
    bool returning;
    bool returnToUnwhite; //Delete once IEnumerator removed; see below
    bool returnToTransparent; //Delete once IEnumerator removed; see below

    public GameObject Player;
    public Image whiteOut;
    #endregion


    // Update is called once per frame
    void Update()
    {
        //white out screen when hit
        if (whitingOut)
        {
            Color newCol = new Color(0, 0, 0, fadeTimeCur/fadeTime);
            whiteOut.color = newCol;

            StartCoroutine(CancelWhiteOut());

            fadeTimeCur += Time.deltaTime;

            if(returnToUnwhite)
            {
                returning = true;

                //respawn player
                Player.GetComponent<PlayerMovement>().Respawn(respawn.transform.position, respawn.transform.eulerAngles);

                fadeTimeCur = fadeTime;
                whitingOut = false;
            }
        }

        //unwhite screen
        if (returning)
        {
             Debug.Log("Unwhiting!");
             Color newCol = new Color(0, 0, 0, fadeTimeCur/fadeTime);
             whiteOut.color = newCol;

            StartCoroutine(CancelTransparency());

             fadeTimeCur -= Time.deltaTime;

            //snap to transparent whiteout
             if (returnToTransparent)
             {
                Color transparent = new Color(0, 0, 0, 0);
                whiteOut.color = transparent;
                fadeTimeCur = 0;
                 returning = false;
             }
            
        }
    }

    //Check for player collision
    void OnTriggerStay (Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //begin respawn process
            whitingOut = true;
            Debug.Log("RIP");
        }
    }


    //NOTE: must use these two IEnumerator for the time being as time randomly stops ticking up when using deltatime??? Weird... But this is particularly innefficient but will do the job
    //TODO: not important, but if I have time, change back to deltatime or absolutetime

    //Once whiting out started, this coroutine will begin the return to transparent process
    IEnumerator CancelWhiteOut()
    {
        yield return new WaitForSeconds(.25f);
        //keep only one instance of this coroutine
        if (returnToUnwhite)
        {
            Debug.Log("BREAK");
            yield break;
        }
        returnToUnwhite = true;
        yield return new WaitForSeconds(.1f);
        returnToUnwhite = false;
    }

    IEnumerator CancelTransparency()
    {
        yield return new WaitForSeconds(.25f);
        //keep only one instance of this coroutine
        if (returnToTransparent)
        {
            Debug.Log("BREAK");
            yield break;
        }
        returnToTransparent = true;
        yield return new WaitForSeconds(.1f);
        returnToTransparent = false;
    }
}
