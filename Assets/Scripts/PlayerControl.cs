﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PlayerControl : MonoBehaviour {
    public float speed = 10000;
    private float force;
    private float mass;
    public float stamina = 0;
    public float stamina_reg = 15;
    public int player = 1;
    private string[] buttons = new string[5];
    public bool keyboardControl;
    new private Rigidbody rigidbody;
    public float jumpspeed = 50;
    public PlatformRow curRow;
    private bool grounded = true;
    private SphereCollider col;
    private Vector3 lastVel;
    private Vector3 dir;
    public float health = 100;
    //public Text hpTxt;
    public Transform healthBar;
    public float initHealthBarWidth;
    private float dashing = 0;
    private float dashingspeed = 20f;
    private float burnCounter;
    ParticleSystem psys;

	// Use this for initialization
	void Awake () {
        
        buttons[0] = ("joystick " + player + " button " + Buttons.DASH);
        buttons[1] = ("joystick " + player + " button " + Buttons.SPRINT);
        buttons[2] = ("joystick " + player + " button " + Buttons.INTERACTTOP);
        buttons[3] = ("joystick " + player + " button " + Buttons.INTERACTBOTTOM);
        buttons[4] = ("joystick " + player + " button " + Buttons.JUMP);
        this.rigidbody = this.GetComponent<Rigidbody>();
        this.col = this.GetComponent<SphereCollider>();
        this.mass = rigidbody.mass;
        this.force = speed;
        this.dir = transform.forward;
        this.healthBar = GameObject.Find("Player" + player + "gui").transform.FindChild("Health");
        this.initHealthBarWidth = this.healthBar.localScale.x;
        this.psys = GetComponent<ParticleSystem>();
    }
	
	void Update() {
		if (keyDown("Dash") && grounded)
        {
            Dash();
        }
        if ((keyDown("InteractTop") || keyDown("InteractBottom")) && grounded && !this.curRow.getIsMoving())
        {
            PlatformManager.instance.swapRow(this.curRow, keyDown("InteractTop"));
            //this.curPlat.changeRow(this.curField);
        }
        if (keyDown("Jump") && grounded)
        {
            this.rigidbody.velocity = new Vector3(this.rigidbody.velocity.x,jumpspeed, this.rigidbody.velocity.z);
        }
        if (burnCounter > 0)
            burnCounter -= Time.deltaTime;
        if (burnCounter < 0)
        {
            burnCounter = 0;
            psys.Stop();
            this.GetComponent<AudioSource>().Stop();
        }
        if (psys.particleCount < 10 && burnCounter <= 0)
            GetComponent<ParticleSystemRenderer>().enabled = false;
        if(dashing > 0)
            this.dashing -= Time.deltaTime;
        if (dashing < 0)
        {
            dashing = 0;
        }

	}
	
	// Update is called once per frame
	void FixedUpdate () {



        stamina += stamina_reg * Time.deltaTime;
        stamina = Mathf.Min(stamina, 100);
        

        Vector2 force = new Vector2(Input.GetAxis("Horizontal"+player), Input.GetAxis("Vertical"+player));
        if (keyboardControl)
            force = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        if(force.magnitude>1)
            force.Normalize();
        rigidbody.mass = dashing > 0 ? mass * Mathf.Max(rigidbody.velocity.magnitude * 2f, 3) : mass * Mathf.Max(rigidbody.velocity.magnitude, 3);
         this.force = speed * Mathf.Min(rigidbody.velocity.magnitude, 1);

        if (keyDown("Sprint"))
        {
            force *= 1.5f;
            this.stamina = Mathf.Max(this.stamina - 1f, 0);
        }
        Vector2 vel = calcVel(force);

        if (grounded)
        {
            rigidbody.velocity = new Vector3(vel.x, rigidbody.velocity.y, vel.y);
        }
        else
        {
            rigidbody.velocity = new Vector3(lastVel.x, rigidbody.velocity.y, lastVel.z);
            lastVel = lastVel + (new Vector3(vel.x, 0, vel.y) - new Vector3(lastVel.x, 0, lastVel.z)) * Time.deltaTime * 3f;
        }

        checkPlatforms();
        groundCheck();

        Vector3 velo = new Vector3(rigidbody.velocity.x, 0, rigidbody.velocity.z);
        if (velo.normalized.magnitude != 0)
            dir = velo.normalized;

        if (grounded)
            lastVel = rigidbody.velocity;

	}


    bool keyDown(string type)
    {
        switch (type)
        {
            case "Dash":
                if (Input.GetKeyDown(buttons[0]) || (keyboardControl && Input.GetKeyDown(KeyCode.X)))
                    return true;
                break;
            case "Sprint":
                if (Input.GetKey(buttons[1]) || (keyboardControl && Input.GetKey(KeyCode.LeftShift)))
                    return true;
                break;
            case "InteractTop":
                if (Input.GetKeyDown(buttons[2]) || (keyboardControl && Input.GetKeyDown(KeyCode.C)))
                    return true;
                break;
            case "InteractBottom":
                if (Input.GetKeyDown(buttons[3]) || (keyboardControl && Input.GetKeyDown(KeyCode.V)))
                    return true;
                break;
            case "Jump":
                if (Input.GetKeyDown(buttons[4]) || (keyboardControl && Input.GetKeyDown(KeyCode.Space)))
                    return true;
                break;
        }
        return false;
    }


    void Dash()
    {
        if (stamina >= 70)
        {
            this.stamina -= 70;
            this.dashing = 0.25f;
            rigidbody.velocity = dir * dashingspeed;
        }
    }

    void checkPlatforms()
    {
        RaycastHit hit;
        Vector3 down = transform.TransformDirection(Vector3.down);
        
        if (Physics.Raycast(transform.position, down, out hit, 2.5f, 1 << LayerMask.NameToLayer("World")))
        {
            PlatformRow row = hit.transform.GetComponent<PlatformRow>();

            if (row != null)
                this.curRow = row;

            if (hit.transform.name.Equals("Lava"))
            {
                this.hurtPlayer(PlatformManager.lavaDmg);
            }
        }
    }

    Vector2 calcVel(Vector2 force)
    {
        Vector2 vel = Vector2.zero;
        if (force.x >= 0)
        {
            if (rigidbody.velocity.x >= 0)
            {
                vel.x = Mathf.Max(force.x * speed, rigidbody.velocity.x);
            }
            else
            {
                vel.x = rigidbody.velocity.x + force.x * speed * 1f;
            }
        }
        else if (force.x < 0)
        {
            if (rigidbody.velocity.x <= 0)
            {
                vel.x = Mathf.Min(force.x * speed, rigidbody.velocity.x);
            }
            else
            {
                vel.x = rigidbody.velocity.x + force.x * speed * 1f;
            }
        }

        if (force.y >= 0)
        {
            if (rigidbody.velocity.z >= 0)
            {
                vel.y = Mathf.Max(force.y * speed, rigidbody.velocity.z);
            }
            else
            {
                vel.y = rigidbody.velocity.z + force.y * speed * 1f;
            }
        }
        else if (force.y < 0)
        {
            if (rigidbody.velocity.z <= 0)
            {
                vel.y = Mathf.Min(force.y * speed, rigidbody.velocity.z);
            }
            else
            {
                vel.y = rigidbody.velocity.z + force.y * speed * 1f;
            }
        }
        return vel;
    }

    void groundCheck()
    {
        grounded = Physics.CheckSphere(col.center + transform.position, col.radius, 1 << LayerMask.NameToLayer("World"));
    }

    /*
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Players"))
        {
            other.collider.attachedRigidbody.AddForce((other.transform.position - this.transform.position) * 1000f * Vector3.Cross(this.rigidbody.velocity , (other.transform.position - this.transform.position)).magnitude);
        //    other.collider.attachedRigidbody.velocity = other.collider.attachedRigidbody.velocity + other.impulse.normalized * 10f;
        //    this.rigidbody.velocity = this.rigidbody.velocity + other.impulse.normalized * -10f;
        }
    }
     * */

    public void hurtPlayer(float damage)
    {
        this.health -= damage;
        this.healthBar.localScale = new Vector3(this.healthBar.localScale.x + (health / 15f - this.healthBar.localScale.x) * 0.25f, 0.5f, 1);

        //this.hpTxt.text = "HP: " + (int)this.health;
        this.burnCounter = 0.5f;
        GetComponent<ParticleSystemRenderer>().enabled = true;
        psys.Play();
        if (!this.GetComponent<AudioSource>().isPlaying)
            this.GetComponent<AudioSource>().Play();

        if (this.health < 0)
        {
            //this.hpTxt.text = "DEAD";
            Destroy(this.gameObject);
            GameManager.instance.playerDied(this);
        }
    }
}
