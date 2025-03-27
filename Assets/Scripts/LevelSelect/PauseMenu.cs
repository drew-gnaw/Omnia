using System.Collections;
using System.Collections.Generic;
using Initializers;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utils;

public class PauseMenu : PersistentSingleton<PauseMenu> {
    public static bool IsPaused = false;
    public static bool ScreenShake = true;
    [SerializeField] private GameObject pauseMenuPanel;
    [SerializeField] private Canvas pauseCanvas;

    [SerializeField] private GameObject history;
    [SerializeField] private GameObject buttonparent;


    [SerializeField] private TextMeshProUGUI historyText;

    [SerializeField] private Slider musicSlider;


    public delegate void PauseMenuEventHandler();

    public static event PauseMenuEventHandler OnPauseMenuActivate;   // for more complex functions that cannot use isPaused
    public static event PauseMenuEventHandler OnPauseMenuDeactivate;


    bool isDialogueHistoryShowing = false;

    public bool DialogueHistoryState {
        get { return isDialogueHistoryShowing; }
        set {
            if (value == true) {
                ShowHistory();
            } else {
                HideHistory();
            }

            isDialogueHistoryShowing = value;
        }
    }

    protected override void OnAwake() {
        return;
    }

    void ShowHistory() {
        RenderHistory();
        history.SetActive(true);
        buttonparent.SetActive(false);
    }

    void HideHistory() {
        history.SetActive(false);
        buttonparent.SetActive(true);
    }

    void RenderHistory() {
        List<DialogueText> list = DialogueManager.Instance?.GetHistory();
        historyText.text = "";
        foreach (DialogueText dialogue in list) {
            historyText.text += "<b>" + dialogue.SpeakerName + "</b>" + "\n" + dialogue.BodyText + "\n\n";
        }
    }

    public void SetHistoryState(bool state) {
        DialogueHistoryState = state;
    }


    private void Start() {
        musicSlider.value = 0.7f;
        MusicVolume();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            TogglePause();
        }
    }

    void TogglePause() {
        if (IsPaused)
        {
            ResumeScene();
        } else
        {
            PauseScene();
        }
    }


    public void ResumeScene()
    {
        pauseMenuPanel.SetActive(false);
        OnPauseMenuDeactivate?.Invoke();
        Time.timeScale = 1f;
        IsPaused = false;
    }

    public void PauseScene() {
        pauseMenuPanel.SetActive(true);
        OnPauseMenuActivate?.Invoke();
        Time.timeScale = 0f;
        IsPaused = true;

        if (isDialogueHistoryShowing) {
            RenderHistory();
        }
    }

    public void LevelSelectScene() {
        ResumeScene();
        SceneInitializer.LoadScene("LevelSelect");
    }

    public void MainMenuScene() {
        ResumeScene();
        SceneInitializer.LoadScene("1_Title");
    }

    public void ResetScene() {
        ResumeScene();
        SceneInitializer.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void SkipScene() {
        ResumeScene();
        LevelManager.Instance.NextLevel();
    }

    public void ToggleMusic() {
        AudioManager.Instance.ToggleAudio();
    }

    public void MusicVolume() {
        AudioManager.Instance.SetBGMVolume(musicSlider.value);
        AudioManager.Instance.SetSFXVolume(musicSlider.value);
        AudioManager.Instance.SetAmbientVolume(musicSlider.value);
    }

    public void ToggleScreenShake() {
        PauseMenu.ScreenShake = !PauseMenu.ScreenShake;
    }
}
