using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIElementPointerDetector : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject Spotter;
    
    void Start()
    {
        Spotter = GameObject.Find("Spotter");
    }

    //Detect if the Cursor starts to pass over the GameObject
    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        Spotter.GetComponent<Spotter>().AddMouseOverObject(gameObject);
        Spotter.GetComponent<Spotter>().isMouseOverUI = true;
    }

    //Detect when Cursor leaves the GameObject
    public void OnPointerExit(PointerEventData pointerEventData)
    {
        Spotter.GetComponent<Spotter>().RemoveMouseOverObject(gameObject);
        Spotter.GetComponent<Spotter>().isMouseOverUI = false;
    }
}
