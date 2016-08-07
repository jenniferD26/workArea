using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class CompareMenu : MonoBehaviour {

    //there's going to be a hashtable of every type of screen because freaking
    // TODO: rewrite code so that screen toss is a stack, to make backing out of screens and attaching screens easier and more intuitive
    private Dictionary<string, GameObject> screenToss = new Dictionary<string, GameObject>();
    private Stack<string> screenStack = new Stack<string>();
    public GameObject optionsPrefab;
    public GameObject chooseGenePrefab;
    public GameObject geneComparePrefab;

    // the success of fail text
    public GameObject successOrFailPrefab;
    private GameObject successOrFail;

    // Buttons to add event listeners to, they should be made when their corrisponding menu is made
    private Dictionary<string, Button> geneButtons = new Dictionary<string, Button>();

    // Helper methods
    //start creates the options as the first screen
    void Start()
    {
        // add all of the screens to the Dictionary
        screenToss["options"] = Instantiate(optionsPrefab) as GameObject;
        screenToss["choose"] = Instantiate(chooseGenePrefab) as GameObject;
        screenToss["compare"] = Instantiate(geneComparePrefab) as GameObject;

        //attach the options to the screen and push onto the stack
        /*screenToss["options"].transform.SetParent(transform, false);
        screenStack.Push("options");
        screenToss[screenStack.Peek()].transform.FindChild("CompareButton").gameObject.GetComponent<Button>().onClick.AddListener(() => CompareGenetics());
        screenToss[screenStack.Peek()].transform.FindChild("BreedCats").gameObject.GetComponent<Button>().onClick.AddListener(() => BreedCats());*/

        //set the exit button to explode
        gameObject.transform.FindChild("Exit").GetComponent<Button>().onClick.AddListener(() => TickTickBoom());
    }

    // destroys only the compare menu
    void TickTickBoom()
    {
        foreach (string key in screenToss.Keys)
        {
            Destroy(screenToss[key]);
        }
        screenStack.Clear();
        Destroy(gameObject);
    }

    // destroys the breed gui and everything associated with it
    void SelfDestruct()
    {
        foreach(string key in screenToss.Keys)
        {
            Destroy(screenToss[key]);
        }

        screenStack.Clear();
        Destroy(GameObject.Find("BreedGUI(Clone)"));

        // on destroy the menu is not displaying and instantiate a success of fail text
        // TODO: should eventually set if the breeding was successful or not based on personality
        // right now it only says success
        Instantiate(successOrFailPrefab as GameObject);
    }

    // Dettaches the revious screen and pushes the new screen onto the stack
    void NextScreen(string key)
    {
        screenToss[screenStack.Peek()].transform.SetParent(null, false);
        screenStack.Push(key);
        screenToss[key].transform.SetParent(transform, false);
    }

    //Button functions
    // pops a menu screen off of the stack
    void Back()
    {
        screenToss[screenStack.Pop()].transform.SetParent(null, false);
        NextScreen(screenStack.Peek());
    }

    // breeds the cats and self destructs the breed gui
    public void BreedCats()
    {
        // Breed the cats then kill them and self destruct, plus check if the male and female are not null before attempting to breed
        // all the checks for null are to prevent breeding multiple times
        if (Info.theMale != null && Info.theFemale != null)
        {
            LoveShack.LShack.BreedCats(Info.theMale, Info.theMale);
            Info.theFemale = null;
            Info.theMale = null;
            SelfDestruct();
        }
    }
    // compare genetics when the compare button is pressed
    public void CompareGenetics()
    {
        //Detach the options menu and attach the choose screen
        NextScreen("choose");

        // set the back button
        screenToss[screenStack.Peek()].transform.FindChild("Back").gameObject.GetComponent<Button>().onClick.AddListener(() => Back());
        Transform grid = screenToss[screenStack.Peek()].transform.FindChild("Grid");

        // add event listeners for the buttons with the curently selected male and female
        foreach (Transform child in grid)
        {
            if (!geneButtons.ContainsKey(child.name))
            {
                geneButtons.Add(child.name, child.gameObject.GetComponent<Button>());
            }
            geneButtons[child.name].onClick.AddListener(() => SetGeneScreen(Info.theFemale.GeneDictionary(child.name), Info.theMale.GeneDictionary(child.name)));
        }
    }

    // gets all of the info from the sire and dam to set the gene screen wit hthe proper info
    private void SetGeneScreen(GeneNode dam, GeneNode sire)
    {
        // Dettach the previous screen and then attach the gene comparing screen and push onto the queue
        NextScreen("compare");

        // get the grid to display the gene combinations
        GameObject grid = screenToss[screenStack.Peek()].transform.FindChild("Grid").gameObject;
        GeneNode tempGene = new GeneNode();

        // display the mother and father genes
        screenToss[screenStack.Peek()].transform.FindChild("SireGeneOne").gameObject.GetComponent<Text>().text = sire.GeneOne.ToString();
        screenToss[screenStack.Peek()].transform.FindChild("SireGeneTwo").gameObject.GetComponent<Text>().text = sire.GeneTwo.ToString();
        screenToss[screenStack.Peek()].transform.FindChild("DamGeneOne").gameObject.GetComponent<Text>().text = dam.GeneOne.ToString();
        screenToss[screenStack.Peek()].transform.FindChild("DamGeneTwo").gameObject.GetComponent<Text>().text = dam.GeneTwo.ToString();

        // first set Father One and Mother One and set it in a temp gene node
        tempGene.GeneOne = dam.GeneOne;
        tempGene.GeneTwo = sire.GeneOne;
        tempGene.Sort();
        grid.transform.FindChild("OneOne").gameObject.GetComponentInChildren<Text>().text = tempGene.GeneToString();

        // set the One Two case
        tempGene.GeneOne = dam.GeneOne;
        tempGene.GeneTwo = sire.GeneTwo;
        tempGene.Sort();
        grid.transform.FindChild("OneTwo").gameObject.GetComponentInChildren<Text>().text = tempGene.GeneToString();

        // Two One case
        tempGene.GeneOne = dam.GeneTwo;
        tempGene.GeneTwo = sire.GeneOne;
        tempGene.Sort();
        grid.transform.FindChild("TwoOne").gameObject.GetComponentInChildren<Text>().text = tempGene.GeneToString();

        // Two Two case
        tempGene.GeneOne = dam.GeneTwo;
        tempGene.GeneTwo = sire.GeneTwo;
        tempGene.Sort();
        grid.transform.FindChild("TwoTwo").gameObject.GetComponentInChildren<Text>().text = tempGene.GeneToString();

        // add event listeners for the buttons
        screenToss[screenStack.Peek()].transform.FindChild("Previous").gameObject.GetComponent<Button>().onClick.AddListener(() => Back());

        // breed the cats and destroy everythign associated with the breed gui
        screenToss[screenStack.Peek()].transform.FindChild("Breed").gameObject.GetComponent<Button>().onClick.AddListener(() => BreedCats());
    }
}
