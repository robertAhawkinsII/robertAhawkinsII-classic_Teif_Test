using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{

    public static GameData dataInstance;

    public bool ToggleCrouch = true;

    // Start is called before the first frame update
    void Awake()
    {
        if(dataInstance == null)
        {
            dataInstance = this;
            if(dataInstance != this)
            {
                Destroy(gameObject);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}


public enum CompasPoint
{
    North, South, East, West
}