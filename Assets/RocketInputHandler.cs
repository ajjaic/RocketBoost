using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class RocketInputHandler : MonoBehaviour
{
    // messages
    void Update()
    {
        ProcessInput();
    }

    private void ProcessInput()
    {
        if (Input.GetButton("Thrust"))
        {
            print("thrust is pressed");   
        }

        if (Input.GetButton("RotateLeft"))
        {
            print("rotating left");   
        }
        else if (Input.GetButton("RotateRight"))
        {
            print("rotating right");   
        }
    }
}
