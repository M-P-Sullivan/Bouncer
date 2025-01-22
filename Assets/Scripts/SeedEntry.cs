using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedEntry : MonoBehaviour
{
    private string seedString;
    private int seed;
    public GameObject MenuManager = null;
    public GameObject RejectionText;

    void Update()
    {
        MenuManager = GameObject.FindWithTag("MenuManager");
    }

    public void setSeed(string newSeed)
    {
        seedString = newSeed;
        int validSeed;
        bool validConversion = int.TryParse(seedString, out validSeed);

        if (validConversion && validSeed > -1 && validSeed < 1000000)
        {
            RejectionText.SetActive(false);
            seed = validSeed;
        }
        else
        {
            seed = MenuManager.GetComponent<MenuManager>().GetSeed();
            RejectionText.SetActive(true);
        }
    }

    public void TransmitSeed()
    {
        MenuManager.GetComponent<MenuManager>().UpdateSeed(seed);
    }

    public void Cancel()
    {
        seed = MenuManager.GetComponent<MenuManager>().GetSeed();
    }
}
