using TextSpeech;//PinkAK9's solution in Native
using UnityEngine;
using UnityEngine.UI;

public class VoiceController : MonoBehaviour
{
    public ChatManager chatManager;
    const string LANG_CODE = "en-US";
    public GameObject loading;

    public float pitch;
    public float rate;

    void Start()
    {
        //store the chat manager first, it's required to access the chat box later
        chatManager = this.gameObject.GetComponent<ChatManager>();
        //currently it's using the US English
        Setup(LANG_CODE);
        loading.SetActive(false);
        
        
#if UNITY_ANDROID
        SpeechToText.instance.onPartialResultsCallback = OnPartialSpeech;

#endif
        SpeechToText.instance.onResultCallback = OnResultSpeech;
        TextToSpeech.instance.onStartCallBack = OnSpeakStart;
        TextToSpeech.instance.onDoneCallback = OnSpeakStop;
    }
    
    //Text-To-Speech Region, can delete the debug later and replace it when it needs to call another function
    #region Text to Speech
    public void StartSpeaking(string message)
    {
        TextToSpeech.instance.StartSpeak(message);
    }

    public void StopSpeaking()
    {
        TextToSpeech.instance.StopSpeak();
    }

    void OnSpeakStart()
    {
        Debug.Log("Talking started...");
    }

    void OnSpeakStop()
    {
        Debug.Log("Talking stopped");
    }
    #endregion

    //Speech-To-Text Region, can delete the debug later and replace it when it needs to call another function
    #region Speech to Text
    public void StartListening()
    {
#if UNITY_EDITOR
#else
        SpeechToText.instance.StartRecording("Speak any");
#endif
    }

    public void StopListening()
    {
#if UNITY_EDITOR
        OnResultSpeech("Not support in editor.");
#else
        SpeechToText.instance.StopRecording();
#endif
#if UNITY_IOS
        loading.SetActive(false);
#endif
    }

    void OnResultSpeech(string result)
    {
        chatManager.chatBox.text = result;
        chatManager.Send();
#if UNITY_IOS
        loading.SetActive(false);
#endif
    }

    //Only available for Android build
    void OnPartialSpeech(string result)
    {
        chatManager.chatBox.text = result;
        chatManager.Send();
    }
    #endregion

    //To Setup the TTS/STT, may change the code, pitch and rate
    void Setup(string code)
    {
        TextToSpeech.instance.Setting(code, pitch, rate);
        SpeechToText.instance.Setting(code);
    }
}
