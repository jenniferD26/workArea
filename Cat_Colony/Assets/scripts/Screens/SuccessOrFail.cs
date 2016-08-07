using UnityEngine;
using System.Collections;

// this gives the appropriate message to the player when it is created based on what the breedGUI constructs it with
// it destroys itself after a certain amoutn of time
public class SuccessOrFail : MonoBehaviour {
    int timer = 0;

    // add constructor here


	// TODO: will set the text here
	void Start () {
	
	}
	
	// Fixed update will count down the timer and destroy itself when the timer is reached
	void FixedUpdate () {
	    if(timer >= 200)
        {
            Destroy(gameObject);
            LoveShack.menuDisplaying = false;
        }
        else
        {
            timer++;
        }
	}
}
