using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : Button
{
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Block"))
            Press();
    }
}
