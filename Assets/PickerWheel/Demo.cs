using UnityEngine ;
using EasyUI.PickerWheelUI ;
using UnityEngine.UI ;

public class Demo : MonoBehaviour {
   [SerializeField] private Button uiSpinButton ;
   [SerializeField] private Text uiSpinButtonText ;
   [SerializeField] private Image uiSpinButtonImage ;

   [SerializeField] private PickerWheel pickerWheel ;
    [SerializeField] Sprite Spin;
    [SerializeField] Sprite Spinning;

   private void Start () {
      uiSpinButton.onClick.AddListener (() => {

         uiSpinButton.interactable = false ; 
         //uiSpinButtonText.text = "Spinning" ;
         uiSpinButtonImage.sprite = Spinning;


         pickerWheel.OnSpinEnd (wheelPiece => {
             ReferenceManager.Instance.uiManager.SpinWheelCanvas.SetActive(false);
             ReferenceManager.Instance.uiManager.CongratPopupCanvas.SetActive(true);
             ReferenceManager.Instance.uiManager.DiscountCoinsText.text = "You Have Won " + wheelPiece.Amount + "% off your pick!";
             GlobalData.SetDiscountPromo(GlobalData.CurrentMainCardSelected, GlobalData.CurrentInventorySelected, wheelPiece.Amount);
             ReferenceManager.Instance.uiManager.CongratCanvasOkayButton.onClick.RemoveAllListeners();
             //ReferenceManager.Instance.mainHandler.ManupulateClaimedPowerUp();
             ReferenceManager.Instance.uiManager.CongratCanvasOkayButton.onClick.AddListener(() => ReferenceManager.Instance.mainHandler.OnClickCongratOkayButton(wheelPiece.Amount));
            Debug.Log (
               @" <b>Index:</b> " + wheelPiece.Index + "           <b>Label:</b> " + wheelPiece.Amount
               + "\n <b>Amount:</b> " + wheelPiece.Amount + "      <b>Chance:</b> " + wheelPiece.Chance + "%"
            ) ;
            uiSpinButton.interactable = true ;
             uiSpinButtonImage.sprite = Spin;

         }) ;

         pickerWheel.Spin () ;

      }) ;

   }

}
