using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Events;

// Manages all of it's dialogues
// also contains "the" male and "the" female..........  it also kills them
// TODO: Create a showdialogue function for breedGui so that it can easiy create the dialogues and communicate with
// them more cleanly
public class BreedGUI : MonoBehaviour {

    // tell if the female and the male have been selected
    public static bool femaleSelected = false;
    public static bool maleSelected = false;

    // fields for comparing genetics
    public GameObject compareMenuPrefab;
    private static GameObject compareMenu;
    public GameObject optionsPrefab;
    private static GameObject options;
    public GameObject choosePrefab;
    private static GameObject choose;
    public GameObject punitPrefab;
    private static GameObject punit;
    public static string currentlyComparing;

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

    static BreedGUI instance;

    // PROPERTIES
    public static GameObject CompareMenu
    {
        get { return compareMenu; }
        set { compareMenu = value; }
    }

    public static GameObject Options
    {
        get{ return options; }
        set { options = value; }
    }

    public static GameObject Choose
    {
        get { return choose; }
        set { choose = value; }
    }

    public static GameObject Punit
    {
        get { return punit; }
        set { punit = value; }
    }

    // breed gui instance
    public static BreedGUI Instance
    {
        get { return instance; }
    }
    void Awake ()
    {
        instance = this;

        // create the male and female lists
        femaleGrid = GameObject.Find("Female Grid");
        maleGrid = GameObject.Find("Male Grid");

        // attach event listeners
        chooseFemaleButton.GetComponent<Button>().onClick.AddListener(() => ReactivateFemale());
        chooseMaleButton.GetComponent<Button>().onClick.AddListener(() => ReactivateMale());
    }

    public void AddInfo(CatGenetics cat)
    {
        // TRAIN OF THOUGHT: the cat should tell the curently created cat info what it's name and other info is
        // no list of all the names
        // no list of all the info, just get the name text
        catInfo = Instantiate(catInfoPrefab) as GameObject;
        catInfo.GetComponent<Button>().interactable = true;

        // add the cat to the appropriate list based on gender
        // NOTE: this should eventually be able to sort the list based on what the user specifies right now sorting is random
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

    // creates the compare menu. Should eventually be show dialogue instead
    public void Compare()
    {
        options = Instantiate(optionsPrefab) as GameObject;
        Options.transform.SetParent(transform, false);
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

    // This destroys the breed gui and takes care of everything that needs to be done
    // TODO: add the success of fail text
    public void SelfDestruct()
    {
        Info.theFemale = null;
        Info.theMale = null;
        Destroy(gameObject);
        LoveShack.menuDisplaying = false;
    }

    // Update is called once per frame
    // TODO: see if there is a way to just call a method once it is determined that breeding is ready
    // TODO: when the user selects to pick different cats from the menu or choose a different single cat it should "reset" the list
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
    }
}
