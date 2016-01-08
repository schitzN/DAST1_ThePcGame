using UnityEngine;
using System.Collections;

public class SelfDestroy : MonoBehaviour
{
    float timer = 2.8f;
    // Use this for initialization
    void Start()
    {
        this.GetComponent<ParticleSystem>().Play();
    }

    // Update is called once per frame
    void Update()
    {
        this.timer -= Time.deltaTime;
        if (timer <= 0)
            Destroy(this.gameObject);
    }
}
