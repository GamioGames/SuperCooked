using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Singletone

    public static GameManager Instance;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    #endregion
    
    // Public
    public enum GameState
    {
        PreGame,
        InGame,
        GameOver
    }
    
    public event EventHandler<int> OnCurrencyChange;
    public event EventHandler<float> OnTimeChange;
    public event EventHandler<RecipeData> OnOrderCreate;
    public event EventHandler OnOrderSuccess;
    
    // Serialized *****
    [SerializeField] private float preGameTime;
    [SerializeField] private float gameTime;
    [SerializeField] private List<RecipeData> recipeDataList = new List<RecipeData>();

    // Private *****
    private GameState _gameState;
    private float _stateTimer;
    private float _timeHelper;
    private float _orderTimer;
    private int _coins;

    private List<RecipeData> _activeRecipes = new List<RecipeData>();

    // MonoBehavior Methods
    private void Start()
    {
        // Idle time
        _stateTimer = preGameTime;
        _coins = 0;
        OnTimeChange?.Invoke(this, gameTime);
    }

    private void Update()
    {
        _stateTimer -= Time.deltaTime;
        _timeHelper -= Time.deltaTime;
        _orderTimer -= Time.deltaTime;

        switch (_gameState)
        {
            case GameState.PreGame:
                if (_stateTimer <= 0)
                {
                    _stateTimer = gameTime;
                    _gameState = GameState.InGame;
                }

                break;
            case GameState.InGame:
                if (_timeHelper <= 0)
                {
                    OnTimeChange?.Invoke(this, _stateTimer);
                    _timeHelper = 1;
                }

                if (_orderTimer <= 0 && recipeDataList.Count > 0)
                {
                    RecipeData newOrder = recipeDataList[0];
                    _activeRecipes.Add(newOrder);
                    OnOrderCreate?.Invoke(this,newOrder);
                    _orderTimer = 35;
                }
                if (_stateTimer <= 0)
                {
                    SetGameOver();
                }
                break;
        }
        

    }
    
    // public Methods *****
    public void SetGameOver()
    {
        _gameState = GameState.GameOver;
    }

    public void SuccessOrder()
    {
        OnOrderSuccess?.Invoke(this, EventArgs.Empty);
    }
    
    // Private Method

}
