using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour {

    public static GameManager instance;

    private Transform Players;
    public Text roundTimeTxt;
    public float curRoundTime = 0;
    private int numPlayers = 4;
    public bool gameRunning = false;
    public float defaultStaminaBar;
    public bool KeyboardControlPlayer4;

	// Use this for initialization
	void Start () {
        instance = this;
        this.Players = GameObject.Find("Players").transform;
        this.defaultStaminaBar = GameObject.Find("Player1gui").transform.FindChild("Health").localScale.x;
        StartCoroutine(this.restart());
    }
	
	// Update is called once per frame
	void Update () {
        if (!gameRunning)
            return;

        this.curRoundTime += Time.deltaTime;
        this.roundTimeTxt.text = "ROUND TIME\n" + (int)this.curRoundTime;
	}

    public void playerDied(PlayerControl player)
    {
        this.numPlayers--;
        GameObject.Find("p" + player.player + " name").GetComponent<Text>().text = "D E A D";

        if (this.numPlayers <= 0)
        {
            this.gameRunning = false;
            StartCoroutine(this.restart());
        }
            
    }

    public IEnumerator restart()
    {
        yield return new WaitForEndOfFrame();

        this.curRoundTime = 0;
        this.numPlayers = 4;
        

        for(int i = 0; i < this.numPlayers; i++)
        {
            GameObject pl = Instantiate(Resources.Load<GameObject>("Player"));

            pl.transform.position = new Vector3(0, 5f,2* PlatformManager.instance.getFieldSize() * (i - (this.numPlayers / 2f)) + 2* PlatformManager.instance.getFieldSize() / 2f);
            pl.transform.SetParent(this.Players);
            pl.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Player" + (i + 1));

            GameObject.Find("p" + (i + 1) + " name").GetComponent<Text>().text = "Player " + (i + 1);

            PlayerControl pc = pl.GetComponent<PlayerControl>();

            pc.gameObject.name = "Player" + (i + 1);
            pc.player = i + 1;

            Color c = new Color(Random.value, Random.value, Random.value, 1f);
            GameObject.Find("Panel" + (i + 1) + " color").GetComponent<Image>().color = c;
            pl.GetComponent<Renderer>().material.color = c;

            if (i == this.numPlayers - 1 && KeyboardControlPlayer4)
                pc.keyboardControl = true;
        }

        PlatformManager.instance.initNewWorld();
        GameObject.Find("Gui").GetComponent<GuiControl>().restart();

        this.gameRunning = true;
        yield return null;
    }

    public int getNumPlayers() { return this.numPlayers; }
}
