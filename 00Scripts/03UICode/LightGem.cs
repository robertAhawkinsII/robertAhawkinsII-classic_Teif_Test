using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LightGem : MonoBehaviour
{
    [SerializeField] PlayerVisibilityBecon player;
    [SerializeField] private Image Gem;
    [SerializeField] private float maxVisibility = 6.0f;
    [SerializeField] private float currentVisibilityCounter;


    // Start is called before the first frame update
    void Start()
    {
        player = PlayerVisibilityBecon.visibilityBecon;
    }

    // Update is called once per frame
    void Update()
    {
        if(player != null)
        {
            float stealthNumber = player.stealthLevel;
            currentVisibilityCounter = stealthNumber / maxVisibility;

        Color gemC = new Color(Gem.color.r, Gem.color.g, Gem.color.b);

        Gem.color = new Color (gemC.r, gemC.g, gemC.b, currentVisibilityCounter);
        }
    }
}
