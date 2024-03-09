using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityBar : MonoBehaviour
{
    [Header("Trees")]
    [SerializeField] BeginnerAbilityTree beginnerAbilityTree;

    [Header("References")]
    [SerializeField] GameObjectRuntimeSet playerReference;
    PlayerEquipment playerEquipment;
    PlayerAbilities playerAbilities;
    [SerializeField] GameObjectRuntimeSet inventoryReference;
    EquipmentManager equipmentManager;

    [Header("Images")]
    [SerializeField] Image basicAbilityImage;
    [SerializeField] Image offensiveAbilityImage;
    [SerializeField] Image mobilityAbilityImage;
    [SerializeField] Image defensiveAbilityImage;
    [SerializeField] Image utilityAbilityImage;
    [SerializeField] Image ultimateAbilityImage;

    public void OnPlayerJoin()
    {
        StartCoroutine(Delay());
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(.5f);

        if (playerReference.items.Count > 0)
        {
            playerEquipment = playerReference.GetItemIndex(0).GetComponent<PlayerEquipment>();
            playerAbilities = playerReference.GetItemIndex(0).GetComponent<PlayerAbilities>();

        }

        if (inventoryReference.items.Count > 0)
        {
            equipmentManager = inventoryReference.GetItemIndex(0).GetComponent<EquipmentManager>();
            equipmentManager.onEquipmentChangedCallback += OnEquipmentChanged;
        }
    }

    // This Method is Called From An Event - BeginnerAbilityTree
    public void OnBasicAbilitySelected()
    {
        if (!playerAbilities.basicAttackBehaviourReference)
        {
            basicAbilityImage.enabled = false;
        }

        if (playerAbilities.basicAttackBehaviourReference)
        {
            if (playerEquipment.IsWeaponEquipt)
            {
                LitSprite();
            }
            else
            {
                DimSprite();
            }
        }
    }

    void OnEquipmentChanged(Equipment newItem, Equipment oldItem)
    {
        OnBasicAbilitySelected();
    }

    void LitSprite()
    {
        basicAbilityImage.enabled = true;
        basicAbilityImage.sprite = beginnerAbilityTree.basicAbilityImage.sprite;

        // Reset color in case it was previously changed
        basicAbilityImage.color = Color.white;
    }

    void DimSprite()
    {
        basicAbilityImage.enabled = true;
        basicAbilityImage.sprite = beginnerAbilityTree.basicAbilityImage.sprite;

        // Set Sprite transparency to half
        Color spriteColor = basicAbilityImage.color;
        spriteColor.a = 0.3f;
        basicAbilityImage.color = spriteColor;
    }
}
