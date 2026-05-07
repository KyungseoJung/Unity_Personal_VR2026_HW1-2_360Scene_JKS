using System.Collections;
using UnityEngine;
using UniVRM10;

public class DatingSceneController : MonoBehaviour
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

        boyAnimator.SetTrigger(bowTrigger);
        SetHappy(boyVrm, 0.5f);
        PlayBoy(niceToMeetYouBoy);

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

        boyAnimator.SetTrigger(talkTrigger);
        boyAnimator.SetTrigger(waveTrigger);
        SetHappy(boyVrm, 0.6f);
        SetMouthA(boyVrm, 0.8f);
        PlayBoy(helloBoy);

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

        boyAnimator.SetTrigger(talkTrigger);
        boyAnimator.SetTrigger(waveTrigger);
        SetHappy(boyVrm, 0.4f);
        SetMouthA(boyVrm, 0.8f);
        PlayBoy(byeByeBoy);

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

        boyAnimator.SetTrigger(laughTrigger);
        SetHappy(boyVrm, 1.0f);
        PlayBoy(laughBoy);

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

        boyAnimator.SetTrigger(angryTrigger);
        SetAngry(boyVrm, 0.9f);
        PlayBoy(ohNoBoy);

        yield return new WaitForSeconds(1.8f);

        ClearExpression(boyVrm);
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