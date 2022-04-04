﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public bool canLift;

    [SerializeField] MeshRenderer materialtoHaveGlow;

    public virtual void Interact()
    {

    }
}
