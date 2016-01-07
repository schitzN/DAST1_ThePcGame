using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerControl : MonoBehaviour {
    public float speed = 10;
    public float stamina = 0;
    public float stamina_reg = 15;
    public int player = 1;
    private string[] buttons = new string[4];
    public bool keyboardControl;
    new private Rigidbody rigidbody;
    public float jumpspeed = 50;
    public Platform curPlat;
    public absField curField;
    private bool grounded = true;
    private SphereCollider col;
    private Vector3 lastVel;
	// Use this for initialization
	void Start () {
        buttons[0] = ("joystick " + player + " button " + Buttons.DASH);
        buttons[1] = ("joystick " + player + " button " + Buttons.SPRINT);
        buttons[2] = ("joystick " + player + " button " + Buttons.INTERACT);
        buttons[3] = ("joystick " + player + " button " + Buttons.JUMP);
        this.rigidbody = this.GetComponent<Rigidbody>();
        this.col = this.GetComponent<SphereCollider>();
	}
	
	void Update() {
		if (keyDown("Dash") && grounded)
        {
            Dash();
        }
        if (keyDown("Interact") && grounded)
        {
            this.curPlat.changeRow(this.curField);
        }
        if (keyDown("Jump") && grounded)
        {
            this.rigidbody.velocity = new Vector3(this.rigidbody.velocity.x,jumpspeed, this.rigidbody.velocity.z);
        }
        if (grounded)
            lastVel = rigidbody.velocity;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        stamina += stamina_reg*Time.deltaTime;
        stamina = Mathf.Min(stamina, 100);
        

        Vector2 force = new Vector2(Input.GetAxis("Horizontal"+player), Input.GetAxis("Vertical"+player));
        if (keyboardControl)
            force = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        if(force.magnitude>1)
            force.Normalize();
        if(keyDown("Sprint"))
            force *= 2;
        if (grounded)
            rigidbody.AddForce(force.x * speed, 0, force.y * speed);
        else
            rigidbody.velocity = new Vector3(lastVel.x, rigidbody.velocity.y, lastVel.z);
        checkPlatforms();
        groundCheck();
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
            case "Interact":
                if (Input.GetKeyDown(buttons[2]) || (keyboardControl && Input.GetKeyDown(KeyCode.C)))
                    return true;
                break;
            case "Jump":
                if (Input.GetKeyDown(buttons[3]) || (keyboardControl && Input.GetKeyDown(KeyCode.Space)))
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
            rigidbody.velocity *= 5;
        }
    }

    void checkPlatforms()
    {
        RaycastHit hit;
        Vector3 down = transform.TransformDirection(Vector3.down);
        if (Physics.Raycast(transform.position, down, out hit, 10f))
        {
            Platform plat = hit.transform.GetComponent<Platform>();

            if (plat != null)
                this.curPlat = plat;

            absField field = hit.collider.transform.parent.GetComponent<absField>();

            if (field != null)
                this.curField = field;
        }
    }

    void groundCheck()
    {
        grounded = Physics.CheckSphere(col.center + transform.position, col.radius, 1 << LayerMask.NameToLayer("World"));
    }
}
