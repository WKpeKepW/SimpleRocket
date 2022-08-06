using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : MonoBehaviour
{
    float Ystartposition;
    [SerializeField] float speed = 1;
    float traveldistans; 
    // Start is called before the first frame update
    void Start()
    {
        Ystartposition = transform.position.y;
        traveldistans = Ystartposition + 20;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x, Ystartposition - Mathf.Repeat(Time.time * speed, traveldistans), transform.position.z);
    }
}
