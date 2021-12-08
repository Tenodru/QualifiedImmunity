using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueButton : MonoBehaviour
{
    public string buttonType;
    public Text buttonText;
    public Image buttonImage;
    public DialogueInfo dialogueInfo;
    private DialogueManager dm;

    [Header("Icons")]
    public Sprite next;
    public Sprite close;
    public Sprite quest;
    public Sprite grayQuest;
    public Sprite talk;

    // Start is called before the first frame update
    void Awake()
    {
        dm = DialogueManager.FindObjectOfType<DialogueManager>();
    }

    public void ButtonClick()
    {
        //if there is not another box of text in this dialogue
        if (dm.currDialogueText == dm.currDialogueInfo.dialogueText.Count - 1)
        {
            if (dm.currDialogueInfo.pTransType == DialogueInfo.PanelTransitionType.FadeIn)
            {
                TransitionPanel.current.Animate("FadeIn");
            }
            else if (dm.currDialogueInfo.pTransType == DialogueInfo.PanelTransitionType.FadeOut)
            {
                TransitionPanel.current.Animate("FadeOut");
            }
            else if (dm.currDialogueInfo.pTransType == DialogueInfo.PanelTransitionType.FadeInAndOut)
            {
                TransitionPanel.current.Animate("FadeInAndOut"); //Alex made this
            }

            if (dm.currDialogueInfo.endMission)
            {
                if (dm.currDialogueInfo.conclusive)
                {
                    MenuManager.current.conclusive = true;
                }
                else
                {
                    MenuManager.current.influenceBar.value -= 1;
                    MenuManager.current.influenceBar.GetComponent<Animator>().SetTrigger("Hurt");
                    MenuManager.current.conclusive = false;
                }

                if (MenuManager.current.influenceBar.value > 0)
                {
                    MenuManager.current.AreYouSureConclusion();
                }
                else
                {
                    MenuManager.current.AreYouSureConclusion("end");
                }
            }
        }

        if (dialogueInfo == dm.currDialogueInfo)
        {
            if (buttonType == "next")
            {
                dm.AdvanceDialogue();
            }
        }
        else
        {
            if (dialogueInfo == null)
            {
                GameEvents.current.CloseDialogue();
            }
            else
            {
                if (dm.currDialogueInfo.newObjective != null)
                {
                    CurrentObjective.current.AssignObjective(dm.currDialogueInfo.newObjective);
                }
                dm.StartDialogue(dialogueInfo, dm.currNPC);
            }
        }
    }

    public void SetUpButton(string style)
    {
        if (style == "next")
        {
            buttonType = "next";
            GetComponent<Image>().color = new Color(0.6059651f, 1, 0.4666667f);
            buttonImage.sprite = next;
        }
        else if (style == "close")
        {
            buttonType = "close";
            GetComponent<Image>().color = new Color(1, 0.3297141f, 0.3160377f);
            buttonImage.sprite = close;
        }
        else if (style == "quest")
        {
            buttonType = "quest";
            GetComponent<Image>().color = new Color(1, 0.5932578f, 0.1462264f);
            buttonImage.sprite = quest;
        }
        else if (style == "inProgressQuest")
        {
            buttonType = "inProgressQuest";
            GetComponent<Image>().color = new Color(0.6792453f, 0.6792453f, 0.6792453f);
            buttonImage.sprite = grayQuest;
        }
        else if (style == "turnInQuest")
        {
            buttonType = "turnInQuest";
            GetComponent<Image>().color = Color.green;
            buttonImage.sprite = quest;
        }
        else if (style == "talk")
        {
            buttonType = "talk";
            GetComponent<Image>().color = new Color(1, 0.9635197f, 0.4669811f);
            buttonImage.sprite = talk;
        }
        else if (style == "talkGreyedOut")
        {
            buttonType = "talkGreyedOut";
            GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f);
            buttonImage.sprite = talk;
        }
        else if (style == "accept")
        {
            buttonType = "accept";
            GetComponent<Image>().color = new Color(0.6059651f, 1, 0.4666667f);
            buttonImage.sprite = next;
        }
        else if (style == "decline")
        {
            buttonType = "decline";
            GetComponent<Image>().color = new Color(1, 0.3297141f, 0.3160377f);
            buttonImage.sprite = close;
        }
    }
}
