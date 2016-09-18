using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Events;

// this asociates the info in the breeding menu with a certain cat
// MOVE ALL OF THE TEXT INSTANTIATION TO THIS CODE
public class Info : MonoBehaviour {
    public CatGenetics catMaster;
    private Text nameText;
    private Text genderText;
    private Text personalityText;
    public static CatGenetics theFemale;
    public static CatGenetics theMale;
    public static GameObject femaleInfo;
    public static GameObject maleInfo;

    // on start create all of the text based on the assigned cat master
    void Start () {

        theFemale = null;
        theMale = null;

        nameText = transform.Find("NameText").gameObject.GetComponent<Text>();
        genderText = transform.Find("GenderText").gameObject.GetComponent<Text>();
        personalityText = transform.Find("PersonalityText").gameObject.GetComponent<Text>();

        // set the name, oh my gosh, why is life so hard sometimes... I swear there must be something in the on click function that I didn't relaize, but so many minutes of frustration and sweat and blood
        // have been leading up to this moment o^o
        nameText.text = catMaster.CatName;
        genderText.text = catMaster.Gender;
        personalityText.text = catMaster.Personality;
    }
	
    // screw buttons
    // This should be moved to the breedgui
    public void SelectCats()
    {
        if(theFemale == null && catMaster.Gender == "Female")
        {
            theFemale = catMaster;
            BreedGUI.femaleSelected = true;
            femaleInfo = gameObject;
        }
        else if (theMale == null && catMaster.Gender == "Male")
        {
            theMale = catMaster;
            BreedGUI.maleSelected = true;
            maleInfo = gameObject;
        }
       
        // check if breeding is ready to go
        // Ask if the player wants to breed the cats and compare genetics
        if (theFemale != null && theMale != null)
        {
            // make sure the selected bools are false for sure, and breed the cats
            BreedGUI.Instance.Compare();
        }
    }
}
