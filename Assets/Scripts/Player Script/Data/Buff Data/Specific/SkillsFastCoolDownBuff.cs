using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AbilityFastCoolDownBuff", menuName = "ScriptableObject/Buff/AbilityFastCoolDownBuff")]
public class SkillsFastCoolDownBuff : Buff
{
    SkillController skillController;

    [Header("Ability Fast Cool Down Component")]
    public float fastCoolDownFactor = 0.3f;
    
    public override void Initialize(Core core, PlayerData data)
    {
        base.Initialize(core, data);

        this.skillController = core.GetCoreComponent<SkillController>();
    }

    public override void Activate()
    {
        base.Activate();

        skillController.finalCoolDownFactor += fastCoolDownFactor;
    }
    
    public override void End()
    {
        skillController.finalCoolDownFactor -= fastCoolDownFactor;
    }


    protected override void ApplyEffect()
    {
        
    }
}
