using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlatformManager : MonoBehaviour {

    public static PlatformManager instance;

    public static readonly int maxPlatforms = 28;
    public static readonly int platformHeight = 20;
    public static readonly int gridSize = 10;
    public static readonly float lavaDmg = 0.5f;
    public static float platformGap = 0;
    public static float platformSpeed = 3f;
    public static float speedUp = 1;
    
    private float basePlatformSpeed;

    private List<PlatformRow> _platformRows;
    private PlatformRow _platToDestroy;
    private Transform Players;

    private float fieldSize;

    // Use this for initialization
    void Awake () {
        // init
        instance = this;
        this.Players = GameObject.Find("Players").transform;
        this.fieldSize = platformHeight / (float)gridSize;
        this.basePlatformSpeed = platformSpeed;
    }
	
	// Update is called once per frame
	void Update () {
        if (!GameManager.instance.gameRunning || GameManager.paused)
            return;

        PlatformManager.platformSpeed = GameManager.instance.curRoundTime > 5? Mathf.Floor(basePlatformSpeed * Mathf.Max((GameManager.instance.curRoundTime+60f)/120f,1)) : Mathf.Max((GameManager.instance.curRoundTime-1)/4f,0) * basePlatformSpeed;
        if (speedUp > 1)
        {
            PlatformManager.platformSpeed = Mathf.Max(basePlatformSpeed * speedUp, PlatformManager.platformSpeed);
            speedUp = 1f;
        }

        foreach (PlatformRow p in this._platformRows)
        {
            //p.getRigidbody().MovePosition(new Vector3(p.transform.position.x + (-1 * platformSpeed * Time.deltaTime), 0, 0));
            p.transform.Translate(Vector3.left * platformSpeed * Time.deltaTime);
            if(p.transform.position.x < (fieldSize + platformGap) * (maxPlatforms / 2) * -1)
            {
                this._platToDestroy = p;
            }
        }
        Players.Translate(Vector3.left * platformSpeed * Time.deltaTime);

        if(this._platToDestroy != null)
        {
            // destroy old
            float x = this._platToDestroy.transform.position.x;
            this._platformRows.Remove(this._platToDestroy);
            Destroy(this._platToDestroy.gameObject);
            this._platToDestroy = null;

            // create new
            this.createPlatform(new Vector3(x + (fieldSize + platformGap) * maxPlatforms, 0, 0));
        }
	}

    private void resetVars()
    {
        if(this._platformRows != null)
            foreach (PlatformRow r in this._platformRows)
                Destroy(r.gameObject);

        this._platformRows = new List<PlatformRow>();
    }

    public void initNewWorld()
    {
        this.resetVars();

        for (int i = -(maxPlatforms / 2); i < (maxPlatforms / 2); i++)
        {
            this.createPlatform(new Vector3((this.fieldSize + platformGap) * i, 0, 0));
        }
    }

    public void swapRow(PlatformRow row, bool fromTop)
    {
        int dir = 1;

        if (!fromTop)
            dir = -1;

        Vector3 moveTarget = new Vector3(0, 0, platformHeight * dir);
        GameObject newRow = this.createPlatform(row.transform.position + moveTarget);
        newRow.GetComponent<PlatformRow>().setNewMoveTarget(row.transform.position, false);

        row.setNewMoveTarget(row.transform.position - moveTarget, true);
    }

    private GameObject createPlatform(Vector3 pos)
    {
        GameObject plat = Instantiate(Resources.Load<GameObject>("PlatformRow"));
        this._platformRows.Add(plat.GetComponent<PlatformRow>());
        plat.transform.SetParent(this.transform);
        plat.transform.position = pos;

        // ensure that the first few rows are always only empty
        bool empty = false;

        if (plat.transform.position.x > -((maxPlatforms / 8) * fieldSize) && plat.transform.position.x < ((maxPlatforms / 8) * fieldSize))
            empty = true;

        plat.GetComponent<PlatformRow>().initRows(empty);

        return plat;
    }

    public float getFieldSize() { return this.fieldSize; }

    public void removePlatformRow(PlatformRow row) { this._platformRows.Remove(row); }
}