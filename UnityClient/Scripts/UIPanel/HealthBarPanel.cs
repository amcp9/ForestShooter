using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HealthBarPanel : BasePanel 
{
    public RectTransform blue, red;

    public Image blueBar;
    public Image blueRampageBar;
    public Image redBar;
    public Image redRampageBar;
    public Text blueText;
    public Text redText;
    public Text VSText;

    public Vector3 bluePos, redPos, VSPos;
    private bool isNeedBlueChange = false;
    private bool isNeedRedChange = false;
    private bool isNeedBlueRamp = false;
    private bool isNeedRedRamp = false;

    private int tempBlueCurrent = -1;
    private int tempRedCurrent = -1;
    private int tempBlueMax = -1;
    private int tempRedMax = -1;

    private int currentBlueRamp;
    private int currentRedRamp;
    private int MaxRampage = 100;


    private void Update()
    {
        if(isNeedBlueChange)
        {
            ChangeBlueHeathBar(tempBlueCurrent, tempBlueMax);
            isNeedBlueChange = false;
        }
        if(isNeedRedChange)
        {
            ChangeRedHeathBar(tempRedCurrent, tempRedMax);
            isNeedRedChange = false;
        }
        if (isNeedBlueRamp)
        {
            ChangeBlueRamp(currentBlueRamp, MaxRampage);
            isNeedBlueRamp = false;
        }
        if (isNeedRedRamp)
        {
            ChangeRedRamp(currentRedRamp, MaxRampage);
            isNeedRedRamp = false;
        }
    }

    private void Awake()
    {
        bluePos = blue.transform.localPosition;
        redPos = red.transform.localPosition;
        VSPos = VSText.transform.localPosition;
    }

    private void Start()
    {
        blue.transform.localPosition = new Vector3(bluePos.x - 1000, bluePos.y, 0f);
        red.transform.localPosition = new Vector3(redPos.x + 1000, redPos.y, 0f);
        VSText.transform.localPosition = new Vector3(VSPos.x, VSPos.y + 500, 0f);
    }

    public override void OnEnter()
    {
        blue.transform.DOLocalMoveX(bluePos.x, 0.5f);
        red.transform.DOLocalMoveX(redPos.x, 0.5f);
        VSText.transform.DOLocalMoveY(VSPos.y, 0.5f);
        blueBar.fillAmount = 1;
        redBar.fillAmount = 1;
        blueRampageBar.fillAmount = 0;
        redRampageBar.fillAmount = 0;
    }

    public override void OnExit()
    {
        blue.transform.DOLocalMoveX(bluePos.x - 1000, 0.5f);
        red.transform.DOLocalMoveX(redPos.x + 1000, 0.5f);
        VSText.transform.DOLocalMoveY(VSPos.y + 500, 0.5f);
    }

    public void SetUserNameText(string b,string r)
    {
        blueText.text = b;
        redText.text = r;
    }

    private void ChangeBlueHeathBar(int current,int max)
    {
        blueBar.fillAmount = (float)current /(float) max;
    }

    private void ChangeRedHeathBar(int current, int max)
    {
        redBar.fillAmount = (float)current / (float)max;
    }

    private void ChangeBlueRamp(int current, int max)
    {
        blueRampageBar.fillAmount = (float)current / (float)max;
    }

    private void ChangeRedRamp(int current, int max)
    {
        redRampageBar.fillAmount = (float)current / (float)max;
    }

    public void ChangeBlueHeathBarOnMain(int current, int max)
    {
        tempBlueCurrent = current;
        tempBlueMax = max;
        isNeedBlueChange = true;
    }

    public void ChangeRedHeathBarOnMain(int current, int max)
    {
        tempRedCurrent = current;
        tempRedMax = max;
        isNeedRedChange = true;
    }

    public void ChangeBlueRampageOnMain(int current)
    {
        currentBlueRamp = current;
        isNeedBlueRamp = true;
    }

    public void ChangeRedRampageOnMain(int current)
    {
        currentRedRamp = current;
        isNeedRedRamp = true;
    }
}
