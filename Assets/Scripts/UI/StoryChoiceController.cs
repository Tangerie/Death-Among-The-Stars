using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;



public class StoryChoiceController : MonoBehaviour
{
    [System.Serializable]
    public class StoryEvent : UnityEvent { };

    [System.Serializable]
    public struct StoryChoice {
        public string Text;
        public StoryEvent OnConfirm;
    }

    public bool isActive {get; private set; }
    public CanvasGroup backGroup;

    public CanvasGroup storyCanvas;
    public float storyFadeDelay = 5f;
    public CanvasGroup choicesCanvas;
    public float choicesFadeDelay = 10f;


    public float fadeTime = 1.0f;

    public GameObject choicePrefab;

    private List<StoryChoice> choiceActions;
    private int selectedChoiceIndex = 0;

    public Color32 selectedChoiceColor = Color.red;

    private void Awake() {
        isActive = false;
    }

    void Start()
    {
        backGroup.alpha = 0;
        choicesCanvas.alpha = 0;
        storyCanvas.alpha = 0;
    }

    private void Update() {
        if(!isActive) return;
        if(choicesCanvas.alpha < 1) return; //Animation not finished

        if(Universe.Instance.RewiredPlayer.GetButtonDown("UIConfirm")) {
            choiceActions[selectedChoiceIndex].OnConfirm.Invoke();
            fadeOut();
        } else if(Universe.Instance.RewiredPlayer.GetAxis("UIHorizontal") < 0) {
            if(selectedChoiceIndex > 0) {
                changeSelectedIndex(selectedChoiceIndex - 1);
            }
        } else if(Universe.Instance.RewiredPlayer.GetAxis("UIHorizontal") > 0) {
            if(selectedChoiceIndex < choicesCanvas.transform.childCount - 1) {
                changeSelectedIndex(selectedChoiceIndex + 1);
            }
        }
    }

    public void ShowStory(string storyText, List<StoryChoice> choices) {
        storyCanvas.GetComponent<TMP_Text>().text = storyText;

        choiceActions = choices;

        foreach(Transform c in choicesCanvas.transform) {
            Destroy(c.gameObject);
            c.SetParent(null); //Cuz destroy doesnt happen until the end of frame
        }

        foreach(var choice in choices) {
            var text = Instantiate(choicePrefab, transform.position, Quaternion.identity, choicesCanvas.transform).GetComponent<TMP_Text>();
            text.text = choice.Text;
            text.gameObject.name = choice.Text;
        }

        changeSelectedIndex(0);
        fadeIn();
    }

    private void changeSelectedIndex(int newIndex) {
        var oldTxt = choicesCanvas.transform.GetChild(selectedChoiceIndex).GetComponent<TMP_Text>();
        oldTxt.color = Color.white;

        selectedChoiceIndex = newIndex;

        var newTxt = choicesCanvas.transform.GetChild(selectedChoiceIndex).GetComponent<TMP_Text>();
        newTxt.color = selectedChoiceColor;
    }

    private void fadeIn() {
        isActive = true;

        LeanTween.alphaCanvas(backGroup, 1, fadeTime).setDelay(0.1f).setIgnoreTimeScale(true).setOnComplete(() => {
            Universe.Instance.PlayerObject.GetComponent<ExamplePlayerController>().canMove = false;
        });
        LeanTween.alphaCanvas(storyCanvas, 1, fadeTime).setDelay(storyFadeDelay).setIgnoreTimeScale(true);
        LeanTween.alphaCanvas(choicesCanvas, 1, fadeTime).setDelay(choicesFadeDelay).setIgnoreTimeScale(true);
    }

    private IEnumerator enablePlayerMovement() {
        yield return new WaitForSeconds(0.5f);

        Universe.Instance.PlayerObject.GetComponent<ExamplePlayerController>().canMove = true;
    }

    private void fadeOut() {
        isActive = false;
        
        LeanTween.alphaCanvas(backGroup, 0, fadeTime).setDelay(0.1f).setIgnoreTimeScale(true);
        LeanTween.alphaCanvas(storyCanvas, 0, fadeTime).setDelay(0.1f).setIgnoreTimeScale(true);
        LeanTween.alphaCanvas(choicesCanvas, 0, fadeTime).setDelay(0.1f).setIgnoreTimeScale(true);

        StartCoroutine(enablePlayerMovement());
    }
}
