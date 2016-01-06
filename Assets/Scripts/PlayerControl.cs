using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerControl : MonoBehaviour {
    public float speed = 10;
    public float stamina = 0;
    public float stamina_reg = 15;
    public int player = 1;
    private string[] buttons = new string[4];
    private bool[] lastState = new bool[4];
    public bool keyboardControl;
    new private Rigidbody rigidbody;
	// Use this for initialization
	void Start () {
        buttons[0] = ("joystick " + player + " button " + Buttons.DASH);
        buttons[1] = ("joystick " + player + " button " + Buttons.SPRINT);
        this.rigidbody = this.transform.GetChild(0).GetComponent<Rigidbody>();
	}
	
	void Update() {
		if (keyDown("Dash"))
        {
            Dash();
        }
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

        rigidbody.AddForce(force.x*speed,0, force.y*speed);

        updateKeystates();
	}


    bool keyDown(string type)
    {
        switch (type)
        {
            case "Dash":
                if (Input.GetKeyDown(buttons[0]) || (keyboardControl && Input.GetKeyDown("x")))
                    return true;
                break;
            case "Sprint":
                if (Input.GetKey(buttons[1]) || (keyboardControl && Input.GetKey(KeyCode.LeftShift)))
                    return true;
                break;
        }
        return false;
    }

    void updateKeystates()
    {
        this.lastState[0] = Input.GetKey(buttons[0]);
    }

    void Dash()
    {
        if (stamina >= 70)
        {
            this.stamina -= 70;
            rigidbody.velocity *= 5;
        }
    }

}
