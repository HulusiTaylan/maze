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
    public int money;
    public List<levelSelectBT> allSelectBt = new List<levelSelectBT>();
    public CanvasGroup bgImg;
    public CanvasGroup characterPanel;
    public List<CharactersClass> characterList;
    void Start()
    {
        characterPanel.gameObject.SetActive(false);
        characterPanel.alpha = 0;

        if (!PlayerPrefs.HasKey("totalMoney"))
            PlayerPrefs.SetInt("totalMoney",10000);
        money = PlayerPrefs.GetInt("totalMoney");
        moneytxt.text = money.ToString() + "$";
        


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
            int levelID = i;  // text olarak tanýmlanan dan farklý yerel bir deðiþken 
            allSelectBt.Add(btClone);

            if (PlayerPrefs.GetInt("LevelLock"+levelID)==0) //seviye klitli
            {
                if (!isBuyBt)
                {

                    isBuyBt = true;
                    btClone.buyBt.gameObject.SetActive(true);
                    btClone.buyBt.onClick.AddListener(() => { //addlistener click eklememize yarýyor. kodla eriþim saðladýk 
                        Debug.Log(levelID+". Buy butonuna basýldý."); 
                        //satýn alma iþlem kontrol.
                        if(money>= 1000) //hafýzadaki para 1000'den büyük veya eþit mi
                        {
                            //satýn alma iþlemý gerçekleþecek 
                            money -= 1000;
                            PlayerPrefs.SetInt("totalMoney", money);
                            moneytxt.text = money.ToString() + "$";
                            moneytxt.transform.DOScale(new Vector3(1.1f, 1.1f), .5f).OnComplete(() => {
                                moneytxt.transform.DOScale(new Vector3(1f, 1f), .5f).SetDelay(.2f);
                            });
                            //butonu aktifleþtirme iþlemi.
                            allSelectBt[levelID].GetComponent<CanvasGroup>().alpha = 1f; //1. levelin butonu gibi bir tanýmlama yaptýk liste ile
                            allSelectBt[levelID].buyBt.gameObject.SetActive(false);
                            allSelectBt[levelID].GetComponent<Button>().interactable = true;
                            PlayerPrefs.SetInt("LevelLock" + levelID, 1); //hafýzadaki deðerde de kilit açýk
                            //buton kaydýrma iþlemi;
                            if (levelID < 9)
                            {
                                allSelectBt[levelID + 1].buyBt.gameObject.SetActive(true); // levelýd+1 derken listede iþlemi yaptýðýmýz butondan 1 sonraki demek.
                                allSelectBt[levelID + 1].buyBt.onClick.AddListener(() => {
                                    if (money >= 1000)// hafýzadaki para 1000den büyük mü eþit mi
                                    {
                                        money -= 1000;
                                        PlayerPrefs.SetInt("totalMoney", money);
                                        moneytxt.text = money.ToString() + "$";
                                        moneytxt.transform.DOScale(new Vector3(1.1f, 1.1f), .5f).OnComplete(() => {
                                            moneytxt.transform.DOScale(new Vector3(1f, 1f), .5f).SetDelay(.2f);
                                        });
                                        //butonu aktifleþtirme iþlemi.
                                        allSelectBt[levelID+1].GetComponent<CanvasGroup>().alpha = 1f; //1. levelin butonu gibi bir tanýmlama yaptýk liste ile
                                        allSelectBt[levelID+1].buyBt.gameObject.SetActive(false);
                                        allSelectBt[levelID+1].GetComponent<Button>().interactable = true;
                                        PlayerPrefs.SetInt("LevelLock" + (levelID+1), 1); //hafýzadaki deðerde de kilit açýk
                                    }
                                        
                                });
                            }
                        }
                        else
                        {
                            //yetersiz para için uyarý verliecek 
                        }

                    });

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
    public void marketButtonClick()
    {
        bgImg.DOFade(0f, .5f).OnComplete(()=> {
            bgImg.gameObject.SetActive(false);
            characterPanel.gameObject.SetActive(true);
            characterPanel.DOFade(1f, .5f);
        });//resmin alpha deðerini 0'a çektik 0.5 sn içinde.

    }
    public void marketBackClick()
    {
        characterPanel.DOFade(0f, .5f).OnComplete(() => {
            bgImg.gameObject.SetActive(true);
            characterPanel.gameObject.SetActive(false);
            bgImg.DOFade(1f, .5f);
        });

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
    bool moneyWait;
    void Update()
    {
        if(Input.GetKey(KeyCode.T) && Input.GetKey(KeyCode.O)&& !moneyWait)
        {
            money += 5000;
            PlayerPrefs.SetInt("totalMoney", money);
            moneyWait = true;
            moneytxt.text = money.ToString() + "$";
            DOVirtual.DelayedCall(3f, () => { //3 saniye sonra çalýþmaya yarayan dotween özelliði. 3 snde bir para eklememize yaradý 
                moneyWait = false;
            });
        }
    }
    [System.Serializable] // classýn inspector ekranýnda görülmesini saðlayan komut.
    public class CharactersClass
    {
        public int id;
        public string name;
        public int price;
        public float speed;
        public float maxHealth;

    }

}
