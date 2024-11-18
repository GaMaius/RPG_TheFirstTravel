using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public int questId;
    public int questActionIndex;
    public GameObject[] questObject;

    Dictionary<int, QuestData> questList;

    private void Awake()
    {
        questList = new Dictionary<int, QuestData>();
        GenerateData();
    }
    
    void GenerateData()
    {
        //퀘스트 제목과와 해당 퀘스트에 연관된 NPC들의 ID 입력. 뒤 NPC ID 순서가 퀘스트 진행 순서.
        questList.Add(10, new QuestData("마을 사람들과 대화하기.", 
                                        new int[] { 1000, 2000 }));
        questList.Add(20, new QuestData("루도의 동전 찾아주기.",
                                        new int[] { 5000, 1000 })); // 2000 -> 1000 (24.11.18. 5pm)
        questList.Add(30, new QuestData("퀘스트 클리어!",
                                        new int[] { 0 }));
    }

    public int GetQuestTalkIndex(int id) //NPC ID를 받아 퀘스트 번호 반환
    {
        // 퀘스트 번호 + 퀘스트 대화 순서 = 퀘스트 대화 ID
        return questId + questActionIndex;
    }

    public string CheckQuest(int id)
    {
        // Next Talk Target
        if (id == questList[questId].npcId[questActionIndex])
        {
            questActionIndex++;
        }

        // Control Quest Object
        ControlObject();

        // Talk Complete & Next Quest
        if (questActionIndex == questList[questId].npcId.Length)
        {
            NextQuest();
        }

        // Quest Name
        return questList[questId].questName;
    }

    public string CheckQuest()
    {
        // Quest Name
        return questList[questId].questName;
    }

    // CheckQuest �Լ��� 2�� �ӿ��� �Ű������� ���̷� ����
    // CheckQuest(id)�� CheckQuest()�� ���� �ٸ��� ���ǵǰ� ȣ��ȴ�.
    // �̷��� �Ű������� ���� �Լ��� ȣ���� �Ǵ°��� "�����ε�(Overloading)"�̶�� �Ѵ�.

    void NextQuest()
    {
        questId += 10;
        questActionIndex = 0;
    }

    void ControlObject()
    {
        switch (questId)
        {
            case 10:
                if (questActionIndex == 2)
                {
                    questObject[0].SetActive(true);
                }
                break;

            case 20:
                if (questActionIndex == 1)
                {
                    questObject[0].SetActive(false);
                }
                break;
        }
    }
}
