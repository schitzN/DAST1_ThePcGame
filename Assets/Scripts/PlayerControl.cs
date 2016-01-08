using UnityEngine;
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
    public Transform healthBar;
    private float dashing = 0;
    private float dashingspeed = 22f;
    private float burnCounter;
    ParticleSystem psys;
    CameraShake camshake;

	// Use this for initialization
	void Start () {
        Debug.Log(player);
        buttons[0] = ("joystick " + player + " button " + Buttons.DASH);
        buttons[1] = ("joystick " + player + " button " + Buttons.SPRINT);
        buttons[2] = ("joystick " + player + " button " + Buttons.INTERACTTOP);
        buttons[3] = ("joystick " + player + " button " + Buttons.INTERACTBOTTOM);
        buttons[4] = ("joystick " + player + " button " + Buttons.JUMP);
        this.rigidbody = this.GetComponent<Rigidbody>();
        this.col = this.GetComponent<SphereCollider>();
        this.mass = rigidbody.mass;
        this.force = speed;
        this.dir = transform.right;
        this.psys = GetComponent<ParticleSystem>();

        this.healthBar = GameObject.Find("Player" + this.player + "gui").transform.FindChild("Health");
        this.healthBar.localScale = new Vector3(GameManager.instance.defaultStaminaBar, this.healthBar.localScale.y, this.healthBar.localScale.z);
        this.camshake = GameObject.Find("Main Camera").GetComponent<CameraShake>();
    }
	
	void Update() {
		if (keyDown("Dash") && grounded)
        {
            Dash();
        }
        if ((keyDown("InteractTop") || keyDown("InteractBottom")) && grounded && !this.curRow.getIsMoving() && this.stamina >= 15)
        {
            stamina -= 15;
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
            this.transform.GetChild(1).gameObject.SetActive(false);
            dashing = 0;
        }

	}
	
	// Update is called once per frame
	void FixedUpdate () {



        stamina += stamina_reg * Time.deltaTime;
        stamina = Mathf.Min(stamina, 100);
        

        Vector2 force = new Vector2(Input.GetAxis("Horizontal"+player), Input.GetAxis("Vertical"+player));
        if (keyboardControl && force.magnitude == 0)
            force = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        if(force.magnitude>1)
            force.Normalize();
        rigidbody.mass = dashing > 0 ? mass * Mathf.Max(rigidbody.velocity.magnitude * 500f, 3) : mass * Mathf.Max(rigidbody.velocity.magnitude, 3);
         this.force = speed * Mathf.Min(rigidbody.velocity.magnitude, 1);

        if (keyDown("Sprint") && stamina > 0)
        {
            this.stamina = Mathf.Max(this.stamina - 1.2f, 0);
            if (stamina > 0)
                force *= 1.8f;
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
                if (Input.GetKeyDown(buttons[0]) || (keyboardControl && Input.GetKeyDown(KeyCode.Y)))
                    return true;
                break;
            case "Sprint":
                if (Input.GetAxis("Trigger"+player)!=0 || (keyboardControl && Input.GetKey(KeyCode.LeftShift)))
                    return true;
                break;
            case "InteractTop":
                if (Input.GetKeyDown(buttons[2]) || (keyboardControl && Input.GetKeyDown(KeyCode.C)))
                    return true;
                break;
            case "InteractBottom":
                if (Input.GetKeyDown(buttons[3]) || (keyboardControl && Input.GetKeyDown(KeyCode.X)))
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
        if (stamina >= 55)
        {
            Vector3 dash = new Vector3(Input.GetAxis("Horizontal" + player),0, Input.GetAxis("Vertical" + player));
            this.GetComponents<AudioSource>()[2].Play();
            this.stamina -= 55;
            this.dashing = 0.4f;
            if(dash.magnitude == 0)
                rigidbody.velocity = dir * dashingspeed;
            else
                rigidbody.velocity = dash.normalized * dashingspeed;
            this.transform.GetChild(1).gameObject.SetActive(true);
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
        float bounceLimit = 6f;
        float learningRate = 0.2f;
        if (force.x >= 0)
        {
            if (rigidbody.velocity.x >= 0)
            {
                vel.x = Mathf.Max(force.x * speed, rigidbody.velocity.x);
            }
            else if (rigidbody.velocity.x >= -bounceLimit)
            {
                vel.x = rigidbody.velocity.x + force.x * speed * 1f;
            }
            else {
                vel.x = rigidbody.velocity.x + force.x * speed * learningRate * Time.deltaTime;
            }
        }
        else if (force.x < 0)
        {
            if (rigidbody.velocity.x <= 0)
            {
                vel.x = Mathf.Min(force.x * speed, rigidbody.velocity.x);
            }
            else if (rigidbody.velocity.x <= bounceLimit)
            {
                vel.x = rigidbody.velocity.x + force.x * speed * 1f;
            }
            else
            {
                vel.x = rigidbody.velocity.x + force.x * speed * learningRate * Time.deltaTime;
            }
        }

        if (force.y >= 0)
        {
            if (rigidbody.velocity.z >= 0)
            {
                vel.y = Mathf.Max(force.y * speed, rigidbody.velocity.z);
            }
            else if (rigidbody.velocity.z >= -bounceLimit)
            {
                vel.y = rigidbody.velocity.z + force.y * speed * 1f;
            }
            else
            {
                vel.y = rigidbody.velocity.z + force.y * speed * learningRate * Time.deltaTime;
            }
        }
        else if (force.y < 0)
        {
            if (rigidbody.velocity.z <= 0)
            {
                vel.y = Mathf.Min(force.y * speed, rigidbody.velocity.z);
            }
            else if (rigidbody.velocity.z <= bounceLimit)
            {
                vel.y = rigidbody.velocity.z + force.y * speed * 1f;
            }
            else
            {
                vel.y = rigidbody.velocity.z + force.y * speed * learningRate * Time.deltaTime;
            }
        }
        return vel;
    }

    void groundCheck()
    {
        grounded = Physics.CheckSphere(col.center + transform.position, col.radius, 1 << LayerMask.NameToLayer("World"));
    }

    
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Players") && other.impulse.magnitude > 1500)
        {
            this.GetComponents<AudioSource>()[1].Play();
            if (other.impulse.magnitude > 5000)
            {
                camshake.shake = 0.5f;
                camshake.shakeAmount = 0.5f;
            }
        }
    }

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
            transform.GetChild(0).gameObject.SetActive(true);
            transform.GetChild(0).parent = transform.root;
            Destroy(this.gameObject);
            GameManager.instance.playerDied(this);
            camshake.shake = 0.8f;
            camshake.shakeAmount = 1.3f;
        }
    }
}
