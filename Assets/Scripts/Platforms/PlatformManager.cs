using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlatformManager : MonoBehaviour {

    public static PlatformManager instance;

    public static readonly int maxPlatforms = 32;
    public static readonly int platformHeight = 20;
    public static readonly int gridSize = 8;
    public static float platformGap = 0;
    public static float platformSpeed = 3;

    private List<PlatformRow> _platformRows;
    private PlatformRow _platToDestroy;
    private Transform Players;

    private float fieldSize;

    // Use this for initialization
    void Start () {
        instance = this;
        this.Players = GameObject.Find("Players").transform;
        this._platformRows = new List<PlatformRow>();

        this.fieldSize = platformHeight / (float)gridSize;

        for (int i = -(maxPlatforms / 2); i < (maxPlatforms / 2); i++)
        {
            this.createPlatform(new Vector3((this.fieldSize + platformGap) * i, 0, 0));
        }
	}
	
	// Update is called once per frame
	void Update () {
        foreach(PlatformRow p in this._platformRows)
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
            this._platformRows.Remove(this._platToDestroy);
            Destroy(this._platToDestroy.gameObject);
            this._platToDestroy = null;

            // create new
            this.createPlatform(new Vector3((fieldSize + platformGap) * (maxPlatforms / 2), 0, 0));

            // TODO: Fix gap when new row is created
        }
	}

    public void swapRow(PlatformRow row)
    {
        Vector3 moveTarget = new Vector3(0, 0, platformHeight);
        GameObject newRow = this.createPlatform(row.transform.position + moveTarget);
        newRow.GetComponent<PlatformRow>().setNewMoveTarget(row.transform.position, false);

        row.setNewMoveTarget(row.transform.position - moveTarget, true);
        //this._platforms.Remove(row);
        //Destroy(row.gameObject);

        
    }

    private GameObject createPlatform(Vector3 pos)
    {
        GameObject plat = Instantiate(Resources.Load<GameObject>("PlatformRow"));
        this._platformRows.Add(plat.GetComponent<PlatformRow>());
        plat.transform.SetParent(this.transform);
        plat.transform.position = pos;

        return plat;
    }

    public float getFieldSize() { return this.fieldSize; }

    public void removePlatformRow(PlatformRow row) { this._platformRows.Remove(row); }
}