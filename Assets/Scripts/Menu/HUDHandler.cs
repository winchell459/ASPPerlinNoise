using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDHandler : MonoBehaviour
{
    public Text debugText;
    [SerializeField] private string debugMessage;
    [SerializeField] private string timer;
    [SerializeField] private int chickenCount, chickenDead, spiderCount;

    public void SetTimer(string value)
    {
        timer = value;
        Display();
    }

    public void DisplayTimer(bool display)
    {
        debugText.gameObject.SetActive(display);
    }

    public virtual void Debug(string message)
    {

        debugMessage += message + "\n";
    }

    protected virtual void Display()
    {
        debugText.gameObject.SetActive(true);
        debugText.text = $"{timer} \n Chickens: {chickenCount} \n Spiders: {spiderCount} \n Kills: {chickenDead} \n {debugMessage} ";
    }

    public void SetCounts(int chickenCount, int chickenDead, int spiderCount)
    {
        this.chickenCount = chickenCount;
        this.chickenDead = chickenDead;
        this.spiderCount = spiderCount;
    }
    public void SetCounts(int chickenAdded, int spiderAdded)
    {
        if (chickenAdded > 0) chickenCount += chickenAdded;
        else
        {
            chickenCount += chickenAdded;
            chickenDead -= chickenAdded;
        }
        if (spiderAdded > 0) spiderCount += spiderAdded;
    }
}
