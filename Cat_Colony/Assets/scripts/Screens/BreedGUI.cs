using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Events;

// Manages all of it's dialogues
public class BreedGUI : MonoBehaviour {

    // tell if the female and the male have been selected
    public static bool femaleSelected = false;
    public static bool maleSelected = false;

    // fields for comparing genetics
    public GameObject compareMenuPrefab;
    private GameObject compareMenu;
    public static bool compare = false;
    public GameObject optionsPrefab;
    private GameObject options;

    private GameObject femaleGrid;
    private GameObject maleGrid;
    public GameObject catInfoPrefab;
    private GameObject catInfo;
    public GameObject chooseFemaleButton;
    public GameObject chooseMaleButton;

    // the cat to add info for
    private CatGenetics cat;

    // displaying the female and the male
    public GameObject displayFemale;
    public GameObject displayMale;

    public GameObject CompareMenu
    {
        get { return compareMenu; }
        set { compareMenu = value; }
    }

    // Use this for initialization
    void Awake ()
    {
        // create the male and female lists
        femaleGrid = GameObject.Find("Female Grid");
        maleGrid = GameObject.Find("Male Grid");

        // attach event listeners
        chooseFemaleButton.GetComponent<Button>().onClick.AddListener(() => ReactivateFemale());
        chooseMaleButton.GetComponent<Button>().onClick.AddListener(() => ReactivateMale());

        //Menu event listeners

    }

    public void AddInfo(CatGenetics cat)
    {
        // TRAIN OF THOUGHT: the cat should tell the curently created cat info what it's name and other info is
        // no list of all the names
        // no list of all the info, just get the name text
        catInfo = Instantiate(catInfoPrefab) as GameObject;
        catInfo.GetComponent<Button>().interactable = true;

        // add the cat to the appropriate list based on gender
        if (cat.Gender == "Male")
        {
            catInfo.transform.SetParent(maleGrid.transform, false);
        }
        else
        {
            catInfo.transform.SetParent(femaleGrid.transform, false);
        }
        catInfo.GetComponent<Info>().catMaster = cat;
    }

    // reactivates all of the cats, attached to choose other cat buttons
    void ReactivateFemale()
    {
        // reattach the female to the list
        Info.femaleInfo.transform.SetParent(femaleGrid.transform);

        foreach (Transform cat in femaleGrid.transform)
        {
            cat.gameObject.GetComponent<Button>().interactable = true;
        }

        // make the female null since the player didn't want her :(
        Info.theFemale = null;

        // deactivate the choose button
        chooseFemaleButton.GetComponent<Button>().interactable = false;
    }

    // same as the above, except for the male
    void ReactivateMale()
    {
        // reattach the male to the list
        Info.maleInfo.transform.SetParent(maleGrid.transform);

        foreach (Transform cat in maleGrid.transform)
        {
            cat.gameObject.GetComponent<Button>().interactable = true;
        }

        // make the female null since the player didn't want her :(
        Info.theMale = null;

        // deactivate the choose button
        chooseMaleButton.GetComponent<Button>().interactable = false;
    }

    // Update is called once per frame
    // TODO: fix each info button so that they have event listeners instead of calling this in update
    void FixedUpdate()
    {
        // check if the female has been selected then make all of the cat info on the female list inactive
        if (femaleSelected)
        {
            foreach (Transform cat in femaleGrid.transform)
            {
                cat.gameObject.GetComponent<Button>().interactable = false;
            }

            // reactivate the choose button
            chooseFemaleButton.GetComponent<Button>().interactable = true;
            chooseFemaleButton.GetComponent<Button>().onClick.AddListener(() => ReactivateFemale());

            // set the female's parent to the displaying transform
            Info.femaleInfo.gameObject.transform.SetParent(displayFemale.transform, false);

            // so that this is only called once
            femaleSelected = false;
        }

        if (maleSelected)
        {
            foreach (Transform cat in maleGrid.transform)
            {
                cat.gameObject.GetComponent<Button>().interactable = false;
            }

            // reactivate the choose button
            chooseMaleButton.GetComponent<Button>().interactable = true;
            chooseMaleButton.GetComponent<Button>().onClick.AddListener(() => ReactivateMale());

            // set the male's parent to the displaying transform
            Info.maleInfo.gameObject.transform.SetParent(displayMale.transform, false);

            // so that this is only called once
            maleSelected = false;
        }

        // instantiate the compare option if compare is true
        //GET RID OF THIS
        if (compare)
        {
            compare = false;
            CompareMenu = Instantiate(compareMenuPrefab) as GameObject;
            CompareMenu.transform.SetParent(transform, false);
        }
    }
}
