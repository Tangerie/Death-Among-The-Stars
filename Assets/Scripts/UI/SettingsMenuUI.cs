using System.Linq.Expressions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SettingsMenuUI : MonoBehaviour
{   
    [System.Serializable]
    public struct NameText {
        public string Name;
        public TextMeshProUGUI Text;
    }

    public NameText[] uiTextEls;

    private Dictionary<string, TextMeshProUGUI> textElements;

    Canvas canvas;

    private void Awake() {
        textElements = new Dictionary<string, TextMeshProUGUI>();
        foreach(NameText n in uiTextEls) {
            textElements.Add(n.Name, n.Text);
        }

        canvas = GetComponent<Canvas>();
    }

    public bool isUIActive = false;

    // Start is called before the first frame update
    void Start()
    {
        canvas.enabled = isUIActive;
        foreach(string key in textElements.Keys) {
            Debug.Log(key);
        }
        textElements["Quality"].text = "Current Quality: " + QualitySettings.names[QualitySettings.GetQualityLevel()];
    }

    // Update is called once per frame
    void Update()
    {   
        if(Universe.Instance.RewiredPlayer.GetButtonDown("Settings")) {
            Universe.Instance.TogglePauseGame();
            isUIActive = !isUIActive;
            canvas.enabled = isUIActive;
        }
        
        if(isUIActive) {
            if(Universe.Instance.RewiredPlayer.GetButtonDown("IncreaseSetting")) {
                QualitySettings.IncreaseLevel(true);
                textElements["Quality"].text = "Current Quality: " + QualitySettings.names[QualitySettings.GetQualityLevel()];
            } else if (Universe.Instance.RewiredPlayer.GetButtonDown("DecreaseSetting")) {
                QualitySettings.DecreaseLevel(true);
                textElements["Quality"].text = "Current Quality: " + QualitySettings.names[QualitySettings.GetQualityLevel()];
            }
        }
    }
}
