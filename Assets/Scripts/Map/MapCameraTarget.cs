using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCameraTarget : MonoBehaviour
{

    [SerializeField] float moveSpeed = 2;

    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 tempVect = new Vector3(h, v, 0);
        tempVect = tempVect.normalized * moveSpeed * Time.deltaTime;
        rb.MovePosition(rb.transform.position + tempVect);
    }
}
