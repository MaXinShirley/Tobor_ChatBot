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

public class ToborAnimator
{
    public Animator toborAnimator;
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

    private string[] chatbotData = new string[] { "tobor_phase1", "tobor_phase2", "tobor_phase3", "tobor_phase4", "tobor_phase5", "tobor_phase6" };

    private const int NORMAL_STATE_INDEX = 0;
    private const int HAPPY_STATE_INDEX = 1;
    private const int SAD_STATE_INDEX = 2;
    private const int HAPPY_BACKNORMAL_STATE_INDEX = 3;
    private const int SAD_BACKNORMAL_STATE_INDEX = 4;
    private const string BODY_ANIM_STATE = "BodyState";


    List<Message> Messages = new List<Message>();

    // Start is called before the first frame update
    void Start()
    {
        phaseInitiated = PlayerPrefs.GetInt("Phase Initiated");
        voiceController = this.gameObject.GetComponent<VoiceController>();
        ARTapToPlaceObject = FindObjectOfType<ARTapToPlaceObject>();

        Debug.Log("phase initiated: " + phaseInitiated);
        Debug.Log("chatbot initiated: " + chatbotData[phaseInitiated-1]);

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
            var txtAsset = (TextAsset)Resources.Load(chatbotData[phaseInitiated-1], typeof(TextAsset));
            var tileFile = txtAsset.text;

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
        /*
        if (Messages.Count >= 2)
        {
            //Remove when too much.
            Destroy(Messages[0].TextObject.gameObject);
            Messages.Remove(Messages[0]);
        }
        */

        var newMessage = new Message { Text = messageText };

        var text = messageType == MessageType.User ? userChatBox.GetComponent<Text>() : toborChatBox.GetComponent<Text>(); 
        text.text = messageText;
        text.color = messageType == MessageType.User ? UserColor : BotColor;
        
        Messages.Add(newMessage);

        //var newText = Instantiate(textObject, chatPanel.transform);

        /*
        if (messageType == MessageType.User)
        {
            var text = userChatBox.GetComponent<Text>();
            text.text = messageText;
            text.color =  UserColor;
            

            Messages.Add(newMessage);
        }
        else
        {
            var text = toborChatBox.GetComponent<Text>();
            text.text = messageText;
            text.color = BotColor;


            Messages.Add(newMessage);
        }
        */

        /*
        newMessage.TextObject = newText.GetComponent<Text>();
        newMessage.TextObject.text = messageText;
        newMessage.TextObject.color = messageType == MessageType.User ? UserColor : BotColor;
        */
        

        //Messages.Add(newMessage);
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

        ExpressionAnim = FindObjectOfType<ExpressionControl>();
        //toborBodyAnimator = FindObjectOfType<Animator>();
        toborBodyAnimator = GameObject.FindGameObjectWithTag("BodyAnimator").GetComponent<Animator>();
    }

    IEnumerator BackToBodyNormal(int seconds, int BodyStateIndex, int ExpressionStateIndex)
    {
        yield return new WaitForSeconds(seconds);
        toborBodyAnimator.SetInteger(BODY_ANIM_STATE, BodyStateIndex);
        ExpressionAnim.SetExpression(ExpressionStateIndex);

    }
}