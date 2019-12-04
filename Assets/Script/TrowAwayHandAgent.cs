using System.Collections;
using System.Collections.Generic;
using MLAgents;
using UnityEngine;

public class TrowAwayHandAgent : Agent {
    public Transform tBall;
    public Rigidbody rBall;
    int catchFlag;
    int count;
    Rigidbody rBody;
    Transform tBody;
    // Start is called before the first frame update
    void Start () {
        rBody = GetComponent<Rigidbody> ();
        tBody = GetComponent<Transform> ();
        catchFlag = 0;
    }

    public override void AgentReset () {
        Debug.Log ("Reset");
        tBall.position=new Vector3 (-1.0f, 5.0f, 0);
        rBall.position = new Vector3 (-1, 5, 0);
        rBody.position = new Vector3 (-1, 1, 0);
        tBody.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
        rBody.velocity = Vector3.zero;
        rBall.velocity = Vector3.zero;
        catchFlag = 0;
        count=0;
    }

    public override void CollectObservations () {
        AddVectorObs (rBall.position);
        AddVectorObs (rBody.position);
        AddVectorObs (rBody.rotation);
        AddVectorObs (catchFlag);
    }

    public override void AgentAction (float[] vectorAction, string textAction) {
        Vector3 RSignal = Vector3.zero;
        Vector3 ForceSginal = Vector3.zero;
        float distanceToBall = Vector3.Distance (tBall.position, rBody.position);
        count++;

        RSignal.x = vectorAction[0];
        RSignal.y = vectorAction[1];
        RSignal.z = vectorAction[2];

        ForceSginal.x = vectorAction[3];
        ForceSginal.y = vectorAction[4];
        ForceSginal.z = vectorAction[5];

        rBody.angularVelocity = RSignal * 0.1f * Mathf.PI;
        if (catchFlag>4) {
            rBody.AddForce (ForceSginal*10, ForceMode.Impulse);
            AddReward (RSignal.magnitude);
            AddReward (tBall.position.x);
        } else {
            rBody.AddForce (ForceSginal*10,ForceMode.Force);            
            AddReward (5-RSignal.magnitude);
            if (distanceToBall < 0.1) {
                Debug.Log ("catch");
                AddReward (100);
                catchFlag++;
            }
        }
        if (tBall.position.y < 1.1) {
            AddReward (tBall.position.x * 100);
            Debug.Log ("Fall Ball");
            Done ();
        }
        if (rBody.position.x > 0) {
            AddReward (-100);
            Debug.Log ("Hand out");
            Done ();
        }
        if(count>100){
            AddReward (-100);
            Debug.Log ("Time out");
            Done();
        }
    }
}

//一括置換 Ctrl+F2
//コメントアウト　Ctrl+K+C
//インデント揃え　Shift + alt + F