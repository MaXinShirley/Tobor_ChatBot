using System;
using UnityEngine;

public class ToborControl : MonoBehaviour
{
    //To control animations
    public ExpressionControl ExpressionAnim;
    public int ExpInt;

    public Animator BodyAnim;
    public int BodyInt;

    private AnimationEventListener _animationEvent;

    //Const
    private const int HAPPY_EXP_INDEX = 4;
    private const int NORMAL_EXP_INDEX = 0;

    private const string BODY_ANIM_STATE = "StateBody";
    private const int POWERUP_ANIM_STATE_INDEX = 3;
    private const int POWERDOWN_ANIM_STATE_INDEX = 2;
    private const int INACTIVE_ANIM_STATE_INDEX = 4;
    private const int CELEBREATE_ANIM_STATE_INDEX = 5;
    private const int DANCE_ANIM_STATE_INDEX = 7;


    public void PlayExpressionAnim(int ExpBtnIndex)
    {
        ExpInt = ExpBtnIndex; //store the Exp index and use in AnimationEventListener to check special cases

        if (ExpressionAnim != null)
        {
            ExpressionAnim.SetExpression(ExpInt);
        }
    }

    public void PlayBodyAnim(int BodyBtnIndex)
    {
        //Used BodyInt in MoveWithAnimation in order to go back to previous body aniamtion after movement animation
        BodyInt = BodyBtnIndex;    
        BodyAnim.SetInteger(BODY_ANIM_STATE, BodyInt);

        //Special case (loading exp for powerup, shutdown and inactive)
        if (BodyInt == POWERDOWN_ANIM_STATE_INDEX || BodyInt == POWERUP_ANIM_STATE_INDEX || BodyInt == INACTIVE_ANIM_STATE_INDEX)
        {
            ExpressionAnim.ShowAllExpression(false);
            ExpressionAnim.ShowLoadingExpression(true);
        }
        //Special case (Happy exp for dance and celebrate)
        else if (BodyInt == DANCE_ANIM_STATE_INDEX || BodyInt == CELEBREATE_ANIM_STATE_INDEX)
        {
            ExpressionAnim.SetExpression(HAPPY_EXP_INDEX);
            ExpressionAnim.ShowLoadingExpression(false);
        }
        //Rest BodyAnim use normal exp
        else
        {
            ExpressionAnim.SetExpression(NORMAL_EXP_INDEX);
            ExpressionAnim.ShowLoadingExpression(false);
        }
    }

    private void OnAnimationFinished(string AnimationName)
    {
        if(AnimationName == "PowerUpFinished")
        {
            ExpressionAnim.SetExpression(NORMAL_EXP_INDEX);
            BodyAnim.SetInteger(BODY_ANIM_STATE, NORMAL_EXP_INDEX);
        }
    }

}
                                                                                                                                                                                                                                                                                                                                                                     