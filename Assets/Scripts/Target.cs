using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BouncerTools;

public class Target : MonoBehaviour
{
    private GameObject Player;
    private GameObject TargetCounter;
    private GameObject GameManager;

    public float playerTriggerDistance;

    void Awake()
    {
        Player = GameObject.FindWithTag("Player");
        TargetCounter = GameObject.FindWithTag("TargetCounter");
        GameManager = GameObject.FindWithTag("GameManager");
    }

    void Update()
    {
        if (transform.parent.GetChild(0) == transform && Vector3.Distance(transform.position, Player.transform.position) < playerTriggerDistance && GameManager.GetComponent<GameManager>().CurrentGameMode == GameMode.Playing)
        {
            Destroy(gameObject);
        }
    }
}
