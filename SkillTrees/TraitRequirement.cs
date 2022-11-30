using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RitualOfAnkaraz.Stats;

namespace RitualOfAnkaraz.Requirements
{
    /// <summary>
    /// This class implements the trait requirements in order to equip items or obtain further traits and ascensions.
    /// </summary>
    public class TraitRequirement : BaseRequirement
    {
        public string traitName;
        public int amount;
        public int maxPoints;

        public TraitRequirement(string traitName, int minPoints, int maxPoints)
        {
            this.traitName = traitName;
            amount = minPoints;
            this.maxPoints = maxPoints;

            reqText = traitName + " (" + amount + "/" + maxPoints + ")";
        }

        /// <summary>
        /// this Method checks weather the requirement is met by the currently selected character.
        /// </summary>
        /// <returns> true = requirement is fulfilled, false = requirement is not fulfilled </returns>
        public override bool CheckReq()
        {
            CharacterStatsBase stats = CustomGameManager.instance.GetAktivePlayerCharacter();
            if (stats == null) { return false; }
            if (!stats.traits.ContainsKey(traitName)) { return false; }
            else
            {
                if (stats.traits[traitName] >= amount)
                {
                    return true;
                }
            }
            return false;
        }
    }
}