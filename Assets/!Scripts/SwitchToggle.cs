
using UnityEngine;
using UnityEngine.UI;

public class SwitchToggle : MonoBehaviour
{
    [SerializeField] RectTransform uiHandleRectTransform;
    [SerializeField] Color backgroundActiveColor;

    public Image backgroundImage, handleImage;
    public Toggle toggle;

    public Image BGimage;
    public Camera mainCam;
    public GameObject arSession;
    public GameObject tobor;
    public Canvas canvas;

    private bool status;
    private ARTapToPlaceObject ARTapToPlaceObject;
    Color backgroundDefaultColor;
    Vector2 handlePosition;

    void Awake()
    {

        handlePosition = uiHandleRectTransform.anchoredPosition;

        toggle.onValueChanged.AddListener(OnSwitch);
        backgroundDefaultColor = backgroundImage.color;
        /*

                if (toggle.isOn)
                    OnSwitch(true);*/
    }

    private void Start()
    {
        ARTapToPlaceObject = FindObjectOfType<ARTapToPlaceObject>();
    }

    void Update()
    {


        if (toggle.isOn == false)
        {
            //Turn On BG Image
            BGimage.GetComponent<Image>().enabled = false;
            //Turn off AR Cam
            arSession.SetActive(true);
            //Turn on Main Cam
            mainCam.gameObject.SetActive(false);
            //Turn On Model
            tobor.SetActive(false);
            //Turn Off AR Model
            ARTapToPlaceObject.objectToPlace.SetActive(true);
            //Set canvas render camera to MainCam
            canvas.worldCamera = null;

        }

        if (toggle.isOn == true)
        {
            //Turn On BG Image
            BGimage.GetComponent<Image>().enabled = true;
            //Turn off AR Cam
            arSession.SetActive(false);
            //Turn on Main Cam
            mainCam.gameObject.SetActive(true);
            //Turn On Model
            tobor.SetActive(true);
            //Turn Off AR Model
            ARTapToPlaceObject.objectToPlace.SetActive(false);
            //Set canvas render camera to MainCam
            canvas.worldCamera = mainCam;
        }

        Debug.Log(toggle.isOn);
    }

    void OnSwitch(bool on)
    {
        uiHandleRectTransform.anchoredPosition = on ? handlePosition * -1 : handlePosition;

        backgroundImage.color = on ? backgroundActiveColor : backgroundDefaultColor;

        status = on;
    }

    void OnDestroy()
    {
        toggle.onValueChanged.RemoveListener(OnSwitch);
    }
}