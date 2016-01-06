using UnityEngine;
using System.Collections;

public abstract class absField : MonoBehaviour
{
    public enum FieldTypes { EMPTY, OBSTACLE };
    protected FieldTypes _fieldType;

    // Use this for initialization
    void Start()
    {
        this.InitField();
    }

    // Update is called once per frame
    void Update()
    {

    }

    protected abstract void InitField();
    public FieldTypes getFieldType() { return this._fieldType; }
}
