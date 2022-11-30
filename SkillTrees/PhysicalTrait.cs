using RitualOfAnkaraz.Stats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RitualOfAnkaraz.Traits
{
    public class PhysicalTrait : TraitBase, IPointerClickHandler
    {
        protected override void Awake()
        {
            base.Awake();
            traitType = TraitType.Physical;
        }
        /// <summary>
        /// This method checks if the active character is able to learn the physical trait. The active character then adds the trait to their trait dictionary. Needed physical or generic trait points are reduced.
        /// </summary>
        /// <param name="eventData"></param>
        public override void OnPointerClick(PointerEventData eventData)
        {
            CharacterStatsBase statInstance = GetCanvasActivePlayerStats();
            if (investedPoints < maxPoints && (statInstance.physicalTaitPointsLeft > 0 || statInstance.genericTraitPointsLeft > 0) && requirements.TrueForAll(req => req.CheckReq()))
            {
                if (statInstance.physicalTaitPointsLeft > 0)
                {
                    statInstance.physicalTaitPointsLeft--;
                }
                else
                {
                    statInstance.genericTraitPointsLeft--;
                }
                investedPoints++;
                fillColorImage.fillAmount = investedPoints / (float)maxPoints;
                if (statInstance.traits.ContainsKey(traitName))
                {
                    statInstance.traits[traitName]++;
                }
                else
                {
                    statInstance.traits.Add(traitName, 1);
                }
                OnTraitObtain(statInstance);
            }
        }

    }
}