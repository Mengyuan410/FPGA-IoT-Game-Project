using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//visual
public class ColdingBar : MonoBehaviour
{
    private Mana mana;
    public Slider slider;

    private void Awake()
    {

        mana = new Mana();
    }

    private void Update()
    {
        mana.Update();

        slider.value = mana.GetManaNormalized();
    }
}

//logic
public class Mana
{
    public const int MANA_MAX = 100;

    private float manaAmount;
    private float manaRegenAmount;
    public Mana() {
        manaAmount = 0;
        manaRegenAmount = 30f;
    }

    public void Update()
    {
        manaAmount += manaRegenAmount * Time.deltaTime;
    }

    public void SpendMana(int amount)
    {
        if(manaAmount >= amount)
        {
            manaAmount -= amount;

        }
    }
    public float GetManaNormalized() {
        return manaAmount / MANA_MAX;
    }

}

