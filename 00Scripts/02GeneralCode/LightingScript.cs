using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightingScript : MonoBehaviour
{
   private float originalLightRange;
   public Light generalLight;

    public float innerLightRadius;
    public float midLightRadius;
    public float outerLightRadius;

    [SerializeField] float distanceFromBecon;
    [SerializeField] PlayerVisibilityBecon player;
    [SerializeField] private LayerMask wallLayer, playerLayer;

    public float addedAmount = 0;
    public float currentAmount = 0;
    public int indexOfPointInList = -1;

    private void OnEnable()
    {
        originalLightRange = generalLight.range;
        setRadiusBasedOnLight();
        player = PlayerVisibilityBecon.visibilityBecon;
    }

    [ContextMenu("SetLight")]
    private void setRadiusBasedOnLight()
    {       
        outerLightRadius = generalLight.range;
        midLightRadius = generalLight.range / 1.5f;
        innerLightRadius = generalLight.range / (generalLight.range + 0.5f);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(transform.position, innerLightRadius);
        Gizmos.DrawWireSphere(transform.position, midLightRadius);
        Gizmos.DrawWireSphere(transform.position, outerLightRadius);

        //Gizmos.DrawLine(transform.position, )
    }

    private void Update()
    {
        if(player != null)
        {
            distanceFromBecon = Vector3.Distance(transform.position, player.transform.position);
        }

        if(distanceFromBecon > outerLightRadius)
        {
            if (player != null)               
                {

                    player.AddtoStealthLevel(-currentAmount);
                if(indexOfPointInList != -1)
                {
                    player.RemoveFromLightingList(this);
                }
                    indexOfPointInList = -1;
                    addedAmount = 0;
                    currentAmount = 0;

                player = null;
                }
            else
            {
                indexOfPointInList = -1;
                addedAmount = 0;
                currentAmount = 0;
            }
        }

        Collider[] hits = Physics.OverlapSphere(transform.position, outerLightRadius);

        foreach (var hit in hits)
        {         
            if(hit.tag == "Player")
            {
                RaycastHit _phit;
             
                Vector3 playerStandPoint = (hit.transform.position - generalLight.transform.position).normalized;

                Debug.DrawRay(generalLight.transform.position,
                    playerStandPoint,
                   Color.blue);
                if(Physics.Raycast(generalLight.transform.position,
                    playerStandPoint,
                    out _phit, outerLightRadius, wallLayer))
                {
                    //Debug.Log("PlayerinShadows");
                    if (player != null)
                    {
                        player.AddtoStealthLevel(-currentAmount);
                        if (indexOfPointInList > -1)
                            player.RemoveFromLightingList(this);
                        indexOfPointInList = -1;
                        addedAmount = 0;
                        currentAmount = 0;
                    }
                    // turn off the visibility addition
                    return;
                }
                if (Physics.Raycast(generalLight.transform.position,
                    playerStandPoint,
                    out _phit, outerLightRadius, playerLayer))
                {
                    //Debug.Log("player in light");
                    player = hit.GetComponent<PlayerVisibilityBecon>();
                    distanceFromBecon = Vector3.Distance(transform.position, player.transform.position);
                    if (indexOfPointInList > -1)
                    {
                        if (addedAmount != currentAmount)
                        {                          
                            player.AddtoStealthLevel(currentAmount);
                            player.AddtoStealthLevel(-addedAmount);
                            addedAmount = currentAmount;
                        }
                    }
                    else
                    {
                        player.AddLightsList(this);
                    }
                    //turn on the visibility addition
                }
            }
        }
    }

    public bool insideOuterLight()
    {
        return distanceFromBecon <= outerLightRadius && distanceFromBecon > midLightRadius;
    }

    public bool insideMidLight()
    {
        return distanceFromBecon <= midLightRadius && distanceFromBecon > innerLightRadius;
    }

    public bool insideInnerLight()
    {
        return distanceFromBecon <= innerLightRadius;
    }

    [ContextMenu("LightsOut!")]
    public void ExstinguishLight()
    {
        generalLight.range = 0;
        setRadiusBasedOnLight();
    }

    [ContextMenu("LightsOn!")]
    public void RelightLight()
    {
        generalLight.range = originalLightRange;
        setRadiusBasedOnLight();
    }
}
