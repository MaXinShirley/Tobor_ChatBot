using UnityEngine;

public class JoystickMove : MonoBehaviour
{
    public FixedJoystick moveJoystick;
    public FixedJoystick heightJoystick;
    public float xMove;
    public float yMove;
    public float zMove;
    public float speed = 2;

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

    void Update()
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