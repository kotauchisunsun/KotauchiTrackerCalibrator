using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackerSettings
{
    public Transform headTracker { get; }
    public Transform bodyTracker { get; }
    public Transform leftHandTracker { get; }
    public Transform leftElbowTracker { get; }
    public Transform rightHandTracker { get; }
    public Transform rightElbowTracker { get; }
    public Transform leftFootTracker { get; }
    public Transform leftKneeTracker { get; }
    public Transform rightFootTracker { get; }
    public Transform rightKneeTracker { get; }

    public TrackerSettings(
        Transform headTracker,
        Transform bodyTracker,
        Transform leftHandTracker,
        Transform leftElbowTracker,
        Transform rightHandTracker,
        Transform rightElbowTracker,
        Transform leftFootTracker,
        Transform leftKneeTracker,
        Transform rightFootTracker,
        Transform rightKneeTracker)
    {
        this.headTracker = headTracker;
        this.bodyTracker = bodyTracker;
        this.leftHandTracker = leftHandTracker;
        this.leftElbowTracker = leftElbowTracker;
        this.rightHandTracker = rightHandTracker;
        this.rightElbowTracker = rightElbowTracker;
        this.leftFootTracker = leftFootTracker;
        this.leftKneeTracker = leftKneeTracker;
        this.rightFootTracker = rightFootTracker;
        this.rightKneeTracker = rightKneeTracker;
    }
};
