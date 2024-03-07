using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static EquipmentManager;

public class AbilityBar : MonoBehaviour
{
    [SerializeField] GameObjectRuntimeSet playerReference;
    PlayerAbilities playerAbilities;

    [SerializeField] GameObjectRuntimeSet playerInventoryReference;
    EquipmentManager equipmentManager;

    [SerializeField] Image Ability1;
    [SerializeField] Image Ability2;
    [SerializeField] Image Ability3;
    [SerializeField] Image Ability4;
    [SerializeField] Image Ability5;
    [SerializeField] Image Ability6;

    private void Start()
    {
        equipmentManager = playerInventoryReference.GetItemIndex(0).GetComponent<EquipmentManager>();
        equipmentManager.onEquipmentChangedCallback += OnEquipmentChanged;
    }

    public void OnPlayerJoin()
    {
        StartCoroutine(Delay());
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(1);

        if (playerReference.items.Count > 0)
        {
            playerAbilities = playerReference.GetItemIndex(0).GetComponent<PlayerAbilities>();
        }
    }

    void OnEquipmentChanged(Equipment newItem, Equipment oldItem)
    {
        if (newItem == null)
            return;
    }

    private void Update()
    {
        if (playerAbilities)
        {
            ClubSlash clubSlash = playerAbilities.basicAttackBehaviourReference as ClubSlash;
            if (clubSlash != null)
            {
                Ability1.enabled = true;

                Sprite icon = clubSlash.icon;
                Ability1.sprite = icon;
            }
        }
    }
}
