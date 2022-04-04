using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newIdleStateData", menuName = "Data/State Data/Base IdleState Data")]
public class D_IdleState : ScriptableObject
{
    public float minIdleTime;
    public float maxIdleTime;
}
