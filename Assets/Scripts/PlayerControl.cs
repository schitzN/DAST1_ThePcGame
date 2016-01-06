using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerControl : MonoBehaviour {
    public float speed = 10;
    public float stamina = 0;
    public float stamina_reg = 15;
    public float player = 1;
    private string[] buttons = new string[4];
    private bool[] lastState = new bool[4];
	// Use this for initialization
	void Start () {
        buttons[0] = ("joystick " + player + " button " + Buttons.DASH);
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
        

        Vector2 force = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        if(force.magnitude>1)
            force.Normalize();
        if(Input.GetAxis("Sprint") == 1)
            force *= 2;

        this.GetComponent<Rigidbody>().AddForce(force.x*speed,0, force.y*speed);

        updateKeystates();
	}

    bool keyDown(string type)
    {
        switch (type)
        {
            case "Dash":
                if (Input.GetKey(buttons[0]) && !this.lastState[0])
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
            this.GetComponent<Rigidbody>().velocity *= 5;
        }
    }

}
