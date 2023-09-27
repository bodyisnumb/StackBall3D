using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameUI : MonoBehaviour
{
    public GameObject homeUI, inGameUI;
    public GameObject allButtons;

    private bool buttons;

    [Header("PreGame")]
    public Button soundButton;
    public Sprite soundOnS, soundOffS;

    [Header("InGame")]
    public Image levelSlider;
    public Image currentLevelImg;
    public Image nextLevelImg;

    private Material ballMat;
    private Ball ball;

    void Awake()
    {
        ballMat = FindObjectOfType<Ball>().transform.GetChild(0).GetComponent<MeshRenderer>().material;
        ball = FindObjectOfType<Ball>();

        levelSlider.transform.parent.GetComponent<Image>().color = ballMat.color + Color.gray;
        levelSlider.color = ballMat.color;
        currentLevelImg.color = ballMat.color;
        nextLevelImg.color = ballMat.color;

        soundButton.onClick.AddListener(() => SoundManager.instance.SoundOnOff());
    }

    void Update()
    {
        if(ball.ballState == Ball.BallState.Prepare)
        {
            if(SoundManager.instance.sound && soundButton.GetComponent<Image>().sprite != soundOnS)
                soundButton.GetComponent<Image>().sprite = soundOnS;
            else if(!SoundManager.instance.sound && soundButton.GetComponent<Image>().sprite != soundOffS)
                soundButton.GetComponent<Image>().sprite = soundOffS;
        }

        if(Input.GetMouseButtonDown(0) && !IgnoreUI() && ball.ballState == Ball.BallState.Prepare)
        {
            ball.ballState = Ball.BallState.Playing;
            homeUI.SetActive(false);
            inGameUI.SetActive(true);
        }  
    }

    private bool IgnoreUI()
    {
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = Input.mousePosition;

        List<RaycastResult> raycastResultList = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, raycastResultList);
        for (int i = 0; i < raycastResultList.Count; i++)
        {
            if(raycastResultList[i].gameObject.GetComponent<Ignore>() != null)
            {
                raycastResultList.RemoveAt(i);
                i--;
            }
        }

        return raycastResultList.Count > 0;
    }

    public void LevelSliderFill(float fillAmount)
    {
        levelSlider.fillAmount = fillAmount;
    }

    public void Settings()
    {
        buttons = !buttons;
        allButtons.SetActive(buttons);
    }
}
