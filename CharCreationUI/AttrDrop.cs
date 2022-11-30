using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RitualOfAnkaraz.UI
{
    /// <summary>
    /// this is receives the draggable UI object to assign stats. It is attached to attributes in the mainMenuscene. It supports drag and drop as well as click to assign
    /// </summary>
    public class AttrDrop : MonoBehaviour, IDropHandler, IPointerClickHandler
    {
        [SerializeField] private Stat stat;
        [SerializeField] private RectTransform textPosition;
        [SerializeField] private TextMeshProUGUI attributeBBonusText;
        [SerializeField] private TextMeshProUGUI ownAttributeValueText;
        public DragableAttrValue dragableAttrValue;
        /// <summary>
        /// this clears the attrDrop Holder. its used when swapping between characters.
        /// </summary>
        public void ResetStatHolder()
        {
            attributeBBonusText.text = "";
            ownAttributeValueText.text = "";
            dragableAttrValue = null;

        }
        /// <summary>
        /// this method is called externally to setup attDropHolders when swapping between characters.
        /// </summary>
        /// <param name="dragableAttrValue"></param> is the saved draggable attribute before character was swapped.
        public void Setup(DragableAttrValue dragableAttrValue)
        {
            if (dragableAttrValue == null)
            {
                return;
            }
            this.dragableAttrValue = dragableAttrValue;
            attributeBBonusText.text = (dragableAttrValue.modifiedValue < 12) ? CharCreationTemplate.GetStatBonus(dragableAttrValue.modifiedValue).ToString() : "+" + CharCreationTemplate.GetStatBonus(dragableAttrValue.modifiedValue).ToString();
            dragableAttrValue.transform.position = textPosition.transform.position;
        }
        /// <summary>
        /// apply stats to creation template set ui of drop holder.
        /// </summary>
        public void ApplyBackgroundBonus()
        {
            if (dragableAttrValue != null)
            {
                switch (stat)
                {
                    case Stat.Str:
                        dragableAttrValue.modifiedValue = (dragableAttrValue.baseValue + CharCreationTemplate.selectedTemplate.StrRacial);
                        dragableAttrValue.valueText.text = dragableAttrValue.modifiedValue.ToString();
                        attributeBBonusText.text = (dragableAttrValue.modifiedValue < 12) ? CharCreationTemplate.GetStatBonus(dragableAttrValue.modifiedValue).ToString() : "+" + CharCreationTemplate.GetStatBonus(dragableAttrValue.modifiedValue).ToString();
                        CharCreationTemplate.selectedTemplate.str = dragableAttrValue.baseValue;
                        break;
                    case Stat.Dex:
                        dragableAttrValue.modifiedValue = (dragableAttrValue.baseValue + CharCreationTemplate.selectedTemplate.DexRacial);
                        dragableAttrValue.valueText.text = dragableAttrValue.modifiedValue.ToString();
                        attributeBBonusText.text = (dragableAttrValue.modifiedValue < 12) ? CharCreationTemplate.GetStatBonus(dragableAttrValue.modifiedValue).ToString() : "+" + CharCreationTemplate.GetStatBonus(dragableAttrValue.modifiedValue).ToString();
                        CharCreationTemplate.selectedTemplate.dex = dragableAttrValue.baseValue;
                        break;
                    case Stat.Int:
                        dragableAttrValue.modifiedValue = (dragableAttrValue.baseValue + CharCreationTemplate.selectedTemplate.IntRacial);
                        dragableAttrValue.valueText.text = dragableAttrValue.modifiedValue.ToString();
                        attributeBBonusText.text = (dragableAttrValue.modifiedValue < 12) ? CharCreationTemplate.GetStatBonus(dragableAttrValue.modifiedValue).ToString() : "+" + CharCreationTemplate.GetStatBonus(dragableAttrValue.modifiedValue).ToString();
                        CharCreationTemplate.selectedTemplate.intellect = dragableAttrValue.baseValue;
                        break;
                    case Stat.Con:
                        dragableAttrValue.modifiedValue = (dragableAttrValue.baseValue + CharCreationTemplate.selectedTemplate.ConRacial);
                        dragableAttrValue.valueText.text = dragableAttrValue.modifiedValue.ToString();
                        attributeBBonusText.text = (dragableAttrValue.modifiedValue < 12) ? CharCreationTemplate.GetStatBonus(dragableAttrValue.modifiedValue).ToString() : "+" + CharCreationTemplate.GetStatBonus(dragableAttrValue.modifiedValue).ToString();
                        CharCreationTemplate.selectedTemplate.con = dragableAttrValue.baseValue;
                        break;
                    case Stat.Wis:
                        dragableAttrValue.modifiedValue = (dragableAttrValue.baseValue);
                        dragableAttrValue.valueText.text = dragableAttrValue.modifiedValue.ToString();
                        attributeBBonusText.text = (dragableAttrValue.modifiedValue < 12) ? CharCreationTemplate.GetStatBonus(dragableAttrValue.modifiedValue).ToString() : "+" + CharCreationTemplate.GetStatBonus(dragableAttrValue.modifiedValue).ToString();
                        CharCreationTemplate.selectedTemplate.wis = dragableAttrValue.baseValue;
                        break;
                    case Stat.Cha:
                        dragableAttrValue.modifiedValue = (dragableAttrValue.baseValue + CharCreationTemplate.selectedTemplate.ChaRacial);
                        dragableAttrValue.valueText.text = dragableAttrValue.modifiedValue.ToString();
                        attributeBBonusText.text = (dragableAttrValue.modifiedValue < 12) ? CharCreationTemplate.GetStatBonus(dragableAttrValue.modifiedValue).ToString() : "+" + CharCreationTemplate.GetStatBonus(dragableAttrValue.modifiedValue).ToString();
                        CharCreationTemplate.selectedTemplate.cha = dragableAttrValue.baseValue;
                        break;
                    default: throw new System.NotImplementedException("Stat not valid");
                }
            }
        }
        /// <summary>
        /// receives the attribute drop
        /// </summary>
        /// <param name="eventData"></param> this is a draggableAttrValue
        public void OnDrop(PointerEventData eventData)
        {
            //check if an attribute is already attached, and detach it
            if(dragableAttrValue != null)
            {
                dragableAttrValue.modifiedValue = dragableAttrValue.baseValue;
                dragableAttrValue.valueText.text = dragableAttrValue.baseValue.ToString();
                dragableAttrValue.layoutElement.ignoreLayout = false;
                LayoutRebuilder.ForceRebuildLayoutImmediate(dragableAttrValue.layout.GetComponent<RectTransform>());
                dragableAttrValue.attributeDropHolder = null;
                dragableAttrValue = null;

            }
            //assign attribute
            dragableAttrValue = eventData.pointerDrag.gameObject.GetComponent<DragableAttrValue>();
            eventData.pointerDrag.transform.position = textPosition.position;
            dragableAttrValue.modifiedValue = dragableAttrValue.baseValue;
            //apply background bonus
            ApplyBackgroundBonus();
            CharCreationTemplate.selectedTemplate.CheckIfSetupIsComplete();
        }
        /// <summary>
        /// implements click to assing for attributes. This will get the highest attribute and assing it to the drop handler.
        /// </summary>
        /// <param name="eventData"></param>
        public void OnPointerClick(PointerEventData eventData)
        {
            
            if (dragableAttrValue != null)
            {
                return;

            }
            List<DragableAttrValue> dragableAttrValues = CharCreationTemplate.SceneDraggableAttr.ToList();
            dragableAttrValues.Sort(DragableAttrValue.SortByBaseValue);
            dragableAttrValues.RemoveAll(item => item.attributeDropHolder != null);
            if(dragableAttrValues.Count == 0)
            {
                return;
            }

            dragableAttrValue = dragableAttrValues[0];
            dragableAttrValue.transform.position = textPosition.position;
            dragableAttrValue.modifiedValue = dragableAttrValue.baseValue;

            ApplyBackgroundBonus();
            dragableAttrValue.layoutElement.ignoreLayout = true;
            dragableAttrValue.attributeDropHolder = this;
            CharCreationTemplate.selectedTemplate.CheckIfSetupIsComplete();
        }
    }
}
