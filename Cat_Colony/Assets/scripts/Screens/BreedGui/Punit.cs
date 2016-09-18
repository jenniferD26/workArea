using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Punit : MonoBehaviour {

    public Button breedButton;
    public Button backButton;
    public Button exitButton;
    public GameObject background;
    public GameObject grid;

    // Use this for initialization
    void Start () {

        breedButton.onClick.AddListener(() => Breed());
        backButton.onClick.AddListener(() => Back());
        exitButton.onClick.AddListener(() => Exit());
    }
	
	// sets the punit table with the gene given and is public so that choose can tell it what to do
    public void SetTable(GeneNode dam, GeneNode sire)
    {
        // get the grid to display the gene combinations
        GeneNode tempGene = new GeneNode();

        // display the mother and father genes
        background.transform.FindChild("SireGeneOne").gameObject.GetComponent<Text>().text = sire.GeneOne.ToString();
        background.transform.FindChild("SireGeneTwo").gameObject.GetComponent<Text>().text = sire.GeneTwo.ToString();
        background.transform.FindChild("DamGeneOne").gameObject.GetComponent<Text>().text = dam.GeneOne.ToString();
        background.transform.FindChild("DamGeneTwo").gameObject.GetComponent<Text>().text = dam.GeneTwo.ToString();

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
    }

    //breeds the cats then tells the breed gui to go kill itself
    void Breed()
    {
        // breed the cats and then kill them and self destruct
        LoveShack.Instance.BreedCats(Info.theMale, Info.theFemale);
        BreedGUI.Instance.SelfDestruct();
    }

    // goes to the previous menu
    void Back()
    {
        BreedGUI.Choose = Instantiate(BreedGUI.Instance.choosePrefab) as GameObject;
        BreedGUI.Choose.transform.SetParent(BreedGUI.Instance.transform, false);
        Destroy(gameObject);
    }

    // exits from the menu
    void Exit()
    {
        Destroy(gameObject);
    }
}
