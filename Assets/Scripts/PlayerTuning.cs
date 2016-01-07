using UnityEngine;
using System.Collections;

public class PlayerTuning : MonoBehaviour
{
    public float speed;
    public float stamina;
    public float staminaReg;
    public float jumpspeed;
    // Use this for initialization
    void Start()
    {
        PlayerControl[] players = transform.GetComponentsInChildren<PlayerControl>();
        foreach (PlayerControl p in players)
        {
            p.speed = speed;
            p.stamina = stamina;
            p.stamina_reg = staminaReg;
            p.jumpspeed = jumpspeed;
        }
    }

    // Update is called once per frame
    void Update() { }
}