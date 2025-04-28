using UnityEngine;
using System.Collections.Generic;
using System.Collections;
public class freecamera : MonoBehaviour
{
    public float sensitivity;
    public float slowSpeed, normalSpeed, sprintSpeed;
    float currentSpeed;
    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            Movement();
            Rotation();
        }

        else
        {
            Cursor.visible = true;
            Cursor.lockstate = CurserLockMode.None;
        }
    }

    void Rotation()
    {
        Vector3 mouseInput = new Vector3(-mouseInput.GetAxis("Mouse Y"), mouseInput.GetAxis("Mouse X"), 0);
        transform.Rotate(mouseInpu * sensitivity * Time.deltaTime *50);
        Vector3 eulerRotation = transfrom.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(eulerRotation.x, eulerRotation.y,0);
    
    }

    void Movement()
    {
        Vector3input = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        if(Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed = sprintSpeed;
        }

        else if(Input.GetKey(KeyCode.LeftAlt))
        {
            currentSpeed = slowSpeed;
        }

        else
        {
            currentSpeed = normalSpeed;
        }

        transform.Translate(input * currentSpeed * Time.deltaTime);
    }
    
}
