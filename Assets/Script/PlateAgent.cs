using System.Collections;
using System.Collections.Generic;
using MLAgents;
using UnityEngine;

public class PlateAgent : Agent {

    public Transform Target;
    public Rigidbody rRoller;
    Rigidbody rBody;
    void Start () {
        rBody = GetComponent<Rigidbody> ();
    }

    public override void AgentReset () {

        rRoller.angularVelocity = Vector3.zero;
        rRoller.velocity = Vector3.zero;
        rRoller.transform.position = new Vector3 (0, 4, 0);
        rBody.rotation = Quaternion.AngleAxis (180.0f, Vector3.up);
        rBody.angularVelocity = new Vector3 (0, 0, 0);

        Target.position = new Vector3 (
            Random.value * 8 - 4,
            0.6f,
            Random.value * 8 - 4
        );
    }

    public override void CollectObservations () {

        AddVectorObs (Target.position);
        AddVectorObs (rRoller.transform.position);

        AddVectorObs (rBody.rotation.x);
        AddVectorObs (rBody.rotation.z);
    }

    public float speed = 10;
    public override void AgentAction (float[] vectorAction, string textAction) {

        // 動作の設定。sizeが2なのでvectorActionは2つ
        Vector3 controlSignal = Vector3.zero;
        controlSignal.x = vectorAction[0];
        controlSignal.z = vectorAction[1];
        rBody.AddTorque (controlSignal * Mathf.PI);

        float distanceToTarget = Vector3.Distance (rRoller.transform.position, Target.position);
        // ターゲットに辿り着いたか
        if (distanceToTarget < 1.42f) {
            SetReward (2);
            Done ();
        } else {
            SetReward (5 - distanceToTarget);
        }

        if (rRoller.transform.position.y < 0) {
            Done ();
        }
    }
}