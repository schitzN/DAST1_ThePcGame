using UnityEngine;
using System.Collections;

public class SpeedModifier : MonoBehaviour {
    public float speedup;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerStay(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Players"))
            PlatformManager.speedUp = speedup;
    }
}
