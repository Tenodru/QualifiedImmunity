using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransitionPanel : MonoBehaviour
{
    public static TransitionPanel current;

    private void Start()
    {
        current = this;
    }

    public void Animate(string type)
    {
        GetComponent<Animator>().SetTrigger(type);
    }
}
