using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Control the expression of Tobor
/// All animation shares the same frames. Toggle the eyes' gamobject to show different expression
/// </summary>
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

    //Setting for expresison
    public List<ExpressionParts> expressions;
    private ExpressionParts _curExpression;

    //Gameobject that controls loading bar expresion
    public List<GameObject> loadingBarsExpression;

    private void Start()
    {
        SetExpression((int)ExpressionType.Normal);
    }

    /// <summary>
    /// Set Tobor's expression to given index
    /// </summary>
    /// <param name="expressionIndex"></param>
    public void SetExpression(int expressionIndex)
    {
        //Handle exception on first time setting expression
        try
        {
            _curExpression.LeftEye.SetActive(false);
            _curExpression.RightEye.SetActive(false);
        }
        catch (Exception)
        {

        }

        _curExpression = expressions[expressionIndex];

        _curExpression.LeftEye.SetActive(true);
        _curExpression.RightEye.SetActive(true);
    }

    /// <summary>
    /// Hide/show all expression
    /// </summary>
    /// <param name="isShow"></param>
    public void ShowAllExpression(bool isShow)
    {
        foreach(ExpressionParts expression in expressions)
        {
            expression.LeftEye.SetActive(isShow);
            expression.RightEye.SetActive(isShow);
        }
    }

    /// <summary>
    /// Hide/show loading expression
    /// </summary>
    /// <param name="isShow"></param>
    public void ShowLoadingExpression(bool isShow)
    {
        foreach(GameObject bar in loadingBarsExpression)
            bar.SetActive(isShow);
    }
}