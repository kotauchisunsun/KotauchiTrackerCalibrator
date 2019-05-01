using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class KotauchiTrackerDetectController : MonoBehaviour
{
    public KotauchiVRIKController controller;

    public Transform tracker01;
    public Transform tracker02;
    public Transform tracker03;
    public Transform tracker04;
    public Transform tracker05;
    public Transform tracker06;
    public Transform tracker07;
    public Transform tracker08;
    public Transform tracker09;
    public Transform tracker10;

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)) {
            Transform[] trackers = new Transform[] {
                tracker01,
                tracker02,
                tracker03,
                tracker04,
                tracker05,
                tracker06,
                tracker07,
                tracker08,
                tracker09,
                tracker10,
            };

            var settings = KotauchiTrackerDetector.Detect(trackers);

            controller.headTracker       = settings.headTracker;
            controller.bodyTracker       = settings.bodyTracker;
            controller.rightHandTracker  = settings.rightHandTracker;
            controller.leftHandTracker   = settings.leftHandTracker;
            controller.rightElbowTracker = settings.rightElbowTracker;
            controller.leftElbowTracker  = settings.leftElbowTracker;
            controller.rightKneeTracker  = settings.rightKneeTracker;
            controller.leftKneeTracker   = settings.leftKneeTracker;
            controller.rightFootTracker  = settings.rightFootTracker;
            controller.leftFootTracker   = settings.leftFootTracker;
        }
    }
}
