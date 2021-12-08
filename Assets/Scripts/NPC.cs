using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class NPC : MonoBehaviour
{
    private UIHoverListener hoverCheck;
    private DialogueManager dm;

    [Header("Dialogue")]
    public DialogueInfo idle;
    public List<ObjectiveInfo> objectives;
    public DialogueInfo[] special;


    [Header("Patrol")]
    public NPCState npcState = NPCState.Waiting;
    public List<Transform> patrolPoints;

    [Header("Talk")]
    public int chance = 100;
    public DialogueInfo[] general;

    // Start is called before the first frame update
    void Start()
    {
        hoverCheck = UIHoverListener.FindObjectOfType<UIHoverListener>();
        dm = DialogueManager.FindObjectOfType<DialogueManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (npcState == NPCState.Patrolling)
        {
            Patrol();
        }
    }

    private void OnMouseOver()
    {
        if (!hoverCheck.isUIOverride && idle != null)
        {
            MouseNotif.current.Show("LC", "Talk");
        }
    }

    private void OnMouseExit()
    {
        MouseNotif.current.Hide();
    }

    private void OnMouseUp()
    {
        if (!hoverCheck.isUIOverride && dm.currNPC != gameObject && !MenuManager.current.analysisMode)
        {
            if (Vector2.Distance(gameObject.transform.position, PlayerController.current.transform.position) < 2f)
            {
                Debug.Log("Distance from " + this.name + " and player is " + Vector2.Distance(gameObject.transform.position, PlayerController.current.transform.position));
                if (!DialogueManager.current.dialogueBox.activeInHierarchy)
                {
                    StartDialogue();
                }
            }
            else
            {
                AlertText.current.ShowAlert("Not close enough to target.", Color.red);
            }
        }
    }

    public void Patrol()
    {
        if (patrolPoints.Count > 0)
        {
            if (GetComponent<AIDestinationSetter>().target == null)
            {
                SetNextTarget();
                Debug.Log("target is null, setting new target manually");
            }
            if (Vector2.Distance(transform.position, GetComponent<AIDestinationSetter>().target.position) < 5f)
            {
                npcState = NPCState.Waiting;
                Invoke("SetNextTarget", 3f);
                Debug.Log("i have reached destination, heading to new target in 3 seconds");
            }
        }
    }

    public void SetNextTarget()
    {
        List<Transform> validPoints = new List<Transform>();
        foreach (Transform point in patrolPoints)
        {
            if (Vector2.Distance(transform.position, point.position) > 2f)
            {
                validPoints.Add(point);
            }
        }
        GetComponent<AIDestinationSetter>().target = validPoints[UnityEngine.Random.Range(0, validPoints.Count)];
        Debug.Log("Next target: " + GetComponent<AIDestinationSetter>().target);
        npcState = NPCState.Patrolling;
    }

    public void StartDialogue(DialogueInfo dialogueInfo = null)
    {
        if (dialogueInfo != null)
        {
            dm.StartDialogue(dialogueInfo, gameObject.GetComponent<NPC>());
        }
        else
        {
            if (objectives.Contains(CurrentObjective.current.currentObjective))
            {
                int idx = objectives.IndexOf(CurrentObjective.current.currentObjective);
                dm.StartDialogue(special[idx], gameObject.GetComponent<NPC>());
            }
            else
            {
                if (idle != null)
                {
                    dm.StartDialogue(idle, gameObject.GetComponent<NPC>());
                }
            }
        }
        npcState = NPCState.Talking;
        GetComponent<AIPath>().canMove = false;
    }

    public void StopDialogue()
    {
        GetComponent<AIPath>().canMove = true;
        npcState = NPCState.Patrolling;
    }

    public void Talk()
    {
        if (general.Length > 0)
        {
            int rand = Random.Range(0, 100);
            if (rand <= chance)
            {
                rand = Random.Range(0, general.Length);
                GetComponentInChildren<InGameTextBox>().ShowTextBox(general[rand]);
                chance -= 100;
            }
            else
            {
                chance += 10;
            }
        }
    }
}
