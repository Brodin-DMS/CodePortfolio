using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RitualOfAnkaraz.Stats;

namespace RitualOfAnkaraz.Buffs
{

    /// <summary>
    /// Class <c>BullStrengthBuff</c> controls the BullStrength status. It is applied by the Skill BullsStrength(arcane or divine magic).
    /// </summary>
    public class BullStrengthBuff : BaseBuffDebuff
    {
        public int buffAmount;
        public BullStrengthBuff(CharacterStatsBase casterStats, CharacterStatsBase targetStats)
        {
            durationCharges = 5;
            if (casterStats.hasLearnedPassiveSkill(SkillDatabase.instance.buffArtist)) { durationCharges += 2; }
            iconPath = "SkillSprites / Skill icons Warrior/ Icons / Filled / SIW 11";
            buffName = "Bull's Strength";
            desc = "Provides the Target with an additional 1d4+1 Strength";
            baseBuffType = BaseBuffType.Buff;
            characterStatsBase = targetStats;
            maxDurationCharges = durationCharges;

            OnBuffApply();
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
                Object.Instantiate(SkillDatabase.instance.applyBuffPrefab, characterStatsBase.transform);
                //add to UI
                uiBuff = Object.Instantiate(SkillDatabase.instance.UibuffPrefab, characterStatsBase.UiBuffHolder);
                uiBuff.GetComponent<BuffUi>().Setup(this, durationCharges.ToString(), iconPath);
                //add playerStats
                buffAmount = Random.Range(1, 5) + 1;
                characterStatsBase.strengthModifikation += buffAmount;

            }
            else
            {
                BullStrengthBuff bs = (BullStrengthBuff)characterStatsBase.buffAndDebuffList.Find(item => item.buffName == buffName);
                bs.durationCharges = maxDurationCharges;
                bs.uiBuff.GetComponent<BuffUi>().UpdateDuration(durationCharges);
                //take value of higher rolled buff.
                if (buffAmount > bs.buffAmount)
                {
                    bs.buffAmount = buffAmount;
                    characterStatsBase.strengthModifikation += buffAmount - bs.buffAmount;
                }

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
                durationCharges--;
                uiBuff.GetComponent<BuffUi>().UpdateDuration(durationCharges);
            }
            else
            {
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
}