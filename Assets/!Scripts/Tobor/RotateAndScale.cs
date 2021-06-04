using UnityEngine;


/// <summary>
/// Touch to scale and rotate the model
/// </summary>
public class RotateAndScale : MonoBehaviour
{
    private Quaternion rotationY;
    private Touch touch;

    public float rotateSpeedModifier = 0.3f;
    private float previousTouchPos;
    private float currentTouchPos;
    private float initialDistance;
    private Vector3 initialScale;

    private const float MIN_X = 0;
    private const float MAX_X = 900;
    private const float MIN_Y = 100;
    private const float MAX_Y = 2000;

    void Update()
    {
        //1 finger touch to rotate
        previousTouchPos = currentTouchPos;

        if (Input.touchCount == 1)
        {
            touch = Input.GetTouch(0);
            //Set a touchable area (does not allow user to touch on other UI part)
            if (touch.phase == TouchPhase.Moved && touch.position.x >= MIN_X && touch.position.x <= MAX_X && touch.position.y >= MIN_Y && touch.position.y <= MAX_Y)
            {
                currentTouchPos = Input.GetTouch(0).position.x;
                float distance = currentTouchPos - previousTouchPos;
                if (distance > 0)
                {
                    rotationY = Quaternion.Euler(
                        0f,
                        -Input.GetTouch(0).position.x * rotateSpeedModifier * Time.deltaTime,
                        0f);

                    transform.rotation = Quaternion.Lerp(transform.rotation, rotationY * transform.rotation, rotateSpeedModifier * Time.deltaTime);
                }
                else
                {
                    rotationY = Quaternion.Euler(
                        0f,
                        Input.GetTouch(0).position.x * rotateSpeedModifier * Time.deltaTime,
                        0f);

                    transform.rotation = Quaternion.Lerp(transform.rotation, rotationY * transform.rotation, rotateSpeedModifier * Time.deltaTime);
                }
            }
        }

        //2 finger pinch to scale
        if (Input.touchCount == 2)
        {
            var touchZero = Input.GetTouch(0);
            var touchOne = Input.GetTouch(1);

            if (touchZero.phase == TouchPhase.Ended || touchZero.phase == TouchPhase.Canceled ||
                touchOne.phase == TouchPhase.Ended || touchOne.phase == TouchPhase.Canceled)
            {
                return;
            }

            if (touchZero.phase == TouchPhase.Began || touchOne.phase == TouchPhase.Began)
            {
                initialDistance = Vector2.Distance(touchZero.position, touchOne.position);
                initialScale = transform.localScale;
            }
            else
            {
                var currentDistance = Vector2.Distance(touchZero.position, touchOne.position);
                if (Mathf.Approximately(initialDistance, 0))
                {
                    return;
                }
                var factor = currentDistance / initialDistance;

                transform.localScale = initialScale * factor;
            }
        }

    }
}
