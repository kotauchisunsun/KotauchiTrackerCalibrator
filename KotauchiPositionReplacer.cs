using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KotauchiPositionReplacer : MonoBehaviour
{
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
    public Transform target;
    public Transform origin;

    public KotauchiTrackerDetectController detector;

    private Transform[] transformTargets;
    private GameObject[] gameObjects;
    private GameObject transformOrigin;
    private float rotY;
    private bool init;

    // Start is called before the first frame update
    void Start()
    {
        gameObjects = new GameObject[10];
        transformTargets = new Transform[10];
        init = false;
        transformOrigin = new GameObject("transformOrigin");

        transformTargets[0] = tracker01;
        transformTargets[1] = tracker02;
        transformTargets[2] = tracker03;
        transformTargets[3] = tracker04;
        transformTargets[4] = tracker05;
        transformTargets[5] = tracker06;
        transformTargets[6] = tracker07;
        transformTargets[7] = tracker08;
        transformTargets[8] = tracker09;
        transformTargets[9] = tracker10;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            rotY = origin.eulerAngles.y - target.eulerAngles.y;
            init = true;

            Vector3 position = new Vector3();
            position.x = target.position.x;
            position.y = origin.position.y;
            position.z = target.position.z;

            Vector3 angle = new Vector3();
            angle.y = target.eulerAngles.y;
            transformOrigin.transform.position    = position;
            transformOrigin.transform.eulerAngles = angle;


            for (int i = 0; i < 10; i++) {
                GameObject obj = new GameObject("target" + i.ToString());
                gameObjects[i] = obj;
            }

            detector.tracker01 = gameObjects[0].transform;
            detector.tracker02 = gameObjects[1].transform;
            detector.tracker03 = gameObjects[2].transform;
            detector.tracker04 = gameObjects[3].transform;
            detector.tracker05 = gameObjects[4].transform;
            detector.tracker06 = gameObjects[5].transform;
            detector.tracker07 = gameObjects[6].transform;
            detector.tracker08 = gameObjects[7].transform;
            detector.tracker09 = gameObjects[8].transform;
            detector.tracker10 = gameObjects[9].transform;
        }

        if (init){
            for (int i = 0; i < 10; i++)
            {
                GameObject obj = gameObjects[i];
                Vector3 localPosition = transformTargets[i].transform.position - transformOrigin.transform.position;

                Vector3 transformForward = transformOrigin.transform.forward;
                Vector3 transformUp      = transformOrigin.transform.up;
                Vector3 transformRight   = transformOrigin.transform.right;

                float forwardCoefficient = Vector3.Dot(transformForward, localPosition);
                float upCoefficient      = Vector3.Dot(transformUp,      localPosition);
                float rightCoefficient   = Vector3.Dot(transformRight,   localPosition);

                Vector3 originForward = origin.forward;
                Vector3 originUp      = origin.up;
                Vector3 originRight   = origin.right;

                Vector3 position = (
                    origin.transform.position
                    + forwardCoefficient * originForward
                    + upCoefficient      * originUp
                    + rightCoefficient   * originRight
                );

                Vector3 rot = transformTargets[i].eulerAngles;
                rot.y += rotY;
                obj.transform.position = position;
                obj.transform.eulerAngles = rot;
            }
        }
    }
}
