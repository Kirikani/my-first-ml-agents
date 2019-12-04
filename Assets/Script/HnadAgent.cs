using System.Collections;
using System.Collections.Generic;
using MLAgents;
using UnityEngine;

public class HnadAgent : Agent {
    public Rigidbody rBall;
    public Rigidbody Goal;
    public Rigidbody BackBoad;
    public bool flag;

    Rigidbody rBody;
    Transform tBody;
    // Start is called before the first frame update
    void Start () {
        rBody = GetComponent<Rigidbody> ();
        tBody = GetComponent<Transform> ();
        flag = false;
    }

    public override void AgentReset () {
        Debug.Log ("Reset");
        rBall.transform.position = new Vector3 (3.0f, 2.0f, 0);
        rBody.transform.position = new Vector3 (3.0f, 1.0f, 0);
        tBody.transform.rotation = Quaternion.Euler (0.0f, 0.0f, 0.0f);
        rBody.velocity = Vector3.zero;
        flag = false;
    }

    public override void CollectObservations () {
        AddVectorObs (Goal.position);
        AddVectorObs (rBall.position);
        AddVectorObs (rBall.position);
        AddVectorObs (rBody.rotation);
    }

    public override void AgentAction (float[] vectorAction, string textAction) {
        Vector3 RSignal = Vector3.zero;
        Vector3 ForceSginal = Vector3.zero;
        float distanceToGoal = Vector3.Distance (rBall.position, Goal.position);
        float distanceToBall = Vector3.Distance (rBall.position, rBody.position);

        RSignal.x = vectorAction[0];
        RSignal.y = vectorAction[1];
        RSignal.z = vectorAction[2];

        ForceSginal.x = vectorAction[3];
        ForceSginal.y = vectorAction[4];
        ForceSginal.z = vectorAction[5];

        rBody.angularVelocity = RSignal * 0.1f * Mathf.PI;
        if (flag) {
            rBody.AddForce (ForceSginal * 0.5f, ForceMode.Impulse);
            if (distanceToGoal < 1.42f) {
                AddReward (2);
                if (rBall.position.y > Goal.position.y) {
                    AddReward (10);
                }
                Done ();
            } else {
                AddReward (10 - distanceToGoal);
            }
        } else {
            rBody.AddForce (ForceSginal);
            AddReward (-RSignal.magnitude);
            if (distanceToBall < 0.1) {
                Debug.Log ("catch");
                AddReward (100);
                flag = true;
            }

        }

        if (rBall.position.y < 0.2) {
            AddReward (-2);
            Done ();
        }
    }
}

//一括置換 Ctrl+F2
//コメントアウト　Ctrl+K+C
//インデント揃え　Shift + alt + F