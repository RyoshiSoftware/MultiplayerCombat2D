using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SkillController : CoreComponent
{
    public List<SkillSlot> skillSlotList;
    public float baseCoolDownFactor = 1;

    public float finalCoolDownFactor = 1;

    Player player;
    PlayerInputHandler inputHandler;
    SkillSlot selectedSkillSlot; 
    public Action onFinishSkill;

    protected override void Awake()
    {
        base.Awake();

        player = GetComponentInParent<Player>();
        inputHandler = GetComponentInParent<PlayerInputHandler>();
        skillSlotList = new List<SkillSlot>(GetComponentsInChildren<SkillSlot>());
    }

    private void Start() {
        foreach(SkillSlot skillSlot in skillSlotList)
        {
            skillSlot.skill.Initialize(player, player.core);
        }
    }

    private void OnEnable() {
        inputHandler.onSkillInputChange += SelectSkillSlot;
    }

    private void OnDisable() {
        inputHandler.onSkillInputChange -= SelectSkillSlot;
    }

    public void SelectSkillSlot(int index)
    {
        if (selectedSkillSlot != null) 
        {
            DeselectSkillSlot();
        }

        selectedSkillSlot = skillSlotList[index];
        selectedSkillSlot.skill.onFinish += FinishSkill;
    }

    void FinishSkill()
    {
        onFinishSkill?.Invoke();

        DeselectSkillSlot();
    }

    void DeselectSkillSlot()
    {
        selectedSkillSlot.skill.onFinish -= FinishSkill;
        selectedSkillSlot = null;
    }


    void Update() 
    {
        SkillUpdateTime();
    }

    void SkillUpdateTime()
    {
        if (selectedSkillSlot == null) return;

        if (selectedSkillSlot.skill.isPlaying)
        {
            selectedSkillSlot.skill.LogicsUpdate();
        }
    }

    public void ActivateSkill()
    {
        selectedSkillSlot.skill.Activate();
    }

    public int GetSkillAnimId()
    {
        return selectedSkillSlot.skill.animId;
    }

    public void CooldownAllSkills(float delta) 
    {
        foreach (SkillSlot skillSlot in skillSlotList)
        {
            skillSlot.Cooldown(delta, finalCoolDownFactor);
        }
    }

    public bool IsSkillEmergency()
    {
        return selectedSkillSlot.skill.isEmergency;
    }

    public bool DoesSkillNeedTarget()
    {
        return selectedSkillSlot.skill.needTarget;
    }

    public bool DoesSkillNeedCharging()
    {
        return selectedSkillSlot.skill.needCharging;
    }

    public void ChargeSkill()
    {
        selectedSkillSlot.skill.StartCharging();
    }

    public void StopCharging()
    {
        selectedSkillSlot.skill.StopCharging();
    }

    public void ResetSkillCharging()
    {
        selectedSkillSlot.skill.ResetSkillCharging();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (selectedSkillSlot == null) return;

        selectedSkillSlot.skill.OnTriggerEnter2D(other);
    }

    private void OnTriggerStay2D(Collider2D other) {
        if (selectedSkillSlot == null) return;

        selectedSkillSlot.skill.OnTriggerEnter2D(other);
    }
}
