using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] private GameObject barParent;
    [SerializeField] private Image healthBar;
    [SerializeField] private bool showBar = true;

    public System.Action OnDeath, OnGameObjectDestroy;
    public System.Action<int> OnDamage;
    public System.Action<int> OnHealthChange;

    [SerializeField] public int health = 100;
    public int maxHealth;

    public float HealthPercentage { get { return (float)health / (float)maxHealth; } }

    private Coroutine barCoroutine;

    private bool isDead;
    public bool isInvincible;

    private Color defBarColor;

    public static Color HealthInteractableColor = Color.yellow;

    private bool isInitialized;
    
    //OnDeath+=

    private void Awake()
    {
        Init();
    }

    public void Init()
    {
        if (isInitialized) return;

        isInitialized = true;

        maxHealth = health;

        defBarColor = healthBar.color;

        healthBar.fillAmount = ((float)health / maxHealth);
        barParent.SetActive(false);
    }

    private void OnDestroy()
    {
        if (!gameObject.scene.isLoaded)
        {
            return;
        }

        OnGameObjectDestroy?.Invoke();
    }

    public void Revive()
    {
        isDead = false;

        Heal();
    }

    public void SetHealth(int newHealth)
    {
        maxHealth = newHealth;
        health = newHealth;

        Heal();
    }

    public void Heal(int amount)
    {
        health += amount;
        if (health > maxHealth) health = maxHealth;

        // if (showBar && gameObject.activeInHierarchy)
        // {
        //     healthBar.fillAmount = ((float)health / maxHealth);
        //
        //     if (barCoroutine != null)
        //     {
        //         StopCoroutine(barCoroutine);
        //     }
        //
        //     //barCoroutine = StartCoroutine(BarVisiblity());
        // }

        OnHealthChange?.Invoke(health);
    }

    public void Heal()
    {
        Init();

        health = maxHealth;
        healthBar.fillAmount = ((float)health / maxHealth);
        barParent.SetActive(false);

        OnHealthChange?.Invoke(health);
    }

    public void Damage(int amount)
    {
        if (isDead || isInvincible) return;

        health -= amount;

        OnDamage?.Invoke(amount);
        OnHealthChange?.Invoke(health);

        if (health <= 0 && !isDead)
        {
            health = 0;
            isDead = true;

            OnDeath?.Invoke();
            barParent.SetActive(false);

            return;
        }

        // if(showBar && gameObject.activeInHierarchy)
        // {
        //     healthBar.fillAmount = ((float)health / maxHealth);
        //
        //     if (barCoroutine != null)
        //     {
        //         StopCoroutine(barCoroutine);
        //     }
        //
        //     //barCoroutine = StartCoroutine(BarVisiblity());
        // }
    }

    // public void SetBarColor(Color color)
    // {
    //     healthBar.color = color;
    // }
    //
    // public void ClearBarColor()
    // {
    //     healthBar.color = defBarColor;
    // }
    //
    // public void ShowBar()
    // {
    //     if (barCoroutine != null)
    //     {
    //         StopCoroutine(barCoroutine);
    //     }
    //
    //     barCoroutine = StartCoroutine(BarVisiblity());
    // }
    //
    // private IEnumerator BarVisiblity()
    // {
    //     barParent.SetActive(true);
    //
    //     yield return new WaitForSeconds(1.5f);
    //
    //     barParent.SetActive(false);
    // }
}
