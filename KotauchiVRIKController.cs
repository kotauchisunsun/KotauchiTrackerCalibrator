using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RootMotion.FinalIK;

public class KotauchiVRIKController : MonoBehaviour
{
    private IKTargetRepositoryInterface repository;

    public VRIK ik;
    public VRIKCalibrator.Settings settings;
    public Transform headTracker;
    public Transform bodyTracker;
    public Transform leftHandTracker;
    public Transform leftElbowTracker;
    public Transform rightHandTracker;
    public Transform rightElbowTracker;
    public Transform leftFootTracker;
    public Transform leftKneeTracker;
    public Transform rightFootTracker;
    public Transform rightKneeTracker;

    void Start()
    {
        repository = new GameObjectIKTargetRepository();
    }

    void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            KotauchiVRIKCalibrator.Calibrate(
                repository,
                ik, 
                settings,
                headTracker,
                bodyTracker, 
                leftHandTracker,
                leftElbowTracker,
                rightHandTracker,
                rightElbowTracker, 
                leftKneeTracker,
                leftFootTracker,
                rightKneeTracker,
                rightFootTracker
            );
        }
    }
}

