using UnityEngine;
using System.Collections;

public class Platform : MonoBehaviour {

    public static readonly int platformSize = 20;
    public static readonly int gridSize = 6;
    public static readonly int maxObstacles = 10;

    private GameObject platObj;
    private absField[,] grid;
    private float fieldSize;
    private int numObstacles = 0;

    // Use this for initialization
    void Start () {
        // init platform
        this.platObj = this.gameObject;
        //this.platObj.transform.localScale = new Vector3(platformSize, 1, platformSize);
        this.grid = new absField[gridSize, gridSize];
        this.fieldSize = platformSize / (float)gridSize;

        // create fields
        for(int x = -(gridSize / 2); x < (gridSize / 2); x++)
        {
            for (int y = -(gridSize / 2); y < (gridSize / 2); y++)
            {
                GameObject field;
                
                if(Random.value < 0.2f && this.numObstacles < maxObstacles)
                {
                    field = Instantiate(Resources.Load<GameObject>("Field_Obstacle"));
                    this.numObstacles++;
                }
                else
                {
                    field = Instantiate(Resources.Load<GameObject>("Field_Empty"));
                }

                field.transform.localScale = new Vector3(fieldSize, 1, fieldSize);
                field.transform.localPosition = new Vector3((x * fieldSize + fieldSize / 2f) + this.transform.position.x, 0, (y * fieldSize + fieldSize / 2f) + this.transform.position.z);
                field.transform.SetParent(this.transform);
                this.grid[x + (gridSize / 2), y + (gridSize / 2)] = field.GetComponent<absField>();
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
