/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArchivedCode : MonoBehaviour
{
        PLAY CODE::
    
        ICE CODE
        if (attributes.Contains("Ice"))                                     //Check if on ice - <<MAY WANT TO UPDATE THIS TO A SWITCH CHECKING FOR VARIOUS ELEMENTS>>
        {
            inputForce = ((acceleration * 0.4f) * movement);
        }
        else
        {
            inputForce = acceleration * movement;
            if (Input.GetButton("Brake"))
            {
                brakeVector = GetComponent<Rigidbody2D>().velocity.normalized * -1 * brakeAccel;
                angle = Vector2.Angle(inputForce, brakeVector);
                inputForce = brakeVector + inputForce - (Convert.ToSingle(Math.Cos(angle)) * inputForce.magnitude * brakeVector.normalized);  //Uses vector math to find the component of inputForce that is perpendicular to the brake direction and only allow that through
            }
        }

        BRAKE CODE
        if (Input.GetButton("Brake"))
        {
            brakeVector = GetComponent<Rigidbody2D>().velocity.normalized * -1 * brakeAccel;
            angle = Vector2.Angle(inputForce, brakeVector);
            inputForce = brakeVector + inputForce - (Convert.ToSingle(Math.Cos(angle)) * inputForce.magnitude * brakeVector.normalized);  //Uses vector math to find the component of inputForce that is perpendicular to the brake direction and only allow that through
        }

}
*/