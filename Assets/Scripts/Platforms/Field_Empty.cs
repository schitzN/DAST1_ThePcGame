﻿using UnityEngine;
using System.Collections;

public class Field_Empty : absField {
    

    // Use this for initialization
    void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    protected override void InitField()
    {
        this._fieldType = FieldTypes.EMPTY;
    }
}
