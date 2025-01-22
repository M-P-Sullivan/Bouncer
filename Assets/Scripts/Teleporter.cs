using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    public int noTeleRepeatFlag;
    public GameObject TeleporterExitPrefab;


    public void GenerateExit(Vector3 exitLocation)
    {
        Instantiate(TeleporterExitPrefab, exitLocation, Quaternion.identity, transform);
    }
}
