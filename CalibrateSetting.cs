using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct CalibrateSetting
{
    public Transform parent { get; }
    public Vector3 position { get; }
    public Quaternion rotation { get; }

    public CalibrateSetting(Transform parent, Vector3 position, Quaternion rotation)
    {
        this.parent = parent;
        this.position = position;
        this.rotation = rotation;
    }
}
