using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Helper;
using System;
using DG.Tweening;
public class MainHandler : MonoBehaviour
{
    #region Variable
    public int MaxCoins = 20;
    private UIManager uiManager;
    public Powerup[] Cards;
    private List<GameObject> myPowerupsCards = new();
    private List<GameObject> buyPowerupsCards = new();
    private List<GameObject> myActiveCards = new();
    private List<GameObject> myExpireCards = new();
    #endregion

    #region MonoBeahviour
    private void Awake()
    {
        uiManager = ReferenceManager.Instance.uiManager;
    }
    private void OnEnable()
    {
        GlobalEvents.OnCurrencyAddition += AddCoins;
        GlobalEvents.OnCurrencyReduction += ReductionCoins;
    }
    private void Start()
    {
        LoadCardInformation();
    }
    private void OnDisable()
    {
        GlobalEvents.OnCurrencyAddition -= AddCoins;
        GlobalEvents.OnCurrencyReduction -= ReductionCoins;
    }
    #endregion

    #region Coin Management
    public void ReductionCoins(float amount)
    {
        if (GlobalData.Coins >= amount)
        {
            GlobalData.Coins -= amount;
            GlobalEvents.InvokeUpdateCurrencyText(GlobalData.Coins);
        }
    }
    public void AddCoins(float amount)
    {
        GlobalData.Coins += amount;
        GlobalEvents.InvokeUpdateCurrencyText(GlobalData.Coins);
    }
    #endregion

    #region PowerupsManagement
    /// <summary>
    /// Clear My Powerups list
    /// </summary>
    public void ClearMyPowerupsList()
    {
        for (int i = 0; i < myPowerupsCards.Count; i++)
        {
            Destroy(myPowerupsCards[i]);
        }
        myPowerupsCards = new List<GameObject>();
    }
    /// <summary>
    /// View Owned Powerup lists
    /// </summary>
    public void ListViewMyPowerups()
    {
        uiManager.DiscardEmpty.SetActive(false);
        uiManager.ClaimedEmpty.SetActive(false);
        uiManager.AvailableClaimedEmpty.SetActive(false);
        ClearMyPowerupsList();
        ClearExpiredCard();
        for (int i = 0; i < Cards.Length; i++)
        {
            for (int j = 0; j < Cards[i].CardInventory; j++)//&& !Cards[i].isActive
            {
                if (Cards[i].inventory[j].isExpired || Cards[i].inventory[j].isDiscard)
                {
                    GameObject myExpiredCard = Instantiate(uiManager.PrefabMyPowerupCard, uiManager.myExpiryPowerupContent);
                    myExpiredCard.GetComponent<ViewCardInfo>().Name.text = Cards[i].name;
                    myExpiredCard.GetComponent<ViewCardInfo>().Logo.sprite = Cards[i].logo;
                    myExpiredCard.GetComponent<ViewCardInfo>().Logo.color = Color.gray;
                    myExpiredCard.GetComponent<ViewCardInfo>().CrossButton.gameObject.SetActive(false);
                    myExpiredCard.GetComponent<ViewCardInfo>().ActivateButton.gameObject.SetActive(false);
                    myExpiredCard.GetComponent<ViewCardInfo>().Timer.GetComponentInParent<Image>().gameObject.SetActive(false);
                    myExpiredCard.GetComponent<ViewCardInfo>().PreserveButton.gameObject.SetActive(false);
                    myExpireCards.Add(myExpiredCard);
                    if (Cards[i].inventory[j].isExpired)
                    {
                        // Add Expired Tag Here..
                        myExpiredCard.GetComponent<ViewCardInfo>().Description.text = "Expired";
                    }
                    if (Cards[i].inventory[j].isDiscard)
                    {
                        // Add Discard Tag Here..
                        myExpiredCard.GetComponent<ViewCardInfo>().Description.text = "Discard";
                    }
                    continue;
                }

                if (Cards[i].inventory[j].isPurchase)
                {
                    int mainCardIndex = i;
                    int inventoryCard = j;
                    // List all cards that owned by user here...
                    GameObject myCard = Instantiate(uiManager.PrefabMyPowerupCard, uiManager.myPowerupContent);
                    myPowerupsCards.Add(myCard);
                    int x = myPowerupsCards.Count - 1;
                    myCard.GetComponent<ViewCardInfo>().Name.text = Cards[i].name;
                    myCard.GetComponent<ViewCardInfo>().Logo.sprite = Cards[i].logo;
                    if (!Cards[i].inventory[j].isActive)
                        myCard.GetComponent<ViewCardInfo>().Description.text = Cards[i].ClaimedDescriptionBefore;
                    else
                    {
                        if (GlobalData.GetSetDiscountPromo(i, j) > 0)
                            myCard.GetComponent<ViewCardInfo>().Description.text = Cards[i].ClaimedDescriptionAfter + " " + GlobalData.GetSetDiscountPromo(i, j) + " %";
                        else
                            myCard.GetComponent<ViewCardInfo>().Description.text = Cards[i].ClaimedDescriptionAfter;
                    }

                    string savedDateTimeString = GlobalData.GetPurchasedCardTime(i, j);
                    DateTime savedDateTime = DateTime.Parse(savedDateTimeString);
                    DateTime currentDateTime = DateTime.Now;
                    TimeSpan difference = currentDateTime - savedDateTime;
                    double hoursDifference = difference.TotalHours;
                    double DisplayTime = 72 - hoursDifference;
                    myCard.GetComponent<ViewCardInfo>().Timer.text = Math.Ceiling(DisplayTime) + " HRS";
                    if (DisplayTime > 48)
                    {
                        myCard.GetComponent<ViewCardInfo>().Timer.color = Color.green;
                    }
                    else if (DisplayTime <= 48 && DisplayTime > 24)
                    {
                        myCard.GetComponent<ViewCardInfo>().Timer.color = Color.yellow;
                    }
                    else if (DisplayTime <= 24 && DisplayTime > 0)
                    {
                        myCard.GetComponent<ViewCardInfo>().Timer.color = Color.red;
                    }
                    else
                    {
                        // card is expired
                        Cards[mainCardIndex].inventory[j].isActive = false;
                        Cards[mainCardIndex].inventory[j].isPurchase = false;
                        Cards[mainCardIndex].inventory[j].isExpired = true;
                        GlobalData.SetCardInventoryExpire(mainCardIndex, j, true);
                        GlobalData.CurrentActiveCardCount -= 1;
                        ListViewMyPowerups();
                    }
                    myCard.GetComponent<ViewCardInfo>().PreserveButton.onClick.RemoveAllListeners();
                    myCard.GetComponent<ViewCardInfo>().PreserveButton.onClick.AddListener(() => OnClickPresever(x, mainCardIndex, inventoryCard));
                    if (Cards[i].inventory[j].isActive)
                    {
                        myCard.GetComponent<ViewCardInfo>().ActivateButton.enabled = false;
                        myCard.GetComponent<ViewCardInfo>().ActivateButton.image.sprite = uiManager.Activated;
                    }
                    else
                    {
                        myCard.GetComponent<ViewCardInfo>().ActivateButton.onClick.RemoveAllListeners();
                        myCard.GetComponent<ViewCardInfo>().ActivateButton.onClick.AddListener(() => OnClickActivate(x, mainCardIndex, inventoryCard));
                    }

                    myCard.GetComponent<ViewCardInfo>().CrossButton.onClick.RemoveAllListeners();
                    myCard.GetComponent<ViewCardInfo>().CrossButton.onClick.AddListener(() => OnClickDiscard(x, mainCardIndex, inventoryCard));
                }
            }

        }
        //ManupulateClaimedPowerUp();
        if (myExpireCards.Count == 0)
        {
            // Empty Card Test is here..
            uiManager.DiscardEmpty.SetActive(true);
        }
        if (myPowerupsCards.Count == 0)
        {
            uiManager.AvailableClaimedEmpty.SetActive(true);
        }
        if (myActiveCards.Count == 0)
        {
            uiManager.ClaimedEmpty.SetActive(true);
        }
    }
    public void ClearExpiredCard()
    {
        for (int i = 0; i < myExpireCards.Count; i++)
        {
            Destroy(myExpireCards[i]);
        }
        myExpireCards = new List<GameObject>();
    }
    /// <summary>
    /// Clear Buy Powerups lidt
    /// </summary>
    public void ClearBuyPowerupsList()
    {
        for (int i = 0; i < buyPowerupsCards.Count; i++)
        {
            Destroy(buyPowerupsCards[i]);
        }
        buyPowerupsCards = new List<GameObject>();
    }
    /// <summary>
    /// List down all buy powerups
    /// </summary>
    public void ListBuyPowerups()
    {
        ClearBuyPowerupsList();
        for (int i = 0; i < Cards.Length; i++)
        {
            //if (!Cards[i].isPurchase)
            {
                // List down the powerup that is not purchased here...
                int mainCardIndex = i;
                GameObject card = Instantiate(uiManager.PrefabBuyPowerupCard, uiManager.buyPowerupContent);
                buyPowerupsCards.Add(card);
                int x = buyPowerupsCards.Count - 1;
                card.GetComponent<BuyCardInfo>().Name.text = Cards[i].name;
                card.GetComponent<BuyCardInfo>().Logo.sprite = Cards[i].logo;
                /*
                if (i == 0)
                {
                    card.GetComponent<BuyCardInfo>().Logo.gameObject.AddComponent<Button>();
                    card.GetComponent<BuyCardInfo>().Logo.GetComponent<Button>().onClick.RemoveAllListeners();
                    card.GetComponent<BuyCardInfo>().Logo.GetComponent<Button>().onClick.AddListener(() => uiManager.SpinWheelCanvas.SetActive(true));
                    if(Cards[0].price < prevCost)
                    {
                        Cards[0].price = prevCost;
                    }
                }
                */
                card.GetComponent<BuyCardInfo>().CoinPrice.text = Cards[i].price.ToString();
                card.GetComponent<Button>().onClick.RemoveAllListeners();
                card.GetComponent<Button>().onClick.AddListener(() => OnClickBuyCard(x, mainCardIndex));
            }
        }
    }
    /// <summary>
    /// On Click Buy Cards.. This will update the description and assign the Purchase button action on Buy Powerup Canvas
    /// </summary>
    /// <param name="cardIndex"></param>
    public void OnClickBuyCard(int cardIndex, int mainCardIndex)
    {
        for (int i = 0; i < buyPowerupsCards.Count; i++)
        {
            buyPowerupsCards[i].GetComponent<BuyCardInfo>().SelectedIcon.SetActive(false);
        }
        buyPowerupsCards[cardIndex].GetComponent<BuyCardInfo>().SelectedIcon.SetActive(true);
        uiManager.BuyCanvasDescriptionText.text = Cards[mainCardIndex].description;
        uiManager.BuyCanvasPurchaseButton.onClick.RemoveAllListeners();
        uiManager.BuyCanvasPurchaseButton.onClick.AddListener(() => OnPuchaseClick(cardIndex, mainCardIndex));
    }
    /// <summary>
    /// On Click Purchase Button in Buy Powerup Canvas
    /// </summary>
    /// <param name="cardIndex"></param>
    public void OnPuchaseClick(int cardIndex, int mainCardIndex)
    {
        uiManager.PurchasePopupCanvas.SetActive(true);
        uiManager.PurchaseCanvasTitleText.text = Cards[mainCardIndex].name;
        uiManager.PurchaseCanvasOkayButton.onClick.RemoveAllListeners();
        uiManager.PurchaseCanvasOkayButton.onClick.AddListener(() => PurchaseCard(cardIndex, mainCardIndex));
    }
    /// <summary>
    /// On Click Ok Button in PurchasePopup canvas.. 
    /// </summary>
    /// <param name="cardIndex"></param>
    public void PurchaseCard(int cardIndex, int mainCardIndex)
    {
        Debug.LogError("Error " + GlobalData.CurrentActiveCardCount);
        if (GlobalData.CurrentActiveCardCount >= 3)
        {
            uiManager.PurchasePopupCanvas.SetActive(false);
            uiManager.MustDiscardPopupCanvas.SetActive(true);
            uiManager.MustDiscardCanvasOkayButton.onClick.RemoveAllListeners();
            uiManager.MustDiscardCanvasOkayButton.onClick.AddListener(() => OnClickMustDiscardOkay());
            return;
        }
        if (GlobalData.Coins >= Cards[mainCardIndex].price)
        {
            GlobalEvents.OnCurrencyReduction(Cards[mainCardIndex].price);
            CardInventory c = new CardInventory();
            Cards[mainCardIndex].inventory.Add(c);
            Cards[mainCardIndex].CardInventory++;
            GlobalData.SetCardInventory(mainCardIndex, Cards[mainCardIndex].CardInventory);
            GlobalData.CurrentActiveCardCount += 1;
            Cards[mainCardIndex].inventory[Cards[mainCardIndex].CardInventory - 1].isPurchase = true;
            GlobalData.SetCardInventoryPurchased(mainCardIndex, Cards[mainCardIndex].CardInventory - 1, true);
            string currentDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            GlobalData.SetPurchasedCardInventoryTime(mainCardIndex, Cards[mainCardIndex].CardInventory - 1, currentDateTime);
            if (Cards[mainCardIndex].inventory[Cards[mainCardIndex].CardInventory - 1].isExpired)
            {
                Cards[mainCardIndex].inventory[Cards[mainCardIndex].CardInventory - 1].isExpired = false;
                GlobalData.SetCardInventoryExpire(mainCardIndex, Cards[mainCardIndex].CardInventory - 1, false);
            }
            if (Cards[mainCardIndex].inventory[Cards[mainCardIndex].CardInventory - 1].isDiscard)
            {
                Cards[mainCardIndex].inventory[Cards[mainCardIndex].CardInventory - 1].isDiscard = false;
                GlobalData.SetCardInventoryDiscard(mainCardIndex, Cards[mainCardIndex].CardInventory - 1, false);
            }
            ListBuyPowerups();
        }
        else
        {
            uiManager.EnoughCoinsPopupCanvas.SetActive(true);
            uiManager.EnoughCoinsCanvasTitleText.text = Cards[mainCardIndex].name;
        }
        uiManager.PurchasePopupCanvas.SetActive(false);
    }
    public void OnClickMustDiscardOkay()
    {
        uiManager.MustDiscardPopupCanvas.SetActive(false);
        uiManager.BuyPowerupsCanvas.SetActive(false);
        uiManager.MyPowerupsCanvas.SetActive(true);
        ListViewMyPowerups();
    }
    /// <summary>
    /// Call at Start to load card data from PlayerPrefs
    /// </summary>
    public void LoadCardInformation()
    {
        for (int i = 0; i < Cards.Length; i++)
        {
            Cards[i].CardInventory = GlobalData.GetCardInventory(i);
            for (int j = 0; j < Cards[i].CardInventory; j++)
            {
                //Cards[i].inventory[j].isPurchase = GlobalData.GetCardInventoryPurchase(i, j);
                //Cards[i].inventory[j].isActive = GlobalData.GetCardInventoryActive(i, j);
                //Cards[i].inventory[j].isExpired = GlobalData.GetCardInventoryExpire(i, j);
                //Cards[i].inventory[j].isDiscard = GlobalData.GetCardInventoryDiscard(i, j);
                
                CardInventory c = new CardInventory();
                c.isPurchase = GlobalData.GetCardInventoryPurchase(i, j);
                c.isActive = GlobalData.GetCardInventoryActive(i, j);
                c.isDiscard = GlobalData.GetCardInventoryDiscard(i, j);
                c.isExpired = GlobalData.GetCardInventoryExpire(i, j);
                Cards[i].inventory.Add(c);
            }
        }
    }
    #endregion

    #region Preserve
    public void OnClickPresever(int cardIndex, int mainCardIndex, int inventoryIndex)
    {
        uiManager.PreservePopupCanvas.SetActive(true);
        uiManager.PreserveCanvasOkayButton.onClick.RemoveAllListeners();
        uiManager.PreserveCanvasOkayButton.onClick.AddListener(() => OnClickYesPreserve(cardIndex, mainCardIndex, inventoryIndex));
    }
    public void OnClickYesPreserve(int cardIndex, int mainCardIndex, int inventoryIndex)
    {
        if (GlobalData.Coins >= 2)
        {
            uiManager.PreservePopupCanvas.SetActive(false);
            GlobalEvents.InvokeCoinReduction(2);
            string savedDateTimeString = GlobalData.GetPurchasedCardTime(mainCardIndex, inventoryIndex);
            DateTime savedDateTime = DateTime.Parse(savedDateTimeString);
            DateTime updatedDateTime = savedDateTime.AddHours(24);
            GlobalData.SetPurchasedCardInventoryTime(mainCardIndex, inventoryIndex, updatedDateTime.ToString());

            string _savedDateTimeString = GlobalData.GetPurchasedCardTime(mainCardIndex, inventoryIndex);
            DateTime _savedDateTime = DateTime.Parse(_savedDateTimeString);
            DateTime currentDateTime = DateTime.Now;
            TimeSpan difference = currentDateTime - _savedDateTime;
            double hoursDifference = difference.TotalHours;
            double DisplayTime = 72 - hoursDifference;
            myPowerupsCards[cardIndex].GetComponent<ViewCardInfo>().Timer.text = Math.Ceiling(DisplayTime) + " HRS";
            if (DisplayTime > 48)
            {
                myPowerupsCards[cardIndex].GetComponent<ViewCardInfo>().Timer.color = Color.green;
            }
            else if (DisplayTime <= 48 && DisplayTime > 24)
            {
                myPowerupsCards[cardIndex].GetComponent<ViewCardInfo>().Timer.color = Color.yellow;
            }
            else if (DisplayTime <= 24 && DisplayTime > 0)
            {
                myPowerupsCards[cardIndex].GetComponent<ViewCardInfo>().Timer.color = Color.red;
            }
            else
            {
                // card is expired
            }
        }
        else
        {
            uiManager.PreservePopupCanvas.SetActive(false);
            uiManager.EnoughCoinsPopupCanvas.SetActive(true);
        }
    }
    #endregion
    #region Activate Powerup
    public void OnClickActivate(int cardIndex, int mainCardIndex, int inventoryIndex)
    {
        uiManager.ActivePopupCanvas.SetActive(true);
        uiManager.ActiveCanvasTitleText.text = Cards[mainCardIndex].name;
        uiManager.ActiveCanvasOkayButton.onClick.RemoveAllListeners();
        uiManager.ActiveCanvasOkayButton.onClick.AddListener(() => OnClickYesActivate(cardIndex, mainCardIndex,  inventoryIndex));

    }
    public void OnClickYesActivate(int cardIndex, int mainCardIndex, int inventoryIndex)
    {
        uiManager.ActivePopupCanvas.SetActive(false);
        //myPowerupsCards[cardIndex].GetComponent<ViewCardInfo>().ActivateButton.enabled = false;
        //myPowerupsCards[cardIndex].GetComponent<ViewCardInfo>().ActivateButton.image.sprite = uiManager.Activated;
        Cards[mainCardIndex].inventory[inventoryIndex].isActive = true;
        GlobalData.CurrentCardSelected = cardIndex;
        GlobalData.CurrentMainCardSelected = mainCardIndex;
        GlobalData.CurrentInventorySelected = inventoryIndex;
        GlobalData.SetCardInventoryActive(mainCardIndex, inventoryIndex, true);
        //ManupulateClaimedPowerUp();
        ListViewMyPowerups();
        ActiveMiniGame(mainCardIndex);
    }
    public void ActiveMiniGame(int mainCardIndex)
    {
        Debug.Log("Main card Index " + mainCardIndex);
        if (mainCardIndex == 0)
        {
            //card.GetComponent<BuyCardInfo>().Logo.gameObject.AddComponent<Button>();
            //card.GetComponent<BuyCardInfo>().Logo.GetComponent<Button>().onClick.RemoveAllListeners();
            //card.GetComponent<BuyCardInfo>().Logo.GetComponent<Button>().onClick.AddListener(() =>
            //
            uiManager.SpinWheelCanvas.SetActive(true);
        }
    }
    //public void ManupulateClaimedPowerUp()
    //{
    //    uiManager.ClaimedEmpty.SetActive(false);
    //    ClearMyActiveCards();
    //    for (int i = 0; i < Cards.Length; i++)
    //    {
    //        if (Cards[i].isActive)
    //        {
    //            GameObject myCard = Instantiate(uiManager.PrefabMyPowerupCard, uiManager.myClaimedPowerupContent);
    //            myCard.GetComponent<ViewCardInfo>().Name.text = Cards[i].name;
    //            myCard.GetComponent<ViewCardInfo>().Logo.sprite = Cards[i].logo;
    //            if (GlobalData.GetSetDiscountPromo(i) > 0)
    //                myCard.GetComponent<ViewCardInfo>().Description.text = Cards[i].ClaimedDescriptionAfter + " " + GlobalData.GetSetDiscountPromo(i) + "%";
    //            else
    //                myCard.GetComponent<ViewCardInfo>().Description.text = Cards[i].ClaimedDescriptionAfter;
    //            myCard.GetComponent<ViewCardInfo>().CrossButton.gameObject.SetActive(false);
    //            myCard.GetComponent<ViewCardInfo>().ActivateButton.gameObject.SetActive(false);
    //            myCard.GetComponent<ViewCardInfo>().Timer.GetComponentInParent<Image>().gameObject.SetActive(false);
    //            myCard.GetComponent<ViewCardInfo>().PreserveButton.gameObject.SetActive(false);
    //            myActiveCards.Add(myCard);
    //        }
    //    }
    //}
    public void ClearMyActiveCards()
    {
        for (int i = 0; i < myActiveCards.Count; i++)
        {
            Destroy(myActiveCards[i]);
        }
        myActiveCards = new List<GameObject>();
    }
    #endregion
    #region Discard Powerups
    public void OnClickDiscard(int cardIndex, int mainCardIndex, int inventoryIndex)
    {
        uiManager.DiscardPopupCanvas.SetActive(true);
        uiManager.DiscardCanvasTitleText.text = Cards[mainCardIndex].name;
        uiManager.DiscardCanvasOkayButton.onClick.RemoveAllListeners();
        uiManager.DiscardCanvasOkayButton.onClick.AddListener(() => OnClickOkayDiscard(cardIndex, mainCardIndex, inventoryIndex));

    }
    public void OnClickOkayDiscard(int cardIndex, int mainCardIndex, int inventoryIndex)
    {
        uiManager.DiscardPopupCanvas.SetActive(false);
        Cards[mainCardIndex].inventory[inventoryIndex].isActive = false;
        Cards[mainCardIndex].inventory[inventoryIndex].isPurchase = false;
        Cards[mainCardIndex].inventory[inventoryIndex].isDiscard = true;
        //GlobalData.SetCardExpire(true, mainCardIndex);
        GlobalData.SetCardInventoryDiscard(mainCardIndex, inventoryIndex, true);
        GlobalData.SetCardInventoryActive( mainCardIndex, inventoryIndex, false);
        GlobalData.SetCardInventoryPurchased(mainCardIndex, inventoryIndex, false);
        GlobalData.CurrentActiveCardCount -= 1;
        ListViewMyPowerups();
    }
    #endregion
    #region Discount Wheel Logics
    float prevCost = 0;
    public void OnClickCongratOkayButton(float disAmount)
    {
        float discountAmount = Cards[0].price * (disAmount / 100);
        Debug.Log("DiscountAmount" + discountAmount);
        prevCost = Cards[0].price;
        float purchasedAmount = Cards[0].price - discountAmount;
        Debug.Log("Purchased Amount After Discount " + purchasedAmount);
        myPowerupsCards[GlobalData.CurrentCardSelected].GetComponent<ViewCardInfo>().Description.text = Cards[0].ClaimedDescriptionAfter + " " + GlobalData.GetSetDiscountPromo(GlobalData.CurrentMainCardSelected, GlobalData.CurrentInventorySelected) + " %";
        //Cards[0].price = purchasedAmount;
        //buyPowerupsCards[0].GetComponent<BuyCardInfo>().CoinPrice.text = purchasedAmount.ToString();  
    }
    #endregion

}
