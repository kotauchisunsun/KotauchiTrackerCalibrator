using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IKTargetRepositoryInterface
{
    Transform getHead();
    Transform getPelvis();
    Transform getLeftHand();
    Transform getLeftElbow();
    Transform getRightHand();
    Transform getRightElbow();
    Transform getLeftKnee();
    Transform getLeftLeg();
    Transform getRightKnee();
    Transform getRightLeg();
}