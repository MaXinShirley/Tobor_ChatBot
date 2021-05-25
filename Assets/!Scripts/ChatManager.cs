using System;
using System.Collections;
using System.Collections.Generic;
using Syn.Bot.Oscova;
using Syn.Bot.Oscova.Attributes;
using Syn.Bot.Oscova.Messages;
using Syn.Workspace;
using UnityEngine;
using UnityEngine.UI;

public class Message
{
    public string Text;
    public Text TextObject;
    public MessageType MessageType;
}

public class BotDialog : Dialog
{
    private List<string> defaultAnswers = new List<string>() { "Would you please rephrase that for me?", "Sorry, I don`t understand", "Huh?",
        "What does it mean?", "I can't find it in my database", "I beg your pardon",
        "I didn't catch what you're trying to say"};
    /// <summary>
    /// This function will create a response for default dialog
    /// Unrecognized intents will be replied with this fallback
    /// </summary>
    /// <param name="context"></param>
    /// <param name="result"></param>
    [Fallback]
    public void DefaultFallback(Context context, Result result)
    {
        int random = UnityEngine.Random.Range(0, defaultAnswers.Count);
        Debug.Log(random);
        
        result.SendResponse(defaultAnswers[random]);
    }
}

public class ChatManager : MonoBehaviour
{
    OscovaBot MainBot;
    VoiceController voiceController;
    public GameObject chatPanel, textObject;
    public GameObject userChatBox, toborChatBox;
    public InputField chatBox;
    public Animator toborAnimator;
    public Color UserColor, BotColor;

    public int phaseInitiated;
    private ExpressionControl ExpressionAnim;
    private Animator toborBodyAnimator;
    private ARTapToPlaceObject ARTapToPlaceObject;

    //Import JSON link from cloud
    private string[] chatbotURL = new string[] { "https://s3.ap-southeast-1.amazonaws.com/eon.project.sg/Tobor/tobor_phase1.json",
                                                "https://s3.ap-southeast-1.amazonaws.com/eon.project.sg/Tobor/tobor_phase2.json",
                                                "https://s3.ap-southeast-1.amazonaws.com/eon.project.sg/Tobor/tobor_phase3.json",
                                                "https://s3.ap-southeast-1.amazonaws.com/eon.project.sg/Tobor/tobor_phase4.json",
                                                "https://s3.ap-southeast-1.amazonaws.com/eon.project.sg/Tobor/tobor_phase5.json",
                                                "https://s3.ap-southeast-1.amazonaws.com/eon.project.sg/Tobor/tobor_phase5.json"};

    //Animation State Constant 
    private const int NORMAL_STATE_INDEX = 0;
    private const int HAPPY_STATE_INDEX = 1;
    private const int SAD_STATE_INDEX = 2;
    private const int HAPPY_BACKNORMAL_STATE_INDEX = 3;
    private const int SAD_BACKNORMAL_STATE_INDEX = 4;
    private const string BODY_ANIM_STATE = "BodyState";


    List<Message> Messages = new List<Message>();
  
    private SwitchToggle switchToggle;

    void Awake()
    {
        switchToggle = FindObjectOfType<SwitchToggle>();
    }

    IEnumerator Start()
    {
        phaseInitiated = PlayerPrefs.GetInt("Phase Initiated");
        voiceController = this.gameObject.GetComponent<VoiceController>();
        ARTapToPlaceObject = FindObjectOfType<ARTapToPlaceObject>();

        Debug.Log("phase initiated: " + phaseInitiated);


        //Download JSON from URL according to phraseInitiated number
        WWW www = new WWW(chatbotURL[phaseInitiated - 1]);
        yield return www;
        if (www.error == null)
        {
            Debug.Log("chatbot initiated: " + chatbotURL[phaseInitiated-1]);
            Debug.Log("Get JSON: " + www.text);
                    
        }
        else
        {
            Debug.Log("ERROR: " + www.error);
        }

        try
        {
            //Create new instance of bot.
            MainBot = new OscovaBot();
            
            OscovaBot.Logger.LogReceived += (s, o) =>
            {
                Debug.Log($"OscovaBot: {o.Log}");
            };

            //Import bot's knowledge-base from an Oryzer Workspace project file.
            //To read the content of this file ensure you've got Oryzer installed. Visit Oryzer.com

            //First read the knowledge.json file and create a workspace.
            /*Note: Oryzer usually saves file in .West file extensions so the file was intentionally
            renamed to knowledge.json so Unity stores it as a resource. */

            //Get downloaded JSON text
            var tileFile = www.text;
            var workspace = new WorkspaceGraph();
            workspace.LoadFromString(tileFile);

            //Import the workspace.
            MainBot.ImportWorkspace(workspace);
            MainBot.Dialogs.Add(new BotDialog());
            MainBot.Trainer.StartTraining();

            Debug.Log("Load rescource JSON: " + tileFile);

             //When the bot generates a response simply display it.
             MainBot.MainUser.ResponseReceived += (sender, evt) =>
            {
                if (evt.Response.Messages.Count == 0)
                {
                    Debug.Log(evt.Response.Text);
                    AddMessage($"Bot: {evt.Response.Text}", MessageType.Bot);
                    /*if(phaseInitiated >= 3)
                    {
                        voiceController.StartSpeaking(evt.Response.Text);
                    }*/
                }
                else
                {
                    foreach (var message in evt.Response.Messages)
                    {
                        if (message is TextMessage textMessage)
                        {
                            var textValue = textMessage.GetRandomText();
                            AddMessage($"Bot: {textValue}", MessageType.Bot);
                           /* if (phaseInitiated >= 3)
                            {
                                voiceController.StartSpeaking(textValue);
                            }*/
                        }
                    }
                }
            };
        }
        catch (Exception ex)
        {
            Debug.LogError(ex);
        }

    }

    public void AddMessage(string messageText, MessageType messageType)
    {
        var newMessage = new Message { Text = messageText };

        var text = messageType == MessageType.User ? userChatBox.GetComponent<Text>() : toborChatBox.GetComponent<Text>(); 
        text.text = messageText;
        text.color = messageType == MessageType.User ? UserColor : BotColor;
        
        Messages.Add(newMessage);
    }

    //Function to send the message to the OSCOVA bot
    //It will then review all the intents in the loaded .west/.json workspace
    //Once it found the nearest match, it will send the response
    public void SendMessageToBot(string inputField)
    {
        var userMessage = inputField;

        if (!string.IsNullOrEmpty(userMessage))
        {
            Debug.Log($"OscovaBot:[USER] {userMessage}");
            AddMessage($"User: {userMessage}", MessageType.User);

            //Create a request for bot to process.
            var request = MainBot.MainUser.CreateRequest(userMessage);

            //Evaluate the request (Compute NLU - Natural Language Understanding)
            var evaluationResult = MainBot.Evaluate(request);

            //Invoke the best suggested intent found. This is compel a response generation.
            evaluationResult.Invoke();

            //Get the intent name to change the Tobor expression
            IntentResult intentResult = evaluationResult.SuggestedIntent;
            string intentName = intentResult.Name.ToString();
            Debug.Log("Intent Name: " + intentName);


            //Phrase1: Image Blink animation

            //Phrase2: Image Blink animation

            //Phrase3: Head with expression animation 
            // - Hide image blink
            // - Active Head and use SetExpression in Expression Control
            // - "Happy", "Normal", "Sad"

            //Phrase4: Full Body with expression animation
            // - Active Body
            // - Still using SetExpression in Expression Control
            // - "Happy", "Normal", "Sad" ...

            //Phrase5: Full Body with expression animation + body animation
            // - Use body animator for body animation



            // if intentname contain "sad", show sad, contain "happy" show happy(check dialogs name) eg: happy_dialog_ep1.happy
            if (phaseInitiated >= 3)
            {
                ExpressionAnim = FindObjectOfType<ExpressionControl>();
                //toborBodyAnimator = FindObjectOfType<Animator>();
                toborBodyAnimator = GameObject.FindGameObjectWithTag("BodyAnimator").GetComponent<Animator>();
            }

            

            switch (phaseInitiated)
                {
                    case 1:
                        if (intentName == "BotDialog.DefaultFallback") toborAnimator.SetBool("Confuse", true);
                        else toborAnimator.SetBool("Happy", true);
                        break;
                    case 2:
                        if (intentName == "BotDialog.DefaultFallback") toborAnimator.SetBool("Confuse", true);
                        else toborAnimator.SetBool("Happy", true);
                        break;
                    case 3:

                        if (intentName == "BotDialog.DefaultFallback")
                        {
                            ExpressionAnim.SetExpression(SAD_STATE_INDEX);

                            StartCoroutine(BackToBodyNormal(3, NORMAL_STATE_INDEX, NORMAL_STATE_INDEX));
                        }
                        else if (intentName.Contains("happy"))
                        {
                            ExpressionAnim.SetExpression(HAPPY_STATE_INDEX);

                            StartCoroutine(BackToBodyNormal(3, NORMAL_STATE_INDEX, NORMAL_STATE_INDEX));
                        }
                        else
                        {
                            ExpressionAnim.SetExpression(NORMAL_STATE_INDEX);
                        }
                        break;

                    case 4:
                        if (intentName == "BotDialog.DefaultFallback")
                        {
                            ExpressionAnim.SetExpression(SAD_STATE_INDEX);

                            StartCoroutine(BackToBodyNormal(3, NORMAL_STATE_INDEX, NORMAL_STATE_INDEX));
                        }
                        else if (intentName.Contains("happy"))
                        {
                            ExpressionAnim.SetExpression(HAPPY_STATE_INDEX);

                            StartCoroutine(BackToBodyNormal(3, NORMAL_STATE_INDEX, NORMAL_STATE_INDEX));
                        }
                        else
                        {
                            ExpressionAnim.SetExpression(NORMAL_STATE_INDEX);
                        }
                        break;
                    case 5:
                        if (intentName == "BotDialog.DefaultFallback")
                        {
                            ExpressionAnim.SetExpression(SAD_STATE_INDEX);
                            toborBodyAnimator.SetInteger(BODY_ANIM_STATE, SAD_STATE_INDEX);

                            StartCoroutine(BackToBodyNormal(3, SAD_BACKNORMAL_STATE_INDEX, NORMAL_STATE_INDEX));
                        }
                        else if (intentName.Contains("happy"))
                        {
                            ExpressionAnim.SetExpression(HAPPY_STATE_INDEX);
                            toborBodyAnimator.SetInteger(BODY_ANIM_STATE, HAPPY_STATE_INDEX);

                            StartCoroutine(BackToBodyNormal(3, HAPPY_BACKNORMAL_STATE_INDEX, NORMAL_STATE_INDEX));
                        }
                        else
                        {
                            ExpressionAnim.SetExpression(NORMAL_STATE_INDEX);
                            toborBodyAnimator.SetInteger(BODY_ANIM_STATE, NORMAL_STATE_INDEX);
                        }
                        break;

                    case 6:
                        if (intentName == "BotDialog.DefaultFallback")
                        {
                            ExpressionAnim.SetExpression(SAD_STATE_INDEX);
                            toborBodyAnimator.SetInteger(BODY_ANIM_STATE, SAD_STATE_INDEX);

                            StartCoroutine(BackToBodyNormal(3, SAD_BACKNORMAL_STATE_INDEX, NORMAL_STATE_INDEX));
                        }
                        else if (intentName.Contains("happy"))
                        {
                            ExpressionAnim.SetExpression(HAPPY_STATE_INDEX);
                            toborBodyAnimator.SetInteger(BODY_ANIM_STATE, HAPPY_STATE_INDEX);

                            StartCoroutine(BackToBodyNormal(3, HAPPY_BACKNORMAL_STATE_INDEX, NORMAL_STATE_INDEX));
                        }
                        else
                        {
                            ExpressionAnim.SetExpression(NORMAL_STATE_INDEX);
                            toborBodyAnimator.SetInteger(BODY_ANIM_STATE, NORMAL_STATE_INDEX);
                        }
                        break;

                }



                //chatBox.Select();
                chatBox.text = "";
          //  }
        }
    }

    //Public function to send the message to the oscova bot
    public void Send()
    {
        SendMessageToBot(chatBox.text);
    }

    //Just for the input test, may delete later
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            //Process user message on enter press.
            SendMessageToBot(chatBox.text);
        }


        //When AR Toggle button is on, turn on the AR scene
        if (!switchToggle.toggle.isOn)
        {
            EnableARTapToPlaceObj(true);
        }
        else
        {
            EnableARTapToPlaceObj(false);
        }
    }

    IEnumerator BackToBodyNormal(int seconds, int BodyStateIndex, int ExpressionStateIndex)
    {
        yield return new WaitForSeconds(seconds);
        toborBodyAnimator.SetInteger(BODY_ANIM_STATE, BodyStateIndex);
        ExpressionAnim.SetExpression(ExpressionStateIndex);

    }

    public void EnableARTapToPlaceObj(bool isEnabled) => ARTapToPlaceObject.enabled = isEnabled;
}