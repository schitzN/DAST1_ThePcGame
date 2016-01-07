using UnityEngine;
using System.Collections;

public class PlatformRow : MonoBehaviour {

    private int maxObstacles = 3;
    private int maxHoles = 3;
    private float maxDifficulty = 20f;
    private float timeDifficulty = 10f; // the higher the slower the difficulty increase
    private float rowSwapSpeed = 4f;

    private Rigidbody _rigid;
    private absField[] _fields;
    private int _numObstacles = 0;
    private int _numHoles = 0;
    private Vector3 _moveTarget = Vector3.zero;
    private float _moveDir;
    private bool _destroyAfterMoveFinished;

    // Use this for initialization
    void Awake () {
        // init row
        this._rigid = this.GetComponent<Rigidbody>();
        this._fields = new absField[PlatformManager.gridSize];
	}
	
	// Update is called once per frame
	void Update () {
	    if(this._moveTarget != Vector3.zero)
        {
            this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z + this._moveDir * rowSwapSpeed * Time.deltaTime);

            if (Mathf.Abs(_moveTarget.z - this.transform.position.z) < 0.05f)
            {
                
                this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, this._moveTarget.z);
                this._moveTarget = Vector3.zero;

                if(this._destroyAfterMoveFinished)
                {
                    this.transform.parent.GetComponent<PlatformManager>().removePlatformRow(this);
                    Destroy(this.gameObject);
                }
            }
        }
	}

    public void initRows(bool empty)
    {
        float fldsize = PlatformManager.instance.getFieldSize();

        for (int y = -(PlatformManager.gridSize / 2); y < (PlatformManager.gridSize / 2); y++)
        {
            GameObject field = this.createRndField(new Vector3(fldsize, 1, fldsize),
                                                    new Vector3(this.transform.position.x, 0, (y * fldsize + fldsize / 2f) + this.transform.position.z),
                                                    y + (PlatformManager.gridSize / 2),
                                                    y + (PlatformManager.gridSize / 2),
                                                    empty);

            field.transform.SetParent(this.transform);
        }
    }

    private GameObject createRndField(Vector3 scale, Vector3 pos, int x, int y, bool forceEmpty)
    {
        GameObject field;

        float chance = ((Time.time / this.timeDifficulty) / this.maxDifficulty) / System.Enum.GetNames(typeof(absField.FieldTypes)).Length;

        if (forceEmpty)
        {
            field = Instantiate(Resources.Load<GameObject>("Field_Empty"));
        }
        else if (Random.value < chance && this._numObstacles < maxObstacles)
        {
            this._numObstacles++;
            field = Instantiate(Resources.Load<GameObject>("Field_Obstacle"));
        }
        else if (Random.value < chance && this._numHoles < maxHoles)
        {
            this._numHoles++;
            field = Instantiate(Resources.Load<GameObject>("Field_Hole"));
        }
        else
        {
            field = Instantiate(Resources.Load<GameObject>("Field_Empty"));
        }
        field.transform.localScale = scale;
        field.transform.position = pos;
        this._fields[x] = field.GetComponent<absField>();

        return field;
    }

    public Rigidbody getRigidbody() { return this._rigid; }

    public bool getIsMoving() { return (this._moveTarget != Vector3.zero); }

    public void setNewMoveTarget(Vector3 target, bool destroyAfterFinish) {
        this._moveTarget = target;
        this._destroyAfterMoveFinished = destroyAfterFinish;
        this._moveDir = _moveTarget.normalized.z - this.transform.position.normalized.z;
    }
}
