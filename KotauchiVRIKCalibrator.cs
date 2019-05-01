using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RootMotion.FinalIK;

public static class KotauchiVRIKCalibrator
{
    public static CalibrateSetting CalibrateRoot(Vector3 rootPosition,Vector3 headTrackerForward, Vector3 headTrackerUp,Vector3 headOffset, Transform headTracker) {
        Vector3 headPos = headTracker.position + headTracker.rotation * Quaternion.LookRotation(headTrackerForward, headTrackerUp) * headOffset;
        Vector3 headForward = headTracker.rotation * headTrackerForward;
        headForward.y = 0f;

        Transform parent = null;
        Vector3 position = new Vector3(headPos.x, rootPosition.y, headPos.z);
        Quaternion rotation = Quaternion.LookRotation(headForward);

        return new CalibrateSetting(parent, position, rotation);
    }

    public static CalibrateSetting CalibrateHead(Quaternion rotation, Vector3 forward, Vector3 up, Vector3 offset, Transform tracker)
    {
        Vector3 headPos = tracker.position + tracker.rotation * Quaternion.LookRotation(forward, up) * offset;
        return new CalibrateSetting(tracker, headPos, rotation);
    }

    public static CalibrateSetting CalibratePelvis(Vector3 position, Quaternion rotation, Transform tracker) {
        return new CalibrateSetting(
            tracker,
            position,
            rotation
        );
    }
   
    public static CalibrateSetting CalibrateLeftHand(Vector3 wristToPalmAxis, Vector3 palmToThumbAxis, Vector3 forward, Vector3 up, Vector3 offset, Transform tracker) {
        Vector3 position = tracker.position + tracker.rotation * Quaternion.LookRotation(forward, up) * offset;

        Vector3 leftHandUp = Vector3.Cross(wristToPalmAxis, palmToThumbAxis);
        Quaternion rotation = RootMotion.QuaTools.MatchRotation(tracker.rotation * Quaternion.LookRotation(forward, up), forward, up, wristToPalmAxis, leftHandUp);

        return new CalibrateSetting(
            tracker,
            position,
            rotation
        );
    }
    
    public static CalibrateSetting CalibrateRightHand(Vector3 wristToPalmAxis, Vector3 palmToThumbAxis, Vector3 forward, Vector3 up, Vector3 offset, Transform tracker) {
        Vector3 position = tracker.position + tracker.rotation * Quaternion.LookRotation(forward, up) * offset;
        Vector3 rightHandUp = -Vector3.Cross(wristToPalmAxis, palmToThumbAxis);
        Quaternion rotation = RootMotion.QuaTools.MatchRotation(tracker.rotation * Quaternion.LookRotation(forward, up), forward, up, wristToPalmAxis, rightHandUp);
       
        return new CalibrateSetting(
            tracker,
            position,
            rotation
        );
    }

    public static CalibrateSetting CalibrateElbow(Vector3 wristToPalmAxis, Vector3 palmToThumbAxis, Vector3 forward, Vector3 up, Vector3 offset, Transform tracker)
    {
        Vector3 position = tracker.position + tracker.rotation * Quaternion.LookRotation(forward, up) * offset;
        Vector3 leftHandUp = Vector3.Cross(wristToPalmAxis, palmToThumbAxis);
        Quaternion rotation = RootMotion.QuaTools.MatchRotation(tracker.rotation * Quaternion.LookRotation(forward, up), forward, up, wristToPalmAxis, leftHandUp);

        return new CalibrateSetting(
            tracker,
            position,
            rotation
        );
    }

    public static CalibrateSetting CalibrateLeftKnee(Vector3 position, Quaternion rotation,Transform tracker) {
        return new CalibrateSetting(
            tracker,
            position,
            rotation
        );
    }

    public static CalibrateSetting CalibrateRightKnee(Vector3 position, Quaternion rotation, Transform tracker)
    {
        return new CalibrateSetting(
            tracker,
            position,
            rotation
        );
    }

    private static Vector3 CalibrateFootTrackerSpaceForward(Transform tracker, Vector3 footTrackerForward, Vector3 footTrackerUp)
    {
        Quaternion trackerSpace = tracker.rotation * Quaternion.LookRotation(footTrackerForward, footTrackerUp);
        Vector3 f = trackerSpace * Vector3.forward;
        f.y = 0f;

        return f;
    }

    private static CalibrateSetting CalibrateLeg(
        Transform tracker,
        Transform lastBone,
        Vector3 rootForward,
        Vector3 footTrackerForward,
        Vector3 footTrackerUp,
        float footForwardOffset,
        float inwardOffset,
        float headingOffset)
    {

        Vector3 spaceForward = CalibrateFootTrackerSpaceForward(tracker, footTrackerForward, footTrackerUp);
        Quaternion trackerSpace = Quaternion.LookRotation(spaceForward);

        // Target position
        Vector3 targetPosition = tracker.position + trackerSpace * new Vector3(inwardOffset, 0f, footForwardOffset);
        Vector3 position = new Vector3(targetPosition.x, lastBone.position.y, targetPosition.z);

        // Rotate target forward towards tracker forward
        Vector3 footForward = RootMotion.AxisTools.GetAxisVectorToDirection(lastBone, rootForward);
        if (Vector3.Dot(lastBone.rotation * footForward, rootForward) < 0f) footForward = -footForward;
        Vector3 fLocal = Quaternion.Inverse(Quaternion.LookRotation(lastBone.rotation * footForward)) * spaceForward;
        float angle = Mathf.Atan2(fLocal.x, fLocal.z) * Mathf.Rad2Deg;

        // Target rotation
        Quaternion rotation = Quaternion.AngleAxis(angle + headingOffset, Vector3.up) * lastBone.rotation;

        return new CalibrateSetting(
            tracker,
            position,
            rotation
        );
    }

    public static void Calibrate(
        IKTargetRepositoryInterface repository,
        VRIK ik,
        VRIKCalibrator.Settings settings,
        Transform headTracker,
        Transform bodyTracker = null, 
        Transform leftHandTracker = null, 
        Transform leftElbowTracker = null,
        Transform rightHandTracker = null,
        Transform rightElbowTracker = null,
        Transform leftKneeTracker = null,
        Transform leftFootTracker = null,
        Transform rightKneeTracker = null,
        Transform rightFootTracker = null)
    {
        if (!ik.solver.initiated)
        {
            Debug.LogError("Can not calibrate before VRIK has initiated.");
            return;
        }

        if (headTracker == null)
        {
            Debug.LogError("Can not calibrate VRIK without the head tracker.");
            return;
        }
        
        ik.solver.FixTransforms();

        // Root position and rotation
        var rootSetting = CalibrateRoot(ik.references.root.position, settings.headTrackerForward, settings.headTrackerUp, settings.headOffset, headTracker);
        ik.references.root.position = rootSetting.position;
        ik.references.root.rotation = rootSetting.rotation;

        // Head
        var headSetting = CalibrateHead(ik.references.head.rotation, settings.headTrackerForward, settings.headTrackerUp, settings.headOffset, headTracker);
        var headTarget = repository.getHead();
        headTarget.parent = headSetting.parent;
        headTarget.position = headSetting.position;
        headTarget.rotation = headSetting.rotation;

        ik.solver.spine.headTarget = headTarget;

        // Size
        float sizeF = (headTarget.position.y - ik.references.root.position.y) / (ik.references.head.position.y - ik.references.root.position.y);
        ik.references.root.localScale *= sizeF * settings.scaleMlp;

        // Body
        if (bodyTracker != null)
        {
            var pelvisSetting = CalibratePelvis(ik.references.pelvis.position, ik.references.pelvis.rotation, bodyTracker);

            Transform pelvisTarget = repository.getPelvis();
            pelvisTarget.parent = pelvisSetting.parent;
            pelvisTarget.position = pelvisSetting.position;
            pelvisTarget.rotation = pelvisSetting.rotation;
            
            ik.solver.spine.pelvisTarget = pelvisTarget;
            ik.solver.spine.pelvisPositionWeight = settings.pelvisPositionWeight;
            ik.solver.spine.pelvisRotationWeight = settings.pelvisRotationWeight;

            ik.solver.plantFeet = false;
            ik.solver.spine.maxRootAngle = 180f;
        }
        else if (leftFootTracker != null && rightFootTracker != null)
        {
            ik.solver.spine.maxRootAngle = 0f;
        }

        // Left Hand
        if (leftHandTracker != null)
        {
            var leftHandSetting = CalibrateLeftHand(
                ik.solver.leftArm.wristToPalmAxis,
                ik.solver.leftArm.palmToThumbAxis, 
                settings.handTrackerForward, 
                settings.handTrackerUp,
                settings.handOffset,
                leftHandTracker
            );

            Transform leftHandTarget = repository.getLeftHand();
            leftHandTarget.parent = leftHandSetting.parent;
            leftHandTarget.position = leftHandSetting.position;
            leftHandTarget.rotation = leftHandSetting.rotation;

            ik.solver.leftArm.target = leftHandTarget;
            ik.solver.leftArm.positionWeight = 1f;
            ik.solver.leftArm.rotationWeight = 1f;
        }
        else
        {
            ik.solver.leftArm.positionWeight = 0f;
            ik.solver.leftArm.rotationWeight = 0f;
        }

        // Left Elbow
        if (leftElbowTracker != null)
        {
            var leftElbowSetting = CalibrateElbow(
                ik.solver.leftArm.wristToPalmAxis,
                ik.solver.leftArm.palmToThumbAxis, 
                settings.handTrackerForward,
                settings.handTrackerUp, 
                settings.handOffset, 
                leftElbowTracker
            );

            Transform leftElbowTarget = repository.getLeftElbow();
            leftElbowTarget.parent = leftElbowSetting.parent;
            leftElbowTarget.position = leftElbowSetting.position;
            leftElbowTarget.rotation = leftElbowSetting.rotation;

            ik.solver.leftArm.bendGoal = leftElbowTarget;
            ik.solver.leftArm.bendGoalWeight = 1.0f;
        }
        else
        {
            ik.solver.leftArm.bendGoalWeight = 0.0f;
        }

        // Right Hand
        if (rightHandTracker != null)
        {
            var rightHandSetting = CalibrateRightHand(
                ik.solver.rightArm.wristToPalmAxis,
                ik.solver.rightArm.palmToThumbAxis,
                settings.handTrackerForward,
                settings.handTrackerUp,
                settings.handOffset,
                rightHandTracker
            );

            Transform rightHandTarget = repository.getRightHand();
            rightHandTarget.parent = rightHandSetting.parent;
            rightHandTarget.position = rightHandSetting.position;
            rightHandTarget.rotation = rightHandSetting.rotation;

            ik.solver.rightArm.target = rightHandTarget;
            ik.solver.rightArm.positionWeight = 1f;
            ik.solver.rightArm.rotationWeight = 1f;
        }
        else
        {
            ik.solver.rightArm.positionWeight = 0f;
            ik.solver.rightArm.rotationWeight = 0f;
        }

        // Right Elbow
        if (rightElbowTracker != null)
        {
            var rightElbowSetting = CalibrateElbow(
                ik.solver.rightArm.wristToPalmAxis,
                ik.solver.rightArm.palmToThumbAxis,
                settings.handTrackerForward, 
                settings.handTrackerUp,
                settings.handOffset,
                rightElbowTracker
            );

            Transform rightElbowTransform = repository.getRightElbow();
            rightElbowTransform.parent = rightElbowSetting.parent;
            rightElbowTransform.position = rightElbowSetting.position;
            rightElbowTransform.rotation = rightElbowSetting.rotation;
        
            ik.solver.rightArm.bendGoal = rightElbowTransform;
            ik.solver.rightArm.bendGoalWeight = 1.0f;
        }
        else
        {
            ik.solver.rightArm.bendGoalWeight = 0.0f;
        }

        // Legs
        if (leftFootTracker != null)
        {
            var lastBone = (ik.references.leftToes != null ? ik.references.leftToes : ik.references.leftFoot);
            var legSetting = CalibrateLeg(
                leftFootTracker,
                lastBone,
                ik.references.root.forward,
                settings.footTrackerForward,
                settings.footTrackerUp,
                settings.footForwardOffset,
                settings.footInwardOffset,
                settings.footHeadingOffset
            );

            Transform target = repository.getLeftLeg();
            target.parent = legSetting.parent;
            target.position = legSetting.position;
            target.rotation = legSetting.rotation;

            var leg = ik.solver.leftLeg;
            leg.target = target;
            leg.positionWeight = 1f;
            leg.rotationWeight = 1f;
        }

        if (leftKneeTracker != null)
        {
            CalibrateSetting leftKneeSetting = CalibrateLeftKnee(
                ik.references.leftCalf.position,
                ik.references.leftCalf.rotation,
                leftKneeTracker
            );

            Transform leftKneeTransform = repository.getLeftKnee();
            leftKneeTransform.parent = leftKneeSetting.parent;
            leftKneeTransform.position = leftKneeSetting.position;
            leftKneeTransform.rotation = leftKneeSetting.rotation;

            leftKneeTransform.transform.localPosition += new Vector3(0, -0.3f, 1f);

            ik.solver.leftLeg.bendGoal = leftKneeTransform;
            ik.solver.leftLeg.bendGoalWeight = 1.0f;
        }

        if (rightFootTracker != null)
        {
            var lastBone = (ik.references.rightToes != null ? ik.references.rightToes : ik.references.rightFoot);
            var legSetting = CalibrateLeg(
                rightFootTracker,
                lastBone,
                ik.references.root.forward,
                settings.footTrackerForward,
                settings.footTrackerUp,
                settings.footForwardOffset,
                settings.footInwardOffset,
                settings.footHeadingOffset
            );

            Transform target = repository.getRightLeg();
            target.parent = legSetting.parent;
            target.position = legSetting.position;
            target.rotation = legSetting.rotation;

            var leg = ik.solver.rightLeg;
            leg.target = target;
            leg.positionWeight = 1f;
            leg.rotationWeight = 1f;
        }

        if (rightKneeTracker != null)
        {
            var rightKneeSetting = CalibrateRightKnee(
                ik.references.rightCalf.position,
                ik.references.rightCalf.rotation,
                rightKneeTracker
            );

            Transform rightKneeTransform = repository.getRightKnee();
            rightKneeTransform.parent = rightKneeSetting.parent;
            rightKneeTransform.position = rightKneeSetting.position;
            rightKneeTransform.rotation = rightKneeSetting.rotation;

            rightKneeTransform.transform.localPosition += new Vector3(0, -0.3f, 1f);

            ik.solver.rightLeg.bendGoal = rightKneeTransform;
            ik.solver.rightLeg.bendGoalWeight = 1.0f;
        }

        // Root controller
        bool addRootController = bodyTracker != null || (leftFootTracker != null && rightFootTracker != null);
        var rootController = ik.references.root.GetComponent<VRIKRootController>();

        if (addRootController)
        {
            if (rootController == null) rootController = ik.references.root.gameObject.AddComponent<VRIKRootController>();
            rootController.Calibrate();
        }
        else
        {
            if (rootController != null) GameObject.Destroy(rootController);
        }

        // Additional solver settings
        ik.solver.spine.minHeadHeight = 0f;
        ik.solver.locomotion.weight = bodyTracker == null && leftFootTracker == null && rightFootTracker == null ? 1f : 0f;
        
    }
}
