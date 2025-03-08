using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class Inventory : MonoBehaviour
{
    [SerializeField] private List<Image> itemImages = new List<Image>();
    [SerializeField] private List<Item> items = new List<Item>();

    // Event system for item addition/removal
    public event Action<Item> onItemAdded;
    public event Action<Item> onItemRemoved;

    public IReadOnlyList<Item> Items => items; // Read-only property to expose items safely

    public static int NumItemSlots { get; internal set; }

    /// <summary>
    /// Finds the first available empty slot in the inventory.
    /// </summary>
    /// <returns>The index of the first empty slot, or -1 if inventory is full.</returns>
    private int FindEmptySlot() => items.FindIndex(item => item == null);

    /// <summary>
    /// Finds the index of a specific item in the inventory.
    /// </summary>
    /// <param name="item">The item to search for.</param>
    /// <returns>The index of the item, or -1 if not found.</returns>
    private int FindItemIndex(Item item) => items.FindIndex(i => i == item);

    /// <summary>
    /// Adds an item to the inventory in the first available empty slot.
    /// </summary>
    /// <param name="itemToAdd">The item to add.</param>
    public void AddItem(Item itemToAdd)
    {
        if (itemToAdd == null)
        {
            Debug.LogWarning($"Attempted to add a null item to inventory.");
            return;
        }

        int index = FindEmptySlot();
        if (index == -1)
        {
            // Inventory is full, so add a new slot for the item
            items.Add(itemToAdd);
            itemImages.Add(null); // You should add an Image manually in the Inspector or dynamically at runtime
            index = items.Count - 1; // Set index to the new slot
        }
        else
        {
            items[index] = itemToAdd;
        }

        // Update the UI to reflect the added item
        UpdateItemImage(index, itemToAdd.sprite, true);

        // Trigger the onItemAdded event
        onItemAdded?.Invoke(itemToAdd);
    }

    /// <summary>
    /// Removes an item from the inventory.
    /// </summary>
    /// <param name="itemToRemove">The item to remove.</param>
    public void RemoveItem(Item itemToRemove)
    {
        if (itemToRemove == null)
        {
            Debug.LogWarning($"Attempted to remove a null item from inventory.");
            return;
        }

        int index = FindItemIndex(itemToRemove);
        if (index == -1)
        {
            Debug.LogWarning($"Item not found in inventory: {itemToRemove.name}");
            return;
        }

        items[index] = null;
        UpdateItemImage(index, null, false);

        // Trigger the onItemRemoved event
        onItemRemoved?.Invoke(itemToRemove);
    }

    /// <summary>
    /// Updates the UI to reflect item changes in the inventory.
    /// </summary>
    /// <param name="index">The inventory slot index to update.</param>
    /// <param name="sprite">The sprite to display (or null to clear).</param>
    /// <param name="enabled">Whether the image component should be enabled.</param>
    private void UpdateItemImage(int index, Sprite sprite, bool enabled)
    {
        if (itemImages == null || itemImages.Count <= index || itemImages[index] == null)
        {
            Debug.LogWarning($"Attempted to update an invalid inventory UI slot: {index}");
            return;
        }

        itemImages[index].sprite = sprite;
        itemImages[index].enabled = enabled;
    }
}
