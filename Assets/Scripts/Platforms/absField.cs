using UnityEngine;
using System.Collections;

public abstract class absField : MonoBehaviour
{
    public enum FieldTypes { EMPTY, OBSTACLE, HOLE };

    protected FieldTypes _fieldType;

    //public bool isMoving = false;


    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public FieldTypes getFieldType() { return this._fieldType; }
}
