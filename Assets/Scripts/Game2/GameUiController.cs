using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
public class GameUiController : MonoBehaviour
{
    [SerializeField] private GameObject _levelWinPanel , _levelLostPanel;
    [SerializeField] private Button _nextLevelButton;
    private GameController _gameController;

    [Inject]
    public  void Construct(GameController gameController) 
    {
        this._gameController = gameController;
        _nextLevelButton.onClick.AddListener(OnNextLevelButtonClicked);
    }
    public void ShowLevelWin()
    {
        _levelWinPanel.gameObject.SetActive(true);
    }

    public void ShowLevelLost()
    {
        _levelLostPanel.gameObject.SetActive(true);
    }
    private void OnNextLevelButtonClicked()
    {
        CloseAllPanels();
        _gameController.StartNextLevel();
    }
    public void CloseAllPanels()
    {
        _levelWinPanel.gameObject.SetActive(false);
        _levelLostPanel.gameObject.SetActive(false);
    }
}
