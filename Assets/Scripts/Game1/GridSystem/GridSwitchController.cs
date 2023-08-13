using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class GridSwitchController : MonoBehaviour
{
   private GridController _gridController;
   private CameraAutoAligner _cameraAutoAliner;
   [SerializeField] private Button _switchButtonLeft, _switchButtonRight , _resetGridButton;
   [SerializeField] private TextMeshProUGUI _counterText;
   private int _counterNumber = 3;
   
   [Inject]
   public  void Construct(GridController gridController , CameraAutoAligner cameraAutoAligner)
   {
      this._gridController = gridController;
      this._cameraAutoAliner = cameraAutoAligner;
   }

   private  void Start()
   {
      if(_gridController != null)
         Debug.Log("Setup is Correct");
       
      SetButtonsClicks();
      UpdateCounterText();
   }

   private void SetButtonsClicks()
   {
      _resetGridButton.onClick.AddListener(OnResetGridButtonClicked);
      _switchButtonLeft.onClick.AddListener(OnSwitchLeft);
      _switchButtonRight.onClick.AddListener(OnSwitchRight);
   }

   private void OnResetGridButtonClicked()
   {
      _gridController.RefleshGrid(_counterNumber);
      _cameraAutoAliner.AlignCamera();
   }

   private void OnSwitchLeft()
   {  
      _counterNumber --;
      if(_counterNumber < 2)
      {
         _counterNumber = 2;
      }   

      UpdateCounterText();   
   }

   private void OnSwitchRight()
   {
      _counterNumber ++;
      if(_counterNumber > 20)
      {
         _counterNumber = 20;
      }

      UpdateCounterText();
   }

   private void UpdateCounterText()
   {
      _counterText.text = "" + _counterNumber;
   }

}
