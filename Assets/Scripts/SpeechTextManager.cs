using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TextSpeech;
using UnityEngine.UI;
using UnityEngine.Android;
using UnityEngine.Events;

public class SpeechTextManager : MonoBehaviour
{   
    [SerializeField] private string language = "es-ES";
    [SerializeField] private Text uIText;
    [SerializeField] private Text levelText;
    [SerializeField] private Text errorText;

    [Serializable]
    public struct VoiceCommand
    {
        public string keyword;
        public UnityEvent response;
    }

    public VoiceCommand[] voiceCommands;

    private Dictionary<string, UnityEvent> commands = new Dictionary<string, UnityEvent>();


    private void Awake()
    {
#if UNITY_ANDROID
        if (!Permission.HasUserAuthorizedPermission(Permission.Microphone))
        {
            Permission.RequestUserPermission(Permission.Microphone);
        }
#endif

        foreach (VoiceCommand command in voiceCommands)
        {
            commands.Add(command.keyword.ToLower(), command.response);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        TextToSpeech.Instance.Setting(language, 1.0f, 1.0f);
        SpeechToText.Instance.Setting(language);

        SpeechToText.Instance.onResultCallback = OnFinalSpeechResult;
        TextToSpeech.Instance.onStartCallBack = OnSpeakStart;
        TextToSpeech.Instance.onDoneCallback = OnSpeakStop;
#if UNITY_ANDROID
        SpeechToText.Instance.onPartialResultsCallback = OnPartialSpeechResult;
#endif

    }

    public void StartListening()
    {
       SpeechToText.Instance.StartRecording();
    }

      public void StopListening()
    {
       SpeechToText.Instance.StopRecording();
    }


    public void OnFinalSpeechResult(string result)
    {
        uIText.text = result;
        if(result != null){
            if (commands.ContainsKey(result.ToLower()))
            {
                commands[result.ToLower()].Invoke();
            }
        }
    }

    public void OnPartialSpeechResult(string result)
    {   
        uIText.text = result;
    }

    public void StartSpeaking(string message){
        TextToSpeech.Instance.StartSpeak(message);
    }

    public void StartSpeakingLevel(){
        TextToSpeech.Instance.StartSpeak(levelText.text);
    }

    public void StartSpeakingError(){
        TextToSpeech.Instance.StartSpeak(errorText.text);
    }

    public void StopSpeaking(){
        TextToSpeech.Instance.StopSpeak();
    }

    public void OnSpeakStart()
    {
        Debug.Log("OnSpeakStart");
    }

    public void OnSpeakStop()
    {
        Debug.Log("OnSpeakStop");
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
