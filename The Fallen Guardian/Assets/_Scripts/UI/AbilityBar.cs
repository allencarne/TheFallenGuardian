using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityBar : MonoBehaviour
{
    [SerializeField] BeginnerAbilityTree AbilityTree;

    [SerializeField] GameObjectRuntimeSet playerReference;
    PlayerEquipment playerEquipment;
    PlayerAbilities playerAbilities;

    [SerializeField] GameObjectRuntimeSet inventoryReference;
    EquipmentManager equipmentManager;

    [SerializeField] Image Ability1;
    [SerializeField] Image Ability2;
    [SerializeField] Image Ability3;
    [SerializeField] Image Ability4;
    [SerializeField] Image Ability5;
    [SerializeField] Image Ability6;

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

    public void UpdateAbility1()
    {
        if (!playerAbilities.basicAttackBehaviourReference)
        {
            Ability1.enabled = false;
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
        UpdateAbility1();
    }

    void LitSprite()
    {
        Ability1.enabled = true;
        Ability1.sprite = AbilityTree.level1AbilityImage.sprite;
        Ability1.color = Color.white; // Reset color in case it was previously changed
    }

    void DimSprite()
    {
        Ability1.enabled = true;
        Ability1.sprite = AbilityTree.level1AbilityImage.sprite;

        // Set Sprite transparency to half
        Color spriteColor = Ability1.color;
        spriteColor.a = 0.3f;
        Ability1.color = spriteColor;
    }
}
