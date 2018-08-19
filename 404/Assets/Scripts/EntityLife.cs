using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityLife : MonoBehaviour
{
    [SerializeField]
    private int maxHealth;

    private int actualHealth;

	public int ActualHealth => actualHealth;

    public void Start()
    {
        actualHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        actualHealth -= damage;

        if (actualHealth <= 0)
        {
            actualHealth = 0;

            Die();
        }
    }

    private void Die()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        if (sr != null)
        {
            sr.color = Color.red;
        }

        Debug.Log(gameObject.name + "died.");
    }

    // TODO : remove
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightControl))
        {
            TakeDamage(1);
        }
    }
}
