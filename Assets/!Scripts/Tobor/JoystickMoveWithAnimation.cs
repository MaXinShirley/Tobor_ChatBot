using UnityEngine;

public class JoystickMoveWithAnimation : MonoBehaviour
{
    public FixedJoystick moveJoystick;
    public FixedJoystick heightJoystick;
    public float xMove;
    public float yMove;
    public float zMove;
    public float speed = 2;

    public Animator anim;

    private float _verticalMov;
    private float _horizontalMov;

    private ToborControl _toborControl;
    private const int BODY_STATE_IDLE = 12;

    /// <summary>
    /// 2 Joysticks that controls 3d model move frontback, leftright and updown
    /// </summary>
    /// <param name="moveJs"></param>
    /// <param name="heighJs"></param>
    public void InitializeJoystick(FixedJoystick moveJs, FixedJoystick heighJs)
    {
        moveJoystick = moveJs;
        heightJoystick = heighJs;
    }

    private void Awake()
    {
        _toborControl = FindObjectOfType<ToborControl>();
    }


    void Update()
    {
        MoveVerWithAnimation();
        JoystickMove();
    }

    private void MoveVerWithAnimation()
    {
        _verticalMov = zMove;
        _horizontalMov = xMove;

        if (_verticalMov != 0 || _horizontalMov != 0)
        {
            anim.SetInteger("StateBody", BODY_STATE_IDLE);
        }
        else if (_verticalMov == 0 && _horizontalMov == 0)
        {
            //Try clean up (dont go back to tobor Control)
            anim.SetInteger("StateBody", _toborControl.BodyInt);
        }

        anim.SetFloat("verticalForward", _verticalMov);
        anim.SetFloat("horizontalLeft", _horizontalMov);
    }

    private void JoystickMove()
    {
        if (moveJoystick != null)
        {
            //Joystick for move left right, front back
            xMove = moveJoystick.Horizontal;
            zMove = moveJoystick.Vertical;

            transform.localPosition += transform.forward.normalized * zMove * speed * Time.deltaTime;
            transform.localPosition += transform.right.normalized * xMove * speed * Time.deltaTime;
        }

        if (heightJoystick != null)
        {
            //Joystick for up down
            yMove = heightJoystick.Vertical;
            transform.localPosition += new Vector3(0f, yMove, 0f) * speed * Time.deltaTime;
        }
    }
}