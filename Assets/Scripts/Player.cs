using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using BouncerTools;
using UnityEngine.UI;


public class Player : MonoBehaviour {
    
    public float acceleration;
    public float brakeAccel;
    public float maxSpeed;
    public float boostPower;
    public float boostCooldown;
    public float brakeCooldown;
    public float boostMaxSpeedMultiplier;
    public float bouncyMaxSpeedMultiplier;
    public List<string> attributes = new List<string>();

    private Vector2 inputForce;
    private Vector2 movement;

    private float angle;
    private float initMaxSpeed;
    private float moveHorizontal;
    private float moveVertical;
    private float lastBoostTime = -100f;
    private float lastBrakeTime = -100f;
    
    private Vector2 pushVector;
    private Vector2 brakeVector;
    private Vector2 perpendicularMagnitude;
    
    private Rigidbody2D rb2d;
    private GameObject[] enemyObjects;
    private float counterForce;

    public GameObject BoostBar;
    public GameObject BrakeBar;
    public GameObject GameManager;
    public GameObject GuidanceController;

    private bool isStunned;
    public float stunTimer;
    private float lastStunTime;
    public GameObject StunX;
    private GameObject StunXInstance;

    public float noTeleportTimer = -100f;
    private float lastTeleportTime = -100f;

    public Text speedometer;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();                                 //Get and store a reference to the Rigidbody2D component so that we can access it
        initMaxSpeed = maxSpeed;
    }

    void Update()
    {
        speedometer.text = $"{Mathf.Round(rb2d.velocity.magnitude)} MPH";
    }

    void FixedUpdate()
    {
        if (isStunned)
        {
            if (lastStunTime + stunTimer > Time.time)
            {
                StunXInstance.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z - 1);
                return;
            }
            else
            {
                isStunned = false;
                Destroy(StunXInstance);
            }
        }
        if (GameManager.GetComponent<GameManager>().CurrentGameMode != GameMode.Playing)
        {
            return;
        }
        if (maxSpeed > initMaxSpeed)
        {
            if (initMaxSpeed > rb2d.velocity.magnitude)
            {
                maxSpeed -= 0.6f;
            }
            else
            {
                maxSpeed -= 0.3f;
            }
        }
        enemyObjects = GameObject.FindGameObjectsWithTag("Enemy Object");

        inputForce.x = 0;
        inputForce.y = 0;
        moveHorizontal = Input.GetAxis("Horizontal");                 
        moveVertical = Input.GetAxis("Vertical");                     
        movement = new Vector2(moveHorizontal, moveVertical);       
        inputForce = acceleration * movement;
        


        rb2d.AddForce(inputForce);
        
        if (GetComponent<Rigidbody2D>().velocity.magnitude > maxSpeed)
        {
            counterForce = maxSpeed - GetComponent<Rigidbody2D>().velocity.magnitude;
            rb2d.AddForce(counterForce * GetComponent<Rigidbody2D>().velocity.normalized, ForceMode2D.Impulse);
        }

        if (Input.GetButton("Boost") && Time.time > lastBoostTime + boostCooldown)
        {
            maxSpeed = Math.Max(maxSpeed, initMaxSpeed * boostMaxSpeedMultiplier);
            rb2d.AddForce(rb2d.velocity.normalized * boostPower, ForceMode2D.Impulse);
            lastBoostTime = Time.time;
            BoostBar.GetComponent<PowerBar>().BeginCooldown();
        }
        
        if (Input.GetButton("Brake") && Time.time > lastBrakeTime + brakeCooldown)
        {
            rb2d.velocity = new Vector2(0,0);
            lastBrakeTime = Time.time;
            BrakeBar.GetComponent<BrakeBar>().BeginCooldown();
        }

        if (Input.GetButtonDown("NextTarget") && GuidanceController.transform.childCount > 0)
        {
            Destroy(GuidanceController.transform.GetChild(0).gameObject);
        }


    }

    public void Teleport(GameObject Teleporter)
    {
        if (lastTeleportTime + noTeleportTimer > Time.time)
        {
            return;
        }
        else
        {
            transform.position = Teleporter.transform.GetChild(0).position;
            lastTeleportTime = Time.time;
        }
    }

    public void StunPlayer(float stunTime)
    {
        if (isStunned)
        {
            Destroy(StunXInstance);
        }
        else
        {
            lastStunTime = Time.time;  //Experimenting with stun not re-setting stun timer so you don't get stunlocked
        }
        isStunned = true;
        stunTimer = stunTime;
        StunXInstance = Instantiate(StunX, this.transform);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Bouncy")
        {
            maxSpeed = initMaxSpeed * bouncyMaxSpeedMultiplier;
        }
        if (col.gameObject.tag == "Stunner")
        {
            StunPlayer(stunTimer);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        GameObject Trigger = col.gameObject;
        if (Trigger.tag == "Teleporter")      //if it's a teleporter and it hasn't set a flag to prevent teleport loops/unintended extra teleports <WRITE CODE TO SET FLAG>
        {
            Teleport(Trigger);
        }
        if (Trigger.tag == "Impulse")
        {
            rb2d.velocity = new Vector2(0, 0);
            rb2d.AddForce(col.gameObject.GetComponent<ImpulsePush>().impulseForce, ForceMode2D.Impulse);
            maxSpeed = initMaxSpeed * bouncyMaxSpeedMultiplier;
        }
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.tag == "Push")           //Code for push floors, incorporate a vector along with the attribute Push in order for it to know where to add force and how much to add
        {
            rb2d.AddForce(col.gameObject.GetComponent<Push>().pushForce);
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Ice")
        {
            attributes.Remove("Ice");
        }
    }
}
