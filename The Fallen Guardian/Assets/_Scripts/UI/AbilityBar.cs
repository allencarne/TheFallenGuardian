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

    [SerializeField] Image abilityBar;
    [SerializeField] Color abilityBarColor;

    public void OnPlayerJoin()
    {
        Invoke("GetPlayer", .5f);
    }

    void GetPlayer()
    {
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
        if (!playerAbilities.basicAbilityReference)
        {
            basicAbilityImage.enabled = false;
        }

        if (playerAbilities.basicAbilityReference)
        {
            if (playerEquipment.IsWeaponEquipt)
            {
                LitSprite(basicAbilityImage, beginnerAbilityTree.basicAbilityImage.sprite);
            }
            else
            {
                DimSprite(basicAbilityImage, beginnerAbilityTree.basicAbilityImage.sprite);
            }
        }
    }

    public void OnOffensiveAbilitySelected()
    {
        if (!playerAbilities.offensiveAbilityReference)
        {
            offensiveAbilityImage.enabled = false;
        }

        if (playerAbilities.offensiveAbilityReference)
        {
            if (playerEquipment.IsWeaponEquipt)
            {
                LitSprite(offensiveAbilityImage, beginnerAbilityTree.offensiveAbilityImage.sprite);
            }
            else
            {
                DimSprite(offensiveAbilityImage, beginnerAbilityTree.offensiveAbilityImage.sprite);
            }
        }
    }

    public void OnOffensiveAbility2Selected()
    {
        if (!playerAbilities.offensiveAbilityReference)
        {
            offensiveAbilityImage.enabled = false;
        }

        if (playerAbilities.offensiveAbilityReference)
        {
            if (playerEquipment.IsWeaponEquipt)
            {
                LitSprite(offensiveAbilityImage, beginnerAbilityTree.offensiveAbility2Image.sprite);
            }
            else
            {
                DimSprite(offensiveAbilityImage, beginnerAbilityTree.offensiveAbility2Image.sprite);
            }
        }
    }

    public void OnMobilitySelected()
    {
        if (!playerAbilities.mobilityAbilityReference)
        {
            mobilityAbilityImage.enabled = false;
        }

        if (playerAbilities.mobilityAbilityReference)
        {
            if (playerEquipment.IsWeaponEquipt)
            {
                LitSprite(mobilityAbilityImage, beginnerAbilityTree.mobilityAbilityImage.sprite);
            }
            else
            {
                DimSprite(mobilityAbilityImage, beginnerAbilityTree.mobilityAbilityImage.sprite);
            }
        }
    }

    void OnEquipmentChanged(Equipment newItem, Equipment oldItem)
    {
        OnBasicAbilitySelected();
        OnOffensiveAbilitySelected();
        OnOffensiveAbility2Selected();
        OnMobilitySelected();
    }

    void LitSprite(Image image, Sprite sprite)
    {
        image.enabled = true;
        image.sprite = sprite;
        image.color = Color.white; // Reset color in case it was previously changed
    }

    void DimSprite(Image image, Sprite sprite)
    {
        image.enabled = true;
        image.sprite = sprite;
        Color spriteColor = image.color;
        spriteColor.a = 0.3f; // Set Sprite transparency to half
        image.color = spriteColor;
    }

    public void OnPlayerEnterCombat()
    {
        abilityBar.color = abilityBarColor;
    }

    public void OnPlayerLeaveCombat()
    {
        abilityBar.color = Color.white;
    }
}
