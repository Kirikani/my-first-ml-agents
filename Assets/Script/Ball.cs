using System.Collections;
using System.Collections.Generic;
using MLAgents;
using UnityEngine;

public class Ball : MonoBehaviour {

    private float speed = 2.0f; //これを追加
    Rigidbody rBody;
    Vector3 controlSignal = Vector3.zero;
    // Use this for initialization
    void Start () {
        //以下を追加
        controlSignal.x = 50;
        rBody = GetComponent<Rigidbody> ();
        rBody.AddForce (-controlSignal * speed);
    }
    void Update () {
        
    }
}