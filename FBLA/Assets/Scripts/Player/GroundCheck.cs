using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    //NOTE: Checks for ground, not too efficient, but it works!
    //TODO: If I have time, try to transfer to raycast based ground check

    public bool colliding;

    //When Colliding, Make Collide Bool True
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Ground")
            colliding = true;
    }

    //When Exiting Collide, Make Collide Bool False
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Ground")
            colliding = false;
    }
}
