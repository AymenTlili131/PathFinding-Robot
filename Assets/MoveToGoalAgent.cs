 using System.Globalization;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class MoveToGoalAgent : Agent
{  [SerializeField] private Transform targetTransform;
    [SerializeField] private Material winMaterial;
    [SerializeField] private Material loseMaterial;

    [SerializeField] public MeshRenderer floorMeshRenderer;

    public override void OnEpisodeBegin()
    {
        transform.localPosition =new Vector3(-0.802773833f,0.200000003f,-1.45099998f);
        transform.localRotation =new Quaternion(-0.70710f,0f,0f,0.7071f);
    }
    public override void OnActionReceived(ActionBuffers actions)
    {
        float moveX= actions.ContinuousActions[0];
        float moveZ= actions.ContinuousActions[1];
        float moveY= actions.ContinuousActions[2];

        float movespeed = 0.8f;
        transform.localPosition +=new Vector3(moveX,moveY*3f,moveZ)*Time.deltaTime *movespeed ;
    }
    
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(targetTransform.localPosition);
    }
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuousActions =actionsOut.ContinuousActions ;
        continuousActions[0] = Input.GetAxisRaw("Horizontal");
        continuousActions[1] = Input.GetAxisRaw("Vertical");
        continuousActions[2] = Input.GetAxisRaw("Jump");
    }
    private void OnTriggerEnter(Collider other){
        if (other.TryGetComponent<Goal>(out Goal goal)){
        SetReward(1f);
        /*floorMeshRenderer = winMaterial;*/
        EndEpisode();}
        if (other.TryGetComponent<Wall>(out Wall wall)){
        SetReward(-1f);
        /*floorMeshRenderer = loseMaterial ;*/
        EndEpisode();}
        if (other.TryGetComponent<Floor>(out Floor floor)){
        SetReward(0.001f);}
        
        }

}

