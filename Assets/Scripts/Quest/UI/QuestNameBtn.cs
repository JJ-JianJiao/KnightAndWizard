using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestNameBtn : MonoBehaviour
{
    public Text questNameText;
    public QuestData_OS currentData;
    //public Text questContentText;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(UpdateQuestContent);
    }

    void UpdateQuestContent() {
        //questContentText.text = currentData.description;
        QuestUI.Instance.SetupRequireList(currentData);

        foreach (Transform item in QuestUI.Instance.rewardTransform)
        {
            Destroy(item.gameObject);
        }

        foreach (var item in currentData.rewards)
        {
            QuestUI.Instance.SetupRewardItem(item.itemData, item.amount);

        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(QuestUI.Instance.QuestContentTrans.GetComponent<RectTransform>());

        //StartCoroutine("UpdateLayout", QuestUI.Instance.QuestContentTrans.GetComponent<RectTransform>());
    }


    IEnumerator UpdateLayout(RectTransform rect)
    {
        yield return new WaitForEndOfFrame();
        LayoutRebuilder.ForceRebuildLayoutImmediate(rect);
    }


    public void SetupNameBtn(QuestData_OS questData) {
        currentData = questData;
        string questName = questData.questName;
        if (questData.isComplete)
        {
            string temp = "Completed" + questName;
            if (temp.Length > 14)
            {
                questNameText.text = temp.Substring(0, 11) + "...";
            }
            else
            {
                questNameText.text = temp;
            }
        }
        else
        {
            if (questName.Length > 14)
            {
                questNameText.text = questName.Substring(0, 11) + "...";
            }
            else {
                questNameText.text = questName;
            }
        }
    }
}
