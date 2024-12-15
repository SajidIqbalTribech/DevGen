using UnityEngine ;
using EasyUI.PickerWheelUI ;
using UnityEngine.UI ;

public class Demo : MonoBehaviour {
   [SerializeField] private Button uiSpinButton ;
   [SerializeField] private Text uiSpinButtonText ;

   [SerializeField] private PickerWheel pickerWheel ;


   private void Start () {
      uiSpinButton.onClick.AddListener (() => {

         uiSpinButton.interactable = false ;
         uiSpinButtonText.text = "Spinning" ;

         pickerWheel.OnSpinEnd (wheelPiece => {
             ReferenceManager.Instance.uiManager.SpinWheelCanvas.SetActive(false);
             ReferenceManager.Instance.uiManager.CongratPopupCanvas.SetActive(true);
             ReferenceManager.Instance.uiManager.DiscountCoinsText.text = "You Have Won" + wheelPiece.Amount + " % off you pick!";
             ReferenceManager.Instance.uiManager.CongratCanvasOkayButton.onClick.RemoveAllListeners();
             ReferenceManager.Instance.uiManager.CongratCanvasOkayButton.onClick.AddListener(() => ReferenceManager.Instance.mainHandler.OnClickCongratOkayButton(wheelPiece.Amount));
            Debug.Log (
               @" <b>Index:</b> " + wheelPiece.Index + "           <b>Label:</b> " + wheelPiece.Label
               + "\n <b>Amount:</b> " + wheelPiece.Amount + "      <b>Chance:</b> " + wheelPiece.Chance + "%"
            ) ;

            uiSpinButton.interactable = true ;
            uiSpinButtonText.text = "Spin" ;
         }) ;

         pickerWheel.Spin () ;

      }) ;

   }

}
