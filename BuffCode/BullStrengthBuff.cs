using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RitualOfAnkaraz.Stats;

/// <summary>
/// Class <c>BullStrengthBuff</c> controls the BullStrength status. It is applied by the Skill BullsStrength(arcane or divine magic).
/// </summary>
public class BullStrengthBuff : BaseBuffDebuff
{
    public int buffAmount;
    public BullStrengthBuff(CharacterStatsBase characterStats, int durationCharges, string iconPath, string buffName, string desc, BaseBuffType type) : base(characterStats, durationCharges, iconPath, buffName, desc, type)
    {

    }
    /// <summary>
    /// This Method applies 1d5 +1 strength to the targeted Character, initializes the duration counter thread, and Instantiates the UI element.
    /// </summary>
    public override void OnBuffApply()
    {
        base.OnBuffApply();
        //check if buff is applied
        if (characterStatsBase.buffAndDebuffList.TrueForAll(item => item.buffName != buffName))
        {
            //add to charStatusList
            characterStatsBase.buffAndDebuffList.Add(this);
            //add to player, temporary
            onPlayerBuff = GameObject.Instantiate(SkillDatabase.instance.applyBuffPrefab, characterStatsBase.transform);
            //add to UI
            uiBuff = GameObject.Instantiate(SkillDatabase.instance.UibuffPrefab, characterStatsBase.UiBuffHolder);
            uiBuff.GetComponent<BuffUi>().Setup(this, durationCharges.ToString(), iconPath);
            //add playerStats
            buffAmount = Random.Range(1, 5) + 1;
            characterStatsBase.strengthModifikation += buffAmount;

        }
        else
        {
            BaseBuffDebuff bs = characterStatsBase.buffAndDebuffList.Find(item => item.buffName == buffName);
            bs.durationCharges = maxDurationCharges;
            bs.uiBuff.GetComponent<BuffUi>().UpdateDuration(durationCharges);

        }
    }
    /// <summary>
    /// This Method only decreases duration charges.
    /// </summary>
    public override void OnBuffTick()
    {
        base.OnBuffTick();
        if (durationCharges > 1)
        {
            if (CustomGameManager.gameStage == GameStage.Combat)
            {
                durationCharges -= 1;
            }
            else
            {
                durationCharges -= 1;
            }
            uiBuff.GetComponent<BuffUi>().UpdateDuration(durationCharges);
        }
        else
        {
            if (CustomGameManager.gameStage == GameStage.Combat)
            {

            }
            OnBuffDrop();

        }

    }
    /// <summary>
    /// This Method removes the strength buff from the statSheet, stops the coroutine, and Destroys the UI Buff Element.
    /// </summary>
    public override void OnBuffDrop()
    {
        characterStatsBase.strengthModifikation -= buffAmount;
        base.OnBuffDrop();

    }
}