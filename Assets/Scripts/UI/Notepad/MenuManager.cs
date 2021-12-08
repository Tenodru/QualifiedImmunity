using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public static MenuManager current;

    public bool analysisMode;
    public GameObject analysisEffect;
    private bool animating = false;
    public Slider influenceBar;
    public GameObject reviewButtons;
    private GameObject currentReviewMenu;
    public GameObject actionButtons;
    public GameObject startButton;
    public AnalysisInfo currentAnalysisInfo;
    private EntryInfo selectedEntry;
    public Animator breakInAnim;
    public DialogueInfo repeatDialogue;
    public DialogueInfo holdUpDialogue;
    public DialogueInfo failDialogue;
    public DialogueInfo gameOverDialogue;
    public DialogueInfo giveUpDialogue;

    [Header("Suspects Menu")]
    public GameObject suspectsMenu;
    public ObjectiveInfo branchingPath;
    public List<GameObject> suspectsToEnable;
    public List<GameObject> suspectsToDisable;

    [Header("Summary")]
    public GameObject summaryMenu;

    [Header("Crime Scene")]
    public GameObject crimeSceneMenu;

    [Header("Notepad")]
    public bool notepadEnabled = true;
    private int currentClues;
    public int totalMissionClues;
    public GameObject notepadMenu;
    public List<EntryInfo> recordedEntries;
    public GameObject entryList;
    public GameObject entryPrefab;
    public Text displayedName;
    public Text displayedDesc;
    public Button confirmButton;
    public Button notepadBackButton;

    [Header("RecapMenu")]
    public GameObject areYouSureConclusion;
    public bool conclusive = false;
    public GameObject recapMenu;

    // Start is called before the first frame update
    void Start()
    {
        current = this;
        currentClues = 0;
        analysisMode = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (notepadEnabled && !DialogueManager.current.dialogueBox.activeInHierarchy)
            {
                ToggleNotepadMenu();
            }
        }
    }

    public void PreventAnimation(float delay)
    {
        animating = true;
        StartCoroutine(Timer(x => animating = false, delay));
    }

    public IEnumerator Timer(Action<bool> assigner, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        assigner(true);
    }

    public void AddEntry(EntryInfo entry)
    {
        if (!recordedEntries.Contains(entry))
        {
            // If entry should be saved, add it to GameManager's list of saved entries.
            if (entry.persist)
            {
                GameManager.current.savedEntries.Add(entry);
            }
            NotepadNotif.current.TriggerNotif(entry);
            GameObject newEntry = Instantiate(entryPrefab, entryList.transform.position, entryList.transform.rotation) as GameObject;
            newEntry.GetComponent<NotepadEntry>().entry = entry;
            newEntry.GetComponent<Image>().sprite = entry.entrySprite;
            newEntry.transform.SetParent(entryList.transform);
            newEntry.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
            newEntry.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            recordedEntries.Add(entry);
            GameEvents.current.CollectClue(entry);
            currentClues += 1;
        }
    }

    public void SelectEntry(EntryInfo entry)
    {
        displayedName.text = entry.entryName;
        displayedDesc.text = entry.entryDescription;
        selectedEntry = entry;
    }

    public void ToggleNotepadMenu()
    {
        if (animating)
        {
            return;
        }
        if (!analysisMode)
        {
            if (currentReviewMenu == null)
            {
                notepadMenu.GetComponent<Animator>().SetTrigger("In");
                currentReviewMenu = notepadMenu;
            }
            else if (currentReviewMenu == notepadMenu)
            {
                notepadMenu.GetComponent<Animator>().SetTrigger("Out");
                currentReviewMenu = null;
            }
            PreventAnimation(0.5f);
        }
    }

    public void StartReview()
    {
        TransitionPanel.current.Animate("BadgeIn");
        StartCoroutine(Timer(x => Review(), 0.6f));
        StartCoroutine(Timer(x => TransitionPanel.current.Animate("BadgeOut"), 0.6f));
    }

    //Part 2 after Start Review
    public void Review()
    {
        analysisMode = true;
        notepadMenu.transform.GetChild(1).gameObject.SetActive(false); //set esc/notepad button inactive
        PlayerController.current.playerCanMove = false;
        analysisEffect.SetActive(true);
        influenceBar.gameObject.SetActive(true);
        reviewButtons.SetActive(true);
        ReviewButton("summary");
        startButton.SetActive(true);
    }

    public void StartCloseReview(DialogueInfo dialogueInfo)
    {
        if (animating)
        {
            return;
        }
        TransitionPanel.current.Animate("BadgeIn");
        StartCoroutine(Timer(x => CloseReview(dialogueInfo), 0.6f));
        StartCoroutine(Timer(x => TransitionPanel.current.Animate("BadgeOut"), 0.6f));
        StartCoroutine(Timer(x => DialogueManager.current.StartDialogue(dialogueInfo), 1.6f));
        PreventAnimation(0.6f);
    }

    public void CloseReview(DialogueInfo dialogueInfo)
    {
        reviewButtons.SetActive(false);
        summaryMenu.SetActive(false);
        crimeSceneMenu.SetActive(false);
        if (currentReviewMenu == notepadMenu)
        {
            notepadMenu.GetComponent<Animator>().SetTrigger("Out");
            currentReviewMenu = null;
        }
        startButton.SetActive(false);
        repeatDialogue = dialogueInfo;
    }

    public void StartAnalysis(AnalysisInfo analysisInfo)
    {
        currentAnalysisInfo = analysisInfo;
        actionButtons.SetActive(true);
        actionButtons.GetComponent<Animator>().SetTrigger("In");
        analysisEffect.transform.GetChild(1).GetComponent<ParticleSystem>().Play();
    }

    public void StartChoosingClues()
    {
        if (animating)
        {
            return;
        }
        notepadMenu.GetComponent<Animator>().SetTrigger("In");
        actionButtons.GetComponent<Animator>().SetTrigger("Out");
        confirmButton.gameObject.SetActive(true);
        notepadBackButton.gameObject.SetActive(true);
        PreventAnimation(0.5f);
    }

    public void StopChoosingClues()
    {
        if (animating)
        {
            return;
        }
        notepadMenu.GetComponent<Animator>().SetTrigger("Out");
        actionButtons.GetComponent<Animator>().SetTrigger("In");
        PreventAnimation(0.5f);
    }

    public void StartAccusation()
    {
        if (animating)
        {
            return;
        }
        suspectsMenu.GetComponent<Animator>().SetTrigger("In");
        if (branchingPath != null)
        {
            if (GameManager.current.objectiveRecord.Contains(branchingPath))
            {
                foreach (GameObject suspect in suspectsToEnable)
                {
                    suspect.SetActive(true);
                }
                foreach (GameObject suspect in suspectsToDisable)
                {
                    suspect.SetActive(false);
                }
            }
        }
        analysisEffect.transform.GetChild(1).GetComponent<ParticleSystem>().Play();
        PreventAnimation(0.5f);
    }

    public void SetAccusationImage(Sprite sprite)
    {
        breakInAnim.transform.GetChild(0).transform.GetChild(2).GetComponent<Image>().sprite = sprite;
    }

    public void Accuse(DialogueInfo dialogueInfo)
    {
        if (animating)
        {
            return;
        }
        suspectsMenu.GetComponent<Animator>().SetTrigger("Out");
        StartCoroutine(Timer(x => AudioHandler.current.PlaySound(AudioHandler.current.exclamation), 0.5f));
        StartCoroutine(Timer(x => breakInAnim.SetTrigger("AccuseIn"), 0.5f));
        StartCoroutine(Timer(x => Continue(dialogueInfo), 2f));
        PreventAnimation(2f);
    }

    public void RepeatDialogue()
    {
        if (animating)
        {
            return;
        }
        if (repeatDialogue != null)
        {
            Continue(repeatDialogue);
            actionButtons.GetComponent<Animator>().SetTrigger("Out");
            TransitionPanel.current.Animate("Repeat");
            PreventAnimation(0.5f);
        }
    }

    public void RestartAccuse(DialogueInfo dialogueInfo)
    {
        actionButtons.GetComponent<Animator>().SetTrigger("Out");
        DialogueManager.current.StartDialogue(dialogueInfo);
    }

    public void Continue(DialogueInfo dialogueInfo)
    {
        reviewButtons.SetActive(false);
        summaryMenu.SetActive(false);
        crimeSceneMenu.SetActive(false);
        startButton.SetActive(false);
        confirmButton.gameObject.SetActive(false);
        DialogueManager.current.StartDialogue(dialogueInfo);
        repeatDialogue = dialogueInfo;
    }

    public void UseClue()
    {
        NotepadEntry[] entries = NotepadEntry.FindObjectsOfType<NotepadEntry>();
        foreach (NotepadEntry entry in entries)
        {
            if (entry.GetComponent<Outline>().enabled)
            {
                selectedEntry = entry.entry;
                entry.GetComponent<Outline>().enabled = false;
                break;
            }
        }
        foreach (AnalysisOptions choice in currentAnalysisInfo.options)
        {
            if (choice.clue == selectedEntry)
            {
                breakInAnim.transform.GetChild(1).GetComponent<Image>().sprite = selectedEntry.entrySprite;
                selectedEntry = null;
                notepadMenu.GetComponent<Animator>().SetTrigger("Out");
                StartCoroutine(Timer(x => AudioHandler.current.PlaySound(AudioHandler.current.exclamation), 0.5f));
                StartCoroutine(Timer(x => breakInAnim.SetTrigger("BreakIn"), 0.5f));
                StartCoroutine(Timer(x => displayedName.text = "Select an Entry", 0.5f));
                StartCoroutine(Timer(x => displayedDesc.text = "-", 0.5f));
                StartCoroutine(Timer(x => Continue(choice.nextDialogue), 2f));
                return;
            }
        }

        //continue but without saving over what the original question was
        reviewButtons.SetActive(false);
        summaryMenu.SetActive(false);
        crimeSceneMenu.SetActive(false);
        notepadMenu.GetComponent<Animator>().SetTrigger("Out");
        startButton.SetActive(false);
        actionButtons.SetActive(false);
        confirmButton.gameObject.SetActive(false);
        influenceBar.value -= 1;
        influenceBar.GetComponent<Animator>().SetTrigger("Hurt");
        if (influenceBar.value <= 0)
        {
            DialogueManager.current.StartDialogue(gameOverDialogue);
        }
        else
        {
            DialogueManager.current.StartDialogue(failDialogue);
        }
    }

    public void GiveUp()
    {
        if (animating)
        {
            return;
        }
        if (currentAnalysisInfo.specialGiveUp == null)
        {
            Continue(giveUpDialogue);
        }
        else
        {
            Continue(currentAnalysisInfo.specialGiveUp);
        }
        actionButtons.GetComponent<Animator>().SetTrigger("Out");
        PreventAnimation(0.5f);
    }

    public void ReviewButton(string buttonType)
    {
        if (animating)
        {
            return;
        }
        if (buttonType == "summary")
        {
            if (currentReviewMenu == summaryMenu)
            {
                return;
            }
            else if (currentReviewMenu == crimeSceneMenu)
            {
                crimeSceneMenu.GetComponent<Animator>().SetTrigger("Out");
            }
            else if (currentReviewMenu == notepadMenu)
            {
                notepadMenu.GetComponent<Animator>().SetTrigger("Out");
            }
            summaryMenu.GetComponent<Animator>().SetTrigger("In");
            currentReviewMenu = summaryMenu;
        }
        else if (buttonType == "crime scene")
        {
            if (currentReviewMenu == summaryMenu)
            {
                summaryMenu.GetComponent<Animator>().SetTrigger("Out");
            }
            else if (currentReviewMenu == crimeSceneMenu)
            {
                return;
            }
            else if (currentReviewMenu == notepadMenu)
            {
                notepadMenu.GetComponent<Animator>().SetTrigger("Out");
            }
            crimeSceneMenu.GetComponent<Animator>().SetTrigger("In");
            currentReviewMenu = crimeSceneMenu;
        }
        else if (buttonType == "notepad")
        {
            if (currentReviewMenu == summaryMenu)
            {
                summaryMenu.GetComponent<Animator>().SetTrigger("Out");
            }
            else if (currentReviewMenu == crimeSceneMenu)
            {
                crimeSceneMenu.GetComponent<Animator>().SetTrigger("Out");
            }
            else if (currentReviewMenu == notepadMenu)
            {
                return;
            }
            notepadMenu.GetComponent<Animator>().SetTrigger("In");
            currentReviewMenu = notepadMenu;
        }
        PreventAnimation(0.5f);
    }

    public void AreYouSureConclusion(string type = "up")
    {
        if (type == "up")
        {
            areYouSureConclusion.GetComponent<Animator>().SetTrigger("In");
        }
        else if (type == "yes")
        {
            TransitionPanel.current.Animate("FadeIn");
            areYouSureConclusion.GetComponent<Animator>().SetTrigger("Out");
            StartCoroutine(Timer(x => OpenRecapMenu(), 0.5f));
        }
        else if (type == "no")
        {
            areYouSureConclusion.GetComponent<Animator>().SetTrigger("Out");
            breakInAnim.SetTrigger("RejectIn");
            StartCoroutine(Timer(x => Continue(holdUpDialogue), 1.5f));
        }
        else if (type == "end")
        {
            TransitionPanel.current.Animate("FadeIn");
            StartCoroutine(Timer(x => OpenRecapMenu(), 0.5f));
        }
    }

    public void OpenRecapMenu()
    {
        recapMenu.SetActive(true);
        recapMenu.transform.GetChild(2).GetComponent<Text>().text = currentClues + "/" + totalMissionClues;
        if (conclusive)
        {
            recapMenu.transform.GetChild(4).GetComponent<Text>().color = Color.green;
            recapMenu.transform.GetChild(4).GetComponent<Text>().text = "Conclusive";
            MissionManager.current.SetMissionCompleteStatus(MissionManager.current.activeMission, true);
        }
        else
        {
            recapMenu.transform.GetChild(4).GetComponent<Text>().color = Color.red;
            recapMenu.transform.GetChild(4).GetComponent<Text>().text = "Inconclusive";
        }
        TransitionPanel.current.Animate("FadeOut");
    }

    public void FinishMissionButton()
    {
        MissionManager.current.SetMissionPlayStatus(MissionManager.current.activeMission, true);
        
        SceneLoader.current.LoadScene("PoliceStation");
    }
}
