using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RitualOfAnkaraz.Stats;


/// <summary>
/// Class <c>BaseBuffDebuff</c> is the base class controlling all applied buffs, debuffs and status effects applied to any Character. 
/// </summary>
public abstract class BaseBuffDebuff
{
    public int durationCharges;
    public int maxDurationCharges;
    protected string iconPath;

    public string buffName;
    public string desc;
    public BaseBuffType baseBuffType;
    protected Coroutine outOffCombatRoutine;
    protected CharacterStatsBase characterStatsBase;
    public GameObject uiBuff;
    public GameObject onPlayerBuff;

    public BaseBuffDebuff(CharacterStatsBase charStats, int durationCharges, string iconPath, string buffName, string desc, BaseBuffType baseBuffType)
    {
        this.durationCharges = durationCharges;
        this.iconPath = iconPath;
        this.buffName = buffName;
        this.desc = desc;
        this.baseBuffType = baseBuffType;
        this.characterStatsBase = charStats;
        this.maxDurationCharges = durationCharges;

        OnBuffApply();
    }
    /// <summary>
    /// This Base Method initializes a thread that reduces the buff duration. It is overwritten by subclasses to instantiate Ui elements and apply changes to the charachterStatSheets.
    /// </summary>
    public virtual void OnBuffApply()
    {
        if (characterStatsBase.buffAndDebuffList.TrueForAll(item => item.buffName != buffName))
        {
            outOffCombatRoutine = CoroutineManager.instance.StartCoroutine(reduceBuffDurationOutOfCombat());
        }
    }
    /// <summary>
    /// This Base method is called when a buff, debuff or status effect is removed by any means. It stops the coroutine, removes applied stats and destroys the Ui element.
    /// </summary>
    public virtual void OnBuffDrop()
    {
        if (outOffCombatRoutine != null)
        {
            CoroutineManager.instance.StopCoroutine(outOffCombatRoutine);
        }

        GameObject.Destroy(uiBuff);
        if (onPlayerBuff != null)
        {
            GameObject.Destroy(onPlayerBuff);
        }
        characterStatsBase.buffAndDebuffList.Remove(this);
    }
    /// <summary>
    /// This Base Method is called each time a buff duration charge is removed. It is overwritten by subclasses, applieng changes to the charachterStatSheet.
    /// </summary>
    public virtual void OnBuffTick()
    {

    }

    IEnumerator reduceBuffDurationOutOfCombat()
    {
        while (durationCharges > 0)
        {
            yield return new WaitForSeconds(30f);
            if (CustomGameManager.gameStage == GameStage.Normal)
            {
                if (durationCharges > 1)
                {
                    OnBuffTick();
                }
                else
                {
                    OnBuffTick();
                    OnBuffDrop();
                    yield break;
                }
            }
        }
    }
    /// <summary>
    /// This Method is called if a buff or debuff drops early by any means.
    /// </summary>
    public void RemoveBuff()
    {
        //buff is removed with spell
        OnBuffDrop();
        CoroutineManager.instance.StopCoroutine(outOffCombatRoutine);
    }

}

public enum BaseBuffType
{
    Buff,
    Debuff
}