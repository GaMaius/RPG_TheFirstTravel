using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class TypeEffect : MonoBehaviour
{
    public GameObject EndCursor;
    public int CPS; // char per seconds
    public bool isTalking;

    Text msgText;
    AudioSource audioSource;
    string targetMsg;

    int index;
    float interval;

    private void Awake()
    {
        msgText = GetComponent<Text>();
        audioSource = GetComponent<AudioSource>();
    }

    public void SetMsg(string msg)
    {
        if (isTalking)
        {
            CancelInvoke();
            msgText.text = targetMsg;
            EffectEnd();
        }
        else
        {
            targetMsg = msg;
            EffectStart();
        }
    }

    // Update is called once per frame
    void EffectStart()
    {
        msgText.text = "";
        index = 0;
        EndCursor.SetActive(false);

        //Start Animation
        interval = 1.0f / CPS;
        // Debug.Log(interval);

        isTalking = true;

        Invoke("Effecting", interval);
    }

    void Effecting()
    {
        // End Animation
        if (msgText.text == targetMsg)
        {
            EffectEnd();
            return;
        }
        msgText.text += targetMsg[index];
     
        // Sound
        if (targetMsg[index] != '.' && targetMsg[index] != ' ')
        {
            // audioSource.Play();
        }
        audioSource.Play();

        index++;

        // Recursive
        Invoke("Effecting", interval);
    }

    void EffectEnd()
    {
        isTalking = false;

        EndCursor.SetActive(true);
    }
}
