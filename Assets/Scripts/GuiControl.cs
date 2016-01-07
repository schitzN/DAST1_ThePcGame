using UnityEngine;
using System.Collections;

public class GuiControl : MonoBehaviour {
    PlayerControl[] players;
    Transform[] playerGUIs;
	// Use this for initialization
	void Start () {
        players = GameObject.Find("Players").transform.GetComponentsInChildren<PlayerControl>();
        playerGUIs = new Transform[] { transform.FindChild("Player1"), transform.FindChild("Player2"), transform.FindChild("Player3"), transform.FindChild("Player4") };
	}
	
	// Update is called once per frame
	void Update () {
        for (int i = 0; i < players.Length; i++)
        {
            Vector3 scale = playerGUIs[i].FindChild("Stamina").localScale;
            playerGUIs[i].FindChild("Stamina").localScale = new Vector3(scale.x + (players[i].stamina/10f - scale.x) *0.25f,1,1);
        }
	}
}
