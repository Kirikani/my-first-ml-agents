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

    // void OnCollisionEnter(Collision collision)
    // {
    //     if(collision.gameObject.tag == "Finish")
    //     {
    //         Debug.Log(Name+"OnCollision");
    //     }
    // }


    public override void AgentReset () {
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
        AddVectorObs (rBall.velocity);
        AddVectorObs (rBody.position);
        AddVectorObs (rBody.rotation);
        AddVectorObs (catchFlag);
    }

    public override void AgentAction (float[] vectorAction, string textAction) {
        Vector3 RSignal = Vector3.zero;
        Vector3 ForceSginal = Vector3.zero;
        float distanceToBall = Vector3.Distance (rBall.position, rBody.position);
        count++;

        RSignal.x = vectorAction[0];
        RSignal.y = vectorAction[1];
        RSignal.z = vectorAction[2];

        ForceSginal.x = vectorAction[3];
        ForceSginal.y = vectorAction[4];
        ForceSginal.z = vectorAction[5];

        rBody.angularVelocity = RSignal * 0.1f * Mathf.PI;
        AddReward(rBall.velocity.x*2);
        if (catchFlag>4) {
            rBody.AddForce (ForceSginal*10, ForceMode.Impulse);
            AddReward(rBall.velocity.x*5);
        } else {
            rBody.AddForce (ForceSginal*10,ForceMode.Force);            
            if (distanceToBall < 0.1 && rBall.position.x>rBody.position.x+0.3) {
                Debug.Log ("catch");
                AddReward (1);
                catchFlag++;
            }
        }
        if (tBall.position.y < 1.1) {
            if(tBall.position.x>0){
                AddReward (tBall.position.x);
            }
            Debug.Log ("Fall Ball"+rBall.velocity.x);
            Done ();
        }
        if (rBody.position.x > 3) {
            AddReward (-1);
            Debug.Log ("Hand out"+rBall.velocity.x);
            //Done ();
        }
        if(count>200){
            AddReward (-1);
            AddReward (tBall.position.x);
            Debug.Log ("Time out"+rBall.velocity.x);
            Done();
        }
    }
}

//一括置換 Ctrl+F2
//コメントアウト　Ctrl+K+C
//インデント揃え　Shift + alt + F