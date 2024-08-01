using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScript07 : MonoBehaviour
{
    public GameObject[] questionGroupArr;
    public QAClass07[] qaArr;
    public GameObject AnwerPanel;
    void Start()
    {
        qaArr = new QAClass07[questionGroupArr.Length];
    }

    
    void Update()
    {

    }
    public void SubmitAnswer()
    {
        for (int i = 0; i < qaArr.Length; i++)
       {
           qaArr[i] = ReadQuestionAndAnswer(questionGroupArr[i]);
        }
    }
     QAClass07 ReadQuestionAndAnswer(GameObject questionGroup)
    {
        QAClass07 result = new QAClass07();
        GameObject q = questionGroup.transform.Find("Question").gameObject;
        GameObject a = questionGroup.transform.Find("Answer").gameObject;

        result.Question = q.GetComponent<Text>().text;

        if (a.GetComponent<InputField>() != null)
        {
           result.Answer = a.transform.Find("Text").GetComponent<Text>().text;
        }
        else if (a.GetComponent<InputField>()== null)
        {
            string s = "";
            int counter = 0;

            for (int i = 0; i < a.transform.childCount; i++)
            {
                if (a.transform.GetChild(i).GetComponent<Toggle>().isOn)
                {
                    if (counter != 0)
                    {
                        s = s + ",";
                    }
                    s = s + a.transform.GetChild(i).Find("Label").GetComponent<Text>().text;
                    counter++;

                }
                if (i == a.transform.childCount - 1)
                {
                    s = s + ".";
                }
            }
            result.Answer = s;

        }
        return result;
    }

    [System.Serializable]
    public class QAClass07
    {
        public string Question = "";
        public string Answer = "";
    }
}


