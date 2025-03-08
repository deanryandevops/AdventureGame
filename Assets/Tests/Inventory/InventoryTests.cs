using NUnit.Framework;
using UnityEngine;

public class InventoryTests
{
    private Inventory inventory;
    private bool itemAddedEventTriggered;
    private Item testItem;

    // Set up the test environment
    [SetUp]
    public void Setup()
    {
        inventory = new GameObject().AddComponent<Inventory>(); // Create a new Inventory instance
        itemAddedEventTriggered = false;

        // Create a simple test item
        testItem = new Item() { name = "TestItem", sprite = null };

        // Subscribe to the onItemAdded event
        inventory.onItemAdded += OnItemAdded;
    }

    // Clean up the test environment
    [TearDown]
    public void TearDown()
    {
        inventory.onItemAdded -= OnItemAdded; // Unsubscribe from the event
    }

    // The method that will be called when the event is triggered
    private void OnItemAdded(Item item)
    {
        itemAddedEventTriggered = true; // Set flag to true when the event is triggered
        Debug.Log($"{item.name} added to inventory.");
    }

    [Test]
    public void TestOnItemAddedEvent_FiresWhenItemAdded()
    {
        // Act: Add an item to the inventory
        inventory.AddItem(testItem);

        // Assert: Check if the event was triggered
        Assert.IsTrue(itemAddedEventTriggered, "The onItemAdded event was not triggered when an item was added.");
    }

    [Test]
    public void TestOnItemAddedEvent_IsNotTriggeredBeforeItemIsAdded()
    {
        // Assert: Ensure the event hasn't been triggered before adding the item
        Assert.IsFalse(itemAddedEventTriggered, "The onItemAdded event was unexpectedly triggered before adding an item.");
    }
}
