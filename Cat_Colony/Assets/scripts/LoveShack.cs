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
    private static LoveShack instance;

    //Properties
    public static LoveShack Instance
    {
        get { return instance; }
    }

    void Start()
    {
        // instantiate the loveshack
        instance = this;

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
        Destroy(breedButton);

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
        SetGeneNode(sire.BlackGene, dam.BlackGene, kitten.BlackGene);
        kitten.BlackGene.Sort();

        // set the gender gene
        SetGeneNode(sire.GenderGene, dam.GenderGene, kitten.GenderGene);
        kitten.GenderGene.Sort();

        // tell the kitten to determine the names of its gene pairs
        kitten.DetermineGenes();

        catList.Add(kitten);

        Debug.Log(kitten.GenderGene.GeneToString());
    }

    // made to make the breeding function smaller, sets a genenode when cats are bred
    public void SetGeneNode(GeneNode sireGene, GeneNode damGene, GeneNode geneNode)
    {
        // the baby cat has a 50/50 chance for each and every gene
        rand = Random.Range(0.0f, 1.0f);

        if (rand < 0.25)
        {
            // kitten's genes are father's and mothers first gene
            geneNode.GeneOne = sireGene.GeneOne;
            geneNode.GeneTwo = damGene.GeneOne;
        }
        else if (rand < 0.5)
        {
            // kitten's genes are father's second gene and mothers fist gene
            geneNode.GeneOne = sireGene.GeneTwo;
            geneNode.GeneTwo = damGene.GeneOne;
        }
        else if (rand < 0.75)
        {
            // kitten's genes are father's second gene and mothers fist gene
            geneNode.GeneOne = sireGene.GeneOne;
            geneNode.GeneTwo = damGene.GeneTwo;
        }
        else if (rand < 1)
        {
            // kitten's genes are father's second gene and mothers fist gene
            geneNode.GeneOne = sireGene.GeneTwo;
            geneNode.GeneTwo = damGene.GeneTwo;
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

        if (catList.Count == 0)
        {
            // the list is a list of genetics so that even if the mesh is gone the genetics still exist in this list
            // add Tom to the list
            catCarrier = Instantiate(catPrefab) as GameObject;
            catCarrier.GetComponentInChildren<CatGenetics>().CatName = "Tom";
            catCarrier.GetComponentInChildren<CatGenetics>().GenderGene.GeneTwo = Genetics.Y;
            catCarrier.GetComponentInChildren<CatGenetics>().BlackGene.GeneOne = Genetics.b;
            catCarrier.GetComponentInChildren<CatGenetics>().BlackGene.GeneTwo = Genetics.b;
            catCarrier.GetComponentInChildren<CatGenetics>().DetermineGenes();
            Debug.Log(catCarrier.GetComponentInChildren<CatGenetics>().GenderGene.GeneToString());
            catList.Add(catCarrier.GetComponentInChildren<CatGenetics>());

            // add Sunshine to the list
            catCarrier = Instantiate(catPrefab) as GameObject;
            catCarrier.GetComponentInChildren<CatGenetics>().CatName = "Sunshine";
            catCarrier.GetComponentInChildren<CatGenetics>().GenderGene.GeneOne = Genetics.XO;
            catCarrier.GetComponentInChildren<CatGenetics>().GenderGene.GeneTwo = Genetics.Xo;
            catCarrier.GetComponentInChildren<CatGenetics>().BlackGene.GeneOne = Genetics.bl;
            catCarrier.GetComponentInChildren<CatGenetics>().BlackGene.GeneTwo = Genetics.bl;
            catCarrier.GetComponentInChildren<CatGenetics>().DetermineGenes();
            Debug.Log(catCarrier.GetComponentInChildren<CatGenetics>().GenderGene.GeneToString());
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
