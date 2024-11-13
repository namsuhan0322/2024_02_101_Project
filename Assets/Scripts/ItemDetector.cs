using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ������ ���� ����
public enum ItemType
{
    Crystal,            // ũ����Ż
    Plant,              // �Ĺ�
    Bush,               // ��Ǯ
    Tree                // ����
}

public class ItemDetector : MonoBehaviour
{
    public float checkRadius = 3.0f;                // ������ ���� ����
    private Vector3 lastPosition;                   // �÷��̾��� ������ ��ġ ���� (�÷��̾ �̵��� ���� ��� �ֺ��� �����ؼ� ������ ȹ��)
    private float moveTreshold = 0.1f;              // �̵� ���� �Ӱ谪 (�÷��̾ �̵��ؾ� �� �ּ� �Ÿ�)
    private CollectibleItem currentNearbyItem;      // ���� ���� ������ �ִ� ���� ������ ������

    void Start()
    {
        lastPosition = transform.position;          // ���� �� ���� ��ġ�� ������ ��ġ�� ����
        CheckForItems();                            // �ʱ� ������ üũ ����
    }

    void Update()
    {
        // �÷��̾ ���� �Ÿ� �̻� �̵��ߴ��� üũ
        if (Vector3.Distance(lastPosition, transform.position) > moveTreshold)
        {
            CheckForItems();                        // �̵��� ������ üũ
            lastPosition = transform.position;      // ���� ��ġ�� ������ ��ġ�� ������Ʈ
        }

        // ����� �������� �ְ� E Ű�� ������ �� ������ ����
        if (currentNearbyItem != null && Input.GetKeyDown(KeyCode.E))
        {
            currentNearbyItem.CollectItem(GetComponent<PlayerInventory>());      // PlayerInventory�� �����Ͽ� ������ ����
        }
    }

    // �ֺ��� ���� ������ �ƾ��ƿ� �����ϴ� �Լ�
    private void CheckForItems()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, checkRadius);       // ���� ���� ���� ��� �ݶ��̴��� ã��

        float closestDistance = float.MaxValue;         // ���� ����� �Ÿ��� �ʱⰪ
        CollectibleItem closestItem = null;             // ���� ����� ������ �ʱ�ȭ

        // �� �ݶ��̴��� �˻��Ͽ� ���� ������ �������� ã��
        foreach (Collider collider in hitColliders)
        {
            CollectibleItem item = collider.GetComponent<CollectibleItem>();            // �������� ����
            if (item != null && item.canCollect)    // �������� �ְ� ���� �������� Ȯ��
            {
                float distance = Vector3.Distance(transform.position, item.transform.position); // �Ÿ� ���
                if (distance < closestDistance)     // �� ����� �������� �߰� �� ������Ʈ
                {
                    closestDistance = distance;
                    closestItem = item;
                }
            }
        }

        if (closestItem != currentNearbyItem)       // ���� ����� �������� ����Ǿ��� ���� �޽��� ǥ��
        {
            currentNearbyItem = closestItem;        // ���� ����� ������ ������Ʈ
            if (currentNearbyItem != null)
            {
                Debug.Log($"[E] Ű�� ���� {currentNearbyItem.itemName} ����");        // ���ο� ������ ���� �޽��� ǥ��
            }
        }
    }
    
    // ���� ������ �ð������� ǥ��
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;                                  // ���� ���� ���� ����
        Gizmos.DrawWireSphere(transform.position, checkRadius);     // ���� ������ ��ü�� ǥ��
    }
}
