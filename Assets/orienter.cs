using UnityEngine;
using System.Collections;

public class orienter : MonoBehaviour
{
    Rigidbody rg;
    // Use this for initialization
    void Start()
    {
        this.rg = transform.parent.GetComponent<Rigidbody>();
    }

    void OnEnable()
    {
        rg = transform.parent.GetComponent<Rigidbody>();
        if (rg != null)
            this.transform.forward = rg.velocity.normalized;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
