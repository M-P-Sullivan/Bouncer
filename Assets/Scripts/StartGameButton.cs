﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGameButton : MonoBehaviour
{
    public int gameplayScene;

    public void StartGame()
    {
        SceneManager.LoadScene(gameplayScene);
    }
}
