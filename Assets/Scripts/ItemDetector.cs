using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 아이템 종류 정의
public enum ItemType
{
    Crystal,            // 크리스탈
    Plant,              // 식물
    Bush,               // 수풀
    Tree,               // 나무
    VegetableStew,      // 야채 스튜 (허기 회복용)
    FruitSalad,         // 과일 샐러드 (허기 회복용)
    RepairKit           // 수리 키드 (우주복 수리용)
}

public class ItemDetector : MonoBehaviour
{
    public float checkRadius = 3.0f;                // 아이템 감지 범위
    private Vector3 lastPosition;                   // 플레이어의 마지막 위치 저장 (플레이어가 이동이 있을 경우 주변을 감지해서 아이템 획득)
    private float moveTreshold = 0.1f;              // 이동 갑지 임계값 (플레이어가 이동해야 할 최소 거리)
    private CollectibleItem currentNearbyItem;      // 현재 가장 가까이 있는 수집 가능한 아이템

    void Start()
    {
        lastPosition = transform.position;          // 시작 시 현재 위치를 마지막 위치로 설정
        CheckForItems();                            // 초기 아이템 체크 수행
    }

    void Update()
    {
        // 플레이어가 일정 거리 이상 이동했는지 체크
        if (Vector3.Distance(lastPosition, transform.position) > moveTreshold)
        {
            CheckForItems();                        // 이동시 아이템 체크
            lastPosition = transform.position;      // 현재 위치를 마지막 위치로 업데이트
        }

        // 가까운 아이템이 있고 E 키를 눌렀을 때 아이템 수집
        if (currentNearbyItem != null && Input.GetKeyDown(KeyCode.E))
        {
            currentNearbyItem.CollectItem(GetComponent<PlayerInventory>());      // PlayerInventory를 참조하여 아이템 수집
        }
    }

    // 주변의 수집 가능한 아아탬울 감지하는 함수
    private void CheckForItems()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, checkRadius);       // 감지 법위 내의 모든 콜라이더를 찾음

        float closestDistance = float.MaxValue;         // 가장 가까운 거리의 초기값
        CollectibleItem closestItem = null;             // 가장 가까운 아이템 초기화

        // 각 콜라이더를 검사하여 수집 가능한 아이템을 찾음
        foreach (Collider collider in hitColliders)
        {
            CollectibleItem item = collider.GetComponent<CollectibleItem>();            // 아이템을 감지
            if (item != null && item.canCollect)    // 아이템이 있고 수집 가능한지 확인
            {
                float distance = Vector3.Distance(transform.position, item.transform.position); // 거리 계산
                if (distance < closestDistance)     // 더 가까운 아이템을 발견 시 업데이트
                {
                    closestDistance = distance;
                    closestItem = item;
                }
            }
        }

        if (closestItem != currentNearbyItem)       // 가장 가까운 아이템이 변경되었을 때만 메시지 표시
        {
            currentNearbyItem = closestItem;        // 가장 가까운 아이템 업데이트
            if (currentNearbyItem != null)
            {
                Debug.Log($"[E] 키를 눌러 {currentNearbyItem.itemName} 수집");        // 새로운 아이템 수집 메시지 표시
            }
        }
    }
    
    // 감지 범위를 시각적으로 표시
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;                                  // 감지 범위 색상 설정
        Gizmos.DrawWireSphere(transform.position, checkRadius);     // 감지 범위를 구체로 표시
    }
}
