using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class KotauchiTrackerDetector
{
    //基準となる中心がcenter,leftVecが左を示すベクトルで，trackerの位置座標から右左の判定をして返す
    public static TransformSetting detectTransformSetting(Transform[] trackers, Vector3 center, Vector3 leftVec) {
        var sortedTrackers = trackers.OrderBy(tracker => {
            var vec = tracker.position - center;
            return Vector2.Dot(leftVec, vec);
        }).ToArray();

        Transform right = sortedTrackers[0];
        Transform left = sortedTrackers[1];

        return new TransformSetting(right, left);
    }

    public static TrackerSettings Detect(Transform[] trackers)
    {
        //y座標で降順にソートする
        var sortedTrackers = trackers.OrderBy(a => -a.position.y).ToArray();
        var head = sortedTrackers[0]; //一番最初の要素は頭のトラッカー
        var body = sortedTrackers[5]; //頭+右ひじ+右手+左ひじ+左手の後，体(pelvis)のトラッカー
        var vecTop = (head.position - body.position).normalized; //体から頭方向へのベクトル

        //腕(eblowとhand)のグループ
        var armGroup = new Transform[]{
            sortedTrackers[1],
            sortedTrackers[2],
            sortedTrackers[3],
            sortedTrackers[4]
        };

        //体の中心に近いもの順にソート
        var averageSortedArmGroup = armGroup.OrderBy(tracker => {
            var vec = tracker.position - body.position;
            var magnitude = Vector3.Dot(vec, vecTop);
            return (vec - magnitude * vecTop).magnitude;
        }).ToArray();

        //体の中心から遠いものは手(hand)
        var handGroup = new Transform[] {
            averageSortedArmGroup[2],
            averageSortedArmGroup[3]
        };

        //体の中心から手へのベクトルを定義
        var vec1 = handGroup[0].position - body.position;
        var vec2 = handGroup[1].position - body.position;

        //手の高さから手の方向へのベクトル
        var modVec1 = vec1 - Vector3.Dot(vecTop, vec1) * vecTop;
        var modVec2 = vec2 - Vector3.Dot(vecTop, vec2) * vecTop;

        //外積ベクトル
        var crossVec = Vector3.Cross(modVec1, modVec2);

        Transform rightHand;
        Transform leftHand;

        //外積ベクトルと体のベクトルの方向から右手か左手を判定する
        //vec1が右手の時，外積ベクトルは下を向く(左手系だから)
        if (Vector3.Dot(vecTop, crossVec) < 0)
        {
            rightHand = handGroup[0];
            leftHand = handGroup[1];
        }
        else
        {
            rightHand = handGroup[1];
            leftHand = handGroup[0];
        }

        //左手方向へのベクトル
        var leftVec = (leftHand.position - body.position).normalized;

        //ArmGroupの中心に近いものは肘
        var elbowGroup = new Transform[] {
            averageSortedArmGroup[0],
            averageSortedArmGroup[1]
        };
        
      　//頭,肘*2,手*2,体,"右ひざ","左ひざ"
        var kneeGroup = new Transform[] {
            sortedTrackers[6],
            sortedTrackers[7]
        };

        //頭,肘*2,手*2,体,ひざ*2,"右足"，"左足"
        var footGroup = new Transform[] {
            sortedTrackers[8],
            sortedTrackers[9]
        };

        var elbowSetting = detectTransformSetting(elbowGroup, body.position, leftVec);
        var kneeSetting = detectTransformSetting(kneeGroup, body.position, leftVec);
        var footSetting = detectTransformSetting(footGroup, body.position, leftVec);

        return new TrackerSettings(
            head,
            body,
            leftHand,
            elbowSetting.left,
            rightHand,
            elbowSetting.right,
            footSetting.left,
            kneeSetting.left,
            footSetting.right,
            kneeSetting.right
        );
    }
}
