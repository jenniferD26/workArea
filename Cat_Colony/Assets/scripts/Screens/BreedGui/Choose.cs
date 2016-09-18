using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Choose : MonoBehaviour {

    public Transform grid;
    public Button backButton;
    public Button exitButton;

    // Buttons to add event listeners to, they should be made when their corrisponding menu is made
    public Dictionary<string, Button> geneButtons = new Dictionary<string, Button>();

    // Use this for initialization
    void Start()
    {
        backButton.onClick.AddListener(() => Back());
        exitButton.onClick.AddListener(() => Exit());

        // add each of the buttons in the menu into a dictionary to keep track of them
        foreach (Transform child in grid)
        {
            if (!geneButtons.ContainsKey(child.name))
            {
                geneButtons.Add(child.name, child.gameObject.GetComponent<Button>());
                geneButtons[child.name].onClick.AddListener(delegate { GeneScreen(); });
            }
        }
    }

    // when a gene is clicked the punit table screen will be set with all of 
    public void GeneScreen()
    {
        string geneName = EventSystem.current.currentSelectedGameObject.name;
        BreedGUI.Punit = Instantiate(BreedGUI.Instance.punitPrefab) as GameObject;
        BreedGUI.Punit.transform.SetParent(BreedGUI.Instance.transform, false);
        BreedGUI.Punit.GetComponent<Punit>().SetTable(Info.theFemale.GeneDictionary(geneName), Info.theMale.GeneDictionary(geneName));
        Destroy(gameObject);
    }

    // goes to the previous menu
    void Back()
    {
        BreedGUI.Options = Instantiate(BreedGUI.Instance.optionsPrefab) as GameObject;
        BreedGUI.Options.transform.SetParent(BreedGUI.Instance.transform, false);
        Destroy(gameObject);
    }

    // exits from the menu
    void Exit()
    {
        Destroy(gameObject);
    }
}
