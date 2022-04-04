using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Entity : MonoBehaviour
{
    public FiniteStateMechine stateMechine;

    public float facingDirection { get; private set; }

    public EnemyBase aI { get; private set; }

    public Animator anim { get; private set; }

    public GameObject eyesGO { get; private set; }
    public GameObject characterGO { get; private set; }

    public NavMeshAgent agent { get; private set; }

    public Vector3 lastKnownPosition;

    public D_entity entityData;

    //sight + memory
    protected bool aiMemorizesPlayer = false;
    protected float increaseingMemoryTime;
    [SerializeField] protected float losLightingFactor;

    //hearing
    protected bool aiHeardPlayer = false;

    protected float dopplerVariable; // search for player noise position
    public float DopplerVar { get { return dopplerVariable; } }

    public Transform strafeRight;
    public Transform strafeLeft;

    //waitTime at waypoint for potrolling
    protected float waitTime;

    public Transform[] moveSpots;


    [SerializeField] PlayerVisibilityBecon targetPlayer;

    public virtual void Start()
    {
        eyesGO = transform.Find("Eyes").gameObject;
        characterGO = transform.Find("Modle").gameObject;
        aI = GetComponent<EnemyBase>();
        anim = characterGO.GetComponent<Animator>();

        agent = GetComponent<NavMeshAgent>();

        stateMechine = new FiniteStateMechine();
        targetPlayer = Gamemanager.instance.PlayerRef;
    }

    public virtual void Update()
    {
        SetSightByLight();
        stateMechine.currentState.LogicUpdate();
    }

    public virtual void FixedUpdate()
    {
        stateMechine.currentState.PhysicsUpdate();
    }

    public virtual void setPlayerLastKnownPoint()
    {
        lastKnownPosition = targetPlayer.transform.position;
    }

    public virtual void moveToTargetPlayer()
    {
        agent.SetDestination(targetPlayer.transform.position);
    }

    public void SetSightByLight()
    {
        if (targetPlayer.stealthLevel >= 6)
        {
            losLightingFactor = entityData.losRadius;
        }
        else if (targetPlayer.stealthLevel <= 5.5 && targetPlayer.stealthLevel >= 4.5)
        {
            losLightingFactor = entityData.losRadius * .7f;
        }
        else if (targetPlayer.stealthLevel <= 4 && targetPlayer.stealthLevel >= 2.5)
        {
            losLightingFactor = entityData.losRadius * .4f;
        }
        else if (targetPlayer.stealthLevel <= 2 && targetPlayer.stealthLevel >= .5)
        {
            losLightingFactor = entityData.losRadius * .25f;
        }
        else if (targetPlayer.stealthLevel <= 0)
        {
            losLightingFactor = entityData.losRadius * .09f;
        }
    }

    public virtual bool insightWithLights()
    {
        Collider[] overlaps = new Collider[10];
        int count = Physics.OverlapSphereNonAlloc(eyesGO.transform.position, losLightingFactor, overlaps);

        for (int i = 0; i < count; i++)
        {
            if (overlaps[i] != null)
            {
                if (overlaps[i].transform == targetPlayer.transform)
                {
                    Vector3 directionBetween = (targetPlayer.gameObject.transform.position - eyesGO.transform.position).normalized;
                    directionBetween.y *= 0;

                    float angle = Vector3.Angle(eyesGO.transform.forward, directionBetween);
                    if (angle <= entityData.FOVAngle)
                    {
                        Ray ray = new Ray(eyesGO.transform.position, targetPlayer.gameObject.transform.position - eyesGO.transform.position);
                        RaycastHit hit;

                        if (Physics.Raycast(ray, out hit, losLightingFactor))
                        {
                            if (hit.transform == targetPlayer.transform)
                                return true;
                        }
                    }
                }
            }
        }
        return false;
    }

    public virtual bool insightWithoutLights()
    {
        Collider[] overlaps = new Collider[10];
        int count = Physics.OverlapSphereNonAlloc(eyesGO.transform.position, entityData.losRadius, overlaps);

        for (int i = 0; i < count; i++)
        {
            if (overlaps[i] != null)
            {
                if (overlaps[i].transform == targetPlayer.transform)
                {
                    Vector3 directionBetween = (targetPlayer.gameObject.transform.position - eyesGO.transform.position).normalized;
                    directionBetween.y *= 0;

                    float angle = Vector3.Angle(eyesGO.transform.forward, directionBetween);
                    if (angle <= entityData.FOVAngle)
                    {
                        Ray ray = new Ray(eyesGO.transform.position, targetPlayer.gameObject.transform.position - eyesGO.transform.position);
                        RaycastHit hit;

                        if (Physics.Raycast(ray, out hit, entityData.losRadius))
                        {
                            if (hit.transform == targetPlayer.transform)
                                return true;
                        }
                    }
                }
            }
        }
        return false;
    }

    public virtual bool inCausionWithoutLights()
    {
        Collider[] overlaps = new Collider[10];
        int count = Physics.OverlapSphereNonAlloc(eyesGO.transform.position, losLightingFactor + (losLightingFactor * .2f), overlaps);

        for (int i = 0; i < count; i++)
        {
            if (overlaps[i] != null)
            {
                if (overlaps[i].transform == targetPlayer.transform)
                {
                    Vector3 directionBetween = (targetPlayer.gameObject.transform.position - eyesGO.transform.position).normalized;
                    directionBetween.y *= 0;

                    float angle = Vector3.Angle(eyesGO.transform.forward, directionBetween);
                    if (angle <= entityData.FOVAngle)
                    {
                        Ray ray = new Ray(eyesGO.transform.position, targetPlayer.gameObject.transform.position - eyesGO.transform.position);
                        RaycastHit hit;

                        if (Physics.Raycast(ray, out hit, losLightingFactor + (losLightingFactor * .2f)))
                        {
                            if (hit.transform == targetPlayer.transform)
                                return true;
                        }
                    }
                }
            }
        }
        return false;
    }

    public virtual bool HeardThePlayer() //when they are inside the radius {Puts them in Aleart
    {
        float distance = Vector3.Distance(targetPlayer.transform.position, transform.position);

        if (distance <= entityData.noiseTravelDistance && targetPlayer.noiseMaker.NoiseRadius > 0)
        {
            if(distance >= distance - targetPlayer.noiseMaker.NoiseRadius * .5f)
            {
                return true;
            }
        }
        
            return false;
    }

    public virtual bool HearedANoise() // when they arn't close enough to 
        //know what the noise came from but know where it is {Puts them in noise Caution 
    {
        float distance = Vector3.Distance(targetPlayer.transform.position, transform.position);

        if (distance <= entityData.noiseTravelDistance && targetPlayer.noiseMaker.NoiseRadius > 0)
        {
            if (distance <=  distance -(targetPlayer.noiseMaker.NoiseRadius * .8f)) //70%
            {
                dopplerVariable = targetPlayer.noiseMaker.NoiseRadius;
                return true;
            }        
        }
            return false;      
    }

    public virtual bool CheckIfPlayerInMinAgroRange()
    {
        return Vector3.Distance(targetPlayer.transform.position, agent.transform.position) < entityData.distToPlayer;
    }

    public virtual bool CheckIfPlayerInMaxAgroHeightRange()
    {
        float ElivationDistance = targetPlayer.transform.position.y - agent.transform.position.y;

        return ElivationDistance > entityData.chaseRadius;
    }

    public virtual bool CheckIfPlayerInMaxAgroRange()
    {
        return insightWithLights();
    }
}
