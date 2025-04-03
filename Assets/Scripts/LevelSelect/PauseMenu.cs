using Initializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utils;
using static Honda;
using static Mercedes;
using static Tesla;
using static IEnum<Fragment>;

public class PauseMenu : PersistentSingleton<PauseMenu> {
    public static bool IsPaused = false;
    public static bool ScreenShake = true;
    [SerializeField] private GameObject pauseMenuPanel;
    [SerializeField] private Canvas pauseCanvas;

    [SerializeField] private GameObject history;
    [SerializeField] private GameObject controls;
    [SerializeField] private GameObject buttonparent;


    [SerializeField] private TextMeshProUGUI historyText;

    [SerializeField] private Slider musicSlider;


    public delegate void PauseMenuEventHandler();

    public static event PauseMenuEventHandler OnPauseMenuActivate;   // for more complex functions that cannot use isPaused
    public static event PauseMenuEventHandler OnPauseMenuDeactivate;


    bool isDialogueHistoryShowing = false;
    bool isControlsShowing = false;

    public bool DialogueHistoryState {
        get { return isDialogueHistoryShowing; }
        set {
            if (value) {
                ShowHistory();
            } else {
                HideHistory();
            }

            isDialogueHistoryShowing = value;
        }
    }

    public bool ControlsState {
        get { return isControlsShowing; }
        set {
            if (value) {
                ShowControls();
            } else {
                HideControls();
            }

            isControlsShowing = value;
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

    void ShowControls() {
        controls.SetActive(true);
        buttonparent.SetActive(false);
    }

    void HideControls() {
        controls.SetActive(false);
        buttonparent.SetActive(true);
    }

    public void SetControlsState(bool state) {
        ControlsState = state;
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
        Time.timeScale = 1f;
        IsPaused = false;
        OnPauseMenuDeactivate?.Invoke();
    }

    public void PauseScene() {
        pauseMenuPanel.SetActive(true);
        Time.timeScale = 0f;
        IsPaused = true;

        if (isDialogueHistoryShowing) {
            RenderHistory();
        }
        OnPauseMenuActivate?.Invoke();
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



public interface IBigCarInfo {
    public int BigCount { get; set; }
}
public interface IMediumCarInfo {
    public int MediumCount { get; set; }
}
public interface ISmallCarInfo {
    public int SmallCount { get; set; }
}

public interface ITimeOfDayInfo {
    public DateTime TimeofDay { get; set; }
}
public interface IAddressInfo {
    public string Address { get; set; }
}


public class ParkingLot : ITeslaInformation, IHondaInformation, IMercedesInformation {
    public int BigCount { get; set; }
    public int MediumCount { get; set; }
    public int SmallCount { get; set; }
    public DateTime TimeofDay { get; set; }
    public string Address { get; set; }
    
    public List<CarPriority<ParkingLot>> carList { get; set; } = new();
    bool AddCar(CarType<ParkingLot>  carType) {
        return carType.HasSpace(this);
    }

    void EvictCar() {
        carList.Sort((a, b) => a.CurrentPriority(this) - b.CurrentPriority(this));
        carList.RemoveAt(carList.Count);
    }


    void Main() {
        Tesla tesla = new();
        Honda honda = new();
        Mercedes mercedes = new();


        AddCar(tesla);
        //AddCar(honda);
        AddCar(mercedes);
    }
}



public interface IEnum<T> where T : IEnum<T> {
    protected static readonly Dictionary<Type, T> valuesMap = new();

    static IEnum() {
        RegisterValues();
    }

    private static void RegisterValues() {
        Type baseType = typeof(T);

        var subTypes = Assembly.GetAssembly(baseType)!
            .GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && baseType.IsAssignableFrom(t));

        foreach (Type subType in subTypes) {
            if (Activator.CreateInstance(subType) is T instance) {
                valuesMap[subType] = instance;
            } else {
                throw new InvalidOperationException($"Could not create instance of {subType}");
            }
        }
    }

    protected static T ParseFromType(Type type) {
        if (valuesMap.TryGetValue(type, out T instance))
            return instance;

        throw new ArgumentException($"No enum value found for {type}");
    }

    protected static IEnumerable<T> Values => valuesMap.Values;
}

public abstract class Buffer { }

public abstract class Fragment : IEnum<Fragment>{


    public static Fragment Get<T>() => ParseFromType(typeof(T));
}

public abstract class RotatingFragment : Fragment, IEnum<RotatingFragment> {

}




public interface CarPriority<in T> {
    public abstract int CurrentPriority(T carInfo);
}


public interface CarType<in T>{
    public abstract bool HasSpace(T carInfo);
}

public class Tesla : CarType<HasSpaceFields>, CarPriority<PriorityFields> {
    public interface PriorityFields : ISmallCarInfo, IMediumCarInfo { }
    public int CurrentPriority(PriorityFields carInfo) {
        return carInfo.SmallCount + carInfo.MediumCount;
    }

    public interface HasSpaceFields : IBigCarInfo, ITimeOfDayInfo { }
    public bool HasSpace(HasSpaceFields carInfo) {
        if (carInfo.BigCount > 0 && carInfo.TimeofDay < DateTime.UtcNow) {
            carInfo.BigCount--;
            return true;
        }
        return false;
    }

    public interface ITeslaInformation: HasSpaceFields, PriorityFields { }

}

public class Honda : CarType<IHondaInformation> {
    public bool HasSpace(IHondaInformation carInfo) {
        if (carInfo.SmallCount > 0 && carInfo.Address != "UBC Agronomy Rd.") {
            carInfo.SmallCount--;
            return true;
        }
        return false;
    }
    public interface IHondaInformation : ISmallCarInfo, IAddressInfo{ }
}

public class Mercedes : CarType<IMercedesInformation> {
    public bool HasSpace(IMercedesInformation carInfo) {
        if (carInfo.SmallCount > 0) {
            carInfo.SmallCount--;
        } else if (carInfo.MediumCount > 0) {
            carInfo.MediumCount--;
        }
        return false;
    }
    public interface IMercedesInformation : IMediumCarInfo, ISmallCarInfo { }
}

