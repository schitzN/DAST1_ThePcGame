using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlatformManager : MonoBehaviour {

    public static PlatformManager instance;

    public static readonly int maxPlatforms = 4;
    public static float platformGap = 1;
    public static float platformSpeed = 5;

    private List<Platform> _platforms;
    private Platform _platToDestroy;

	// Use this for initialization
	void Start () {
        instance = this;
        this._platforms = new List<Platform>();

        for(int i = -1; i < maxPlatforms - 1; i++)
        {
            this.createPlatform(new Vector3((Platform.platformSize + platformGap) * i, 0, 0));
        }
	}
	
	// Update is called once per frame
	void Update () {
        foreach(Platform p in this._platforms)
        {
            p.getRigidbody().MovePosition(new Vector3(p.transform.position.x + (-1 * platformSpeed * Time.deltaTime), 0, 0));
            //p.transform.Translate(Vector3.left * platformSpeed * Time.deltaTime);
            if(p.transform.position.x < (Platform.platformSize + platformGap) * -2)
            {
                this._platToDestroy = p;
            }
        }

        if(this._platToDestroy != null)
        {
            // destroy old
            this._platforms.Remove(this._platToDestroy);
            Destroy(this._platToDestroy.gameObject);
            this._platToDestroy = null;

            // create new
            this.createPlatform(new Vector3((Platform.platformSize + platformGap) * (maxPlatforms - 2), 0, 0));
        }
	}

    private void createPlatform(Vector3 pos)
    {
        GameObject plat = Instantiate(Resources.Load<GameObject>("Platform"));
        this._platforms.Add(plat.GetComponent<Platform>());
        plat.transform.SetParent(this.transform);
        plat.transform.localPosition = pos;
    }
}