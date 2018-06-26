using UnityEngine;
using UnityEngine.UI;
using System.Collections;
// using UnityEditor.SceneManagement;
using UnityEngine.AI;

public class BookMarkButton : MonoBehaviour {
    
    // Links with book logic
    public BookLogic Book;
    
    // How long the user can gaze at this before the button is clicked.
    public float TimerDuration = 1f;

    // Count time the player has been gazing at the button.
    private float _lookTimer = 0f;
    
    // Whether the Google Cardboard user is gazing at this button.
    private bool _isLookedAt = false;

    public void SetGazedAt(bool gazedAt) {
        _isLookedAt = gazedAt;
    }

    void Update () {
        // While player is looking at this button.
        if (_isLookedAt) {
            // Increment the gaze timer.
            _lookTimer += Time.deltaTime;
            // Gaze time exceeded limit - button is considered clicked.
            if (_lookTimer > TimerDuration)
            {
                _lookTimer = 0f;
                Book.SaveProgress();
                
            }
        }
        else
        {
            _lookTimer = 0f;
        }
    }
    
    /*
     * Hides / displays the button according to the view mode.
     */
    public void Hide()
    {
        if (Book.HideBook)
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(false);
            }
        }
        else
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(true);
            }
            
        }
    }
    
    void Start()
    {
        if (Book.HideBook)
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(false);
				
            }
        }
    }
		
}