using UnityEngine;
using System.Collections;

public class GuiControl : MonoBehaviour {
    public PlayerControl[] players;
    public Transform[] playerGUIs;
	// Use this for initialization
	void Start () {
	}
	
    public void restart()
    {
        players = new PlayerControl[GameManager.instance.getNumPlayers()];
        playerGUIs = new Transform[GameManager.instance.getNumPlayers()];

        for(int i = 0; i < GameManager.instance.getNumPlayers(); i++)
        {
            players[i] = GameObject.Find("Player" + (i + 1)).transform.GetComponent<PlayerControl>();
            playerGUIs[i] = GameObject.Find("Player" + (i + 1) + "gui").transform;
        }
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
