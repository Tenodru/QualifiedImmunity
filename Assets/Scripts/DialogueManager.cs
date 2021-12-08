using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager current;
    public DialogueInfo startingDialogue;

    public GameObject dialogueBox;
    public Text speakerName;
    public Image speakerPortrait;
    private bool displayImageBoxActive = false;
    public Image displayImageBox;
    public Text dialogueText;
    [HideInInspector]
    public NPC currNPC;
    [HideInInspector]
    public int currDialogueText;
    public DialogueInfo currDialogueInfo;
    public DialogueButton[] buttons;

    public Sprite diaBoxSal;
    public Sprite diaBoxAglia;
    public Sprite diaBoxPolice;
    public Sprite diaBoxNPC;

    // Start is called before the first frame update
    void Start()
    {
        current = this;
        StartCoroutine(LateStart(0.1f));
    }

    IEnumerator LateStart(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        GameEvents.current.onCloseDialogue += CloseDialogue;

        if (startingDialogue != null)
        {
            StartDialogue(startingDialogue);
        }
    }

    IEnumerator Timer(Action<bool> assigner, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        assigner(true);
    }

    public void StartDialogue(DialogueInfo dialogueInfo, NPC npcName = null)
    {
        GameManager.current.RecordDialogue(dialogueInfo);
        PlayerController.current.playerCanMove = false;
        dialogueBox.SetActive(true);
        currNPC = npcName;
        currDialogueInfo = dialogueInfo;
        currDialogueText = 0;
        foreach (DialogueButton button in buttons)
        {
            button.gameObject.SetActive(false);
        }
        // Apply italics and bold settings to dialogue text if applicable.
        if (dialogueInfo.dialogueText[currDialogueText].italics && dialogueInfo.dialogueText[currDialogueText].bold)
        {
            dialogueText.fontStyle = FontStyle.BoldAndItalic;
        }
        else if (dialogueInfo.dialogueText[currDialogueText].italics)
        {
            dialogueText.fontStyle = FontStyle.Italic;
        }
        else if (dialogueInfo.dialogueText[currDialogueText].bold)
        {
            dialogueText.fontStyle = FontStyle.Bold;
        }
        else
        {
            dialogueText.fontStyle = FontStyle.Normal;
        }

        // Change dialogue box sprite based on this text's NPC...but via the NAME!
        if (true)
        {
            if (dialogueInfo.dialogueText[currDialogueText].name == "Sal")
            {
                dialogueBox.GetComponent<Image>().sprite = diaBoxSal;
            }
            else if (dialogueInfo.dialogueText[currDialogueText].name == "Aglia")
            {
                dialogueBox.GetComponent<Image>().sprite = diaBoxAglia;
            }
            else if (dialogueInfo.dialogueText[currDialogueText].name == "Rickman" || currDialogueInfo.dialogueText[currDialogueText].name == "Chief Rotini" || currDialogueInfo.dialogueText[currDialogueText].name == "Rotini")
            {
                dialogueBox.GetComponent<Image>().sprite = diaBoxPolice;
            }
            else
            {
                dialogueBox.GetComponent<Image>().sprite = diaBoxNPC;
            }
        }

        // Change dialogue box sprite based on this text's NPC.
        if (dialogueInfo.dialogueText[currDialogueText].character != Character.None)
        {
            if (dialogueInfo.dialogueText[currDialogueText].character == Character.Sal)
            {
                dialogueBox.GetComponent<Image>().sprite = diaBoxSal;
            }
            else if (dialogueInfo.dialogueText[currDialogueText].character == Character.Aglia)
            {
                dialogueBox.GetComponent<Image>().sprite = diaBoxAglia;
            }
            else if (dialogueInfo.dialogueText[currDialogueText].character == Character.Police)
            {
                dialogueBox.GetComponent<Image>().sprite = diaBoxPolice;
            }
            else if (dialogueInfo.dialogueText[currDialogueText].character == Character.NPC)
            {
                dialogueBox.GetComponent<Image>().sprite = diaBoxNPC;
            }
        }

        // Play sound if this dialogue text has one associated with it.
        if (dialogueInfo.dialogueText[currDialogueText].sound != null)
        {
            Debug.Log("Played sound.");
            AudioHandler.current.PlaySound(dialogueInfo.dialogueText[currDialogueText].sound);
        }

        // Do teleport stuff.
        if (dialogueInfo.dialogueType == DialogueInfo.DialogueType.Teleport)
        {
            if (dialogueInfo.teleportLoc == TeleportLocation.Office)
            {
                PlayerController.current.transform.position = GameObject.FindGameObjectWithTag("OfficeRoomTele").transform.position;
            }
            else if (dialogueInfo.teleportLoc == TeleportLocation.ChiefRoom)
            {
                PlayerController.current.transform.position = GameObject.FindGameObjectWithTag("ChiefRoomTele").transform.position;
            }
            else if (dialogueInfo.teleportLoc == TeleportLocation.CultistFail)
            {
                PlayerController.current.transform.position = GameObject.FindGameObjectWithTag("CultistFailTele").transform.position;
            }
            else if (dialogueInfo.teleportLoc == TeleportLocation.CultistWin)
            {
                PlayerController.current.transform.position = GameObject.FindGameObjectWithTag("CultistWinTele").transform.position;
            }
        }

        // Make an NPC disappear.
        if (dialogueInfo.dialogueType == DialogueInfo.DialogueType.DisappearNPC)
        {
            GameObject.FindGameObjectWithTag("DisappearNPC").SetActive(false);
        }

        StartCoroutine(TypeSentence(dialogueInfo.dialogueText[currDialogueText].dialogueText));
        speakerName.text = dialogueInfo.dialogueText[currDialogueText].name;
        speakerPortrait.sprite = dialogueInfo.dialogueText[currDialogueText].portrait;
        if (dialogueInfo.dialogueText[currDialogueText].displayImage != null)
        {
            if (!displayImageBoxActive)
            {
                displayImageBox.GetComponent<Animator>().SetTrigger("In");
                displayImageBoxActive = true;
                displayImageBox.transform.GetChild(0).GetComponent<Image>().sprite = currDialogueInfo.dialogueText[currDialogueText].displayImage;
            }
            else
            {
                displayImageBox.GetComponent<Animator>().SetTrigger("Out");
                StopCoroutine("Timer");
                StartCoroutine(Timer(x => displayImageBox.GetComponent<Animator>().SetTrigger("In"), 0.25f));
                StartCoroutine(Timer(x => displayImageBox.transform.GetChild(0).GetComponent<Image>().sprite = currDialogueInfo.dialogueText[currDialogueText].displayImage, 0.25f));
            }
        }
        else
        {
            if (displayImageBoxActive)
            {
                displayImageBox.GetComponent<Animator>().SetTrigger("Out");
            }
            displayImageBoxActive = false;
        }

        //focus camera on speaker
        if (GameObject.Find(dialogueInfo.dialogueText[currDialogueText].name) != null)
        {
            CameraMovement.current.target = GameObject.Find(dialogueInfo.dialogueText[currDialogueText].name).transform;
        }
    }

    public void AdvanceDialogue()
    {
        currDialogueText += 1;
        foreach (DialogueButton button in buttons)
        {
            button.gameObject.SetActive(false);
        }
        // Apply italics and bold settings to dialogue text if applicable.
        if (currDialogueInfo.dialogueText[currDialogueText].italics && currDialogueInfo.dialogueText[currDialogueText].bold)
        {
            dialogueText.fontStyle = FontStyle.BoldAndItalic;
        }
        else if (currDialogueInfo.dialogueText[currDialogueText].italics)
        {
            dialogueText.fontStyle = FontStyle.Italic;
        }
        else if (currDialogueInfo.dialogueText[currDialogueText].bold)
        {
            dialogueText.fontStyle = FontStyle.Bold;
        }
        else
        {
            dialogueText.fontStyle = FontStyle.Normal;
        }

        // Change dialogue box sprite based on this text's NPC...but via the NAME!
        if (true)
        {
            if (currDialogueInfo.dialogueText[currDialogueText].name == "Sal")
            {
                dialogueBox.GetComponent<Image>().sprite = diaBoxSal;
            }
            else if (currDialogueInfo.dialogueText[currDialogueText].name == "Aglia")
            {
                dialogueBox.GetComponent<Image>().sprite = diaBoxAglia;
            }
            else if (currDialogueInfo.dialogueText[currDialogueText].name == "Rickman" || currDialogueInfo.dialogueText[currDialogueText].name == "Chief Rotini" || currDialogueInfo.dialogueText[currDialogueText].name == "Rotini")
            {
                dialogueBox.GetComponent<Image>().sprite = diaBoxPolice;
            }
            else
            {
                dialogueBox.GetComponent<Image>().sprite = diaBoxNPC;
            }
        }

        // Change dialogue box sprite based on this text's NPC.
        if (currDialogueInfo.dialogueText[currDialogueText].character != Character.None)
        {
            if (currDialogueInfo.dialogueText[currDialogueText].character == Character.Sal)
            {
                dialogueBox.GetComponent<Image>().sprite = diaBoxSal;
            }
            else if (currDialogueInfo.dialogueText[currDialogueText].character == Character.Aglia)
            {
                dialogueBox.GetComponent<Image>().sprite = diaBoxAglia;
            }
            else if (currDialogueInfo.dialogueText[currDialogueText].character == Character.Police)
            {
                dialogueBox.GetComponent<Image>().sprite = diaBoxPolice;
            }
            else if (currDialogueInfo.dialogueText[currDialogueText].character == Character.NPC)
            {
                dialogueBox.GetComponent<Image>().sprite = diaBoxNPC;
            }
        }

        // Play sound if this dialogue text has one associated with it.
        if (currDialogueInfo.dialogueText[currDialogueText].sound != null)
        {
            Debug.Log("Played sound.");
            AudioHandler.current.PlaySound(currDialogueInfo.dialogueText[currDialogueText].sound);
        }

        StartCoroutine(TypeSentence(currDialogueInfo.dialogueText[currDialogueText].dialogueText));
        speakerName.text = currDialogueInfo.dialogueText[currDialogueText].name;
        speakerPortrait.sprite = currDialogueInfo.dialogueText[currDialogueText].portrait;
        if (currDialogueInfo.dialogueText[currDialogueText].displayImage != null)
        {
            if (!displayImageBoxActive)
            {
                displayImageBox.GetComponent<Animator>().SetTrigger("In");
                displayImageBoxActive = true;
                displayImageBox.transform.GetChild(0).GetComponent<Image>().sprite = currDialogueInfo.dialogueText[currDialogueText].displayImage;
            }
            else
            {
                if (displayImageBox.transform.GetChild(0).GetComponent<Image>().sprite != currDialogueInfo.dialogueText[currDialogueText].displayImage)
                {
                    displayImageBox.GetComponent<Animator>().SetTrigger("Out");
                    StopCoroutine("Timer");
                    StartCoroutine(Timer(x => displayImageBox.GetComponent<Animator>().SetTrigger("In"), 0.25f));
                    StartCoroutine(Timer(x => displayImageBox.transform.GetChild(0).GetComponent<Image>().sprite = currDialogueInfo.dialogueText[currDialogueText].displayImage, 0.25f));
                }
            }
        }
        else
        {
            if (displayImageBoxActive)
            {
                displayImageBox.GetComponent<Animator>().SetTrigger("Out");
            }
            displayImageBoxActive = false;
        }

        //focus camera on speaker
        if (GameObject.Find(currDialogueInfo.dialogueText[currDialogueText].name) != null)
        {
            CameraMovement.current.target = GameObject.Find(currDialogueInfo.dialogueText[currDialogueText].name).transform;
        }
    }

    IEnumerator TypeSentence(string s = "")
    {
        string nextLine = "";
        dialogueText.text = nextLine;
        foreach (char letter in s.ToCharArray())
        {
            nextLine += letter;
            dialogueText.text = nextLine;
            yield return null;
        }
        yield return .4f;

        //if there is another box of text in this dialogue
        if (currDialogueText < currDialogueInfo.dialogueText.Count - 1)
        {
            //create a next button
            CreateButton("next", "Next", currDialogueInfo);
        }
        else
        {
            if (currDialogueInfo.dTransType == DialogueInfo.DialogueTransitionType.Automatic)
            {
                if (currDialogueInfo.options.Count > 0)
                {
                    foreach (DialogueOptions option in currDialogueInfo.options)
                    {
                        if (CheckRequirements(option))
                        {
                            CreateButton("next", "Next", option.nextDialogue);
                            break;
                        }
                    }
                }
            }
            else if (currDialogueInfo.dTransType == DialogueInfo.DialogueTransitionType.Manual)
            {
                if (currDialogueInfo.options.Count > 0)
                {
                    /*for (int i = 0; i < currDialogueInfo.options.Count; i++)
                    {
                        CreateButton("talk", currDialogueInfo.options[i].nextDialogue.name, currDialogueInfo.options[i].nextDialogue);
                    }*/
                    foreach (DialogueOptions option in currDialogueInfo.options)
                    {
                        /*if (CheckRequirements(option))
                        {
                            CreateButton("talk", option.optionName, option.nextDialogue);
                        }*/
                        if (!GameManager.current.dialogueRecord.Contains(option.nextDialogue))
                        {
                            CreateButton("talk", option.optionName, option.nextDialogue);
                        }
                        else
                        {
                            CreateButton("talkGreyedOut", option.optionName, option.nextDialogue);
                        }
                    }
                }
            }
            else if (currDialogueInfo.dTransType == DialogueInfo.DialogueTransitionType.Exit)
            {
                //create a close button
                CreateButton("close", "Close");
            }

            if (currDialogueInfo.dialogueType == DialogueInfo.DialogueType.Clue)
            {
                MenuManager.current.AddEntry(currDialogueInfo.clue);
            }

            if (currDialogueInfo.dialogueType == DialogueInfo.DialogueType.CultistDetection)
            {
                GameManager.current.cultistDetection += currDialogueInfo.cultistChange;
            }
        }
    }

    private void CloseDialogue()
    {
        CameraMovement.current.target = PlayerController.current.transform;

        if (!MenuManager.current.analysisMode)
        {
            PlayerController.current.playerCanMove = true;
        }
        foreach (DialogueButton button in buttons)
        {
            button.gameObject.SetActive(false);
        }
        dialogueBox.SetActive(false);

        //look for new objective, if there is one, assign it to player
        if (currDialogueInfo.newObjective != null)
        {
            CurrentObjective.current.AssignObjective(currDialogueInfo.newObjective);
        }

        if (currDialogueInfo.dialogueType == DialogueInfo.DialogueType.StartReview)
        {
            MenuManager.current.StartReview();
        }

        if (currDialogueInfo.dialogueType == DialogueInfo.DialogueType.Accuse)
        {
            MenuManager.current.StartAccusation();
        }

        if (currDialogueInfo.dialogueType == DialogueInfo.DialogueType.Analysis)
        {
            if (currDialogueInfo.analysis != null)
            {
                MenuManager.current.StartAnalysis(currDialogueInfo.analysis);
            }
            else
            {
                MenuManager.current.StartAnalysis(MenuManager.current.currentAnalysisInfo);
            }
        }

        if (currDialogueInfo.dialogueType == DialogueInfo.DialogueType.IntermissionEnd)
        {
            MissionManager.current.SetIntermissionCompleteStatus(MissionManager.current.activeMission, true);
            MissionManager.current.intermissionActive = false;
        }

        if (currDialogueInfo.dialogueType == DialogueInfo.DialogueType.CultistDetection)
        {
            ObjectiveRequirement[] objReqs = ObjectiveRequirement.FindObjectsOfType<ObjectiveRequirement>();
            foreach (ObjectiveRequirement objReq in objReqs)
            {
                if (objReq.objectiveType == ObjectiveRequirement.ObjectiveType.cultistTalk)
                {
                    objReq.AddToCultistCount();
                }
            }
        }

        if (currDialogueInfo.endGame == Ending.BadEnd)
        {
            SceneManager.LoadScene("EndingBad");
        }
        else if (currDialogueInfo.endGame == Ending.GoodEnd)
        {
            SceneManager.LoadScene("EndingOk");
        }
        else if (currDialogueInfo.endGame == Ending.TrueEnd)
        {
            SceneManager.LoadScene("EndingGood");
        }

        if (currDialogueInfo.dialogueType == DialogueInfo.DialogueType.Credits)
        {
            
        }

        currDialogueText = 0;

        if (currNPC != null)
        {
            currNPC.Invoke("StopDialogue", 3f);
            currNPC = null;
        }

        if (displayImageBoxActive)
        {
            displayImageBox.GetComponent<Animator>().SetTrigger("Out");
            displayImageBoxActive = false;
        }
    }

    public void CreateButton(string buttonType, string buttonText, DialogueInfo dialogueInfo = null)
    {
        bool buttonFound = false;
        int i = 0;
        while (!buttonFound)
        {
            if (buttons[i].gameObject.activeInHierarchy)
            {
                i += 1;
            }
            else
            {
                buttonFound = true;
            }
        }
        buttons[i].gameObject.SetActive(true);
        buttons[i].SetUpButton(buttonType);
        buttons[i].buttonText.text = buttonText;
        buttons[i].dialogueInfo = dialogueInfo;
    }

    public bool CheckRequirements(DialogueOptions dialogueOption)
    {
        if (dialogueOption.comparisonType == DialogueOptions.ComparisonType.Equal) //EQUAL
        {
            if (dialogueOption.worldStat == DialogueOptions.WorldStats.citizenScore)
            {
                if (GameManager.current.citizenScore == dialogueOption.threshold)
                {
                    return true;
                }
            }
            else if (dialogueOption.worldStat == DialogueOptions.WorldStats.missionSuccesses)
            {
                if (GameManager.current.missionSuccesses == dialogueOption.threshold)
                {
                    return true;
                }
            }
            else if (dialogueOption.worldStat == DialogueOptions.WorldStats.missionFails)
            {
                if (GameManager.current.missionFails == dialogueOption.threshold)
                {
                    return true;
                }
            }
        }
        else if (dialogueOption.comparisonType == DialogueOptions.ComparisonType.LessThan) //LESS THAN
        {
            if (dialogueOption.worldStat == DialogueOptions.WorldStats.citizenScore)
            {
                if (GameManager.current.citizenScore < dialogueOption.threshold)
                {
                    return true;
                }
            }
            else if (dialogueOption.worldStat == DialogueOptions.WorldStats.missionSuccesses)
            {
                if (GameManager.current.missionSuccesses <= dialogueOption.threshold)
                {
                    return true;
                }
            }
            else if (dialogueOption.worldStat == DialogueOptions.WorldStats.missionFails)
            {
                if (GameManager.current.missionFails <= dialogueOption.threshold)
                {
                    return true;
                }
            }
        }
        else if (dialogueOption.comparisonType == DialogueOptions.ComparisonType.GreaterThan) //GREATER THAN
        {
            if (dialogueOption.worldStat == DialogueOptions.WorldStats.citizenScore)
            {
                if (GameManager.current.citizenScore > dialogueOption.threshold)
                {
                    return true;
                }
            }
            else if (dialogueOption.worldStat == DialogueOptions.WorldStats.missionSuccesses)
            {
                if (GameManager.current.missionSuccesses > dialogueOption.threshold)
                {
                    return true;
                }
            }
            else if (dialogueOption.worldStat == DialogueOptions.WorldStats.missionFails)
            {
                if (GameManager.current.missionFails > dialogueOption.threshold)
                {
                    return true;
                }
            }
        }
        else if (dialogueOption.comparisonType == DialogueOptions.ComparisonType.None)
        {
            return true;
        }
        return false;
    }

    public void OnDestroy()
    {
        GameEvents.current.onCloseDialogue -= CloseDialogue;
    }
}
