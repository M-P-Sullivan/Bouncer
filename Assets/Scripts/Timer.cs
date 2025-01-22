using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Timer : MonoBehaviour
{ 
    //public Text myText = "new";

    private float timeElapsed = 0f;
    void Update()
    {
        timeElapsed += Time.deltaTime;
    }
}
