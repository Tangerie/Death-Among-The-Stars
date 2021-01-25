using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class Universe : MonoBehaviour
{
    public const double BIG_G = 6.673;
    
    public static Universe Instance;

    private void Awake() {
        if(Instance == null) {
            Instance = this;
        } else {
            Debug.LogWarning("Universe has multiple instances. One has been removed");
            Destroy(gameObject);
            return;
        }

        PlayerObject = FindObjectOfType<RigidbodyPlayerController>().gameObject;
        RewiredPlayer = ReInput.players.GetPlayer(0);
        ChoiceController = FindObjectOfType<StoryChoiceController>();
        FauxGravityAttractors = new List<FauxGravityAttractor>();
        FauxGravityBodies = new List<FauxGravityBody>();
    }

    public Player RewiredPlayer;
    public GameObject PlayerObject;

    public StoryChoiceController ChoiceController;

    public bool isPaused = false;

    public List<FauxGravityAttractor> FauxGravityAttractors;
    public List<FauxGravityBody> FauxGravityBodies;

    public void SetPause(bool pause) {
        isPaused = pause;

        Time.timeScale = isPaused ? 0 : 1;
        AudioListener.pause = isPaused;
    }

    public void TogglePauseGame() {
        isPaused = !isPaused;

        Time.timeScale = isPaused ? 0 : 1;
        AudioListener.pause = isPaused;
    }
}
