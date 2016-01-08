using UnityEngine;
using System.Collections;

public class SelfDestroy : MonoBehaviour
{
    float timer = 3f;
    ParticleSystem psy;
    // Use this for initialization
    void Start()
    {
        psy = this.GetComponent<ParticleSystem>();
        psy.Play();
    }

    // Update is called once per frame
    void Update()
    {
        this.timer -= Time.deltaTime;
        if (psy.particleCount < 10 && this.timer <= 0)
            Destroy(this.gameObject);
    }
}
