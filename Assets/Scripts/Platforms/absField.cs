using UnityEngine;
using System.Collections;

public abstract class absField : MonoBehaviour
{
    public enum FieldTypes { EMPTY, OBSTACLE };

    protected FieldTypes _fieldType;

    protected bool _isMoving = false;


    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public FieldTypes getFieldType() { return this._fieldType; }
    public bool getIsMoving() { return this._isMoving; }
}
