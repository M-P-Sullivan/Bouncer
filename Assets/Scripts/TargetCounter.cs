using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetCounter : MonoBehaviour
{
    public static TargetCounter instance;

    public Text targetCounter;
    public int totalTargets;
    public int TargetsRemaining { get; set; }

    public GameObject Spotter;
    public GameObject Item;

    void Start()
    {
        instance = this;
        totalTargets = Item.GetComponent<SpotterItems>().totalItems;
        TargetsRemaining = totalTargets;
        targetCounter.text = $"{TargetsRemaining}/{totalTargets}";
    }

    void Update()
    {
        totalTargets = Item.GetComponent<SpotterItems>().totalItems;
        TargetsRemaining = Item.GetComponent<SpotterItems>().itemsInInventory;
        targetCounter.text = $"{TargetsRemaining}/{totalTargets}";
    }

    public void DecrementTargets()
    {
        TargetsRemaining--;
        targetCounter.text = $"{TargetsRemaining}/{totalTargets}";
    }

}
