using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public TalkManager talkManager;
    public QuestManager questManager;
    public GameObject menuSet;
    public GameObject scanObject;
    public GameObject player;
    public Animator textPanel;
    public Animator portraitAnim;
    public Image portraitImg;
    public Sprite prevPortrait;
    public TypeEffect talk;
    public Text questText;
    public int talkIndex;
    public bool isAction=false;
    public bool activeMenu=false;

    // Make invisible first
    private void Start()
    {
        // GameLoading
        GameLoad();

        // Quest Update
        questText.text = questManager.CheckQuest();
    }

    private void Update()
    {
        // Sub Menu
        if (Input.GetButtonDown("Cancel"))
        {
            activeMenu = !activeMenu;
            menuSet.SetActive(activeMenu);
            
        }
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
        int questTalkIndex = 0;
        string talkData = "";

        // Set Talk Data
        if (talk.isTalking)
        {
            talk.SetMsg("");
            return;
        }
        else
        {
            questTalkIndex = questManager.GetQuestTalkIndex(id);
            talkData = talkManager.GetTalk(id + questTalkIndex, talkIndex);
        }

        // End Talk
        if(talkData == null)
        {
            isAction = false;
            talkIndex = 0;
            // Quest Update
            questText.text = questManager.CheckQuest(id);
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

    public void GameSave()
    {
        if (!PlayerPrefs.HasKey("PlayerX"))
        {
            return;
        }

        PlayerPrefs.SetFloat("PlayerX", player.transform.position.x);
        PlayerPrefs.SetFloat("PlayerY", player.transform.position.y);
        PlayerPrefs.SetInt("QuestId", questManager.questId);
        PlayerPrefs.SetInt("QuestActionIndex", questManager.questActionIndex);
        PlayerPrefs.Save();

        menuSet.SetActive(false);

        // player.x and y
        // Quest Id
        // Quest Action Index 저장
    }

    public void GameLoad()
    {
        float x = PlayerPrefs.GetFloat("PlayerX");
        float y = PlayerPrefs.GetFloat("PlayerY");
        int questId = PlayerPrefs.GetInt("QuestId");
        int questActionIndex = PlayerPrefs.GetInt("QuestActionIndex");

        player.transform.position = new Vector3(x, y, 0);
        questManager.questId = questId;
        questManager.questActionIndex = questActionIndex;

        questManager.ControlObject();
    }

    public void GameExit()
    {
        Application.Quit();
    }
}
