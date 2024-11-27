using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingDetector : MonoBehaviour
{
    public float checkRadius = 3.0f;                                // ������ ���� ����
    private Vector3 lastPosition;                                   // �÷��̾��� ������ ��ġ ���� (�÷��̾ �̵��� ���� ��� �ֺ��� �����ؼ� ������ ȹ��)
    private float moveTreshold = 0.1f;                              // �̵� ���� �Ӱ谪 (�÷��̾ �̵��ؾ� �� �ּ� �Ÿ�)
    private ConstructibleBuilding currentNearbyBuilding;            // ���� ������ �ִ� �Ǽ� ������ �ǹ�
    private BuildingCrafter currentBuildingCrafter;                 // �߰� : ���� �ǹ��� ���� �ý���

    void Start()
    {
        lastPosition = transform.position;
        CheckForBuliding();
    }

    void Update()
    {
        // �÷��̾ ���� �Ÿ� �̻� �̵��ߴ��� üũ
        if (Vector3.Distance(lastPosition, transform.position) > moveTreshold)
        {
            CheckForBuliding();                         // �̵��� ������ üũ
            lastPosition = transform.position;          // ���� ��ġ�� ������ ��ġ�� ������Ʈ
        }

        // ����� �������� �ְ� F Ű�� ������ �� ������ ����
        if (currentNearbyBuilding != null && Input.GetKeyDown(KeyCode.F))
        {
            if (!currentNearbyBuilding.isConstructed)                                           // �ǹ��� �ϼ��� ���� �ʾ��� ��
            {
                currentNearbyBuilding.StartConstruction(GetComponent<PlayerInventory>());       // PlayerInventory�� �����Ͽ� �Ǽ� ���� �Լ� ȣ��
            }
            else if (currentBuildingCrafter != null)
            {
                Debug.Log($"{currentNearbyBuilding.buildingName}�� ���� �޴� ����");
                CraftingUIManager.Instance?.ShowUI(currentBuildingCrafter);                     // �̱������� �����ؼ� UI ǥ�ø� �Ѵ�.
            }
        }
    }

    // �ֺ��� ���� ������ �ƾ��ƿ� �����ϴ� �Լ�
    private void CheckForBuliding()
    { 
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, checkRadius);       // ���� ���� ���� ��� �ݶ��̴��� ã��

        float closestDistance = float.MaxValue;                     // ���� ����� �Ÿ��� �ʱⰪ
        ConstructibleBuilding closestBuilding = null;               // ���� ����� ������ �ʱ�ȭ
        BuildingCrafter closestCrafter = null;                      // �߰�

        foreach (Collider collider in hitColliders)
        {
            ConstructibleBuilding building = collider.GetComponent<ConstructibleBuilding>();            // �ǹ� ����
            if (building != null)                                                                       // ��� �ǹ� ������ ����
            {
                float distance = Vector3.Distance(transform.position, building.transform.position);     // �Ÿ� ���
                if (distance < closestDistance)                                                         // �� ����� �������� �߰� �� ������Ʈ
                {
                    closestDistance = distance;
                    closestBuilding = building;
                    closestCrafter = building.GetComponent<BuildingCrafter>();                          // ���⼭ ũ������ ��������
                }
            }
        }

        if (closestBuilding != currentNearbyBuilding)       // ���� ����� �ǹ��� ����Ǿ��� ���� �޽��� ǥ��
        {
            currentNearbyBuilding = closestBuilding;        // ���� ����� �ǹ� ������Ʈ
            currentBuildingCrafter = closestCrafter;        // �߰�

            if (currentNearbyBuilding != null && !currentNearbyBuilding.isConstructed)
            {
                if (FloatingTextManager.Instance != null)
                {
                    FloatingTextManager.Instance.Show(
                        $"[F]Ű�� {currentNearbyBuilding.buildingName} �Ǽ� (���� {currentNearbyBuilding.requiredTree}) �� �ʿ�",
                        currentNearbyBuilding.transform.position + Vector3.up);
                }
            }
        }
    }
}
