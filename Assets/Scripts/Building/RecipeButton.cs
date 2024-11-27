using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RecipeButton : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI recipeName;                      // 레시피 이름
    public TextMeshProUGUI materialsText;                   // 필요 재료 텍스트
    public Button craftButton;                              // 제작 버튼

    private CraftingRecipe recipe;                          // 레시피 데이터
    private BuildingCrafter crafter;                        // 건물의 제작 시스템
    private PlayerInventory playerInventory;                // 플레이어 인벤토리

    public void Setup(CraftingRecipe recipe, BuildingCrafter crafter)
    {
        this.recipe = recipe;
        this.crafter = crafter;
        playerInventory = FindFirstObjectByType<PlayerInventory>();

        recipeName.text = recipe.itemName;                          // 레시피 정보 표시

        craftButton.onClick.AddListener(OnCraftButtonCliked);       // 제작 버튼에 이벤트 연결
    }

    private void UpdateMaterialsText()                              // 재료 정보 업데이트
    {
        string materials = "필요 재료 :\n";
        for (int i = 0; i < recipe.requiredItems.Length; i++)
        {
            ItemType item = recipe.requiredItems[i];
            int required = recipe.requiredAmounts[i];
            int has = playerInventory.GetItemCount(item);
            materials += $"{item} : {has}/{required}\n";
        }
        materialsText.text = materials;
    }

    private void OnCraftButtonCliked()                              // 제작 버튼 클릭 처리
    {
        crafter.TryCraft(recipe, playerInventory);
        UpdateMaterialsText();
    }
}
