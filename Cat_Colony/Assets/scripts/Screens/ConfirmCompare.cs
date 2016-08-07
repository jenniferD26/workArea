using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;

//On start label all of the components and add event handlers for all of the buttons
public class ConfirmCompare : MonoBehaviour {

    Button compareButton;
    Button breedButton;

	// Use this for initialization
	void Start () {
        compareButton = transform.FindChild("Compare").GetComponent<Button>();
        breedButton = transform.FindChild("Breed").GetComponent<Button>();

        compareButton.onClick.AddListener(() => Compare());
    }
	
    //tells the breed gui to compare the genetics
    void Compare()
    {
        Debug.Log("Compare");
    }

    //tells the breed gui to breed the cats
    void Breed()
    {
        Debug.Log("Breed");
    }
}
