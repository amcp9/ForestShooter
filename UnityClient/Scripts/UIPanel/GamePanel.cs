using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Common;

public class GamePanel : BasePanel {
    private Text timeText;
    private int time = -1;
    private bool isNeedGameOver = false;
    private ReturnCode returnCode;
    public Text gameOverText;


    private void Update()
    {
        if(time != -1)
        {
            ShowTimer(time);
            time = -1;
        }
        if(isNeedGameOver)
        {
            OnGameOver(returnCode);
            isNeedGameOver = false;
        }
    }

    private void Start()
    {
        timeText = transform.Find("Timer").GetComponent<Text>();
        gameOverText.gameObject.SetActive(false);
    }


    public void ShowTimerOnMain(int time)
    {
        this.time = time;
    }

    public void ShowTimer(int time)
    {
        timeText.gameObject.SetActive(true);

        timeText.text = time.ToString();
        timeText.transform.localScale = Vector3.one;

        Color temp = timeText.color;
        temp.a = 1;
        timeText.color = temp;

        timeText.transform.DOScale(2, 0.3f).SetDelay(0.3f);
        timeText.DOFade(0, 0.3f).SetDelay(0.3f).OnComplete(() => timeText.gameObject.SetActive(false));
        switch (time)
        {
            case 3:
                facade.PlayNormalSound(AudioManager.Sound_Time_3);
                break;
            case 2:
                facade.PlayNormalSound(AudioManager.Sound_Time_2);
                break;
            case 1:
                facade.PlayNormalSound(AudioManager.Sound_Time_1);
                break;
        }
    }

    private void OnGameOver(ReturnCode code)
    {
        gameOverText.gameObject.SetActive(true);
        gameOverText.transform.localScale = Vector3.zero;
        GameFacade.Instance.GameOver();
        switch (code)
        {
            case ReturnCode.Success:
                gameOverText.text = "You Win";
                GameFacade.Instance.PlayNormalSound(AudioManager.Sound_Win);
                break;
            case ReturnCode.Fail:
                gameOverText.text = "You Lose";
                GameFacade.Instance.PlayNormalSound(AudioManager.Sound_Lose);
                break;
        }
        gameOverText.transform.DOScale(1, 1f);
        Invoke("Restart", 2.5f);
    }


    public void OnGameOverResponse(ReturnCode returnCode)
    {
        switch (returnCode)
        {
            case ReturnCode.Success:
                this.returnCode = returnCode;
                isNeedGameOver = true;
                break;
            case ReturnCode.Fail:
                this.returnCode = returnCode;
                isNeedGameOver = true;
                break;
        }
    }

    private void Restart()
    {
        gameOverText.gameObject.SetActive(false);
        GameFacade.Instance.Restart();
    }


}
