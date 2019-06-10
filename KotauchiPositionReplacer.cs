using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KotauchiPositionReplacer : MonoBehaviour
{
    public Transform[] transformTargets;
    public Transform   target;
    public Transform   origin;
    public GameObject  transformOrigin;

    public GameObject[] gameObjects;
    public KotauchiTrackerDetectController detector;

    public float rotY;
    bool init;

    // Start is called before the first frame update
    void Start()
    {
        gameObjects = new GameObject[10];
        init = false;

        /*
        transformOrigin = GameObject.CreatePrimitive(PrimitiveType.Cube);
        transformOrigin.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
        transformOrigin.name = "transformOrigin";
        */    
       transformOrigin = new GameObject("transformOrigin");
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
                /*
                GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
                obj.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
                obj.name = "target" + i.ToString();
                */
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
