using UnityEngine;
using System.Collections;

public class orienter : MonoBehaviour {
    Rigidbody rg;
	// Use this for initialization
	void Start () {
        this.rg = transform.parent.GetComponent<Rigidbody>();
	}

    void OnEnable()
    {
        this.transform.forward = rg.velocity.normalized;
    }
	
	// Update is called once per frame
	void Update () {
	    
	}
}
