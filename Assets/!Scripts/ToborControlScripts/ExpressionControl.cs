using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpressionControl : MonoBehaviour
{
    public enum ExpressionType
    {
        Normal,
        Confused,
        Sad,
        Angry,
        Happy,
        Suspicious
    }

    [Serializable]
    public struct ExpressionParts
    {
        public ExpressionType type;
        public GameObject LeftEye;
        public GameObject RightEye;
    }

    public List<ExpressionParts> expressions;
    private ExpressionParts curExpression;
    public List<GameObject> loadingBars;

    private void Start()
    {
        SetExpression((int)ExpressionType.Normal);
    }

    public void SetExpression(int expressionIndex)
    {
        try
        {
            curExpression.LeftEye.SetActive(false);
            curExpression.RightEye.SetActive(false);
        }
        catch (Exception)
        {

        }

        curExpression = expressions[expressionIndex];

        curExpression.LeftEye.SetActive(true);
        curExpression.RightEye.SetActive(true);

        //StartCoroutine(BackToNormal());
    }

/*    IEnumerator BackToNormal()
    {
        //yield on a new YieldInstruction that waits for 10 seconds.
        yield return new WaitForSeconds(10);
        SetExpression((int)ExpressionType.Normal);
    }*/

}