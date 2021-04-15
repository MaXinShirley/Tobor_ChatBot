using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    private Animator phaseOneAnimator;
    private ExpressionControl toborExpressionControl;
    private const int NORMAL_STATE_INDEX = 0;

    // Start is called before the first frame update
    void Start()
    {
        phaseOneAnimator = this.gameObject.GetComponent<Animator>();
    }

    //Called in Animation Controller Event for the emoticon
    //Set all the parameters back to false
    public void BackToIdle()
    {
        phaseOneAnimator.SetBool("Happy", false);
        phaseOneAnimator.SetBool("Confuse", false);

    }


}
