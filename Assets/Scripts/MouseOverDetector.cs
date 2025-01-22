using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseOverDetector : MonoBehaviour
{
    public GameObject Spotter;

    void Start()
    {
        Spotter = GameObject.Find("Spotter");
    }

    public void OnMouseEnter()
    {
        if (!Spotter.GetComponent<Spotter>().isMouseOverUI)
        {
            Spotter.GetComponent<Spotter>().AddMouseOverObject(gameObject);
            Spotter.GetComponent<Spotter>().isMouseOverSprite = true;
        }
    }

    public void OnMouseExit()
    {
        Spotter.GetComponent<Spotter>().RemoveMouseOverObject(gameObject);
        Spotter.GetComponent<Spotter>().isMouseOverSprite = false;
    }
}
