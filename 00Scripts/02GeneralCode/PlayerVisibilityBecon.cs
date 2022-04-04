using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisibilityBecon : MonoBehaviour
{
    public static PlayerVisibilityBecon visibilityBecon;

    public float stealthLevel;

    [SerializeField] private List<LightingScript> suroundingLight;
    public FootStepsNoiseMaker noiseMaker;

    private void Awake()
    {
        visibilityBecon = this;
        suroundingLight = new List<LightingScript>();
        noiseMaker = GetComponent<FootStepsNoiseMaker>();
        if(visibilityBecon != this)
        {
            Destroy(this);
        }
    }

    public void AddLightsList(LightingScript setLight)
    {
        foreach (var lights in suroundingLight)
        {
            if (setLight == lights)
            {           
                if (setLight.indexOfPointInList != lights.indexOfPointInList)
                {
                    setLight.indexOfPointInList = lights.indexOfPointInList;
                    return;
                }
                else
                return;
            }
        }
        suroundingLight.Add(setLight);
        setLight.indexOfPointInList = suroundingLight.Count - 1;
        CheckVisibilityBasedOnLights();
    }

    public void RemoveFromLightingList(LightingScript index)
    {
        if(suroundingLight.Count != 0)
        {

            if (index = suroundingLight.Find(x => x == index))
            {
                index.indexOfPointInList = -1;
                suroundingLight.Remove(index);
            }

            if(suroundingLight.Count <= 0)
            {
                suroundingLight = new List<LightingScript>();
            }
        }
    }

    private void FixedUpdate()
    {
        if(suroundingLight.Count != 0)
        {
            CheckVisibilityBasedOnLights();
        }

    }

    private void CheckVisibilityBasedOnLights()
    {
        foreach (LightingScript light in suroundingLight.ToArray())
        {
            if (!light.insideInnerLight() && !light.insideMidLight() && !light.insideOuterLight())
            {
                suroundingLight.Remove(light);
            }

            if (light.insideInnerLight() && !light.insideMidLight() && !light.insideOuterLight())
            {
                light.currentAmount = 6;
            }

            if (!light.insideInnerLight() && light.insideMidLight() && !light.insideOuterLight())
            {
                light.currentAmount = 2;
            }

            if (!light.insideInnerLight() && !light.insideMidLight() && light.insideOuterLight())
            {
                light.currentAmount = 0.5f;
            }
        }

    }

    public void AddtoStealthLevel(float ammount)
    {
        stealthLevel += ammount;
        if(stealthLevel >= 6)
        {
            stealthLevel = 6;
        }

        if(stealthLevel < 0)
        {
            stealthLevel = 0;
        }
    }
}
