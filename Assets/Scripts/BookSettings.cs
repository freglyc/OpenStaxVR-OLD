using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Buttons{
    towardPlayer, awayPlayer, rotateTopBook, rotateBotBook
 }

public class BookSettings : MonoBehaviour {

	// Gets the book
	public GameObject book;

	// Button choices
	public Buttons current;

	// How long the user can gaze at this before the button is clicked.
	public float timerDuration = 1f;

	// Count time the player has been gazing at the button.
	private float lookTimer = 0f;

	// Whether the Google Cardboard user is gazing at this button.
	private bool isLookedAt = false;

	public void SetGazedAt(bool gazedAt) {
		isLookedAt = gazedAt;
	}
	
	// Update is called once per frame
	void Update () {

		// Player looking at the buttons
		if(isLookedAt){

			// Increment the gaze timer.
			lookTimer += Time.deltaTime;

			// Gaze time exceeded limit - button is considered clicked.
			if (lookTimer > timerDuration) {
				lookTimer = 0f;

				switch (current){
					case Buttons.towardPlayer: // Moves the book toward the user
						book.transform.position += new Vector3(0,0,-1);
						break;
					case Buttons.awayPlayer: // Moves the book away from the user
						book.transform.position += new Vector3(0,0,1);
						break;
					case Buttons.rotateTopBook: // Rotates the top of the book toward the user
						book.transform.Rotate(-10,0,0);
						break;
					case Buttons.rotateBotBook: // Rotates the bottom of the book toward the user
						book.transform.Rotate(10,0,0);
						break;
				}
			}
		} else {
			lookTimer = 0f;
		}
	}
}
