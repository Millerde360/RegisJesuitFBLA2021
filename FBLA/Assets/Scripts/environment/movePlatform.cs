using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movePlatform : MonoBehaviour
{
    public Vector3 pointA;
    public Vector3 pointB;
    public float speed;
    Vector3 deltaMove;
    Vector3 moveBack;
    int currentPoint = 1;
    public bool collidingWithPlayer;
    public bool stillCollidingWithPlayer;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (currentPoint == 0)
        {
            deltaMove = -(moveBack - Vector3.MoveTowards(transform.position, pointA, speed * Time.deltaTime));
            moveBack = Vector3.MoveTowards(transform.position, pointA, speed * Time.deltaTime);
            transform.position = Vector3.MoveTowards(transform.position, pointA, speed * Time.deltaTime);
            if (Vector3.Distance(transform.position, pointA) < 0.25f)
                currentPoint = 1;
        }
        else
        {
            deltaMove = -(moveBack - Vector3.MoveTowards(transform.position, pointB, speed * Time.deltaTime));
            moveBack = Vector3.MoveTowards(transform.position, pointB, speed * Time.deltaTime);
            transform.position = Vector3.MoveTowards(transform.position, pointB, speed * Time.deltaTime);
            if (Vector3.Distance(transform.position, pointB) < 0.25f)
                currentPoint = 0;
        }

        if (collidingWithPlayer)
        {
            stillCollidingWithPlayer = true;
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().platformVel = deltaMove;
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().noiseThreshold = deltaMove.magnitude;
        }
        else if (!collidingWithPlayer && stillCollidingWithPlayer)
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().platformVel = Vector3.zero;
            stillCollidingWithPlayer = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 9)
            collidingWithPlayer = true;
        if (other.gameObject.layer == 13)
            other.transform.parent = transform;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 9)
            collidingWithPlayer = false;
        if (other.gameObject.layer == 13)
            other.transform.parent = null;
    }
}