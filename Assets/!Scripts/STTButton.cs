using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.Android;//To call the Mic permission

public class STTButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public VoiceController speechToText;
    public GameObject effect;
    public float speedEffect = 1;
    public float scaleEffect = 1.2f;
    float speed;
    float scale = 1;

    void Start()
    {
        effect.SetActive(false);
        speed = speedEffect;

        CheckPermission();
    }

    void Update()
    {
        if (effect.activeSelf)
        {
            scale += Time.deltaTime * speed;
            if (scale > scaleEffect)
            {
                speed = -speedEffect;
            }
            if (scale < scaleEffect - 0.1f)
            {
                speed = speedEffect;
            }
            effect.transform.localScale = new Vector3(scale, scale, 1);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        effect.SetActive(true);
        scale = 1;
        speechToText.StartListening();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        effect.SetActive(false);
        speechToText.StopListening();
    }

    //To check the microphone permission in Android starting for Phase 3 onwards
    void CheckPermission()
    {
#if UNITY_ANDROID
        if (!Permission.HasUserAuthorizedPermission(Permission.Microphone))
        {
            Permission.RequestUserPermission(Permission.Microphone);
        }
#endif
    }
}
