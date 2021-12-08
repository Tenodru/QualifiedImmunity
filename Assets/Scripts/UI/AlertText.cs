using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlertText : MonoBehaviour
{
    public static AlertText current;

    // Start is called before the first frame update
    void Start()
    {
        current = this;
    }

    public void ShowAlert(string alert, Color color)
    {
        GetComponent<Text>().text = alert;
        GetComponent<Text>().color = color;
        GetComponent<Animator>().SetTrigger("Alert");
    }

    public void HideAlert()
    {
        GetComponent<Text>().text = "";
    }
}
