using UnityEngine;

public class HideButton : MonoBehaviour
{
    // How long the user can gaze at this before the button is clicked.
    public float TimerDuration = 1f;

    // Whether the Google Cardboard user is gazing at this button.
    private bool _isLookedAt = false;

    // Count time the player has been gazing at the button.
    private float _lookTimer = 0f;

    // Links with book logic
    public BookLogic Book;
    public NextOrBackButton Next;
    public NextOrBackButton Back;
    public AutoSaveButton AutoSave;

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
                Book.Hide();
                Next.Hide();
                Back.Hide();
            }
        }
        else
        {
            _lookTimer = 0f;
        }
    }
}