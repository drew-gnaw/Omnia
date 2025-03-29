using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class DialogueText
{
    [SerializeField, TextArea] private string bodyText;
    [SerializeField] private string speakerName;
    [SerializeField] private Sprite displayingImage;
    [SerializeField] private List<string> highlightedWords;
    [SerializeField, Range(0f,1f)] private float shadow = 0f;

    private bool italics;
    private bool bold;
    public bool broadcastAnEvent = false;


    public bool Italics { get { return italics; } set { italics = value; } }
    public bool Bold { get { return bold; } set { bold = value; } }


    public string BodyText { get { return bodyText; } set { bodyText = value; } }
    public string SpeakerName { get { return speakerName; } set { speakerName = value; } }
    public Sprite DisplayingImage { get { return displayingImage; } set { displayingImage = value; } }
    public List<string> HighlightedWords { get => new(highlightedWords); }
    public float Shadow { get { return shadow; } set { shadow = value; } }

    DialogueText(string bodyText, string speakerName, List<string> highlightedWords, Sprite givenImage)
    {
        this.bodyText = bodyText;
        this.speakerName = speakerName;
        this.displayingImage = givenImage;
        this.highlightedWords = highlightedWords;
        this.shadow = shadow;
    }
}
