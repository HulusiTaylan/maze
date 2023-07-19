using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public Transform playButton;
    public Transform Logo;
    public Transform ButtonGroup;
    public Transform levelSelectPanel;
    public levelSelectBT levelBtPrefab;
    public Transform content;
    public ScrollRect levelScroll;
    public Text moneytxt;
    void Start()
    {
        if (!PlayerPrefs.HasKey("totalMoney"))
            PlayerPrefs.SetInt("totalMoney", 9999);
        moneytxt.text = PlayerPrefs.GetInt("totalMoney").ToString() + "$";
        
        
        if (PlayerPrefs.HasKey("LevelLock0") == false) // hafýzada bu parametre var mý 
        {
            PlayerPrefs.SetInt("LevelLock0", 1); //parametreyi 1 e sabitledik. kilit açtýk 
        }

        //level butonlarý oluþtur
        bool isBuyBt = false;
        for (int i = 0; i < 10; i++)
        {
            levelSelectBT btClone = Instantiate(levelBtPrefab); // prefab klonlama
            btClone.transform.SetParent(content); //parent atamasý yapýldý.
            btClone.levelID.text = (i + 1).ToString(); //klonlanan butonun levelýd textini deðiþtirdik.
            int levelID = i;
            
            if (PlayerPrefs.GetInt("LevelLock"+levelID)==0) //seviye klitli
            {
                if (!isBuyBt)
                {
                    isBuyBt = true;
                    btClone.buyBt.gameObject.SetActive(true);
                }
                else
                {
                    btClone.buyBt.gameObject.SetActive(false);
                }
                
                //btClone.buyBt.gameObject.SetActive(true); //satýn al butonu kilitli bölümlerde gözükecek.
                btClone.GetComponent<CanvasGroup>().alpha = 0.5f;
                btClone.GetComponent<Button>().interactable = false; // butonun týklanabilme özelliði kapalý

            }
            else //kilitli deðil
            {
                btClone.buyBt.gameObject.SetActive(false);//satýn al butonu kilidi açýk seviyelerde gözükmeyecek.
                btClone.GetComponent<CanvasGroup>().alpha = 1f;
                btClone.GetComponent<Button>().interactable = true; // butonun týklanabilme özelliði açýk
            }
            btClone.GetComponent<Button>().onClick.AddListener(() => {
                Debug.Log((levelID+1) + ".Bölüme týklandý");
            
            });
        }
        
        levelScroll.horizontalNormalizedPosition = 0f; //scroll u baþtan baþlatmak için
        
        
        
        levelSelectPanel.gameObject.SetActive(false);
        levelSelectPanel.localScale = Vector3.zero;

        Logo.DOLocalMoveY(356f, 1f).OnComplete(()=> {
            Logo.DORotate(new Vector3(0, 0, 4), 1f).SetLoops(-1, LoopType.Yoyo);
        }).SetDelay(.5f);
        playButton.DOScale(1.1f, 1f).SetLoops(-1, LoopType.Yoyo);
        ButtonGroup.DOLocalMoveY(-224f, 1f).SetEase(Ease.OutBounce).SetDelay(.5f);
    }
    public void playButtonClick()
    {
        levelSelectPanel.gameObject.SetActive(true);
        levelSelectPanel.DOScale(Vector3.one, 0.7f).SetEase(Ease.OutBounce).SetUpdate(true); //time scale 0 olsa bile bu dotween çalýþacak. diðerleri çalýþmaz setupdate true yapmak gerekiyor.
    }
    
    public void selectPanelBack()
    {
        levelSelectPanel.DOScale(Vector3.zero, .7f).SetEase(Ease.OutBounce).OnComplete(()=> {
            levelSelectPanel.gameObject.SetActive(false);
        });
    }
    void Update()
    {
        
    }

}
