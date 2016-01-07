using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour {

    public static GameManager instance;

    private Transform Players;
    public Text roundTimeTxt;
    public float curRoundTime = 0;
    private float numPlayers = 4;

	// Use this for initialization
	void Start () {
        instance = this;
        this.Players = GameObject.Find("Players").transform;

        this.restart();
	}
	
	// Update is called once per frame
	void Update () {
        this.curRoundTime += Time.deltaTime;
        this.roundTimeTxt.text = "ROUND TIME\n" + (int)this.curRoundTime;
	}

    public void playerDied()
    {
        this.numPlayers--;

        if (this.numPlayers <= 0)
            this.restart();
    }

    public void restart()
    {
        this.curRoundTime = 0;
        this.numPlayers = 4;

        for(int i = 0; i < this.numPlayers; i++)
        {
            GameObject pl = Instantiate(Resources.Load<GameObject>("Player"));

            pl.transform.position = new Vector3(0, 5f, PlatformManager.instance.getFieldSize() * (i - (this.numPlayers / 2f)) + PlatformManager.instance.getFieldSize() / 2f);
            pl.transform.SetParent(this.Players);

            PlayerControl pc = pl.GetComponent<PlayerControl>();
            pc.player = i + 1;
            pc.hpTxt = GameObject.Find("p" + (i + 1) + " health").GetComponent<Text>();
            pc.hpTxt.text = "HP: " + 100;

            if (i == this.numPlayers - 1)
                pc.keyboardControl = true;
        }

        PlatformManager.instance.initNewWorld();
    }
}
