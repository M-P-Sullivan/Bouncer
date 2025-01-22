using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameMenuManager : MonoBehaviour
{
    public int menuScene;

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(menuScene);
    }
}
