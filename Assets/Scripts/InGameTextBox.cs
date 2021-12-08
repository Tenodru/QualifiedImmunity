using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameTextBox : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public Image textBox;
    public Text textBoxText;
    private bool fadeOut, fadeIn;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 dist = new Vector3(textBox.transform.position.x - transform.parent.position.x, textBox.transform.position.y - transform.parent.position.y, 0);
        Vector3[] positions = new Vector3[] { textBox.transform.position, textBox.transform.position - (dist * 0.75f) };
        lineRenderer.SetPositions(positions);

        if (fadeOut)
        {
            Color objectColor = textBox.color;
            float newAlpha = objectColor.a - (5 * Time.deltaTime);

            textBox.color = new Color(textBox.color.r, textBox.color.g, textBox.color.b, newAlpha);
            textBoxText.color = new Color(textBoxText.color.r, textBoxText.color.g, textBoxText.color.b, newAlpha);
            lineRenderer.material.color = new Color(lineRenderer.material.color.r, lineRenderer.material.color.g, lineRenderer.material.color.b, newAlpha);

            if (objectColor.a <= 0)
            {
                fadeOut = false;
            }
        }

        if (fadeIn)
        {
            Color objectColor = textBox.color;
            float newAlpha = objectColor.a + (5 * Time.deltaTime);

            textBox.color = new Color(textBox.color.r, textBox.color.g, textBox.color.b, newAlpha);
            textBoxText.color = new Color(textBoxText.color.r, textBoxText.color.g, textBoxText.color.b, newAlpha);
            lineRenderer.material.color = new Color(lineRenderer.material.color.r, lineRenderer.material.color.g, lineRenderer.material.color.b, newAlpha);

            if (objectColor.a >= 1)
            {
                fadeIn = false;
                Invoke("FadeOutObject", 3f);
            }
        }
    }

    public void ShowTextBox(DialogueInfo dialogueInfo)
    {
        if (PlayerController.current.transform.position.x > transform.position.x)
        {
            textBox.GetComponent<RectTransform>().localPosition = new Vector3(-150, 100, 0);
        }
        else
        {
            textBox.GetComponent<RectTransform>().localPosition = new Vector3(150, 100, 0);
        }
        FadeInObject();
        textBoxText.text = dialogueInfo.dialogueText[0].dialogueText;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GetComponentInParent<NPC>().Talk();
        }
    }

    public void FadeOutObject()
    {
        fadeOut = true;
    }

    public void FadeInObject()
    {
        fadeIn = true;
    }
}
