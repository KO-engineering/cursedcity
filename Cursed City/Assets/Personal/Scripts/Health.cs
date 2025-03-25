using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public UnityEvent OnDie;
    public int maxHp = 100;
    public int hp = 100;
    public Image healthBar;

    bool dead = false;

    void Update()
    {
        if(healthBar != null)
            healthBar.fillAmount = Mathf.InverseLerp(0, maxHp, hp);
    }

    public void ChangeHealth(int amount)
    {
        hp += amount;

        if (hp == 0 && !dead)
        {
            dead = true;
            hp = 0;
            OnDie?.Invoke();
            KillCount.Instance.killCount += 1;
        }

    }
}
