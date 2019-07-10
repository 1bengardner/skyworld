using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICoffee : MonoBehaviour
{
    [SerializeField]
    Transform player;
    Slider slider;
    const float k_magic = 50;
    bool paused;

    void OnEnable()
    {
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
        
        slider = GetComponent<Slider>();
        gameObject.SetActive(false);
    }

    void StartSlider()
    {
        paused = false;
        gameObject.SetActive(true);
    }

    void PauseSlider()
    {
        paused = true;
    }

    void StopSlider()
    {
        gameObject.SetActive(false);
    }

    void ChangeCoffeeState(Coffee.CoffeeState newState)
    {
        if (newState == Coffee.CoffeeState.ReadyToBoost)
        {
            StartSlider();
        }
        else if (newState == Coffee.CoffeeState.Boosted)
        {
            PauseSlider();
        }
        else if (newState == Coffee.CoffeeState.Grounded && !paused)
        {
            StopSlider();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!paused)
        {
            slider.value = k_magic * player.GetComponent<Rigidbody2D>().velocity.y / player.GetComponent<PlayerMovement>().jumpForce;
        }
    }
}
