using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RitualOfAnkaraz.Requirements
{
    /// <summary>
    /// This is the Base class for all requirements in the game. Requirements are displayable in UI tool tips and control weather an item, feat, or ascension is wearable/obtainable.
    /// </summary>
    public abstract class BaseRequirement
    {
        public string reqText;
        /// <summary>
        /// This Method is overriden by subclasses, to check if trait, ascension or attribute requirements are met when equipping Items or obtaining traits and ascensions.
        /// </summary>
        /// <returns> true = requirement is fulfilled, false = requirement is not fulfilled </returns>
        public abstract bool CheckReq();
    }
}