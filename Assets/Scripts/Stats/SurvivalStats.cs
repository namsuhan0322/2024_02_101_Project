using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class SurvivalStats : MonoBehaviour
{
    [Header("Hunger Settings")]
    public float maxHunger = 100;                       // �ִ� ��ⷮ
    public float currentHunger;                         // ���� ��ⷮ
    public float hungerDecreaseRate = 1;                // �ʴ� ��� ���ҷ�

    [Header("Space Suit Settings")]
    public float maxSuitDurability = 100;               // �ִ� ���ֺ� ������
    public float currentSuitDurability;                 // ���� ���ֺ� ������
    public float harvestingDamage = 5.0f;                // ������ ���ֺ� ������
    public float craftingDamage = 3f;                   // ���۽� ���ֺ� ������

    private bool isGameOver = false;                    // ���� ���� ����
    private bool isPaused = false;                      // �Ͻ� ���� ����
    private float hungerTimer = 0;                      // ��� ���� Ÿ�̸�

    void Start()
    {
        // ���� ���۽� ���ݵ��� �ִ� �� ���·� ����
        currentHunger = maxHunger;
        currentSuitDurability = maxSuitDurability;
    }

    void Update()
    {
        if (isGameOver || isPaused) return;

        hungerTimer += Time.deltaTime;

        if (hungerTimer >= 1.0f)
        {
            currentHunger = Mathf.Max(0, currentHunger - hungerDecreaseRate);
            hungerTimer = 0;

            CheckDeath();
        }
    }

    private void CheckDeath()
    {
        if (currentHunger <= 0 || currentSuitDurability <= 0)
        {
            PlayerDeath();
        }
    }

    // ������ ������ ���ֺ� ������
    public void DamageOnHarvesting()
    {
        if (isGameOver || isPaused) return;

        currentSuitDurability = Mathf.Max(0, currentSuitDurability - harvestingDamage);          // 0�� ���Ϸ� �� �������� ���� ���ؼ�
        CheckDeath();
    }

    // ������ ���۽� ���ֺ� ������
    public void DamageOnCrafting()
    {
        if (isGameOver || isPaused) return;

        currentSuitDurability = Mathf.Max(0, currentSuitDurability - harvestingDamage);          // 0�� ���Ϸ� �� �������� ���� ���ؼ�
        CheckDeath();
    }

    // ���� ����� ��� ȸ��
    public void EatFood(float amount)
    {
        if (isGameOver || isPaused) return;                                                     // ���� ���ᳪ �ߴ� ���¿����� �������� �ʰ�

        currentHunger = Mathf.Min(maxHunger, currentHunger + amount);                           // maxHunger ���� �ѱ��� �ʱ� ����

        if (FloatingTextManager.Instance != null)
        {
            FloatingTextManager.Instance.Show($"��� ȸ�� ���� + {amount}", transform.position + Vector3.up);
        }
    }

    // ���ֺ� ���� (ũ����Ż�� ������ ���� ŰƮ ���)
    public void RepairSuit(float amount)
    {
        if (isGameOver || isPaused) return;                                                     // ���� ���ᳪ �ߴ� ���¿����� �������� �ʰ�

        currentSuitDurability = Mathf.Min(maxSuitDurability, currentSuitDurability + amount);   // maxSuitDurability ���� �ѱ��� �ʱ� ����

        if (FloatingTextManager.Instance != null)
        {
            FloatingTextManager.Instance.Show($"���ֺ� ���� + {amount}", transform.position + Vector3.up);
        }
    }

    private void PlayerDeath()                              // �÷��̾� ��� ó�� üũ �Լ�
    {
        isGameOver = true;
        Debug.Log("�÷��̾� ���!");
        // TODO : ��� ó�� �߰� (���ӿ��� UI, ������ ���)
    }

    public float GetHungerPercentage()                      // ����� % ���� �Լ�
    {
        return (currentHunger / maxHunger) * 100;
    }

    public float GetSuitDurabilityPercentage()              // ��Ʈ % ���� �Լ�
    {
        return (currentSuitDurability / maxSuitDurability) * 100;
    }

    public bool IsGameOver()                                // ���� ���� Ȯ�� �Լ�
    {
        return isGameOver;
    }

    public void ResetStats()        // ���� �Լ� �ۼ� (������ �ʱ�ȭ �뵵)
    {
        isGameOver = false;
        isPaused = false;
        currentHunger = maxHunger;
        currentSuitDurability = maxSuitDurability;
        hungerTimer = 0;
    }
}
