using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using RitualOfAnkaraz.Stats;
using RitualOfAnkaraz.Requirements;

/// <summary>
/// Base class to all traits and ascension UI Elements
/// </summary>
public class TraitBase : MonoBehaviour, IPointerClickHandler
{
    public string traitName;
    public string traitDesc;
    [SerializeField] protected TraitType traitType;
    [SerializeField] protected Sprite icon;

    public int investedPoints;
    public int maxPoints;

    public Image fillColorImage;
    protected List<BaseRequirement> requirements;
    public string[] descriptions;
    protected virtual void Awake()
    {
        requirements = new List<BaseRequirement>();
    }
    protected virtual void Start()
    {
        fillColorImage.fillAmount = 0;
    }
    /// <summary>
    /// This method is overriden by specific traits to control weather a trait is obtainable. The active character then learns the trait and needed trait points are reduced.
    /// </summary>
    /// <param name="eventData"></param>
    public virtual void OnPointerClick(PointerEventData eventData)
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
    /// <summary>
    /// this Method is overriden by subclasses and defines the specific behavior when a character learns a specific trait.
    /// </summary>
    /// <param name="stats"></param> statSheet of the character obtaining the trait.
    protected virtual void OnTraitObtain(CharacterStatsBase stats)
    {
        
    }
}
public enum TraitType
{
    Physical,
    Agile,
    Magic,
    Ascension
}
