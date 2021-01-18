using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLook : MonoBehaviour
{
    //VARIABLES
    public Transform playerBody;
    public float mouseSensitivity = 100f;

    float xRot = 0f;

    void Start()
    {
        //Lock Cursor to Center of Screen
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        //Get Mouse Input
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        //Rotate Entire Player Entity on X-axis
        playerBody.Rotate(Vector3.up * mouseX);

        //Rotate Camera Up/Down
        xRot -= mouseY;
        xRot = Mathf.Clamp(xRot, -90f, 90f);
        transform.localRotation = Quaternion.Euler(xRot, 0f, 0f);
    }
}
