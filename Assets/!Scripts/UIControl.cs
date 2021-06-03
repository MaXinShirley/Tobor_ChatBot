using UnityEngine;
using UnityEngine.UI;

public class UIControl : MonoBehaviour
{
    public GameObject selFacialIcon;
    public GameObject selBodyIcon;
    public Image[] bodyAndFacialTitle;
    public Sprite[] bodyAndFacialTitleSelect;
    public GameObject[] animationList;

    public Button[] ExpButtons;
    public Button[] BodyButtons;

    public GameObject[] ExpStateUIs;
    private GameObject ExpCurUI;

    public GameObject[] BodyStateUIs;
    private GameObject BodyCurUI;

    public int status;

    public void OnExpAnimSelected(int ExpIndex)
    {
        if (ExpIndex >= ExpStateUIs.Length) return;

        SetExpState(ExpIndex);
        Debug.Log(ExpIndex);
        Events.OnSelectExpression(ExpIndex);
    }

    public void OnBodyAnimSelected(int BodyIndex)
    {
        if (BodyIndex >= BodyStateUIs.Length) return;

        SetBodyState(BodyIndex);
        Debug.Log(BodyIndex);
        Events.OnSelectAnimation(BodyIndex);
    }

    private void SetExpState(int index)
    {
        //If there is a current expression highlight UI, turn it off
        if (ExpCurUI != null) ExpCurUI.SetActive(false);
        //Register the new expression highlight UI
        ExpCurUI = ExpStateUIs[index]; //
        //Turn on the new expression highlight UI
        ExpCurUI.SetActive(true);
    }

    private void SetBodyState(int index)
    {
        //If there is a current body highlist UI, turn it off
        if (BodyCurUI != null) BodyCurUI.SetActive(false);
        //Register the new body highlist UI
        BodyCurUI = BodyStateUIs[index]; //
        //Turn on the new body highlist UI
        BodyCurUI.SetActive(true);
    }
    public void ClickExpAndBodyUI()
    {
        int whenBodyIconClicked = 0;
        int whenFacialIconClicked = 1;

        if (status == whenBodyIconClicked)
        {
            bodyAndFacialTitle[1].sprite = bodyAndFacialTitleSelect[0];
            bodyAndFacialTitle[0].sprite = bodyAndFacialTitleSelect[1];
            animationList[0].SetActive(false);
            animationList[1].SetActive(true);
            status = whenFacialIconClicked;
        }
        else
        {
            bodyAndFacialTitle[0].sprite = bodyAndFacialTitleSelect[0];
            bodyAndFacialTitle[1].sprite = bodyAndFacialTitleSelect[1];
            animationList[0].SetActive(true);
            animationList[1].SetActive(false);
            status = whenBodyIconClicked;
        }
    }

}
