using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gamemanager : MonoBehaviour
{
    public static Gamemanager instance;

    public PlayerVisibilityBecon PlayerRef;

    private void Awake()
    {
        instance = this;
        if (this != instance)
            Destroy(gameObject);

       PlayerRef = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerVisibilityBecon>();
    }
}
