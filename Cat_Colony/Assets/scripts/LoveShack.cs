using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.UI;

//The love shack is a little old place where we can get together!
/*
           _________##
          @\\\\\\\\\##
         @@@\\\\\\\\##\
        @@ @@\\\\\\\\\\\
       @@@@@@@\\\\\\\\\\\
      @@@@@@@@@----------|
      @@ @@@ @@__________|
      @@@@@@@@@__________|
      @@@@ .@@@__________|
_\|/__@@@@__@@@__________|__

*/

// The breedgui and all of it's dialogues talks directly with this script
// CONTAINS THE CAT LIST

public class LoveShack : MonoBehaviour {

    public GameObject breedButonPrefab;
    private GameObject breedButton;

    // The list of cats
    public static List<CatGenetics> catList = new List<CatGenetics>();
    public GameObject catPrefab;
    private GameObject catCarrier;

    // menu fields
    public GameObject breedGUIPrefab;
    private GameObject breedGUI;
    public static bool menuDisplaying = false;
    private static bool listDone = true;

    //breeding gatos
    float rand = 0f;
    private CatGenetics kitten;
    public static string[] catNames;

    // create an instance of the love shack just so that other scripts can breed cats
    private static LoveShack loveShack;

    //Properties
    public static LoveShack LShack
    {
        get { return loveShack; }
    }

    void Start()
    {
        // instantiate the loveshack
        loveShack = this;

        catNames = new string[12];
        catNames[0] = "Plague";
        catNames[1] = "Priss";
        catNames[2] = "Joy";
        catNames[3] = "Flapjack";
        catNames[4] = "K'Knuckles";
        catNames[5] = "Blazeheart";
        catNames[6] = "Fuzzydare";
        catNames[7] = "Tobias";
        catNames[8] = "Willis";
        catNames[9] = "Emporer Bossy McPaws";
        catNames[10] = "Sir Pouncy McPaws";
        catNames[11] = "Tater McMuffin";
    }
    // Create the breed button when the loveshack is hovered over
    void OnMouseOver()
    {
        // only instantiate if the breed button doesn't exist
        if(breedButton == null && !menuDisplaying)
        {
            breedButton = Instantiate(breedButonPrefab) as GameObject;
        }
    }

    // destroy the breed button when the mouse exits
    void OnMouseExit()
    {
        Destroy(breedButton);
    }

    // instantiates the breed menu
    public void BreedStart()
    {
        // the menu is displaying now
        menuDisplaying = true;
        listDone = false;

        // create the breed menu
        breedGUI = Instantiate(breedGUIPrefab);
    }

    // given a male and a female breed the two cats to make a baby
    public void BreedCats(CatGenetics sire, CatGenetics dam)
    {
        // the baby! :D
        catCarrier = Instantiate(catPrefab) as GameObject;
        kitten = catCarrier.GetComponentInChildren<CatGenetics>();

        // randomize the kitten's name
        rand = Random.Range(0, catNames.Length);
        kitten.CatName = catNames[(int)rand];

        // set the black gene
        SetGeneNode(sire.BlackGene, dam.BlackGene, kitten, kitten.BlackGene);
        kitten.BlackGene.Sort();

        catList.Add(kitten);

        Debug.Log(kitten.BlackGene.GeneToString());
    }

    // made to make the breeding function smaller, sets a genenode when cats are bred
    public void SetGeneNode(GeneNode sireGene, GeneNode damGene, CatGenetics baby, GeneNode geneNode)
    {
        // the baby cat has a 50/50 chance for each and every gene
        rand = Random.Range(0.0f, 1.0f);

        if (rand < 0.25)
        {
            // kitten's genes are father's and mothers fist gene
            geneNode.GeneOne = sireGene.GeneOne;
            geneNode.GeneTwo = damGene.GeneOne;
        }
        else if (rand < 0.5)
        {
            // kitten's genes are father's second gene and mothers fist gene
            baby.BlackGene.GeneOne = sireGene.GeneTwo;
            baby.BlackGene.GeneTwo = damGene.GeneOne;
        }
        else if (rand < 0.75)
        {
            // kitten's genes are father's second gene and mothers fist gene
            baby.BlackGene.GeneOne = sireGene.GeneOne;
            baby.BlackGene.GeneTwo = damGene.GeneTwo;
        }
        else if (rand < 1)
        {
            // kitten's genes are father's second gene and mothers fist gene
            baby.BlackGene.GeneOne = sireGene.GeneTwo;
            baby.BlackGene.GeneTwo = damGene.GeneTwo;
        }
    }

    // Update will populate the list then since it's being stupid >:(
    void Update()
    {
        // ADD THIS TO A FUNCTION OUTSIDE OF UPDATE
        // as long as the list is not done being populated add each cat into the info list
        if (!listDone)
        {
            breedGUI = GameObject.Find("BreedGUI(Clone)");

            foreach (CatGenetics cat in catList)
            {
                breedGUI.GetComponent<BreedGUI>().AddInfo(cat);
            }
            listDone = true;
        }
        if (menuDisplaying)
        {
            Destroy(breedButton);
        }

        if (catList.Count == 0)
        {
            // the list is a list of genetics so that even if the mesh is gone the genetics still exist in this list
            // add Tom to the list
            catCarrier = Instantiate(catPrefab) as GameObject;
            catCarrier.GetComponentInChildren<CatGenetics>().CatName = "Tom";
            catCarrier.GetComponentInChildren<CatGenetics>().Gender = "Male";
            catCarrier.GetComponentInChildren<CatGenetics>().BlackGene.GeneTwo = Genetics.b;
            catList.Add(catCarrier.GetComponentInChildren<CatGenetics>());

            // add Sunshine to the list
            catCarrier = Instantiate(catPrefab) as GameObject;
            catCarrier.GetComponentInChildren<CatGenetics>().CatName = "Sunshine";
            catCarrier.GetComponentInChildren<CatGenetics>().Gender = "Female";
            catCarrier.GetComponentInChildren<CatGenetics>().BlackGene.GeneTwo = Genetics.bl;
            catList.Add(catCarrier.GetComponentInChildren<CatGenetics>());
        }

        //DEBUGGING
        if (Input.GetKeyDown(KeyCode.Space))
        {
            catCarrier = Instantiate(catPrefab) as GameObject;
            catList.Add(catCarrier.GetComponent<CatGenetics>());
        }
    }
}
