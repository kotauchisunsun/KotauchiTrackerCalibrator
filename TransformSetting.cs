using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformSetting
{
    public Transform right { get; }
    public Transform left { get; }

    public TransformSetting(Transform right, Transform left)
    {
        this.right = right;
        this.left = left;
    }
}
