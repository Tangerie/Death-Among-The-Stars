using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class Universe : MonoBehaviour
{
    public static Universe Instance;

    private void Awake() {
        if(Instance == null) {
            Instance = this;
        } else {
            Debug.LogWarning("Universe has multiple instances. One has been removed");
            Destroy(gameObject);
            return;
        }

        PlayerObject = FindObjectOfType<ExamplePlayerController>().gameObject;
        RewiredPlayer = ReInput.players.GetPlayer(0);
        ChoiceController = FindObjectOfType<StoryChoiceController>();
    }

    public Player RewiredPlayer;
    public GameObject PlayerObject;

    public StoryChoiceController ChoiceController;

    public bool isPaused = false;

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
