using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectIKTargetRepository : IKTargetRepositoryInterface
{
    private GameObject head;
    private GameObject pelvis;
    private GameObject leftHand;
    private GameObject leftElbow;
    private GameObject rightHand;
    private GameObject rightElbow;
    private GameObject leftKnee;
    private GameObject leftLeg;
    private GameObject rightKnee;
    private GameObject rightLeg;

    public GameObjectIKTargetRepository()
    {
        head       = new GameObject("Head Target");
        pelvis     = new GameObject("Pelvis Target");
        leftHand   = new GameObject("Left Hand Target");
        leftElbow  = new GameObject("Left Elbow Target");
        rightHand  = new GameObject("Right Hand Target");
        rightElbow = new GameObject("Right Elbow Target");
        leftKnee   = new GameObject("Left Knee Target");
        leftLeg    = new GameObject("Left Leg Target");
        rightKnee  = new GameObject("Right Knee Target");
        rightLeg   = new GameObject("Right Leg target");
    }

    public Transform getHead()
    {
        return head.transform;
    }

    public Transform getPelvis()
    {
        return pelvis.transform;
    }

    public Transform getLeftHand()
    {
        return leftHand.transform;
    }

    public Transform getLeftElbow()
    {
        return leftElbow.transform;
    }

    public Transform getRightHand()
    {
        return rightHand.transform;
    }

    public Transform getRightElbow()
    {
        return rightElbow.transform;
    }

    public Transform getLeftKnee()
    {
        return leftKnee.transform;
    }

    public Transform getLeftLeg()
    {
        return leftLeg.transform;
    }

    public Transform getRightKnee()
    {
        return rightKnee.transform;
    }

    public Transform getRightLeg()
    {
        return rightLeg.transform;
    }
}
