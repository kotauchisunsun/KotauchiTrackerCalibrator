using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RootMotion.FinalIK;

public class VRIKTargetRepository : IKTargetRepositoryInterface
{
    VRIK ik;

    public VRIKTargetRepository(VRIK ik)
    {
        this.ik = ik;
    }

    public Transform getHead()
    {
        return ik.solver.spine.headTarget;
    }

    public Transform getPelvis()
    {
        return ik.solver.spine.pelvisTarget;
    }

    public Transform getLeftHand()
    {
        return ik.solver.leftArm.target;
    }

    public Transform getLeftElbow()
    {
        return ik.solver.leftArm.bendGoal;
    }

    public Transform getRightHand()
    {
        return ik.solver.rightArm.target;
    }

    public Transform getRightElbow()
    {
        return ik.solver.rightArm.bendGoal;
    }

    public Transform getLeftKnee()
    {
        return ik.solver.leftLeg.bendGoal;
    }

    public Transform getLeftLeg()
    {
        return ik.solver.leftLeg.target;
    }

    public Transform getRightKnee()
    {
        return ik.solver.rightLeg.bendGoal;
    }

    public Transform getRightLeg()
    {
        return ik.solver.rightLeg.target;
    }
}

