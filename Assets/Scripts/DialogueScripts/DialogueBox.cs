using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

//This class handles Rolling Text behavior, Use this in place of TextMeshProUGUI components to set text
public class DialogueBox : MonoBehaviour
{
    public const string RENDERING_LAYER = "DialogueLayer";
    private const float DEFAULTROLLSPEED = 50f;

    [SerializeField] private TextMeshProUGUI bodyText;
    [SerializeField] private TextMeshProUGUI nameText;

    [SerializeField] private Image displayingImage;
    [SerializeField] private AspectRatioFitter aspectRatioFitter;

    private string currentLine = string.Empty;
    private List<string> highlightedWords = new();

    public float rollingSpeed;

    private bool lineIsFinished;

    public delegate void DialogueBoxDelegate();

    public static event DialogueBoxDelegate DialogueBoxEvent; //New event to help you coordinate cool things during dialogue

    public void SetLine(DialogueText line)
    {
        if (line.DisplayingImage == null)
        {
            displayingImage.enabled = false;
        }
        else
        {
            aspectRatioFitter.aspectRatio = line.DisplayingImage.bounds.size.x / line.DisplayingImage.bounds.size.y;
            displayingImage.enabled = true;
            displayingImage.sprite = line.DisplayingImage;
        }

        SetFontStyles(line);

        bodyText.text = string.Empty;
        this.currentLine = line.BodyText;
        this.highlightedWords = line.HighlightedWords;
        nameText.text = line.SpeakerName;

        if (line.broadcastAnEvent) DialogueBoxEvent?.Invoke();
        StartDialogue();
    }

    private void Awake()
    {
        if (rollingSpeed == 0)
        {
            rollingSpeed = DEFAULTROLLSPEED;
        }
    }

    public static void ClearDialogueEvents()
    {
        DialogueBoxEvent = null;
    }

    void SetFontStyles(DialogueText line)
    {
        if (line.Bold)
        {
            bodyText.fontStyle |= FontStyles.Bold;
        }
        else
        {
            bodyText.fontStyle &= ~FontStyles.Bold;
        }

        if (line.Italics)
        {
            bodyText.fontStyle |= FontStyles.Italic;
        }
        else
        {
            bodyText.fontStyle &= ~FontStyles.Italic;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    public void StopScrollingText() {
        if (bodyText.text != currentLine) {
            StopAllCoroutines();
            bodyText.text = PreProcessText(currentLine);
            lineIsFinished = true;
        }
    }

    private string PreProcessText(string modifiedText) {
        highlightedWords.ForEach(word => modifiedText = modifiedText.Replace(word, $"<color=#ADD8E6>{word}</color>"));
        return modifiedText;
    }

    void StartDialogue()
    {
        StartCoroutine(TypeLine());
    }

    
    private string HighlightCharacter(string str) => $"<color=#99cfe0>{str}</color>";
  

    IEnumerator TypeLine() {
        lineIsFinished = false;
        int currentIndex = 0;
        string displayedText = "";
        int highlightedWordLength = 0;

        bool IsStartOfHighlightedWord(int currentIndex, out int wordLength) {
            foreach (string word in highlightedWords) {
                if (currentLine.Substring(currentIndex).StartsWith(word)) {
                    wordLength = word.Length;
                    return true;
                }
            }
            wordLength = 0;
            return false;
        }

        while (currentIndex < currentLine.Length) {
            if (highlightedWordLength == 0 && IsStartOfHighlightedWord(currentIndex, out int wordLength)) {
                highlightedWordLength = wordLength;
            }

            if (highlightedWordLength > 0) {
                displayedText += HighlightCharacter(currentLine[currentIndex].ToString());
                highlightedWordLength--;
            } else {
                displayedText += currentLine[currentIndex];
            }

            currentIndex++;
            bodyText.text = displayedText;

            float delay = 1f / rollingSpeed;
            float timer = 0f;
            while (timer < delay) {
                timer += Time.unscaledDeltaTime;
                yield return null;
            }
        }

        lineIsFinished = true;
    }




    public bool FinishedLine()
    {
        return lineIsFinished;
    }
}
