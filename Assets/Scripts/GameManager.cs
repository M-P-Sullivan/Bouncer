using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BouncerTools;

public class GameManager : MonoBehaviour {

    public static GameManager instance;

    public GameObject TimeControllerObject;
    public GameObject SpotterUI;
    public GameObject Spotter;
    public GameObject Player;
    public GameObject MainCamera;
    public GameObject GoalCounter;
    public GameObject EndGameMenu;
    public GameObject MenuManager;
    public GameObject PlayerUI;

    public GameMode CurrentGameMode { get; set; }
    public GameVersion CurrentGameVersion { get; set; }


    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        MenuManager = GameObject.Find("MenuManager");
        CurrentGameMode = GameMode.Spotting;
        CurrentGameVersion = MenuManager.GetComponent<MenuManager>().Mode;
        EndGameMenu.SetActive(false);
        PlayerUI.SetActive(false);
    }

    private void Update()
    {
        switch (CurrentGameMode)
        {
            case GameMode.Spotting:
                if (Input.GetButtonDown("StartGame"))
                {
                    SetModeToReady();
                }
                break;
            case GameMode.Ready:
                if (Input.GetButtonDown("StartGame"))
                {
                    SetModeToPlay();
                }
                break;
            case GameMode.Playing:
                if (TimeControllerObject.GetComponent<TimeController>().TimerGoing == false)
                {
                    TimeControllerObject.GetComponent<TimeController>().SetModeToPlay();
                }
                if (GoalCounter.GetComponent<GoalCounter>().goalsScored == GoalCounter.GetComponent<GoalCounter>().totalGoals)
                {
                    SetModeToComplete();
                }
                break;
            case GameMode.Complete:
                break;
            default:
                break;
        }
    }

    public void SetModeToPlay()
    {
        CurrentGameMode = GameMode.Playing;
        PlayerUI.SetActive(true);
        MainCamera.GetComponent<CameraFollow>().setMode(2);
    }


    public void SetModeToReady()
    {
        CurrentGameMode = GameMode.Ready;
        TimeControllerObject.GetComponent<TimeController>().SetModeToReady();
        SpotterUI.SetActive(false);
        Spotter.SetActive(false);
        MainCamera.GetComponent<CameraFollow>().setPlayer(Player);
        MainCamera.GetComponent<CameraFollow>().mainCamera.orthographicSize = MainCamera.GetComponent<CameraFollow>().defaultPlayCameraSize;
        GameObject PlayerStart = GameObject.FindWithTag("PlayerStart");
        if (PlayerStart != null)
        {
            Player.transform.position = PlayerStart.transform.position;
        }
        MainCamera.transform.position = new Vector3(Player.transform.position.x, Player.transform.position.y, MainCamera.transform.position.z);
        Destroy(PlayerStart);
        Destroy(Spotter.GetComponent<Spotter>().HandImage);
        Destroy(Spotter.GetComponent<Spotter>().LineParent);
    }

    public void SetModeToComplete()
    {
        CurrentGameMode = GameMode.Complete;
        TimeControllerObject.GetComponent<TimeController>().SetModeToComplete();
        float runtime = TimeControllerObject.GetComponent<TimeController>().elapsedTime;
        int goalsScored = GoalCounter.GetComponent<GoalCounter>().goalsScored;
        float result;
        bool isTime;
        if (goalsScored == GoalCounter.GetComponent<GoalCounter>().totalGoals)
        {
            result = runtime;
            isTime = true;
        }
        else
        {
            result = (float)goalsScored;
            isTime = false;
        }
        MenuManager.GetComponent<MenuManager>().RunResult(isTime, result);
        EndGameMenu.SetActive(true);

    }
}
