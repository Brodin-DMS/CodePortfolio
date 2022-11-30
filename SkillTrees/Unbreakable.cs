using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using RitualOfAnkaraz.Stats;
using RitualOfAnkaraz.Requirements;

namespace RitualOfAnkaraz.Traits
{
    /// <summary>
    /// This class implements the unbreakable feat, that obtainable in the physical feat tree.
    /// </summary>
    public class Unbreakable : PhysicalTrait
    {
        protected override void Awake()
        {
            base.Awake();
            traitName = "Unbreakable";
            traitDesc = "+3 hardness\n" +
                "+3 magical hardness";
            traitType = TraitType.Physical;

            maxPoints = 1;
            descriptions = new string[maxPoints];
            descriptions[0] = "+3 hardness\n" +
                "+3 magical hardness";

            requirements.Add(new TraitRequirement("Body And Soul", 1, 1));
            requirements.Add(new TraitRequirement("Heavy Weight", 1, 1));

        }
        /// <summary>
        /// This Method adds stats to the character upon obtaining the Unbreakable feat. it is executed in the onPointerClick Method in the physicalTrait class.
        /// </summary>
        /// <param name="stats"></param>
        protected override void OnTraitObtain(CharacterStatsBase stats)
        {
            //change stat Base
            stats.hardnessBase += 3;
            stats.magicalHardnessBase += 3;
            //update UI
            stats.ThrowUpdateSecondaryStatUIEvent(stats.Hardness, SecondaryStat.Hardness);
            stats.ThrowUpdateSecondaryStatUIEvent(stats.MagicalHardness, SecondaryStat.MagicalHardness);
            TraitManager.UpdateTraitUi(stats);
            TraitToolTip.instance.UpdateOverlay(this);
        }
    }
}