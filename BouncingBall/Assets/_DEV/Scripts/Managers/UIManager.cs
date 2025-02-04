using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("UI")] 
    public GameObject pause_panel;
    public GameObject TopHalf_GO;
    public GameObject BottomHalf_GO;
    public GameObject crashPanel_GO;
    public GameObject streak_GO;
    public GameObject highestStreak_GO;
    public GameObject restart_GO;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            
        }
    }
    public void customDoTweenAnimation(Transform objectToMove, float ValueToMove, float duration, Ease easeAnimation)
    {
        objectToMove.DOMoveX(ValueToMove, duration).SetEase(easeAnimation).SetRelative(true);
    }
    
    public void OpenPanel(GameObject panelToOpen)
    {
        panelToOpen.SetActive(true);
       
        panelToOpen.GetComponent<CanvasGroup>().alpha = 0;
        panelToOpen.GetComponent<RectTransform>().transform.localPosition = new Vector3(0f,-1000f,0f);
        panelToOpen.GetComponent<RectTransform>().DOAnchorPos(new Vector2(0f, 0f), 1f, false).SetEase(Ease.OutElastic);
        panelToOpen.GetComponent<CanvasGroup>().DOFade(1, 1);
    }

    public void ClosePanel(GameObject panelToClose)
    {
       
        panelToClose.GetComponent<CanvasGroup>().alpha = 1;
        panelToClose.GetComponent<RectTransform>().transform.localPosition = new Vector3(0f,0f,0f);
        panelToClose.GetComponent<RectTransform>().DOAnchorPos(new Vector2(0f, 0f), 1f, false).SetEase(Ease.InOutQuint);
        panelToClose.GetComponent<CanvasGroup>().DOFade(0, 0.5f).OnComplete((delegate
        {
            panelToClose.SetActive(false);
        }));
       
       
    }
}