using UnityEngine;
public class Health : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;
    public HealthBar healthBar;

    private void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    void Update()
    {
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        FindObjectOfType<audioManager>().Play("MonsterPain");

        healthBar.SetHealth(currentHealth);
    }

    public void Heal(float amount)
    {
        currentHealth += amount;

        FindObjectOfType<audioManager>().Play("Heal");

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        healthBar.SetHealth(currentHealth);
    }

    void Die()
    {
        Debug.Log("Died");
    }
}
