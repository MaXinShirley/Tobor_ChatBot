using Syn.Bot.Oscova;
//using Syn.Bot.Oscova.Attributes;
using Syn.Workspace;
using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.Windows.Speech;

/*
public class Message
{
    public string Text;
    public Text TextObject;
    public MessageType MessageType;
    public GameObject BotBG;
    public GameObject UserBG;
}
*/

public enum MessageType
{
    User, Bot
}

/*
public class BotDialog : Dialog
{
    private KeywordRecognizer keywordRecognizer;
    private Dictionary<string, Action> actions = new Dictionary<string, Action>();

    void Start()
    {
        //actions.Add("Hello", HelloAnswers(0));
        //actions.Add("Goodbye", Goodbye);

        keywordRecognizer = new KeywordRecognizer(actions.Keys.ToArray());
        keywordRecognizer.OnPhraseRecognized += ReconizedSpeech;
    }

    private void ReconizedSpeech(PhraseRecognizedEventArgs speech)
    {
        Debug.Log(speech.text);
        actions[speech.text].Invoke();
    }


    private List<string> DefaultAnswers = new List<string>() { "Can you please rephrase that for me?", "Sorry, I don`t understand", "Ha??", "What does it mean?" };
    /// <summary>
    /// This function will create a response for default dialog
    /// Anything that no answer cant be recognized will send to this
    /// </summary>
    /// <param name="context"></param>
    /// <param name="result"></param>
    [Fallback]
    public void DefaultFallback(Context context, Result result)
    {
        float random = UnityEngine.Random.Range(0f, 1f);

        if (random <= 0.25f)
            result.SendResponse(DefaultAnswers[0]);
        if (0.25f < random && random <= 0.5f)
            result.SendResponse(DefaultAnswers[1]);
        if (0.5f < random && random <= 0.75f)
            result.SendResponse(DefaultAnswers[2]);
        if (0.75f < random)
            result.SendResponse(DefaultAnswers[3]);

        //result.SendResponse("Can you please rephrase that for me?");
    }

    /// <summary>
    /// 
    /// </summary>
    private List<string> HelloAnswersRam = new List<string>() { "Hello User!", "Hi! How are you?", "What`s up!", "Nice to see you again User!" };

    [Expression("Hello Bot")]
    [Expression("Hello Tobor")]
    public void Hello(Context context, Result result)
    {
        float random = UnityEngine.Random.Range(0f, 1f);
        /-*int index = 0;
        float percent = 1f / answers.Count;
        index = Mathf.FloorToInt(1f / percent);
        Debug.Log(Mathf.FloorToInt(1f / percent));
        result.SendResponse(answers[index]);*-/
        if (random <= 0.25f)
            result.SendResponse(HelloAnswersRam[0]);
        if (0.25f < random && random <= 0.5f)
            result.SendResponse(HelloAnswersRam[1]);
        if (0.5f < random && random <= 0.75f)
            result.SendResponse(HelloAnswersRam[2]);
        if (0.75f < random)
            result.SendResponse(HelloAnswersRam[3]);
            


        //result.SendResponse("Hello User!");
    }

    [Expression("My name")]
    public void MyName(Context context, Result result)
    {
        result.SendResponse("Shirley");
    }

    [Expression("Your name")]
    public void YourName(Context context, Result result)
    {
        result.SendResponse("My name is Tobor");
    }

    [Expression("Name")]
    public void Name(Context context, Result result)
    {
        result.SendResponse("Who`s name you are asking?");
    }

    [Expression("Age")]
    public void Age(Context context, Result result)
    {
        result.SendResponse("I am 24 years old.");
    }

    [Expression("How old are you?")]
    public void HowOld(Context context, Result result)
    {
        result.SendResponse("I am 24 years old.");
    }

    [Expression("Bye")]
    public void Bye(Context context, Result result)
    {
        result.SendResponse("Nice talking to you, goodbye!");
    }

    [Expression("Goodbye")]
    public void Goodbye(Context context, Result result)
    {
        result.SendResponse("Nice talking to you, goodbye!");
    }
}
*/

public class GameManager : MonoBehaviour
{
    OscovaBot MainBot;

    public GameObject chatPanel, textObject;
    public InputField chatBox;

    public Color UserColor, BotColor;

    List<Message> Messages = new List<Message>();

    // Start is called before the first frame update
    void Start()
    {
        try
        {
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
            var txtAsset = (TextAsset)Resources.Load("tobor_phase_1_v_0.1", typeof(TextAsset));
            var tileFile = txtAsset.text;
            Debug.Log(txtAsset);
            Debug.Log(tileFile);

            var workspace = new WorkspaceGraph();
            workspace.LoadFromString(tileFile);
            
            //Import the workspace.
            MainBot.ImportWorkspace(workspace);
            MainBot.Trainer.StartTraining();

            //When the bot generates a response simply display it.
            MainBot.MainUser.ResponseReceived += (sender, evt) =>
            {
                //StartCoroutine(Response(evt.Response.Text));
                Debug.Log(evt.Response.Messages);
                AddMessage($"Bot: {evt.Response.Messages}", MessageType.Bot);
            };

            /*Note: Originally from Ma Xin
            MainBot.Recognizers.EntityRecognized += Recognizers_EntityRecognized;
            MainBot.Recognizers.EntitiesExtracted += Recognizers_EntitiesExtracted;
                
            MainBot.Dialogs.Add(new BotDialog());
            //MainBot.ImportWorkspace("Assets/bot.json");//original Assets/SimpleText.west
            MainBot.Trainer.StartTraining();

            MainBot.MainUser.ResponseReceived += (sender, evt) =>
            {
                StartCoroutine(Response(evt.Response.Text));
                StartCoroutine(ShowText());
                Debug.Log(evt.Response.Text);
                //AddMessage($"Bot: {evt.Response.Text}", MessageType.Bot);
            };
            */

        }
        catch (Exception ex)
        {
            Debug.LogError(ex);
        }
    
    }


    private void Recognizers_EntitiesExtracted(object sender, Syn.Bot.Oscova.Events.EntitiesExtractedEventArgs e)
    {
        Debug.Log("Recognizers_EntitiesExtracted");
        Debug.Log(e.Entities.Count);

        if (e.Entities.Count == 0)
        {
            //AddMessage($"Bot: Invalid Input", MessageType.Bot);
        }
    }

    private void Recognizers_EntityRecognized(object sender, Syn.Bot.Oscova.Events.EntityRecognizedEventArgs e)
    {
        Debug.Log("Recognizers_EntityRecognized");
        
    }

    IEnumerator Response(string reply)
    {
        yield return new WaitForSeconds(2f);
        
        AddMessage($"Bot: {reply}", MessageType.Bot);
    }

    /// <summary>
    /// Somethjinf ghere
    /// </summary>
    /// <param name="messageText"></param>
    /// <param name="messageType"></param>
    public void AddMessage(string messageText, MessageType messageType)
    {
        if (Messages.Count >= 25)
        {
            //Remove when too much.
            Destroy(Messages[0].TextObject.gameObject);
            Messages.Remove(Messages[0]);
        }

        var newMessage = new Message { Text = messageText };

        var newText = Instantiate(textObject, chatPanel.transform);

        newMessage.TextObject = newText.GetComponent<Text>();
        newMessage.TextObject.text = messageText;
        /*
        newUser = UserColor;
        newUser.a = 255;
        UserColor = newUser;

        newBot = BotColor;
        newBot.a = 255;
        BotColor = newBot;
        */
        newMessage.TextObject.color = messageType == MessageType.User ? UserColor : BotColor;

        Messages.Add(newMessage);
    }

    public void SendMessageToBot()
    {
        var userMessage = chatBox.text;

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

            chatBox.Select();
            chatBox.text = "";
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SendMessageToBot();
            
        }
    }
}
