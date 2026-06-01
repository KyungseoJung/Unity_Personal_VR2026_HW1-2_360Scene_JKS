using System.Collections;
using UnityEngine;
using UniVRM10;
using System;   // #HW4 //#4 


// #HW4 //#4 

public class DatingSceneControllerHW4 : MonoBehaviour
{
    // #HW4 //#4  -----------------------------------
    [Serializable]
    public class GirlActionData
    {
        public string voice;
        public string action;
        public string situation;

        public GirlActionData(string voice, string action, string situation)
        {
            this.voice = voice;
            this.action = action;
            this.situation = situation;
        }
    }

    [Serializable]
    /*
        voice      = Boy가 말할 문장
        action     = Boy가 수행할 몸 동작
        expression = Boy의 표정
        emotion    = Boy의 감정 상태
    */
    public class BoyResponseData
    {
        public string voice;
        public string action;
        public string expression;
        public string emotion;

        public BoyResponseData(string voice, string action, string expression, string emotion)
        {
            this.voice = voice;
            this.action = action;
            this.expression = expression;
            this.emotion = emotion;
        }
    }
    // -----------------------------------

    [Header("Animators")]
    public Animator girlAnimator;
    public Animator boyAnimator;

    [Header("VRM Instances")]
    public Vrm10Instance girlVrm;
    public Vrm10Instance boyVrm;

    [Header("Audio Sources")]
    public AudioSource girlAudioSource;
    public AudioSource boyAudioSource;

    [Header("Girl Audio Clips")]
    public AudioClip niceToMeetYouGirl;
    public AudioClip helloGirl;
    public AudioClip goodByeGirl;
    public AudioClip goodJokeGirl;
    public AudioClip badJokeGirl;

    [Header("Boy Audio Clips")]
    public AudioClip niceToMeetYouBoy;
    public AudioClip helloBoy;
    public AudioClip byeByeBoy;
    public AudioClip laughBoy;
    public AudioClip ohNoBoy;

    [Header("Animator Trigger Names")]
    public string bowTrigger = "Bow";
    public string talkTrigger = "Talk";
    public string laughTrigger = "Laugh";
    public string angryTrigger = "Angry";
    public string waveTrigger = "WaveHand";

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            StartCoroutine(NiceToMeetYouSequence());

        if (Input.GetKeyDown(KeyCode.Alpha2))
            StartCoroutine(HelloSequence());

        if (Input.GetKeyDown(KeyCode.Alpha3))
            StartCoroutine(GoodByeSequence());

        if (Input.GetKeyDown(KeyCode.Alpha4))
            StartCoroutine(GoodJokeSequence());

        if (Input.GetKeyDown(KeyCode.Alpha5))
            StartCoroutine(BadJokeSequence());
    }

    IEnumerator NiceToMeetYouSequence()
    {
        ClearAllExpressions();

        GirlActionData girlData = new GirlActionData(   // #HW4 //#4
            "Nice to meet you.",
            "Bow",
            "FirstMeeting"
        );
        DebugGirlActionData(girlData);


        girlAnimator.SetTrigger(bowTrigger);
        SetHappy(girlVrm, 0.5f);
        PlayGirl(niceToMeetYouGirl);

        yield return new WaitForSeconds(1.4f);

        ClearExpression(girlVrm);

        // boyAnimator.SetTrigger(bowTrigger);
        // SetHappy(boyVrm, 0.5f);
        // PlayBoy(niceToMeetYouBoy);
        // #HW4 //#4 위 3줄을 아래 1줄로 변경-> 크게 2개 코드로 변경
        BoyResponseData boyResponse = new BoyResponseData(
            "Nice to meet you too.",
            "Bow",
            "Happy",
            "Positive"
        );
        ApplyBoyResponse(boyResponse, niceToMeetYouBoy);
        
        
        yield return new WaitForSeconds(1.4f);

        ClearExpression(boyVrm);
    }

    IEnumerator HelloSequence()
    {
        ClearAllExpressions();

        GirlActionData girlData = new GirlActionData(   // #HW4 //#4
            "Hello.",
            "WaveHand",
            "Greeting"
        );
        DebugGirlActionData(girlData);


        girlAnimator.SetTrigger(talkTrigger);
        girlAnimator.SetTrigger(waveTrigger);
        SetHappy(girlVrm, 0.6f);
        SetMouthA(girlVrm, 0.8f);
        PlayGirl(helloGirl);

        yield return new WaitForSeconds(1.2f);

        ClearExpression(girlVrm);

        // boyAnimator.SetTrigger(talkTrigger);
        // boyAnimator.SetTrigger(waveTrigger);
        // SetHappy(boyVrm, 0.6f);
        // SetMouthA(boyVrm, 0.8f);
        // PlayBoy(helloBoy);
        // #HW4 //#4 위 5줄을 아래 1줄로 변경-> 크게 2개 코드로 변경
        // ApplyBoyResponse("WaveHand", "HappyTalk", helloBoy);
        BoyResponseData boyResponse = new BoyResponseData(
            "Hello.",
            "WaveHand",
            "HappyTalk",
            "Positive"
        );
        ApplyBoyResponse(boyResponse, helloBoy);


        yield return new WaitForSeconds(1.2f);

        ClearExpression(boyVrm);
    }

    IEnumerator GoodByeSequence()
    {
        ClearAllExpressions();

        GirlActionData girlData = new GirlActionData(   // #HW4 //#4
            "Goodbye.",
            "WaveHand",
            "Farewell"
        );
        DebugGirlActionData(girlData);


        girlAnimator.SetTrigger(talkTrigger);
        girlAnimator.SetTrigger(waveTrigger);
        SetHappy(girlVrm, 0.4f);
        SetMouthA(girlVrm, 0.8f);
        PlayGirl(goodByeGirl);

        yield return new WaitForSeconds(1.2f);

        ClearExpression(girlVrm);

        // boyAnimator.SetTrigger(talkTrigger);
        // boyAnimator.SetTrigger(waveTrigger);
        // SetHappy(boyVrm, 0.4f);
        // SetMouthA(boyVrm, 0.8f);
        // PlayBoy(byeByeBoy);
        // #HW4 //#4 위 5줄을 아래 1줄로 변경 -> 크게 2개 코드로 변경
        // ApplyBoyResponse("WaveHand", "HappyTalk", byeByeBoy);
        BoyResponseData boyResponse = new BoyResponseData(
            "Goodbye.",
            "WaveHand",
            "HappyTalk",
            "Neutral"
        );
        ApplyBoyResponse(boyResponse, byeByeBoy);



        yield return new WaitForSeconds(1.2f);

        ClearExpression(boyVrm);
    }

    IEnumerator GoodJokeSequence()
    {
        ClearAllExpressions();

        GirlActionData girlData = new GirlActionData(   // #HW4 //#4
            "I told a funny joke.",
            "Talk",
            "GoodJoke"
        );
        DebugGirlActionData(girlData);


        girlAnimator.SetTrigger(talkTrigger);
        SetHappy(girlVrm, 0.5f);
        SetMouthA(girlVrm, 0.9f);
        PlayGirl(goodJokeGirl);

        yield return new WaitForSeconds(4.0f);

        ClearExpression(girlVrm);   // 이전에 적용했던 얼굴 표정을 초기화(0으로 리셋)

        // boyAnimator.SetTrigger(laughTrigger);
        // SetHappy(boyVrm, 1.0f);
        // PlayBoy(laughBoy);
        // #HW4 //#4 위 3줄을 아래 1줄로 변경-> 크게 2개 코드로 변경
        // ApplyBoyResponse("Laugh", "Happy", laughBoy);
        BoyResponseData boyResponse = new BoyResponseData(
            "That was funny!",
            "Laugh",
            "Happy",
            "Positive"
        );
        ApplyBoyResponse(boyResponse, laughBoy);


        yield return new WaitForSeconds(1.8f);

        ClearExpression(boyVrm);
    }

    IEnumerator BadJokeSequence()
    {
        ClearAllExpressions();

        GirlActionData girlData = new GirlActionData(   // #HW4 //#4
            "I told an awkward joke.",
            "Talk",
            "BadJoke"
        );
        DebugGirlActionData(girlData);


        girlAnimator.SetTrigger(talkTrigger);
        SetHappy(girlVrm, 0.3f);
        SetMouthA(girlVrm, 0.9f);
        PlayGirl(badJokeGirl);

        yield return new WaitForSeconds(4.0f);

        ClearExpression(girlVrm);   // 이전에 적용했던 얼굴 표정을 초기화(0으로 리셋)

        // boyAnimator.SetTrigger(angryTrigger);
        // SetAngry(boyVrm, 0.9f);
        // PlayBoy(ohNoBoy);
        // #HW4 //#4 위 5줄을 아래 1줄로 변경   
        // ApplyBoyResponse("Frown", "Angry", ohNoBoy);-> 크게 2개 코드로 변경
        BoyResponseData boyResponse = new BoyResponseData(
            "That was awkward.",
            "Frown",
            "Angry",
            "Negative"
        );
        ApplyBoyResponse(boyResponse, ohNoBoy);


        yield return new WaitForSeconds(1.8f);

        ClearExpression(boyVrm);
    }

    // #HW4 //#4 나중에 AI서버가 쉽게 동작을 수행하도록 할 수 있도록 함수 추가
    void ApplyBoyResponse(BoyResponseData response, AudioClip voiceClip)
    {
        DebugBoyResponseData(response);

        string action = response.action;
        string expression = response.expression;

        // 1. Boy animation
        if (boyAnimator != null)
        {
            if (action == "Bow")
            {
                boyAnimator.SetTrigger(bowTrigger);
            }
            else if (action == "WaveHand")
            {
                boyAnimator.SetTrigger(talkTrigger);
                boyAnimator.SetTrigger(waveTrigger);
            }
            else if (action == "Talk")
            {
                boyAnimator.SetTrigger(talkTrigger);
            }
            else if (action == "Laugh")
            {
                boyAnimator.SetTrigger(laughTrigger);
            }
            else if (action == "Frown")
            {
                boyAnimator.SetTrigger(angryTrigger);
            }
            else if (action == "Angry")
            {
                boyAnimator.SetTrigger(angryTrigger);
            }
        }

        // 2. Boy facial expression
        if (expression == "Happy")
        {
            SetHappy(boyVrm, 0.8f);
        }
        else if (expression == "Angry")
        {
            SetAngry(boyVrm, 0.9f);
        }
        else if (expression == "Talk")
        {
            SetMouthA(boyVrm, 0.8f);
        }
        else if (expression == "HappyTalk")
        {
            SetHappy(boyVrm, 0.6f);
            SetMouthA(boyVrm, 0.8f);
        }
        else if (expression == "Neutral")
        {
            // Do nothing
        }

        // 3. Boy voice
        PlayBoy(voiceClip);
    }

    
    void DebugGirlActionData(GirlActionData data)   // #HW4 //#4 나중에 AI서버가 쉽게 동작을 수행하도록 할 수 있도록 함수 추가
    {
        Debug.Log(
            "[GirlActionData] " +
            "voice=" + data.voice +
            ", action=" + data.action +
            ", situation=" + data.situation
        );
    }

    void DebugBoyResponseData(BoyResponseData data) // #HW4 //#4 나중에 AI서버가 쉽게 동작을 수행하도록 할 수 있도록 함수 추가
    {
        Debug.Log(
            "[BoyResponseData] " +
            "voice=" + data.voice +
            ", action=" + data.action +
            ", expression=" + data.expression +
            ", emotion=" + data.emotion
        );
    }

    void PlayGirl(AudioClip clip)
    {
        if (girlAudioSource != null && clip != null)
            girlAudioSource.PlayOneShot(clip);
    }

    void PlayBoy(AudioClip clip)
    {
        if (boyAudioSource != null && clip != null)
            boyAudioSource.PlayOneShot(clip);
    }

    void SetHappy(Vrm10Instance vrm, float weight)
    {
        SetExpression(vrm, ExpressionPreset.happy, weight);
    }

    void SetAngry(Vrm10Instance vrm, float weight)
    {
        SetExpression(vrm, ExpressionPreset.angry, weight);
    }

    void SetMouthA(Vrm10Instance vrm, float weight)
    {
        SetExpression(vrm, ExpressionPreset.aa, weight);
    }

    void SetExpression(Vrm10Instance vrm, ExpressionPreset preset, float weight)
    {
        if (vrm == null) return;

        var key = ExpressionKey.CreateFromPreset(preset);
        vrm.Runtime.Expression.SetWeight(key, weight);
    }

    void ClearAllExpressions()
    {
        ClearExpression(girlVrm);
        ClearExpression(boyVrm);
    }

    void ClearExpression(Vrm10Instance vrm) //이전에 적용했던 얼굴 표정을 초기화(0으로 리셋)
    {
        if (vrm == null) return;

        SetExpression(vrm, ExpressionPreset.happy, 0f);
        SetExpression(vrm, ExpressionPreset.angry, 0f);
        SetExpression(vrm, ExpressionPreset.sad, 0f);
        SetExpression(vrm, ExpressionPreset.blink, 0f);

        SetExpression(vrm, ExpressionPreset.aa, 0f);
        SetExpression(vrm, ExpressionPreset.ih, 0f);
        SetExpression(vrm, ExpressionPreset.ou, 0f);
        SetExpression(vrm, ExpressionPreset.ee, 0f);
        SetExpression(vrm, ExpressionPreset.oh, 0f);
    }
}