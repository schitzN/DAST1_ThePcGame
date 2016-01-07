using UnityEngine;
using System.Collections;

public class PlatformRow : MonoBehaviour {

    public static readonly int maxObstacles = 4;
    public static readonly int maxHoles = 4;
    public static float rowSwapSpeed = 4f;

    private Rigidbody _rigid;
    private absField[] _fields;
    private int _numObstacles = 0;
    private int _numHoles = 0;
    private Vector3 _moveTarget = Vector3.zero;
    private float _moveDir;
    private bool _destroyAfterMoveFinished;

    // Use this for initialization
    void Start () {
        // init row
        this._rigid = this.GetComponent<Rigidbody>();
        this._fields = new absField[PlatformManager.gridSize];

        float fldsize = PlatformManager.instance.getFieldSize();

        // create rows
        for (int y = -(PlatformManager.gridSize / 2); y < (PlatformManager.gridSize / 2); y++)
        {
            GameObject field = this.createRndField( new Vector3(fldsize, 1, fldsize),
                                                    new Vector3(this.transform.position.x, 0, (y * fldsize + fldsize / 2f) + this.transform.position.z),
                                                    y + (PlatformManager.gridSize / 2),
                                                    y + (PlatformManager.gridSize / 2),
                                                    false);
                
            field.transform.SetParent(this.transform);
        }
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
            /*else
            {
                this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, target.z);
            }*/
        }
	}

    private GameObject createRndField(Vector3 scale, Vector3 pos, int x, int y, bool forceEmpty)
    {
        GameObject field;
        float rnd = Random.value;

        if (forceEmpty)
        {
            field = Instantiate(Resources.Load<GameObject>("Field_Empty"));
        }
        else if (rnd < 0.2f && this._numObstacles < maxObstacles)
        {
            this._numObstacles++;
            field = Instantiate(Resources.Load<GameObject>("Field_Obstacle"));
        }
        else if (rnd > 0.2f && rnd < 0.4f && this._numHoles < maxHoles)
        {
            this._numHoles++;
            field = Instantiate(Resources.Load<GameObject>("Field_Hole"));
        }
        else
        {
            field = Instantiate(Resources.Load<GameObject>("Field_Empty"));
        }

        // fix flickering
        //Vector3 fixScale = new Vector3(scale.x + 0.001f, scale.y + 0.001f, scale.z + 0.001f);

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
        //this._initPos = this.transform.position;
        this._moveDir = _moveTarget.normalized.z - this.transform.position.normalized.z;
    }
}
