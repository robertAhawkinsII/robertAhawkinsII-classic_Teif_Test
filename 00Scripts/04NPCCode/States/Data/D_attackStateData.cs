using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newAttackStateData", menuName = "Data/State Data/Base Attack Data")]
public class D_attackStateData : ScriptableObject
{
    public int attackPower = 1;

    public float attackTime = 2f;
}
