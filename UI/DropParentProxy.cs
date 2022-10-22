using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RitualOfAnkaraz.UI
{
    /// <summary>
    /// This script can be attached to any object that overlays drag and drop. It will successfully pass the eventData to the OnDrop of any script that implements IDropHandler.
    /// </summary>
    public class DropParentProxy : MonoBehaviour, IDropHandler
    {
        [SerializeField] public AttrDrop parentDrop;

        public void OnDrop(PointerEventData eventData)
        {
            parentDrop.OnDrop(eventData);
        }
    }

}