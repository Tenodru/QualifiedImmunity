using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public enum RotationDirection { Clockwise, CounterClockwise }

    public RotationDirection direction;
    public float rotationRate;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (direction == RotationDirection.Clockwise)
        {
            transform.Rotate(0, 0, -rotationRate);
        }
        else if (direction == RotationDirection.CounterClockwise)
        {
            transform.Rotate(0, 0, rotationRate);
        }
    }
}
