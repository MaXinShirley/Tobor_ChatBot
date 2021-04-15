
using UnityEngine;
using UnityEngine.UI;

public class SwitchToggle : MonoBehaviour
{
    [SerializeField] RectTransform uiHandleRectTransform;
    [SerializeField] Color backgroundActiveColor;

    public Image backgroundImage, handleImage;
    public Toggle toggle;
    private bool status;
    Color backgroundDefaultColor;
    Vector2 handlePosition;

    void Awake()
    {

        handlePosition = uiHandleRectTransform.anchoredPosition;

        toggle.onValueChanged.AddListener(OnSwitch);
        backgroundDefaultColor = backgroundImage.color;


        if (toggle.isOn)
            OnSwitch(true);
    }

    void Update()
    {
        if (status == true)
        {
            toggle.isOn = true;
        }else if(status == false)
        {
            toggle.isOn = false;
        }
    }

    void OnSwitch(bool on)
    {
        uiHandleRectTransform.anchoredPosition = on ? handlePosition * -1 : handlePosition ;

        backgroundImage.color = on ? backgroundActiveColor : backgroundDefaultColor;

        status = on;
    }

    void OnDestroy()
    {
        toggle.onValueChanged.RemoveListener(OnSwitch);
    }
}