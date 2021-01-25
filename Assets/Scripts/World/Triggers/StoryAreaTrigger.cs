using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class StoryAreaTrigger : AreaTrigger
{

    public string storyText = "Placeholder Story";
    public List<StoryChoiceController.StoryChoice> choices;

    private void Awake() {
        EntryActions = (other) => {
            if(other.gameObject == Universe.Instance.PlayerObject) {
                Universe.Instance.ChoiceController.ShowStory(storyText, choices);
            }
        };
    }
}
