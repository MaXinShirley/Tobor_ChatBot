
using UnityEngine;
using UnityEngine.UI;

public class SwitchToggle : MonoBehaviour
{
    [SerializeField] RectTransform uiHandleRectTransform;
    [SerializeField] Color backgroundActiveColor;

    //UI for toggle button
    public Image backgroundImage, handleImage;
    public Toggle toggle;
    
    public Image BGimage;
    public Camera mainCam;
    public GameObject arSession;
    public GameObject tobor;
    public GameObject toborControlUI;
    public GameObject chatUI;
    public Canvas canvas;

    private ARTapToPlaceObject ARTapToPlaceObject;
    Color backgroundDefaultColor;
    Vector2 handlePosition;

    void Awake()
    {

        handlePosition = uiHandleRectTransform.anchoredPosition;

        toggle.onValueChanged.AddListener(OnSwitch);
        backgroundDefaultColor = backgroundImage.color;

    }

    private void Start()
    {
        ARTapToPlaceObject = FindObjectOfType<ARTapToPlaceObject>();
    }

    void OnSwitch(bool on)
    {
        uiHandleRectTransform.anchoredPosition = on ? handlePosition * -1 : handlePosition;
        backgroundImage.color = on ? backgroundActiveColor : backgroundDefaultColor;
        
        //Disable AR when toggle button status is on
        BGimage.GetComponent<Image>().enabled = on;
        arSession.SetActive(!on);
        mainCam.gameObject.SetActive(on);
        tobor.SetActive(on);
        ARTapToPlaceObject.objectToPlace.SetActive(!on);
        canvas.worldCamera = on ? mainCam : null;
        toborControlUI.SetActive(!on);
        chatUI.SetActive(on);

    }

    void OnDestroy()
    {
        toggle.onValueChanged.RemoveListener(OnSwitch);
    }
}