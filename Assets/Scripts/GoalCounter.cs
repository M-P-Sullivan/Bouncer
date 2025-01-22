using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoalCounter : MonoBehaviour
{
    public static GoalCounter instance;

    public GameObject levelGenerator;
    public Text goalCounter;
    public int totalGoals;
    public int goalsScored;
    public GameObject MenuManager;

    public void incrementGoals()
    {
        goalsScored++;
        goalCounter.text = $"{goalsScored}/{totalGoals}";

    }


    void Start()
    {
        MenuManager = GameObject.Find("MenuManager");
        totalGoals = MenuManager.GetComponent<MenuManager>().goalCount;
        //totalGoals = levelGenerator.GetComponent<LevelGenerator>().goalCount;
        instance = this;
        goalCounter.text = $"{goalsScored}/{totalGoals}";
        goalsScored = 0;
    }
}
