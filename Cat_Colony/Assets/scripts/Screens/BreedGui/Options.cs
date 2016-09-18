using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;

//On start label all of the components and add event handlers for all of the buttons
public class Options : MonoBehaviour
{
    public Button compareButton;
    public Button breedButton;
    public Button exitButton;

    // Use this for initialization
    void Start()
    {
        compareButton.onClick.AddListener(() => Compare());
        breedButton.onClick.AddListener(() => Breed());
        exitButton.onClick.AddListener(() => Exit());
    }

    //tells the breed gui to compare the genetics
    void Compare()
    {
        BreedGUI.Choose = Instantiate(BreedGUI.Instance.choosePrefab) as GameObject;
        BreedGUI.Choose.transform.SetParent(BreedGUI.Instance.transform, false);
        Destroy(gameObject);
    }

    //breeds the cats then tells the breed gui to go kill itself
    void Breed()
    {
        // breed the cats and then kill them and self destruct
        LoveShack.Instance.BreedCats(Info.theMale, Info.theFemale);
        BreedGUI.Instance.SelfDestruct();
    }

    // suicidle lion
    void Exit()
    {
        Destroy(gameObject);
    }
}
