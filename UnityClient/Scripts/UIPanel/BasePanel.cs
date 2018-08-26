using UnityEngine;
using System.Collections;
using DG.Tweening;

public class BasePanel : MonoBehaviour {

    /// <summary>
    /// 每个panel都要继承自该类,控制每个UI面板的实例化
    /// </summary>

    protected UIManager uIManager;
    protected GameFacade facade;

    public GameFacade Facade
    {
        set { facade = value; }
    }
    public UIManager UIManager
    {
        set { uIManager = value; }
    }

    protected void PlayClickSound()
    {
        facade.PlayNormalSound(AudioManager.Sound_ButtonClick);
    }

    /// <summary>
    /// 界面被显示出来
    /// </summary>
    public virtual void OnEnter()
    {
        transform.localScale = Vector3.zero;
        transform.gameObject.SetActive(true);
        transform.DOScale(1, 0.2f);

    }

    /// <summary>
    /// 界面暂停
    /// </summary>
    public virtual void OnPause()
    {

    }

    /// <summary>
    /// 界面继续
    /// </summary>
    public virtual void OnResume()
    {

    }

    /// <summary>
    /// 界面不显示,退出这个界面，界面被关系
    /// </summary>
    public virtual void OnExit()
    {
        transform.DOScale(0,0.2f).OnComplete(()=>transform.gameObject.SetActive(false));
        
    }
}
