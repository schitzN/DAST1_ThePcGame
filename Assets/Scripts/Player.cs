using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
    public Platform curPlat;
    public absField curField;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    if (Input.GetKeyDown("u"))
        {
            this.curPlat.changeRow(this.curField);
        }
	}

    void FixedUpdate()
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
}
