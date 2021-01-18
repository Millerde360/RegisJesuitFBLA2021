using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    public AudioClip press;
    bool pressed = false;
    [Header("Set to true if this is a button that opens a door/elevator")]
    public bool DoorOrSpawner;
    [Header("Only do door gameobject if door enabled, otherwise do the box spawner")]
    public GameObject door;
    public GameObject spawner;

    public void Press()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<AudioSource>().PlayOneShot(press, .5f);
        if (DoorOrSpawner)
        {
            if (door.GetComponent<Door>() != null)
                door.GetComponent<Door>().Open();
            else if (!pressed && door.GetComponent<Door>() == null)
            {
                door.GetComponent<Elevator>().speed *= -1;
                door.GetComponent<BoxCollider>().center = new Vector3(door.GetComponent<BoxCollider>().center.x, door.GetComponent<BoxCollider>().center.y + 3.28f, door.GetComponent<BoxCollider>().center.z);
            }
        }
        else
        {
            spawner.GetComponent<Dispenser>().Dispense();
        }
        pressed = true;
    }
}
