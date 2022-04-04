using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterValues : MonoBehaviour
{
    [SerializeField] private float MaxHP;
    private float currentHealth { get { return currentHealth; }
        set { currentHealth = Mathf.Clamp(value, 0, MaxHP); } }

    [SerializeField] private float lowHealthThreshold;

    public float HP { get { return currentHealth; } }

    public float lowHPThreshhold { get { return lowHealthThreshold; } }
}
