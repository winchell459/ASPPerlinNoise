using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AIHud : MonoBehaviour
{
    public Text textDisplay;
    public Animal animal;
    public Transform target;

    public void AddHealth()
    {
        animal.health += 1;
        Display();
    }

    public void AddFood()
    {
        animal.food += 1;
        Display();
    }

    public void Display()
    {
        textDisplay.text = $" Health: {animal.health}/{((Dog)animal).maxHealth}  \n Food: {animal.food}/{((Dog)animal).maxFood} \n Exp: {animal.experience}";
        transform.forward = transform.position - target.position;
    }
}
