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
    public Transform startPoint; // karakterin baþlangýç pozisyonu
    public Transform centerPoint; // karakterin orta pozisyonu 
    public Transform exitPoint; //karakterin çýkýþ pozistonu 
    int selectCharID;
    public Button marketLeftBt;
    public Button marketRightBt;
    public Text playerNameTxt;
    public GameObject buyBtObj;
    public GameObject selectBtObj;
    public Text buyTxt; // oyuncunun fiyatý yazacak 
    int selectedPlayerID; //seçilen oyuncunun id'si
    
    void Start()
    {
        selectedPlayerID = PlayerPrefs.GetInt("SelectedPlayer");
        buyBtObj.GetComponent<Button>().onClick.AddListener(buyButtonOnClick);
        marketLeftBt.interactable = false;
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

        CharacterControl();
    }
    public void CharacterControl()
    {
        if (!PlayerPrefs.HasKey("Character0")) // hafýzada böyle bir deðper var mý ? yoksa;
        {
            PlayerPrefs.SetInt("Character0", 1);  // kilit açmak için 1 e set ettik 
        }
        for (int i = 0; i < characterList.Count; i++) // karakter listesi kadar i döndür
        {
            if (PlayerPrefs.GetInt("Character" + i) == 0)
            {
                characterList[i].isLocked = true; // i==0 ise karakteri kitledik. 0.yý 1 e setlediðimizx için o açýk olacak 
            }
            else
            {
                characterList[i].isLocked = false; // kilit açýlý 

            }
            characterList[i].player.position = new Vector3(startPoint.position.x, characterList[i].player.position.y, characterList[i].player.position.z);// start pointin x ine götürdük ama karakterin y ve z pozisyonunu koruduk 
        }
    }
    public void marketButtonClick()
    {
        bgImg.DOFade(0f, .5f).OnComplete(()=> {
            bgImg.gameObject.SetActive(false);
            characterPanel.gameObject.SetActive(true);
            characterPanel.DOFade(1f, .5f);
            characterList[0].player.DOMoveX(centerPoint.position.x, 1f);
        });//resmin alpha deðerini 0'a çektik 0.5 sn içinde.
        getPlayerInfo(); // burda da çaðýrdýk ki market butonuna týklandýðý gibi isimler gelsin.
    }
    public void marketrightclick()
    {
        marketLeftBt.interactable = true;
        characterList[selectCharID].player.DOMoveX(exitPoint.position.x, 1f); // seçili ýd li karakteri haraket ettirdik sonra alttaki if ile ýd yi 1 arttýrdýk sonra bi alttaki ifin üstünden devam ettik 
        if (selectCharID < characterList.Count - 1) // ýd tanýmladýk karakter listesinin sayýsýndan -1 çýakrdýk ki charlistin sayýsýndan fazla olmadý
        {
            selectCharID++;
        }
        characterList[selectCharID].player.DOMoveX(centerPoint.position.x, 1f); // id artýnca yeni id ye eþit olan karakteri ortaya çektik 
        if (selectCharID == characterList.Count-1)
        {
            marketRightBt.interactable = false;
        }
        getPlayerInfo(); //kaydýrýnca charýn ismi deðiþsin 
    }
    public void marketleftclick()
    {
        marketRightBt.interactable = true;
        characterList[selectCharID].player.DOMoveX(startPoint.position.x, 1f);
        if (selectCharID > 0)
        {
            selectCharID--;
        }
        characterList[selectCharID].player.DOMoveX(centerPoint.position.x, 1f);
        if (selectCharID == 0)
        {
            marketLeftBt.interactable = false;
        }
        getPlayerInfo();
    }
    void getPlayerInfo()
    {
        playerNameTxt.text = characterList[selectCharID].name;
        if (characterList[selectCharID].isLocked) // oyuncu kilitli mi?
        {
            buyBtObj.SetActive(true);
            buyTxt.text = "BUY - $" + characterList[selectCharID].price;
            selectBtObj.SetActive(false);
        }
        else
        {
            buyBtObj.SetActive(false);
            if (selectCharID== selectedPlayerID) { // seçili çarýn ýd si playera eþit olunca elect butonunu kaldýrdýk. çünkü zaten karakteri seçmiþ oluyoruz
                selectBtObj.SetActive(false);
            }
            else
            {
                selectBtObj.SetActive(true);
            }
            
        }
    }
    public void buyButtonOnClick()
    {
        if (money >= characterList[selectCharID].price)// param satýn almaya yetiyorsa
        {
            buyBtObj.SetActive(false);
            selectedPlayerID = selectCharID;
            PlayerPrefs.SetInt("SelectedPlayer", selectedPlayerID);
            money -= characterList[selectCharID].price; // parayý azalttýk.
            PlayerPrefs.SetInt("totalMoney", money);
            moneytxt.text = money.ToString() + "$";
            moneytxt.transform.DOScale(new Vector3(1.1f, 1.1f), .5f).OnComplete(() =>
            {
                moneytxt.transform.DOScale(new Vector3(1f, 1f), .5f).SetDelay(.2f);
            });
            characterList[selectCharID].isLocked = false;
            PlayerPrefs.SetInt("Character" + selectCharID, 1);
         }
        else
        {
            //TODO : para yetersiz ise kýrmýzý yanýp sönsün
        }

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
        public string name;
        public int id;    
        public int price;
        public float speed;
        public float health;  //saðlýk
        public int Level = 1; // default olarak 1. levelden baþlasýn 
        public int maxLevel; // seviyenin ulaþabileceði max deðer
        public bool isLocked; // karakterin kilitli olup olmadýðýný kontrol etmek için deðiþken 
        public Transform player; // oyuncunun transformu
    }

}
