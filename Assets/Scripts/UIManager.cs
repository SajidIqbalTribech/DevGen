using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
public class UIManager : MonoBehaviour
{
    #region Variables
    [Header("All Canvases/Windows")]
    [Space]
    [Space]
    [SerializeField] GameObject LandingCanvas;
    [SerializeField] public GameObject BuyPowerupsCanvas;
    [SerializeField] public GameObject MyPowerupsCanvas;
    [SerializeField] public GameObject ActivePopupCanvas;
    [SerializeField] public GameObject DiscardPopupCanvas;
    [SerializeField] public GameObject PurchasePopupCanvas;
    [SerializeField] public GameObject EnoughCoinsPopupCanvas;
    [SerializeField] public GameObject MustDiscardPopupCanvas;
    [SerializeField] public GameObject PreservePopupCanvas;
    [SerializeField] public GameObject SpinWheelCanvas;
    [SerializeField] public GameObject CongratPopupCanvas;


    [Header("Text")]
    [Space]
    [Space]
    [SerializeField] TextMeshProUGUI LandingScreenCoinsText;
    [SerializeField] TextMeshProUGUI BuyPowerScreenCoinsText;
    [SerializeField] TextMeshProUGUI MyPowerScreenCoinsText;
    [SerializeField] public TextMeshProUGUI DiscountCoinsText;
    [SerializeField] Slider CoinSlider;
    [SerializeField] public TextMeshProUGUI BuyCanvasDescriptionText;
    [SerializeField] public Button BuyCanvasPurchaseButton;
    [SerializeField] public Button PurchaseCanvasOkayButton;
    [SerializeField] public Button PreserveCanvasOkayButton;
    [SerializeField] public Button ActiveCanvasOkayButton;
    [SerializeField] public Button DiscardCanvasOkayButton;
    [SerializeField] public Button MustDiscardCanvasOkayButton;
    [SerializeField] public Button CongratCanvasOkayButton;

    [Header("Cards Prefabs")]
    [Space]
    [Space]
    [SerializeField] public GameObject PrefabMyPowerupCard;
    [SerializeField] public Transform myPowerupContent;
    [SerializeField] public GameObject PrefabBuyPowerupCard;
    [SerializeField] public Transform buyPowerupContent;
    [SerializeField] public Sprite Activated;
    [SerializeField] public Transform myClaimedPowerupContent;
    [SerializeField] public Transform myExpiryPowerupContent;
    #endregion


    #region MonoBehaviour
    private void Awake()
    {
    }
    private void OnEnable()
    {
        GlobalEvents.OnUpdateCurrencyText += UpdateCoinsText;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (GlobalData.FirstRun == 0)
        {
            GlobalData.FirstRun = 1;
            GlobalData.Coins = 20;
            GlobalEvents.InvokeUpdateCurrencyText(GlobalData.Coins);
        }
        else
        {
            GlobalEvents.InvokeUpdateCurrencyText(GlobalData.Coins);
        }
    }
    private void OnDisable()
    {
        GlobalEvents.OnUpdateCurrencyText -= UpdateCoinsText;
    }

    #endregion

    #region Coins Upgrade
    public void UpdateCoinsText(float currentCoins)
    {
        LandingScreenCoinsText.text = currentCoins.ToString();
        BuyPowerScreenCoinsText.text = currentCoins.ToString();
        MyPowerScreenCoinsText.text = currentCoins.ToString();
        float value = currentCoins / ReferenceManager.Instance.mainHandler.MaxCoins;
        CoinSlider.DOValue(value, 0.5f).SetEase(Ease.InQuad);
    }
    #endregion

    #region ActionListners
    public void OnClickMyPowerups()
    {
        LandingCanvas.SetActive(false);
        MyPowerupsCanvas.SetActive(true);
        ReferenceManager.Instance.mainHandler.ListViewMyPowerups();
    }
    public void OnClickBuyPowerups()
    {
        LandingCanvas.SetActive(false);
        BuyPowerupsCanvas.SetActive(true);
        ReferenceManager.Instance.mainHandler.ListBuyPowerups();
    }
    #endregion
}
