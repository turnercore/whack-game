using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Juice : MonoBehaviour
{
    [SerializeField]
    protected GameObject juiceTarget;

    [SerializeField]
    protected bool isActive = false;
}
