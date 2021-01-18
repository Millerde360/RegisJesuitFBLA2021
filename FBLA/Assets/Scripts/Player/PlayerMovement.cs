using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    //VARIABLES
    public CharacterController controller;
    public float speed = 12f;
    float vSpeed;
    #region FlipVars
    public float gravMultiplier;
    bool gravFlipInput;
    public bool flipping;
    public float flipTime;
    float curFlipTime;
    bool countdown;
    public bool DIRECTION = true;
    int flipGrav = 1;
    public GameObject groundCheck;
    bool turning;
    bool flipcancel = false;
    public float flipInterpolationRatio;
    #endregion
    public float raycastDistance;
    public Text interactText;
    public GameObject grabbedObject;
    public Vector3 platformVel;
    [Header("Footstep Stuff")]
    public AudioClip[] footstepNoises;
    public int footstepNum;
    public bool footNoisePlaying;
    public float stepGap;
    public float noiseThreshold;
    [Header("Object Grabber")]
    public float CorrectionForce = 50.0f;
    public float StabilizationFactor = 0.5f;
    public float PointDistance = 3.0f;
    public Camera camera;
    public bool multiplayer;


    void Start()
    {
        curFlipTime = flipTime;
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("joystick button 3"))
            multiplayer = true;

        //Get Input Axis & Apply to Movement Vector
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        Vector3 move = (transform.right * x + transform.forward * z).normalized;

        //CURRENT FLIPPING FUNCTION
        #region Flip
        //Flip Input
        if (Input.GetButton("Flip") && !multiplayer) gravFlipInput = true;
        else if (Input.GetButtonUp("Flip") && !multiplayer) gravFlipInput = false;

        if(Input.GetKeyDown("joystick button 3")) gravFlipInput = true;
        else if (Input.GetKeyUp("joystick button 3")) gravFlipInput = false;

        //Check If Flip Is Possible and If So, Flip
        if (gravFlipInput && !flipping && Grounded())
        {
            flipping = true;
            turning = true;

            int rotVal;
            if (transform.eulerAngles.z > 90 && transform.eulerAngles.z < 270)
            {
                rotVal = 0;
                DIRECTION = false;
                flipGrav = -1;
            }
            else
            {
                rotVal = 180;
                DIRECTION = true;
                flipGrav = 1;
            }

            StartCoroutine(Flip(transform.rotation, Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, rotVal)));
            return;
        }

        //Check if Flip is Completed
        if (flipping && Grounded())
            flipping = false;

        if (turning && (transform.eulerAngles.z == 180 || transform.eulerAngles.z == 0) && flipcancel)
        {
            turning = false;
            flipcancel = false;
        }
        #endregion

        //Apply Gravity
        vSpeed -= 9.8f * (DIRECTION ? -1 : 1) * Time.deltaTime;

        //Stops Downward Movement if Grounded
        if (Grounded())
            vSpeed = 0;

        Vector3 vertMove = new Vector3(0, vSpeed, 0);

        //Establish Final Vector
        move = move * speed * Time.deltaTime;
        vertMove = vertMove * gravMultiplier * Time.deltaTime;
        Vector3 ComMove = move + vertMove + platformVel;

        //Move Player Entity
        controller.Move(ComMove);

        //Get Viewed object and do something based upon object
        GameObject viewed = whatLookingAt();
        if(interactText != null)
            interactText.text = "";
        if (viewed != null)
        {
            if (viewed.CompareTag("Button"))
            {
                interactText.text = "Press E to Use";
                if(Input.GetKeyDown(KeyCode.E))
                    viewed.GetComponent<Button>().Press();
            }
            if (viewed.CompareTag("Block"))
            {
                if(grabbedObject == viewed)
                {
                    if (!multiplayer)
                    {
                        if (Input.GetMouseButtonDown(0))
                        {
                            grabbedObject = null;
                            return;
                        }
                        if (Input.GetMouseButtonDown(1))
                        {
                            grabbedObject.GetComponent<Block>().gravity *= -1;
                            grabbedObject = null;
                            return;
                        }
                    }
                    else
                    {
                        if (Input.GetKeyDown("joystick button 2"))
                        {
                            grabbedObject = null;
                            return;
                        }
                        if(Input.GetKeyDown("joystick button 5"))
                        {
                            grabbedObject.GetComponent<Block>().gravity *= -1;
                            grabbedObject = null;
                            return;
                        }
                    }
                }
                if(!multiplayer)
                    interactText.text = "Left click for pick up \n Right Click for gravity flip";
                if(multiplayer)
                    interactText.text = "X for pick up \n RB for gravity flip";

                if (Input.GetMouseButtonDown(0) && !multiplayer)
                    grabbedObject = viewed;
                if(Input.GetKeyDown("joystick button 2") && multiplayer)
                    grabbedObject = viewed;
            }

        }

        //Play footstep noise
        if(Grounded() && controller.velocity.magnitude > 2f+noiseThreshold)
        {
            if (!footNoisePlaying)
                StartCoroutine(FootNoisePlaying());
            else
                return;

            GetComponent<AudioSource>().PlayOneShot(footstepNoises[footstepNum], .25f);
            footstepNum++;
            if (footstepNum >= footstepNoises.Length)
                footstepNum = 0;
        }
    }

    void FixedUpdate()
    {
        if (grabbedObject == null)
            return;
        else
            interactText.text = "Left click for pick up \n Right Click for gravity flip";

        if (Input.GetMouseButtonDown(0) && !multiplayer)
        {
            grabbedObject = null;
            return;
        }
        if(Input.GetKeyDown("joystick button 2"))
        {
            grabbedObject = null;
            return;
        }

        if (Input.GetMouseButtonDown(1) && !multiplayer)
        {
            grabbedObject.GetComponent<Block>().gravity *= -1;
            grabbedObject = null;
            return;
        }
        if(Input.GetKeyDown("joystick button 5"))
        {
            grabbedObject.GetComponent<Block>().gravity *= -1;
            grabbedObject = null;
            return;
        }

        if (grabbedObject.GetComponent<Rigidbody>().constraints != RigidbodyConstraints.FreezeRotation)
            grabbedObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
        if (grabbedObject.GetComponent<Rigidbody>().useGravity)
            grabbedObject.GetComponent<Rigidbody>().useGravity = false;

        Vector3 targetPoint = camera.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        targetPoint += camera.transform.forward * PointDistance;
        Vector3 force = targetPoint - grabbedObject.GetComponent<Rigidbody>().transform.position;

        grabbedObject.GetComponent<Rigidbody>().velocity = force.normalized * grabbedObject.GetComponent<Rigidbody>().velocity.magnitude;
        grabbedObject.GetComponent<Rigidbody>().AddForce(force * CorrectionForce);

        grabbedObject.GetComponent<Rigidbody>().velocity *= Mathf.Min(1.0f, force.magnitude / 2);

        grabbedObject.GetComponent<Rigidbody>().AddForce(force * CorrectionForce);
    }

    IEnumerator Flip(Quaternion originalRot, Quaternion target)
    {
        transform.rotation = originalRot;
        float t = 0f;
        while (t < flipInterpolationRatio)
        {
            transform.rotation = Quaternion.Slerp(originalRot, target, t / flipInterpolationRatio);
            yield return null;
            t += Time.deltaTime;
        }

        transform.rotation = target;
    }

    //prevents noting starting rotation
    IEnumerator FlipCancel()
    {
        yield return new WaitForSeconds(1);
        flipcancel = true;
    }

    //Check if Grounded
    bool Grounded()
    {
        if (groundCheck.GetComponent<GroundCheck>().colliding)
            return true;
        else
            return false;
    }

    //Respawn for whatever reason
    public void Respawn(Vector3 position, Vector3 rotation)
    {
        //reset position
        GetComponent<CharacterController>().enabled = false;
        transform.position = position;
        GetComponent<CharacterController>().enabled = true;

        //reset gravity
        int rotVal;
        transform.eulerAngles = new Vector3 (rotation.x, rotation.y, 0);
        if (rotation.z > 90 && rotation.z < 270)
        {
            rotVal = 0;
            DIRECTION = false;
            flipGrav = -1;
        }
        else
        {
            rotVal = 180;
            DIRECTION = true;
            flipGrav = 1;
        }

        StartCoroutine(Flip(transform.rotation, Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, rotVal)));
    }

    IEnumerator FootNoisePlaying()
    {
        footNoisePlaying = true;
        yield return new WaitForSeconds(stepGap);
        footNoisePlaying = false;
    }

    //NOTE: This function gets what gameobject player is in front of, doesn't do anything with that, just returns it.
    GameObject whatLookingAt()
    {
        RaycastHit[] hit;
        hit = Physics.RaycastAll(transform.position, transform.forward, raycastDistance);
        foreach (var HIT in hit)
        {
            if (HIT.collider.gameObject.tag != "Player" && HIT.collider.tag != "Ground" && HIT.collider.tag != null)
                return HIT.collider.gameObject;
        }
        return null;
    }
}
