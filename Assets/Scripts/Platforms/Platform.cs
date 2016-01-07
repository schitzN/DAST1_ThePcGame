using UnityEngine;
using System.Collections;

public class Platform : MonoBehaviour {

    public static readonly int platformSize = 20;
    public static readonly int gridSize = 6;
    public static readonly int maxObstacles = 10;
    public static float fieldSwapSpeed = 5f;

    //private GameObject platObj;
    private Rigidbody rigid;
    private absField[,] grid;
    private float fieldSize;
    private int numObstacles = 0;

    // Use this for initialization
    void Start () {
        // init platform
        //this.platObj = this.gameObject;
        this.rigid = this.GetComponent<Rigidbody>();
        this.grid = new absField[gridSize, gridSize];
        this.fieldSize = platformSize / (float)gridSize;

        // create fields
        for(int x = -(gridSize / 2); x < (gridSize / 2); x++)
        {
            for (int y = -(gridSize / 2); y < (gridSize / 2); y++)
            {
                GameObject field = this.createRndField( new Vector3(fieldSize, 1, fieldSize),
                                                        new Vector3((x * fieldSize + fieldSize / 2f) + this.transform.position.x, 0, (y * fieldSize + fieldSize / 2f) + this.transform.position.z),
                                                        x + (gridSize / 2),
                                                        y + (gridSize / 2));
                
                field.transform.SetParent(this.transform);
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void changeRow(absField field)
    {
        if (field.getIsMoving())
        {
            Debug.Log("Field already moving!");
            return;
        }

        int row = this.findRow(field);

        if (row == -1)
        {
            Debug.Log("Field not found: " + field);
            return;
        }
            
        for (int y = 0; y < gridSize; y++)
        {
            absField fld = this.grid[row, y];

            if (fld.getFieldType() == absField.FieldTypes.OBSTACLE)
                this.numObstacles--;

            //Debug.Log(this.numObstacles + ", " + fld.getFieldType());
            Vector3 newPos = new Vector3(fld.transform.position.x, fld.transform.position.y, fld.transform.position.z + this.fieldSize * gridSize);
            this.createRndField(fld.transform.localScale, newPos, row, y).transform.SetParent(fld.transform.parent);

            //Destroy(fld.gameObject);
        }
    }

    private int findRow(absField field)
    {
        for(int x = 0; x < gridSize; x++)
            for (int y = 0; y < gridSize; y++)
                if (this.grid[x, y] == field)
                    return x;

        return -1;
    }

    private GameObject createRndField(Vector3 scale, Vector3 pos, int x, int y)
    {
        GameObject field;

        if (Random.value < 0.2f && this.numObstacles < maxObstacles)
        {
            this.numObstacles++;
            field =  Instantiate(Resources.Load<GameObject>("Field_Obstacle"));
        }
        else
        {
            field = Instantiate(Resources.Load<GameObject>("Field_Empty"));
        }

        field.transform.localScale = scale;
        field.transform.localPosition = pos;
        this.grid[x, y] = field.GetComponent<absField>();

        return field;
    }

    public Rigidbody getRigidbody() { return this.rigid; }
}
