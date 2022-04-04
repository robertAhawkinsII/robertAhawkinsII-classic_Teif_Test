using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootStepsNoiseMaker : MonoBehaviour
{

    [SerializeField] FloorDetector floor;

    public List<AudioClip> footStepsGroundSounds = new List<AudioClip>(); // halfs the sounds of the foot steps
    public List<AudioClip> footStepsWoodSounds = new List<AudioClip>(); // nutral 
    public List<AudioClip> footStepsStoneSounds = new List<AudioClip>(); // increases the sound of steps
    public List<AudioClip> footStepsCarpetSounds = new List<AudioClip>(); // muffels the sound off foot steps by 1/3


    public AudioSource audioSource;


    [SerializeField] [Range(0, 5)] float FootStepRadius;
    [SerializeField] float MaxNoisePosibile;
    [SerializeField] float volumeOffset = 5f;

    public float NoiseRadius => FootStepRadius;

    bool makingNoise;
    float currentradius;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(makingNoise == false)
        {
            FootStepRadius = Mathf.Clamp(FootStepRadius - 3 * Time.deltaTime, 0, currentradius);
        }

        if (FootStepRadius > 10)
            FootStepRadius = 10;


    }

    public void SetAudioBasedOnFloor(int random)
    {
        floor.CheckFloor();
        switch (floor.StandingFloor)
        {
            case CurentFloorStanding.Wood:
                audioSource.clip = footStepsWoodSounds[random];
                break;
            case CurentFloorStanding.Ground:
                audioSource.clip = footStepsGroundSounds[random];
                break;
            case CurentFloorStanding.Stone:
                audioSource.clip = footStepsStoneSounds[random];
                break;
            case CurentFloorStanding.Carpet:
                audioSource.clip = footStepsCarpetSounds[random];
                break;
        }
    }

    public void FireNoise(float speed)
    {
        makingNoise = true;
        switch (floor.StandingFloor)
        {
            case CurentFloorStanding.Wood: //normal sound levels 
                FootStepRadius = audioSource.volume*volumeOffset;
                break;
            case CurentFloorStanding.Ground: // half sound levels
                FootStepRadius = (audioSource.volume * volumeOffset) / 2;
                break;
            case CurentFloorStanding.Stone: // * 1.5 sound levels
                FootStepRadius = (audioSource.volume * volumeOffset) * 1.5f;
                break;
            case CurentFloorStanding.Carpet: //one third sound levels
                FootStepRadius = (audioSource.volume * volumeOffset) / 3f;
                break;
        }
        currentradius = FootStepRadius;
    }

    public void stopFireingNoises()
    {
        makingNoise = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(transform.position, FootStepRadius);
    }
}
