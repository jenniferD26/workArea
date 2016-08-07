using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
            |\___/|
            )     (
           =\     /=
             )===(
            /     \
            |     |
           /       \
           \       /
      jgs   \__  _/
              ( (
               ) )
              (_(

*/
// should contain all of the cat's genetics + name. Coat, Attributes, Health
public class CatGenetics : MonoBehaviour {

    public GameObject geneticsGUIPrefab;
    private GameObject geneticsGUI;

    private string catName = "Name";
    private string coatColor = "Black";
    private string gender = "";
    private PersonalityType personality;
    public float chance = 0.5f;     //Chance of the common case happening
    private float rand = 0f;

    // dictionary of all the gene nodes and their names as strings, to make it easier for other scripts to access
    // info about the gene
    private Dictionary<string, GeneNode> geneDictionary = new Dictionary<string, GeneNode>();

    // the individual gene node
    private GeneNode blackGene = new GeneNode();

    //Properties
    public string CatName
    {
        get { return catName; }
        set { catName = value; }
    }

    // quick coat color for gui design
    public string CoatColor
    {
        get { return coatColor; }
        set { coatColor = value; }
    }

    // the gender string
    public string Gender
    {
        get { return gender; }
        set { gender = value; }
    }

    // the personality string
    // TODO: needs custom setter
    public string Personality
    {
        get { return personality.ToString(); }
    }

    // access to both genes for breeding and punit tables
    public GeneNode BlackGene
    {
        get { return blackGene; }
        set { blackGene = value; }
    }

    // custom getter for the geneDictionary, can only get no one else should be able to set
    public GeneNode GeneDictionary(string key)
    {
        return geneDictionary[key];
    }

    // RECURSIVE FUNCTION, is given an index and will compare it to each chance given
    // dictionary, the dictionary to search for the personality
    // index, the current personality in the dictionary, is also assigned the returned personality
    // random, the rand that was rolled to determine which personality is chosen
    private PersonalityType SelectPersonality(Dictionary<PersonalityType, Personality> dictionary, PersonalityType index, float random)
    {
        // if the chance of the current personality is greater than the rand then step deeper into recursion
        if (random > dictionary[index].chance)
        {
            index = SelectPersonality(dictionary, index + 1, random);
        }

        // send index back to caller
        return index;
    }

    // Create the cat completely before any other script is run
    // TODO: possibly make the below it's own funtion so that this isn't done everytime a cat is made?
    void Awake () {

        #region black gene
        // set the black gene to a random value at start, first gene is always B
        blackGene.GeneOne = Genetics.B;

        // seed the random number
        rand = Random.Range(0.0f, 1.0f);

        // if rand is less than the common case then set the second gene to B
        if(rand < chance)
        {
            blackGene.GeneTwo = Genetics.B;
        }

        // The chance of b, brown coat, of happening is 75% of the percentage remaining
        else if(rand < chance + (chance * 0.75f))
        {
            blackGene.GeneTwo = Genetics.b;
        }

        // otherwise the bl gene is assigned
        else
        {
            blackGene.GeneTwo = Genetics.bl;
        }
        #endregion

        #region Gender
        // the gender is 50/50
        rand = Random.Range(0.0f, 1.0f);
        if (rand < 0.5)
        {
            gender = "Male";
        }
        else
        {
            gender = "Female";
        }
        #endregion

        #region Personality
        // the personality, first it selects the category(dictionary) based on the constant chances in the personality dictionary
        // next it uses the recursuve SelectPersonality function to choose the specific personality
        rand = Random.Range(0.0f, 1.0f);
        if(rand < PersonalityDictionary.CUTE_CHANCE)
        {
            rand = Random.Range(0.0f, 1.0f);
            personality = SelectPersonality(PersonalityDictionary.Cute, PersonalityType.Derpy, rand);
        }
        else if (rand < PersonalityDictionary.AGILE_CHANCE)
        {
            rand = Random.Range(0.0f, 1.0f);
            personality = SelectPersonality(PersonalityDictionary.Agile, PersonalityType.Alert, rand);
        }
        else if (rand < PersonalityDictionary.STRENGTH_CHANCE)
        {
            rand = Random.Range(0.0f, 1.0f);
            personality = SelectPersonality(PersonalityDictionary.Strength, PersonalityType.Tough, rand);
        }
        #endregion

        // at the end of all the genetics being set populate the gene dictionary with all of the genenodes
        geneDictionary.Add("BlackGene", blackGene);
    }

    // on mouse down is for when the cat is clicked it creates the genetics menu/GUI
    void OnMouseDown()
    {
        geneticsGUI = Instantiate(geneticsGUIPrefab);
        geneticsGUI.GetComponent<GeneticsGUI>().CatMaster = GetComponent<CatGenetics>();
    }
}
