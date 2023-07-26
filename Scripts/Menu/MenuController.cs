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
    public Transform startPoint; // karakterin ba�lang�� pozisyonu
    public Transform centerPoint; // karakterin orta pozisyonu 
    public Transform exitPoint; //karakterin ��k�� pozistonu 
    int selectCharID;
    public Button marketLeftBt;
    public Button marketRightBt;
    public Text playerNameTxt;
    public GameObject buyBtObj;
    public GameObject selectBtObj;
    public Text buyTxt; // oyuncunun fiyat� yazacak 
    int selectedPlayerID; //se�ilen oyuncunun id'si
    
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
        


        if (PlayerPrefs.HasKey("LevelLock0") == false) // haf�zada bu parametre var m� 
        {
            PlayerPrefs.SetInt("LevelLock0", 1); //parametreyi 1 e sabitledik. kilit a�t�k 
        }

        //level butonlar� olu�tur
        bool isBuyBt = false;
        for (int i = 0; i < 10; i++)
        {
            levelSelectBT btClone = Instantiate(levelBtPrefab); // prefab klonlama
            btClone.transform.SetParent(content); //parent atamas� yap�ld�.
            btClone.levelID.text = (i + 1).ToString(); //klonlanan butonun level�d textini de�i�tirdik.
            int levelID = i;  // text olarak tan�mlanan dan farkl� yerel bir de�i�ken 
            allSelectBt.Add(btClone);

            if (PlayerPrefs.GetInt("LevelLock"+levelID)==0) //seviye klitli
            {
                if (!isBuyBt)
                {

                    isBuyBt = true;
                    btClone.buyBt.gameObject.SetActive(true);
                    btClone.buyBt.onClick.AddListener(() => { //addlistener click eklememize yar�yor. kodla eri�im sa�lad�k 
                        Debug.Log(levelID+". Buy butonuna bas�ld�."); 
                        //sat�n alma i�lem kontrol.
                        if(money>= 1000) //haf�zadaki para 1000'den b�y�k veya e�it mi
                        {
                            //sat�n alma i�lem� ger�ekle�ecek 
                            money -= 1000;
                            PlayerPrefs.SetInt("totalMoney", money);
                            moneytxt.text = money.ToString() + "$";
                            moneytxt.transform.DOScale(new Vector3(1.1f, 1.1f), .5f).OnComplete(() => {
                                moneytxt.transform.DOScale(new Vector3(1f, 1f), .5f).SetDelay(.2f);
                            });
                            //butonu aktifle�tirme i�lemi.
                            allSelectBt[levelID].GetComponent<CanvasGroup>().alpha = 1f; //1. levelin butonu gibi bir tan�mlama yapt�k liste ile
                            allSelectBt[levelID].buyBt.gameObject.SetActive(false);
                            allSelectBt[levelID].GetComponent<Button>().interactable = true;
                            PlayerPrefs.SetInt("LevelLock" + levelID, 1); //haf�zadaki de�erde de kilit a��k
                            //buton kayd�rma i�lemi;
                            if (levelID < 9)
                            {
                                allSelectBt[levelID + 1].buyBt.gameObject.SetActive(true); // level�d+1 derken listede i�lemi yapt���m�z butondan 1 sonraki demek.
                                allSelectBt[levelID + 1].buyBt.onClick.AddListener(() => {
                                    if (money >= 1000)// haf�zadaki para 1000den b�y�k m� e�it mi
                                    {
                                        money -= 1000;
                                        PlayerPrefs.SetInt("totalMoney", money);
                                        moneytxt.text = money.ToString() + "$";
                                        moneytxt.transform.DOScale(new Vector3(1.1f, 1.1f), .5f).OnComplete(() => {
                                            moneytxt.transform.DOScale(new Vector3(1f, 1f), .5f).SetDelay(.2f);
                                        });
                                        //butonu aktifle�tirme i�lemi.
                                        allSelectBt[levelID+1].GetComponent<CanvasGroup>().alpha = 1f; //1. levelin butonu gibi bir tan�mlama yapt�k liste ile
                                        allSelectBt[levelID+1].buyBt.gameObject.SetActive(false);
                                        allSelectBt[levelID+1].GetComponent<Button>().interactable = true;
                                        PlayerPrefs.SetInt("LevelLock" + (levelID+1), 1); //haf�zadaki de�erde de kilit a��k
                                    }
                                        
                                });
                            }
                        }
                        else
                        {
                            //yetersiz para i�in uyar� verliecek 
                        }

                    });

                }
                else
                {
                    btClone.buyBt.gameObject.SetActive(false);
                }
                
                //btClone.buyBt.gameObject.SetActive(true); //sat�n al butonu kilitli b�l�mlerde g�z�kecek.
                btClone.GetComponent<CanvasGroup>().alpha = 0.5f;
                btClone.GetComponent<Button>().interactable = false; // butonun t�klanabilme �zelli�i kapal�

            }
            else //kilitli de�il
            {
                btClone.buyBt.gameObject.SetActive(false);//sat�n al butonu kilidi a��k seviyelerde g�z�kmeyecek.
                btClone.GetComponent<CanvasGroup>().alpha = 1f;
                btClone.GetComponent<Button>().interactable = true; // butonun t�klanabilme �zelli�i a��k
            }
            btClone.GetComponent<Button>().onClick.AddListener(() => {
                Debug.Log((levelID+1) + ".B�l�me t�kland�");
            
            });
        }
        
        levelScroll.horizontalNormalizedPosition = 0f; //scroll u ba�tan ba�latmak i�in
        
        
        
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
        if (!PlayerPrefs.HasKey("Character0")) // haf�zada b�yle bir de�per var m� ? yoksa;
        {
            PlayerPrefs.SetInt("Character0", 1);  // kilit a�mak i�in 1 e set ettik 
        }
        for (int i = 0; i < characterList.Count; i++) // karakter listesi kadar i d�nd�r
        {
            if (PlayerPrefs.GetInt("Character" + i) == 0)
            {
                characterList[i].isLocked = true; // i==0 ise karakteri kitledik. 0.y� 1 e setledi�imizx i�in o a��k olacak 
            }
            else
            {
                characterList[i].isLocked = false; // kilit a��l� 

            }
            characterList[i].player.position = new Vector3(startPoint.position.x, characterList[i].player.position.y, characterList[i].player.position.z);// start pointin x ine g�t�rd�k ama karakterin y ve z pozisyonunu koruduk 
        }
    }
    public void marketButtonClick()
    {
        bgImg.DOFade(0f, .5f).OnComplete(()=> {
            bgImg.gameObject.SetActive(false);
            characterPanel.gameObject.SetActive(true);
            characterPanel.DOFade(1f, .5f);
            characterList[0].player.DOMoveX(centerPoint.position.x, 1f);
        });//resmin alpha de�erini 0'a �ektik 0.5 sn i�inde.
        getPlayerInfo(); // burda da �a��rd�k ki market butonuna t�kland��� gibi isimler gelsin.
    }
    public void marketrightclick()
    {
        marketLeftBt.interactable = true;
        characterList[selectCharID].player.DOMoveX(exitPoint.position.x, 1f); // se�ili �d li karakteri haraket ettirdik sonra alttaki if ile �d yi 1 artt�rd�k sonra bi alttaki ifin �st�nden devam ettik 
        if (selectCharID < characterList.Count - 1) // �d tan�mlad�k karakter listesinin say�s�ndan -1 ��akrd�k ki charlistin say�s�ndan fazla olmad�
        {
            selectCharID++;
        }
        characterList[selectCharID].player.DOMoveX(centerPoint.position.x, 1f); // id art�nca yeni id ye e�it olan karakteri ortaya �ektik 
        if (selectCharID == characterList.Count-1)
        {
            marketRightBt.interactable = false;
        }
        getPlayerInfo(); //kayd�r�nca char�n ismi de�i�sin 
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
            if (selectCharID== selectedPlayerID) { // se�ili �ar�n �d si playera e�it olunca elect butonunu kald�rd�k. ��nk� zaten karakteri se�mi� oluyoruz
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
        if (money >= characterList[selectCharID].price)// param sat�n almaya yetiyorsa
        {
            buyBtObj.SetActive(false);
            selectedPlayerID = selectCharID;
            PlayerPrefs.SetInt("SelectedPlayer", selectedPlayerID);
            money -= characterList[selectCharID].price; // paray� azaltt�k.
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
            //TODO : para yetersiz ise k�rm�z� yan�p s�ns�n
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
        levelSelectPanel.DOScale(Vector3.one, 0.7f).SetEase(Ease.OutBounce).SetUpdate(true); //time scale 0 olsa bile bu dotween �al��acak. di�erleri �al��maz setupdate true yapmak gerekiyor.
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
            DOVirtual.DelayedCall(3f, () => { //3 saniye sonra �al��maya yarayan dotween �zelli�i. 3 snde bir para eklememize yarad� 
                moneyWait = false;
            });
        }
    }
    [System.Serializable] // class�n inspector ekran�nda g�r�lmesini sa�layan komut.
    public class CharactersClass
    {
        public string name;
        public int id;    
        public int price;
        public float speed;
        public float health;  //sa�l�k
        public int Level = 1; // default olarak 1. levelden ba�las�n 
        public int maxLevel; // seviyenin ula�abilece�i max de�er
        public bool isLocked; // karakterin kilitli olup olmad���n� kontrol etmek i�in de�i�ken 
        public Transform player; // oyuncunun transformu
    }

}
