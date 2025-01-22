using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpulsePush : MonoBehaviour
{
    public float impulseMagnitude;
    public Vector3 impulseForce;

    void Awake()
    {
        impulseForce = transform.right * impulseMagnitude;
    }
}
