using UnityEngine;

[CreateAssetMenu(fileName = "D_Entity", menuName = "ScriptableObject/Data/D_Entity")]
public class D_Entity : ScriptableObject
{
    [Header("Material Component")]
    public Material defaultMaterial;
    
    [Header("Super Flight Component")]
    public float superFlightTimeElapse = 0f; // todo set private
    public float superFlightMaxTime = 5f;
    public bool isSuperFlight; 

    [Header("Blocking Gauge Component")]
    public float blockingMaxGauge;
    public float blockingCurrentGauge;
    public bool isBlocking; 

    [Header("Aura Component")]
    public Vector2 auraUpPos;
    public Vector2 auraDownPos;
    public Vector2 auraLeftPos;
    public Vector2 auraRightPos;

    public void Initialize() 
    {        
        superFlightTimeElapse = 0f;
        isSuperFlight = false;

        blockingCurrentGauge = blockingMaxGauge;
        isBlocking = false;
    }

    public void Cooldown(float delta) 
    {
        SuperFlightCooldown(delta);

        BlockingGaugeCooldown(delta);
    }


    public void SuperFlightResetTime() 
    {
        superFlightTimeElapse = 0;
        isSuperFlight = false;
    }

    private void SuperFlightCooldown(float delta)
    {
        if (!isSuperFlight)
        {
            superFlightTimeElapse += delta;
            if (superFlightTimeElapse >= superFlightMaxTime)
            {
                isSuperFlight = true;
            }
        }
    }

    private void BlockingGaugeCooldown(float delta)
    {
        if (!isBlocking)
        {
            if (blockingCurrentGauge < blockingMaxGauge)
            {
                blockingCurrentGauge += delta;
                if (blockingCurrentGauge >= blockingMaxGauge)
                {
                    blockingCurrentGauge = blockingMaxGauge;
                }
            }
        }
    }
}
