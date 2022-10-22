using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using RitualOfAnkaraz.Stats;

/// <summary>
/// Base class to all traits and ascension UI Elements
/// </summary>
public class TraitBase : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject generalToolTip;

    public string traitName;
    public string traitDesc;
    public TraitType traitType;

    public Sprite icon;

    //TODO check if its calss varaible form supercalss
    public int investedPoints;
    public int maxPoints;

    public Image fillColorImage;

    public List<BaseRequirement> requirements;

    public string[] descriptions;
    protected virtual void Awake()
    {
        requirements = new List<BaseRequirement>();
    }
    protected virtual void Start()
    {
        fillColorImage.fillAmount = 0;
    }
    public virtual void OnPointerClick(PointerEventData eventData)
    {

    }

    public void OnPointerEnter(PointerEventData eventData)
    {

    }

    public void OnPointerExit(PointerEventData eventData)
    {

    }
    /// <summary>
    /// returns the stat sheet of the currently selected player
    /// </summary>
    /// <returns></returns>
    protected CharacterStatsBase GetCanvasActivePlayerStats()
    {
        return CustomGameManager.instance.players[CanvasManager.SelectedCharacter-1].GetComponent<CharacterStatsBase>();
    }
    /// <summary>
    /// Returns the Requirement string that represents requirements in order to invest points in any trait.
    /// </summary>
    /// <returns></returns>
    public virtual string GetRequirementsString() 
    {
        string allReqs = "";
        foreach(BaseRequirement requirement in requirements)
        {
            Debug.Log("Invoking GetRquirementsString");
            if (!requirement.CheckReq())
            {
                allReqs += requirement.reqText + "\n";
                Debug.Log(requirement.reqText);
            }
        }
        Debug.Log("all reqs =" + allReqs);
        return allReqs; 
    }
}
public enum TraitType
{
    Physical,
    Agile,
    Magic,
    Ascension
}
