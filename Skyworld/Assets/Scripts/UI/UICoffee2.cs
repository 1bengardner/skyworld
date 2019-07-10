using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICoffee2 : MonoBehaviour
{
    [SerializeField]
    Transform player;
    bool animating;

    void OnEnable()
    {
        animating = false;
        transform.SetAsLastSibling();
    }

    // Use this for initialization
    void Start()
    {
        Coffee coffee = player.GetComponentInChildren<Coffee>();
        if (coffee != null)
        {
            coffee.OnChangeState += ChangeCoffeeState;
        }
        
        gameObject.SetActive(false);
    }

    void ChangeCoffeeState(Coffee.CoffeeState newState)
    {
        if (newState == Coffee.CoffeeState.ReadyToBoost)
        {
            // Reset if animating
            if (animating)
            {
                gameObject.SetActive(false);
            }
            gameObject.SetActive(true);
        }
        else if (newState == Coffee.CoffeeState.Boosting)
        {
            animating = true;
            GetComponent<Animator>().SetTrigger("Use");
        }
        else if (newState == Coffee.CoffeeState.Grounded && !animating)
        {
            Disable();
        }
    }

    void Disable()
    {
        gameObject.SetActive(false);
    }
}
