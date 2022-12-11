using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cookware : Item
{
    // Public *****
    public enum CookwareState
    {
        Empty, // Applies if its not full
        RawValidRecipe, 
        Cooking, // Change to this state depends on Stove
        Cooked,
        CookedCooking,
        Burned
    }
    // Public *****
    public EventHandler<Cookware> OnFullAndValidRecipe;
    
    // Serialized
    [Header("Cookware")]
    [SerializeField] private int maxCapacity = 1;
    // TODO: Change this to a list
    [SerializeField] private RecipeData recipe;
    [SerializeField] private ParticleSystem steamParticles;
    
    [Header("Pot")]
    //TODO: CHANGE TO a specific pot class
    [SerializeField] private Transform potSoup;
    [SerializeField] private Vector2 yMinMaxPotSoup;
    [SerializeField] private ProgressUI _progressUI;

    // Private *****
    private CookwareState _cookwareState;
    private List<FoodData> _foodDataList = new List<FoodData>();
    private float _cookwareTimer;

    // MonoBehavior Callbacks *****

    private void Start()
    {
        potSoup.position = new Vector3(potSoup.position.x, yMinMaxPotSoup.x, potSoup.position.z);
    }

    // Private Methods *****
    private void Update()
    {
        if (!IsCooking()) return;
        
        _cookwareTimer -= Time.deltaTime;

        if (_cookwareState == CookwareState.Cooking)
        {
            OnItemProgressChange?.Invoke(this, _cookwareTimer/ recipe.cookingTime);
        }
        
        if (!(_cookwareTimer <= 0)) return;
        
        switch (_cookwareState)
        {
            case CookwareState.Cooking:
                steamParticles.Play();
                _cookwareState = CookwareState.CookedCooking;
                _cookwareTimer = 5; // 5 seconds to burn
                break;
            case CookwareState.CookedCooking:
                Debug.Log("your comnida is burn");
                _cookwareState = CookwareState.Burned;
                break;
        }
    }
    
    // Private MEthods *****
    private float GetSoupLevel()
    {
        float percentage = (float)_foodDataList.Count / maxCapacity;
        float limit = yMinMaxPotSoup.x + yMinMaxPotSoup.y;

        return percentage * limit;
    }


    // Public Methods *****
    public bool AddFood(FoodData foodData)
    {
        if (_foodDataList.Count >= maxCapacity || foodData.foodType != FoodType.Processed) return false;
        
        _foodDataList.Add(foodData);
        potSoup.localPosition = new Vector3(potSoup.localPosition.x, GetSoupLevel(), potSoup.localPosition.z);
        
        //CookwareFull
        if (_foodDataList.Count >= maxCapacity)
        {
            // TODO: Check if is a valid recipe
            _cookwareState = CookwareState.RawValidRecipe;
            OnFullAndValidRecipe?.Invoke(this, this);
        }
        
        return true;
    }

    public void StartCook()
    {
        if(_cookwareState == CookwareState.RawValidRecipe)
        {
            _cookwareState = CookwareState.Cooking;
            _cookwareTimer = recipe.cookingTime;
            
            ProgressUI progressUI = Instantiate(_progressUI);
            progressUI.transform.SetParent(GameObject.FindGameObjectWithTag("CanvasInteraction").transform);
            progressUI.Set(this);
            
        }
        else if (_cookwareState == CookwareState.Cooked)
        {
            _cookwareState = CookwareState.CookedCooking;
            _cookwareTimer = 5f;
        }
    }

    public void StopCook()
    {
        if(_cookwareState == CookwareState.Cooking) _cookwareState = CookwareState.RawValidRecipe;
        else if (_cookwareState == CookwareState.CookedCooking) _cookwareState = CookwareState.Cooked;
    }

    public bool CanBeCooked() =>
        (_cookwareState == CookwareState.RawValidRecipe || _cookwareState == CookwareState.Cooked);
    
    private bool IsCooking() => (_cookwareState == CookwareState.Cooking || _cookwareState == CookwareState.CookedCooking);

    public bool TryGetDish(out FoodData foodCooked)
    {
        if (_cookwareState == CookwareState.Cooked)
        {
            foodCooked = recipe.result;
            _foodDataList.Clear();
            _cookwareState = CookwareState.Empty;
            steamParticles.Stop();
            potSoup.localPosition = new Vector3(potSoup.localPosition.x, GetSoupLevel(), potSoup.localPosition.z);
            return true;
        }

        foodCooked = null;
        return false;
    }
}
