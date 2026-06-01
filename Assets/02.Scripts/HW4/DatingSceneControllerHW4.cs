using System.Collections;
using UnityEngine;
using UniVRM10;

// #HW4 //#4 

public class DatingSceneControllerHW4 : MonoBehaviour
{
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

        girlAnimator.SetTrigger(bowTrigger);
        SetHappy(girlVrm, 0.5f);
        PlayGirl(niceToMeetYouGirl);

        yield return new WaitForSeconds(1.4f);

        ClearExpression(girlVrm);

        // boyAnimator.SetTrigger(bowTrigger);
        // SetHappy(boyVrm, 0.5f);
        // PlayBoy(niceToMeetYouBoy);
        // #HW4 //#4 위 3줄을 아래 1줄로 변경
        ApplyBoyResponse("Bow", "Happy", niceToMeetYouBoy);

        yield return new WaitForSeconds(1.4f);

        ClearExpression(boyVrm);
    }

    IEnumerator HelloSequence()
    {
        ClearAllExpressions();

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
        // #HW4 //#4 위 5줄을 아래 1줄로 변경
        ApplyBoyResponse("WaveHand", "HappyTalk", helloBoy);

        yield return new WaitForSeconds(1.2f);

        ClearExpression(boyVrm);
    }

    IEnumerator GoodByeSequence()
    {
        ClearAllExpressions();

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
        // #HW4 //#4 위 5줄을 아래 1줄로 변경
        ApplyBoyResponse("WaveHand", "HappyTalk", byeByeBoy);

        yield return new WaitForSeconds(1.2f);

        ClearExpression(boyVrm);
    }

    IEnumerator GoodJokeSequence()
    {
        ClearAllExpressions();

        girlAnimator.SetTrigger(talkTrigger);
        SetHappy(girlVrm, 0.5f);
        SetMouthA(girlVrm, 0.9f);
        PlayGirl(goodJokeGirl);

        yield return new WaitForSeconds(4.0f);

        ClearExpression(girlVrm);   // 이전에 적용했던 얼굴 표정을 초기화(0으로 리셋)

        // boyAnimator.SetTrigger(laughTrigger);
        // SetHappy(boyVrm, 1.0f);
        // PlayBoy(laughBoy);
        // #HW4 //#4 위 3줄을 아래 1줄로 변경
        ApplyBoyResponse("Laugh", "Happy", laughBoy);


        yield return new WaitForSeconds(1.8f);

        ClearExpression(boyVrm);
    }

    IEnumerator BadJokeSequence()
    {
        ClearAllExpressions();

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
        ApplyBoyResponse("Frown", "Angry", ohNoBoy);

        yield return new WaitForSeconds(1.8f);

        ClearExpression(boyVrm);
    }

    // #HW4 //#4 나중에 AI서버가 쉽게 동작을 수행하도록 할 수 있도록 함수 추가
    void ApplyBoyResponse(string action, string expression, AudioClip voiceClip)
    {
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