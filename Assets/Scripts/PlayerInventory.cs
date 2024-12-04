using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    private SurvivalStats survivalStats;                // Ŭ���� ����

    // ������ ������ ������ �����ϴ� ����
    public int crystalCount = 0;
    public int plantCount = 0;
    public int bushCount = 0;
    public int treeCount = 0;

    // �߰��� ������
    public int vegetableStewCount = 0;                  // ��ä ��Ʃ ����
    public int fruitSaledCount = 0;                     // ���� ������ ����
    public int repairKitCount = 0;                      // ���� ŰƮ ����

    public void Start()
    {
        survivalStats = GetComponent<SurvivalStats>();
    }

    public void UseItem(ItemType itemType)
    {
        if (GetItemCount(itemType) <= 0)     // �ش� �������� �ִ��� Ȯ��
        {
            return;
        }

        switch (itemType)                   // ������ Ÿ�Կ� ���� ȿ�� ����
        {
            case ItemType.VegetableStew:
                Removeitem(ItemType.VegetableStew, 1);
                survivalStats.EatFood(RecipeList.KitchenRecipes[0].hungerRestoreAmount);
                break;
            case ItemType.FruitSalad:
                Removeitem(ItemType.FruitSalad, 1);
                survivalStats.EatFood(RecipeList.KitchenRecipes[1].hungerRestoreAmount);
                break;
            case ItemType.RepairKit:
                Removeitem(ItemType.RepairKit, 1);
                survivalStats.RepairSuit(RecipeList.WorkbenchRecipes[0].repairAmount);
                break;
        }
    }

    // ���� �������� �Ѳ����� ȹ��
    public void AddItem(ItemType itemType, int amount)
    {
        // amount ��ŭ ������ AddItem ȣ��
        for (int i = 0; i < amount; i++)
        {
            AddItem(itemType);
        }
    }

    // �������� �߰��ϴ� �Լ�, ������ ������ ���� �ش� �������� ������ ������Ŵ
    public void AddItem(ItemType itemType)
    {
        // ������ ������ ���� �ٸ� ���� ����
        switch (itemType)
        {
            case ItemType.Crystal:
                crystalCount++;     // ũ����Ż ���� ����
                Debug.Log($"ũ����Ż ȹ��! ���� ���� : {crystalCount}");              // ���� ũ����Ż ���� ���
                break;
            case ItemType.Plant:
                plantCount++;       // �Ĺ� ���� ����
                Debug.Log($"�Ĺ� ȹ��! ���� ���� : {plantCount}");                      // ���� �Ĺ� ���� ���
                break;
            case ItemType.Bush:
                bushCount++;        // ��Ǯ ���� ����
                Debug.Log($"��Ǯ ȹ��! ���� ���� : {bushCount}");                       // ���� ��Ǯ ���� ���
                break;
            case ItemType.Tree:
                treeCount++;        // ���� ���� ����
                Debug.Log($"���� ȹ��! ���� ���� : {treeCount}");                       // ���� ���� ���� ���
                break;
            case ItemType.VegetableStew:
                vegetableStewCount++;
                Debug.Log($"��ä ��Ʃ ȹ��! ���� ���� : {vegetableStewCount}");         // ���� ��ä ��Ʃ ���� ���
                break;
            case ItemType.FruitSalad:
                fruitSaledCount++;
                Debug.Log($"���� ������ ȹ��! ���� ���� : {fruitSaledCount}");          // ���� ���� ������ ���� ���
                break;
            case ItemType.RepairKit:
                repairKitCount++;
                Debug.Log($"����ŰƮ ȹ��! ���� ���� : {repairKitCount}");              // ���� ����ŰƮ ���� ���
                break;
        }
    }

    // �ƾ����� �����ϴ� �Լ�
    public bool Removeitem(ItemType itemType, int amount = 1)
    {
        // ������ ������ ���� �ٸ� ���� ����
        switch (itemType)
        {
            case ItemType.Crystal:
                if (crystalCount >= amount)         // ������ �ִ� ������ ������� Ȯ��
                {
                    crystalCount -= amount;         // ũ����Ż ���� ����
                    Debug.Log($"ũ����Ż {amount} ���! ���� ���� : {crystalCount}");        // ���� ũ����Ż ���� ���
                    return true;
                }
                break;
            case ItemType.Plant:
                if (plantCount >= amount)           // ������ �ִ� ������ ������� Ȯ��
                {
                    plantCount -= amount;           // �Ĺ� ���� ����
                    Debug.Log($"�Ĺ� {amount} ȹ��! ���� ���� : {plantCount}");              // ���� �Ĺ� ���� ���
                    return true;
                }     
                break;
            case ItemType.Bush:
                if (bushCount >= amount)            // ������ �ִ� ������ ������� Ȯ��
                {
                    bushCount -= amount;            // ��Ǯ ���� ����
                    Debug.Log($"��Ǯ {amount} ȹ��! ���� ���� : {bushCount}");               // ���� ��Ǯ ���� ���
                    return true;
                }
                break;
            case ItemType.Tree:
                if (treeCount >= amount)            // ������ �ִ� ������ ������� Ȯ��
                {
                    treeCount -= amount;            // ���� ���� ����
                    Debug.Log($"���� {amount} ȹ��! ���� ���� : {treeCount}");               // ���� ���� ���� ���
                    return true;
                }
                break;
            case ItemType.VegetableStew:
                if (vegetableStewCount >= amount)       // ������ �ִ� ������ ������� Ȯ��
                {
                    vegetableStewCount -= amount;       // ��ä ��Ʃ ���� ����
                    Debug.Log($"���� {amount} ȹ��! ���� ���� : {vegetableStewCount}");              // ���� ��ä ��Ʃ ���� ���
                    return true;
                }
                break;
            case ItemType.FruitSalad:
                if (fruitSaledCount >= amount)          // ������ �ִ� ������ ������� Ȯ��
                {
                    fruitSaledCount -= amount;          // ���� ������ ���� ����
                    Debug.Log($"���� {amount} ȹ��! ���� ���� : {fruitSaledCount}");                // ���� ���� ������ ���� ���
                    return true;
                }
                break;
            case ItemType.RepairKit:
                if (repairKitCount >= amount)           // ������ �ִ� ������ ������� Ȯ��
                {
                    repairKitCount -= amount;           // ����ŰƮ ���� ����
                    Debug.Log($"���� {amount} ȹ��! ���� ���� : {repairKitCount}");                 // ���� ����ŰƮ ���� ���
                    return true;
                }
                break;
        }

        Debug.Log($"{itemType} �������� �����մϴ�.");
        return false;
    }

    // Ư�� �������� ���� ������ ��ȯ �ϴ� �Լ�
    public int GetItemCount(ItemType itemType)
    {
        switch (itemType)
        {
            case ItemType.Crystal:
                return crystalCount;
            case ItemType.Plant:
                return plantCount;
            case ItemType.Bush:
                return bushCount;
            case ItemType.Tree:
                return treeCount;
            case ItemType.VegetableStew:
                return vegetableStewCount;
            case ItemType.FruitSalad:
                return fruitSaledCount;
            case ItemType.RepairKit:
                return repairKitCount;
            default:
                return 0;
        }
    }

    void Update()
    {
        // I Ű�� �������� �κ��丮 �α� ������ ������
        if (Input.GetKeyDown(KeyCode.I))
        {
            ShowInventory();            // �κ��丮 ��� �Լ� ȣ��
        }
    }

    private void ShowInventory()
    {
        Debug.Log("====�κ��丮====");
        Debug.Log($"ũ����Ż : {crystalCount}��");                     // ũ����Ż ���� ���
        Debug.Log($"�Ĺ� : {plantCount}��");                           // �Ĺ� ���� ���
        Debug.Log($"��Ǯ : {bushCount}��");                            // ��Ǯ ���� ���
        Debug.Log($"���� : {treeCount}��");                            // ���� ���� ���
        Debug.Log($"��ä ��Ʃ : {vegetableStewCount}��");              // ��ä ��Ʃ ���� ���
        Debug.Log($"���� ������ : {fruitSaledCount}��");               // ���� ������ ���� ���
        Debug.Log($"����ŰƮ : {repairKitCount}��");                   // ����ŰƮ ���� ���
        Debug.Log("===============");
    }
}
