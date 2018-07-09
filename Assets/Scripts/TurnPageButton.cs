using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NextBack{
    next, back, goToPage
 }

public class TurnPageButton : MonoBehaviour {

	public NextBack dropdown;

    public int goToPage;

	// Whether the Google Cardboard user is gazing at this button.
    private bool _isLookedAt = false;

    // How long the user can gaze at this before the button is clicked.
    public float TimerDuration = 1f;

    // Count time the player has been gazing at the button.
    private float _lookTimer = 0f;

    // Links with book logic
    public Book book;

    public void SetGazedAt(bool gazedAt)
    {
        _isLookedAt = gazedAt;
    }


    void Update()
    {
        // While player is looking at this button.
        if (_isLookedAt)
        {
            // Increment the gaze timer.
            _lookTimer += Time.deltaTime;

            // Gaze time exceeded limit - button is considered clicked.
            if (_lookTimer > TimerDuration)
            {
                _lookTimer = 0f;

                switch(dropdown)
                {
                	case NextBack.next:
                		book.TurnNextPage();
                		break;
                	case NextBack.back:
                		book.TurnBackPage();
                		break;
                    case NextBack.goToPage:
                        book.GoToPage(goToPage);
                        break;
                }
            }
        }
        else
        {
            _lookTimer = 0f;
        }
    }
}
