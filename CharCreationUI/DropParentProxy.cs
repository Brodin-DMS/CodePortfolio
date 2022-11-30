using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RitualOfAnkaraz.UI
{
    /// <summary>
    /// This script can be attached to any UI element that overlays drag and drop in the CharCreationMenu menu.
    /// </summary>
    public class DropParentProxy : MonoBehaviour, IDropHandler
    {
        public AttrDrop parentDrop;

        public void OnDrop(PointerEventData eventData)
        {
            parentDrop.OnDrop(eventData);
        }
    }

}