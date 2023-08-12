using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class GridSwitchController : MonoBehaviour
{
   private GridController _gridController;
   [SerializeField] private Button _switchButtonLeft, _switchButtonRight , _resetGridButton;
   [SerializeField] private TextMeshProUGUI _counterText;
   
   [Inject]
   public  void Construct(GridController gridController)
   {
      this._gridController = gridController;
   }

   private void Start()
   {
      if(_gridController != null)
         Debug.Log("Setup is Correct");
   }
}
