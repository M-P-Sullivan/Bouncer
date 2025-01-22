using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BouncerTools;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    private static MenuManager instance;
    public static MenuManager Instance
    {
        get { return instance; }
    }

    public int seed;
    public string seedString;
    public BouncerTools.GameVersion Mode { get; set; }
    public float playTime;
    public float spotTime;
    public int goalCount;

    public TextMeshProUGUI seedTextMesh;

    public GameObject NewResult;
    public Vector3 resultLocation;
    public float resultYMovement;
    public float resultXMovement;

    private RunResult CurrentRun;
    public bool runComplete = false;

    public int targetCount;
    public int boostCount;
    public int bouncePadCount;
    public int teleportCount;

    public bool resetSettingsYet = false;


    System.Random rand;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        rand = new System.Random();
        UpdateSeed(rand.Next(999999));
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            if (seedTextMesh == null && GameObject.Find("Seed Value") != null)
            {
                seedTextMesh = GameObject.Find("Seed Value").GetComponent<TextMeshProUGUI>();
                RandomizeSeed();
            }
            if (runComplete == true)
            {
                UpdateRuns();
            }
            if (resetSettingsYet == false)
            {
                playTime = 45;
                spotTime = 120;
                goalCount = 10;
                targetCount = 30;
                boostCount = 3;
                bouncePadCount = 2;
                teleportCount = 1;
                SetMode(false);
                resetSettingsYet = true;
            }
        }

        if (resetSettingsYet == true && SceneManager.GetActiveScene().buildIndex == 1)
        {
            resetSettingsYet = false;
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void UpdateSeed(int newSeed)
    {
        seed = newSeed;
        seedString = seed.ToString("D6");
        seedTextMesh.SetText(seedString);

    }

    public void RandomizeSeed()
    {
        UpdateSeed(rand.Next(999999));
    }

    public int GetSeed()
    {
        return seed;
    }

    public void BeginRun()
    {
        CurrentRun = new RunResult();
        CurrentRun.runDateTime = DateTime.Now;
        CurrentRun.mode = Mode;
        CurrentRun.seed = seed;
    }

    public void RunResult(bool isTime, float result)
    {
        CurrentRun.result = result;
        CurrentRun.resultIsTime = isTime;
        runComplete = true;
    }

    public void UpdateRuns()
    {
        Transform tempTransform;
        GameObject Reference = GameObject.Find("ReferenceStorage");
        GameObject Results = Reference.GetComponent<ReferenceStorage>().Results;
        Debug.Log("Check: ");
        Debug.Log(Results);
        for (int i = 0; i < Results.transform.childCount; i++)
        {
            if (i < 5 || (i > 5 && i < 12))
            {
                tempTransform = Results.transform.GetChild(i);
                Results.transform.GetChild(i).position = new Vector3(tempTransform.position.x, tempTransform.position.y + resultYMovement, tempTransform.position.z);
            }
            else if (i == 5)
            {
                tempTransform = Results.transform.GetChild(i);
                Results.transform.GetChild(i).position = new Vector3(tempTransform.position.x + resultXMovement, tempTransform.position.y - 5*resultYMovement, tempTransform.position.z);
            }
            else
            {
                Destroy(Results.transform.GetChild(i).gameObject);
            }
        }

        //Instantiate(NewResult, resultLocation, Quaternion.identity, Results.transform);
        PopulateRun(NewResult);
        Debug.Log(Instantiate(NewResult, Results.transform, false));
        runComplete = false;
    }

    public void PopulateRun(GameObject Result)
    {
        Result.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = CurrentRun.DateString();
        Result.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = CurrentRun.ModeString();
        Result.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().text = CurrentRun.ResultString();
        Result.transform.GetChild(3).gameObject.GetComponent<TextMeshProUGUI>().text = CurrentRun.SeedString();
    }

    public void SetMode(bool isGoals)
    {
        if (isGoals)
        {
            Mode = BouncerTools.GameVersion.Goal;
        }
        else
        {
            Mode = BouncerTools.GameVersion.Clock;
        }
    }

    public void PresetShort()
    {
        targetCount = 15;
        boostCount = 2;
        bouncePadCount = 1;
        teleportCount = 0;
    }

    public void PresetMedium()
    {
        targetCount = 30;
        boostCount = 3;
        bouncePadCount = 2;
        teleportCount = 1;
    }

    public void PresetLong()
    {
        targetCount = 40;
        boostCount = 5;
        bouncePadCount = 3;
        teleportCount = 2;
    }

    public void PresetVeryLong()
    {
        targetCount = 50;
        boostCount = 6;
        bouncePadCount = 4;
        teleportCount = 2;
    }
}
