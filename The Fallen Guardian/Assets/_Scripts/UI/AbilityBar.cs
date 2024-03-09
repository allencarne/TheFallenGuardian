using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class AbilityBar : MonoBehaviour
{
    [SerializeField] BeginnerAbilityTree AbilityTree;

    [SerializeField] GameObjectRuntimeSet playerReference;
    PlayerEquipment playerEquipment;

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

        }

        if (inventoryReference.items.Count > 0)
        {
            equipmentManager = inventoryReference.GetItemIndex(0).GetComponent<EquipmentManager>();
            equipmentManager.onEquipmentChangedCallback += OnEquipmentChanged;
        }
    }

    public void UpdateAbility1()
    {
        if (playerEquipment != null)
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
        if (oldItem != null && oldItem is Weapon)
        {
            Debug.Log("Weapon Un-Equipped");

            if (Ability1.enabled == true)
            {
                DimSprite();
            }
        }
        else if (newItem != null)
        {
            Weapon equippedWeapon = newItem as Weapon;

            if (equippedWeapon != null)
            {
                Debug.Log("Weapon Equipped");

                if (Ability1.enabled == true)
                {
                    LitSprite();
                }
            }
        }
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
        spriteColor.a = 0.5f; // 0 is completely transparent, 1 is completely opaque
        Ability1.color = spriteColor;
    }
}
