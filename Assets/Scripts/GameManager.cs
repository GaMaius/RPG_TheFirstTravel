using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public TalkManager talkManager;
    public QuestManager questManager;
    public GameObject scanObject;
    public Animator textPanel;
    public Animator portraitAnim;
    public Image portraitImg;
    public Sprite prevPortrait;
    public TypeEffect talk;
    public int talkIndex;
    public bool isAction=false;

    // Make invisible first
    private void Start()
    {
        Debug.Log(questManager.CheckQuest());
    }
    public void Action(GameObject scanObj)
    { 
        // Get Current Object
        scanObject = scanObj;
        Objdata objdata = scanObject.GetComponent<Objdata>();
        Talk(objdata.id, objdata.isNpc);

        // Visible Talk for Action
        textPanel.SetBool("isShow", isAction);
    }

    void Talk(int id, bool isNpc)
    {
        // Set Talk Data
        int questTalkIndex = questManager.GetQuestTalkIndex(id);
        string talkData = talkManager.GetTalk(id + questTalkIndex, talkIndex);

        // End Talk
        if(talkData == null)
        {
            isAction = false;
            talkIndex = 0;
            Debug.Log(questManager.CheckQuest(id));
            return;
        }

        // Conticue Talk - is it NPC?
        if(isNpc)
        {
            // talkText.text = talkData.Split('=')[0];
            talk.SetMsg(talkData.Split('=')[0]);
            
            // Show Portrait
            portraitImg.sprite = talkManager.GetPortrait(id, int.Parse(talkData.Split('=')[1]));
            portraitImg.color = new Color(1, 1, 1, 1);

            // Animation Portrait
            if (prevPortrait != portraitImg.sprite)
            {
                portraitAnim.SetTrigger("doEffect");
                prevPortrait = portraitImg.sprite;
            }
        }
        else // Show text and the portraitImg make invisible
        {
            // talkText.text = talkData;
            talk.SetMsg(talkData);

            portraitImg.color = new Color(1, 1, 1, 0 );
        }

        isAction = true;
        talkIndex++;
    }
}
