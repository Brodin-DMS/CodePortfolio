using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RitualOfAnkaraz.UI
{
    /// <summary>
    /// This class will handle the attribute assignment in the class creation. Its responsible for dragging 
    /// </summary>
    public class DragableAttrValue : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler ,IComparable<DragableAttrValue>
    {
        private Transform parent;
        public static bool isDragging;
        private Vector3 moveVector = new Vector3(0, 0, 0);

        private CanvasGroup canvasGroup;
        public HorizontalLayoutGroup layout;
        public LayoutElement layoutElement;

        public int baseValue;
        public int modifiedValue;
        public TextMeshProUGUI valueText;

        public AttrDrop attributeDropHolder;

        /// <summary>
        /// Used to Load the values of the attributes when swapping between different characters in creation.
        /// </summary>
        /// <param name="attrData"></param>
        public void Setup(DraggableAttrData attrData)
        {
            this.baseValue = attrData.baseValue;
            this.modifiedValue = attrData.modifiedValue;
            this.valueText.text = baseValue.ToString();
            this.attributeDropHolder = null;
            this.layoutElement.ignoreLayout = false;

            //when loaded after swapping roster slot
            if(attrData.attrDrop != null)
            {
                this.attributeDropHolder = attrData.attrDrop;
                this.valueText.text = modifiedValue.ToString();
                this.layoutElement.ignoreLayout = true;
                attributeDropHolder.Setup(this);
            }
        }
        private void Awake()
        {
            this.valueText = GetComponentInChildren<TextMeshProUGUI>();
            this.parent = transform.parent;
            this.layout = parent.GetComponent<HorizontalLayoutGroup>();
            this.canvasGroup = GetComponent<CanvasGroup>();
            this.layoutElement = GetComponent<LayoutElement>();
        }
        /// <summary>
        /// resets applied background bonuses when start dragging and enables raycast on drop object
        /// </summary>
        /// <param name="eventData"></param>
        public void OnBeginDrag(PointerEventData eventData)
        {
            if (attributeDropHolder != null)
            {
                attributeDropHolder.ResetStatHolder();
                attributeDropHolder = null;
            }

            canvasGroup.blocksRaycasts = false;
            isDragging = true;
            valueText.text = baseValue.ToString();
            modifiedValue = baseValue;


        }

        public void OnDrag(PointerEventData eventData)
        {
            moveVector.x = eventData.delta.x;
            moveVector.y = eventData.delta.y;
            transform.position += moveVector;
        }
        /// <summary>
        /// Handles the behavior of the dragable attribute.
        /// 
        /// </summary>
        /// <param name="eventData"></param> this is looking for an Attribute UI element to drop the stat value
        public void OnEndDrag(PointerEventData eventData)
        {
            //is dropped on an attribute
            //attrDrop.OnDrop() handles the assignment of data.
            if (eventData.pointerCurrentRaycast.gameObject != null && eventData.pointerCurrentRaycast.gameObject.CompareTag("DropHolder"))
            {
                layoutElement.ignoreLayout = true;
                attributeDropHolder = eventData.pointerCurrentRaycast.gameObject.GetComponent<AttrDrop>();
                if(attributeDropHolder == null)
                {
                    attributeDropHolder = eventData.pointerCurrentRaycast.gameObject.GetComponent<DropParentProxy>().parentDrop;
                }
            }
            //will snap back to original position if mouse is released outside of AttrSlots.
            else
            {
                layoutElement.ignoreLayout = false;
                isDragging = false;
                LayoutRebuilder.ForceRebuildLayoutImmediate(layout.GetComponent<RectTransform>());

            }
            canvasGroup.blocksRaycasts = true;
        }
        //IDK if its better to implement recursive IComparable here. seems like a lot of work when I'm just telling him compare the base value anyway. Seems like a violation to the KISS Principle.
        //makes attributes sortable.
        public static int SortByBaseValue(DragableAttrValue c1, DragableAttrValue c2)
        {
            return c2.baseValue.CompareTo(c1.baseValue);
        }
        //TODO think about if this is better or worse.
        public int CompareTo(DragableAttrValue other)
        {
            throw new NotImplementedException();
        }
    }
}