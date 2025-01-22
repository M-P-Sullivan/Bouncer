using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BouncerTools;

public class TimeController : MonoBehaviour
{
    public static TimeController instance;

    public Text timeCounter;
    public Text gameMode;

    private TimeSpan timePlaying;
    public bool TimerGoing { get; set; }
    private float startTime;
    public float elapsedTime;

    private float timeRemaining;
    private int gameModeNum;

    public GameObject GameManager;
    public GameObject MenuManager;
    public GameObject PauseUI;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        if (MenuManager == null)
        {
            MenuManager = GameObject.Find("MenuManager");
        }

        gameModeNum = 0;
        timeCounter.text = "00:00.00";
        TimerGoing = false;
        BeginSpotterTimer();
        Resume();
    }
    
    void Update()
    {

        if (Input.GetButtonDown("Escape"))
        {
            if (Time.timeScale == 0.0f)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public int getMode()
    {
        return gameModeNum;
    }

    public void SetGameModeText(string newGameMode)
    {
        gameMode.text = newGameMode;
    }

    public void BeginSpotterTimer()
    {
        TimerGoing = true;
        timeRemaining = MenuManager.GetComponent<MenuManager>().spotTime;
        elapsedTime = 0f;

        StartCoroutine(SpotterTimerDown());
    }

    public void BeginPlayTimerUp()
    {
        TimerGoing = true;
        startTime = Time.time;
        elapsedTime = 0f;
        StartCoroutine(PlayTimerUp());
    }

    public void BeginPlayTimerDown()
    {
        TimerGoing = true;
        timeRemaining = MenuManager.GetComponent<MenuManager>().playTime;
        elapsedTime = 0f;

        StartCoroutine(PlayTimerDown());
    }

    private IEnumerator PlayTimerUp()
    {
        while (TimerGoing)
        {
            elapsedTime += Time.deltaTime;
            timePlaying = TimeSpan.FromSeconds(elapsedTime);
            timeCounter.text = timePlaying.ToString("mm':'ss'.'f");

            yield return null;
        }
    }

    private IEnumerator SpotterTimerDown()
    {
        while (TimerGoing)
        {
            timeRemaining -= Time.deltaTime;
            timePlaying = TimeSpan.FromSeconds(timeRemaining);
            timeCounter.text = timePlaying.ToString("mm':'ss'.'f");


            if (timeRemaining <= 0)
            {
                GameManager.GetComponent<GameManager>().SetModeToReady();
            }

            yield return null;
        }
    }

    private IEnumerator PlayTimerDown()
    {
        while (TimerGoing)
        {
            timeRemaining -= Time.deltaTime;
            timePlaying = TimeSpan.FromSeconds(timeRemaining);
            timeCounter.text = timePlaying.ToString("mm':'ss'.'f");

            if (timeRemaining <= 0)
            {
                GameManager.GetComponent<GameManager>().SetModeToComplete();
            }

            elapsedTime = timeRemaining;
            yield return null;
        }
    }

    public void SetModeToReady()
    {
        SetGameModeText("Ready Mode");
        timePlaying = TimeSpan.Zero;
        timeCounter.text = timePlaying.ToString("mm':'ss'.'f");
        TimerGoing = false;
    }

    public void SetModeToPlay()
    {
        SetGameModeText("Play Mode");
        if (GameManager.GetComponent<GameManager>().CurrentGameVersion == GameVersion.Goal)
        {
            BeginPlayTimerUp();
        }
        else if (GameManager.GetComponent<GameManager>().CurrentGameVersion == GameVersion.Clock)
        {
            BeginPlayTimerDown();
        }

    }

    public void SetModeToComplete()
    {
        TimerGoing = false;
        SetGameModeText("Game Over");
    }

    public void Pause()
    {
        Time.timeScale = 0.0f;
        PauseUI.SetActive(true);
    }

    public void Resume()
    {
        Time.timeScale = 1.0f;
        PauseUI.SetActive(false);
    }
}
