using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GlobalVariable :Singleton<GlobalVariable>
{
    public static readonly Vector3 HANDGUN_MUZZLE_POSITION = new(1.783f, -0.56f);
    public static readonly Vector3 SHOTGUN_MUZZLE_POSITION = new(1.825f, -0.494f);
    public static readonly Vector3 RIFLE_MUZZLE_POSITION = new(1.387f, -0.546f);

    public static readonly Dictionary<GunType, Vector2> MUZZLE_POSITION = new()
    {
        { GunType.Rifle, RIFLE_MUZZLE_POSITION},
        { GunType.ShotGun, SHOTGUN_MUZZLE_POSITION},
        { GunType.HandGun, HANDGUN_MUZZLE_POSITION},
    };
    public GameObject CollectableItems;

    public static Dictionary<GunType, Vector2> GUN_ANIMATOROVERIDER;

    public PlayerReferences playerReferences;

    protected override void OnApplicationQuit()
    {
        playerReferences.playerInventory.clear();
        playerReferences.playerEquipment.clear();
        base.OnApplicationQuit();
    }
}

[Serializable]
public struct PlayerReferences
{
    public Transform playerTransform;

    public Inventory playerInventory;
    public Inventory playerEquipment;

    public InventoryUI inventoryUI;
    public InventoryUI equipmentUI;

    // UI
    public UIHealthBar UIHealthBar;
    public TextMeshProUGUI StatUI;
    public TextMeshProUGUI MeleeStatUI;
    public TextMeshProUGUI RangedStatUI;

    public Vector3 GetPlayerPosition()
    {
        return playerTransform.position;
    }
}

public static class MouseData
{
    public static InventoryUI UI;
    public static GameObject slotBeingDrag;
    public static GameObject slotHover;
    public static InventorySlot highLightSlot;

}
