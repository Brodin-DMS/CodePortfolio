using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using RitualOfAnkaraz.Stats;

namespace RitualOfAnkaraz.Ascension
{
    /// <summary>
    /// This class manages allows to obtain the Monstrosity trait. Its assigned in the Ascension Menu.
    /// </summary>
    public class Monstrosity : TraitBase, IPointerClickHandler
    {

        protected override void Awake()
        {
            base.Awake();
            traitName = "Monstrosity";
            traitDesc = "";
            traitType = TraitType.Ascension;

            maxPoints = 1;
            descriptions = new string[maxPoints];

            descriptions[0] = "+15 Armor Class\n" +
                "+20 Strength\n" +
                "+25 Constitution\n" +
                "Can't wear Leg Armor\n" +
                "Can't wear Chest Armor";

            requirements.Add(new TraitRequirement("Demonic", 3, 5));
            requirements.Add(new TraitRequirement("Spell Eater", 3, 5));

        }
        public override void OnPointerClick(PointerEventData eventData)
        {
            CharacterStatsBase statInstance = GetCanvasActivePlayerStats();
            if (investedPoints < maxPoints && statInstance.ascensionPointsLeft > 0 && statInstance.traits.ContainsKey("Demonic") && statInstance.traits["Demonic"] >= 3 && statInstance.traits.ContainsKey("Spell Eater") && statInstance.traits["Spell Eater"] >= 3)
            {
                statInstance.ascensionPointsLeft--;
                investedPoints++;
                fillColorImage.fillAmount = (float)investedPoints / (float)maxPoints;
                if (statInstance.traits.ContainsKey(traitName))
                {
                    statInstance.traits[traitName]++;
                }
                else
                {
                    statInstance.traits.Add(traitName, 1);
                }
                statInstance.acBase += 15;
                statInstance.strengthBase += 20;
                statInstance.ChangeConstitutionBase(25);
                statInstance.InitPassiveSkillList();
                statInstance.ChangeStatDisplay();
                TraitManager.UpdateTraitUi(statInstance);
                TraitToolTip.instance.UpdateOverlay(this);
            }
        }

    }

}