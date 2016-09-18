using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;

// contains all of the members for the genetics menu, the gene nodes are each contained in a list and made here
// allows changing the name, and displays descriptions for each genenode
public class GeneticsGUI : MonoBehaviour {

    private CatGenetics catMaster;

    // the texts that need to be changed
    public Text nameText;
    public Text personalityText;
    public GameObject geneticsGrid;

    // objects to hide or show for editing the name
    public GameObject nameBox;
    public Text nameLabel;
    public GameObject editButton;
    public InputField changeName;
    private string oldName;

    // objects for each genetic node
    public GameObject geneticsInfoPrefab;
    private GameObject geneticsInfo;

    //fields to display the description text
    public GameObject description;

    //Properties
    public CatGenetics CatMaster{
        get { return catMaster; }
        set { catMaster = value; }
    }

	// Use this for initialization
	void Start () {

        // onstart update the name and personality with the catmaster's info, make sure everything stops
        nameText.text = catMaster.CatName;
        personalityText.text = catMaster.Personality;
        LoveShack.menuDisplaying = true;

        // dettach the change name input field and add event listener for the edit button
        changeName.transform.SetParent(null, false);
        oldName = catMaster.CatName;
        editButton.GetComponent<Button>().onClick.AddListener(() => ChangeName());

        // create a new genetic info for each genetic node in the cat
        // TODO: this should eventually iterate through a list of each of the cat's gene nodes

        // gender
        geneticsInfo = Instantiate(geneticsInfoPrefab);
        geneticsInfo.transform.SetParent(geneticsGrid.transform, false);
        geneticsInfo.transform.Find("Genetics Type").GetComponent<Text>().text = catMaster.Gender;
        geneticsInfo.transform.Find("Node Text").GetComponent<Text>().text = catMaster.GenderGene.GeneToString();
        geneticsInfo.GetComponent<Button>().onClick.AddListener(() => StartDescription(catMaster.Gender));

        // black gene
        geneticsInfo = Instantiate(geneticsInfoPrefab);
        geneticsInfo.transform.SetParent(geneticsGrid.transform, false);
        geneticsInfo.transform.Find("Genetics Type").GetComponent<Text>().text = catMaster.CoatColor;
        geneticsInfo.transform.Find("Node Text").GetComponent<Text>().text = catMaster.BlackGene.GeneToString();
        geneticsInfo.GetComponent<Button>().onClick.AddListener(() => StartDescription(catMaster.CoatColor));

        // dettach the description panel and add event listener for the exit button
        description.transform.SetParent(null, false);
        description.transform.FindChild("Exit").gameObject.GetComponent<Button>().onClick.AddListener(() => ExitDescription());

    }

    // allows th user to edit the name, by creating the input field and changing the text in the edit button
    void ChangeName()
    {
        // reset the edit button and the inputfield
        editButton.GetComponent<Button>().onClick.RemoveAllListeners();

        nameText.text = "";
        nameLabel.text = "";
        changeName.transform.SetParent(nameBox.transform, false);
        editButton.GetComponentInChildren<Text>().text = "Done";

        // set the event listeners
        editButton.GetComponent<Button>().onClick.AddListener(() => SetName());
    }

    // sets the name to the new value and adds the event listener back onto the button
    void SetName()
    {
        // reset the edit button and the inputfield
        changeName.onEndEdit.RemoveAllListeners();
        editButton.GetComponent<Button>().onClick.RemoveAllListeners();

        // undo everything that changename did and change the name
        changeName.transform.SetParent(null, false);

        // check if a value was entered into the inputfield
        if(changeName.text != "")
        {

            catMaster.CatName = changeName.textComponent.text;
        }
        nameText.text = catMaster.CatName;

        nameLabel.text = "Name";
        changeName.text = string.Empty;
        editButton.GetComponentInChildren<Text>().text = "Edit";

        // set the event listeners
        editButton.GetComponent<Button>().onClick.AddListener(() => ChangeName());
    }

    // exits from the description panel
    void ExitDescription()
    {
        description.transform.SetParent(null, false);
    }

    // starts the description dialogue
    void StartDescription(string key)
    {
        description.transform.SetParent(transform, false);
        description.transform.FindChild("Name").gameObject.GetComponent<Text>().text = key;
        description.transform.FindChild("Description").gameObject.GetComponent<Text>().text = Descriptions.description[key];
    }

    // Update is called once per frame
    void Update () {

        // when escape is pressed destroy this menu and allow camera control again
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            LoveShack.menuDisplaying = false;
            Destroy(gameObject);
        }
    }

    // make sure the description is destroyed
    void OnDestroy()
    {
        Destroy(description);
    }
}
