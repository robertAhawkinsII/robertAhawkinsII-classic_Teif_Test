using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newDetectionStateData", menuName = "Data/State Data/Base DetectionState Data")]
public class D_PlayerDetected : ScriptableObject
{

    public float t_minStrafe = .01f;
    public float t_maxStraif = .05f;


    public float attackStartTime = 1.5f;
}
