using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonScript : MonoBehaviour
{
    public GameObject MenuManager = null;
    public MenuManager MenuScript;


    void Update()
    {
        if (MenuManager == null)
        {
            MenuManager = GameObject.Find("MenuManager");
            MenuScript = MenuManager.GetComponent<MenuManager>();
        }
    }

    public void MenuManagerSetMode(bool isModeGoals)
    {
        MenuScript.SetMode(isModeGoals);
    }

    public void MenuManagerQuitGame()
    {
        MenuScript.QuitGame();
    }

    public void MenuManagerRandomizeSeed()
    {
        MenuScript.RandomizeSeed();
    }

    public void MenuManangerBeginRun()
    {
        MenuScript.BeginRun();
    }

    public void MenuManagerSetPlayTime(float playTime)
    {
        MenuScript.playTime = playTime;
    }

    public void MenuManagerSetSpotTime(float spotTime)
    {
        MenuScript.spotTime = spotTime;
    }

    public void MenuManagerSetGoalCount(int goalCount)
    {
        MenuScript.goalCount = goalCount;
    }

    public void MenuPresetShort()
    {
        MenuScript.PresetShort();
    }

    public void MenuPresetMedium()
    {
        MenuScript.PresetMedium();
    }

    public void MenuPresetLong()
    {
        MenuScript.PresetLong();
    }

    public void MenuPresetVeryLong()
    {
        MenuScript.PresetVeryLong();
    }

    public void MenuUpdateRuns()
    {
        MenuScript.UpdateRuns();
    }

    public void Click()
    {
        gameObject.GetComponent<Button>().onClick.Invoke();
    }
}
