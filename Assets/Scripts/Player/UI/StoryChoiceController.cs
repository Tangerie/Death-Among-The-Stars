using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryChoiceController : MonoBehaviour
{
    public bool isActive {get; private set; }
    public CanvasGroup backGroup;

    public CanvasGroup storyCanvas;
    public float storyFadeDelay = 5f;
    public CanvasGroup choicesCanvas;
    public float choicesFadeDelay = 10f;


    public float fadeTime = 1.0f;

    private void Awake() {
        isActive = false;
    }

    void Start()
    {
        backGroup.alpha = 0;
        choicesCanvas.alpha = 0;
        storyCanvas.alpha = 0;
    }

    [ContextMenu("FadeIn")]
    public void FadeIn() {
        isActive = true;
        Universe.Instance.SetPause(true);

        LeanTween.alphaCanvas(backGroup, 1, fadeTime).setDelay(0.1f).setIgnoreTimeScale(true);
        LeanTween.alphaCanvas(storyCanvas, 1, fadeTime).setDelay(storyFadeDelay).setIgnoreTimeScale(true);
        LeanTween.alphaCanvas(choicesCanvas, 1, fadeTime).setDelay(choicesFadeDelay).setIgnoreTimeScale(true);
    }

    [ContextMenu("FadeOut")]
    public void FadeOut() {
        isActive = false;
        Universe.Instance.SetPause(false);

        LeanTween.alphaCanvas(backGroup, 0, fadeTime).setDelay(0.1f).setIgnoreTimeScale(true);
        LeanTween.alphaCanvas(storyCanvas, 0, fadeTime).setDelay(0.1f).setIgnoreTimeScale(true);
        LeanTween.alphaCanvas(choicesCanvas, 0, fadeTime).setDelay(0.1f).setIgnoreTimeScale(true);
    }
}
