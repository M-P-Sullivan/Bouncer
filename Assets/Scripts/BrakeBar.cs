using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BrakeBar : MonoBehaviour
{
    public GameObject player;
    private float cooldown;
    private Image cooldownImage;
    private float cooldownStartTime;

    void Awake()
    {
        cooldown = player.GetComponent<Player>().brakeCooldown;
        cooldownStartTime = -100f;
    }

    void Start()
    {
        cooldownImage = GetComponent<Image>();
        cooldownImage.fillAmount = 0.5f;
    }

    void Update()
    {
        float elapsedTime = Time.time - cooldownStartTime;
        if (cooldownStartTime + cooldown > Time.time)
        {
            float fillAmount = elapsedTime / cooldown;
            cooldownImage.fillAmount = fillAmount;
        }
        else
        {
            cooldownImage.fillAmount = 1f;
        }
    }

    public void BeginCooldown()
    {
        cooldownImage.fillAmount = 0f;
        cooldownStartTime = Time.time;
        Debug.Log("Cooldown");
    }
}
