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
   private int _counterNumber = 3;
   [SerializeField] private Button _switchButtonLeft, _switchButtonRight , _resetGridButton;
   [SerializeField] private TextMeshProUGUI _counterText;
   
   
   [Inject]
   public  void Construct(GridController gridController , CameraAutoAligner cameraAutoAligner)
   {
      this._gridController = gridController;
      this._cameraAutoAliner = cameraAutoAligner;
   }

   private  void Start()
   {  
      SetButtonsClicks();
      UpdateCounterText();
      OnResetGridButtonClicked();
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
      if(_counterNumber < GridConstants.MinGridSize)
      {
         _counterNumber = GridConstants.MinGridSize;
      }   

      UpdateCounterText();   
   }

   private void OnSwitchRight()
   {
      _counterNumber ++;
      if(_counterNumber > GridConstants.MaxGridSize)
      {
         _counterNumber = GridConstants.MaxGridSize;
      }
      UpdateCounterText();
   }

   private void UpdateCounterText()
   {
      _counterText.text = "" + _counterNumber;
   }

}
