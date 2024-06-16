using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine.UIElements;
using UnityEngine;

[Serializable]
public class ItemDetails
{
    public string Name;
    public string GUID;
    public Sprite Icon;
    public bool CanDrop;
}

public enum InventoryChangeType
{
    Pickup,
    Drop
}

namespace Assets.WUG.Scripts
{


    public class InventoryUIController : MonoBehaviour
    {
        public List<InventorySlot> InventoryItems = new List<InventorySlot>();
        public Inventory inventory;
        private VisualElement m_Root;
        private VisualElement m_SlotContainer;
        private static VisualElement m_GhostIcon;

        private static bool m_IsDragging;
        private static InventorySlot m_OriginalSlot;

        private void Awake()
        {
            //Store the root from the UI Document component
            m_Root = GetComponent<UIDocument>().rootVisualElement;
            m_GhostIcon = m_Root.Query<VisualElement>("GhostIcon");

            inventory = GameObject.FindFirstObjectByType<Inventory>();

            //Search the root for the SlotContainer Visual Element
            m_SlotContainer = m_Root.Q<VisualElement>("inventory");

            //Create 20 InventorySlots and add them as children to the SlotContainer
            for (int i = 0; i < 24; i++)
            {
                InventorySlot item = new InventorySlot();

                InventoryItems.Add(item);

                m_SlotContainer.Add(item);
            }

            //Register event listeners
            Global.Save();

            m_GhostIcon.RegisterCallback<PointerMoveEvent>(OnPointerMove);
            m_GhostIcon.RegisterCallback<PointerUpEvent>(OnPointerUp);
        }

        /// <summary>
        /// Initiate the drag
        /// </summary>
        /// <param name="position">Mouse Position</param>
        /// <param name="originalSlot">Inventory Slot that the player has selected</param>
        public static void StartDrag(Vector2 position, InventorySlot originalSlot)
        {
            //Set tracking variables
            m_IsDragging = true;
            m_OriginalSlot = originalSlot;

            //Set the new position
            m_GhostIcon.style.top = position.y - m_GhostIcon.layout.height / 2;
            m_GhostIcon.style.left = position.x - m_GhostIcon.layout.width / 2;

            //Set the image
            //m_GhostIcon.style.backgroundImage = GameController.GetItemByGuid(originalSlot.ItemGuid).Icon.texture;

            //Flip the visibility on
            m_GhostIcon.style.visibility = Visibility.Visible;

        }

        /// <summary>
        /// Perform the drag
        /// </summary>
        private void OnPointerMove(PointerMoveEvent evt)
        {
            //Only take action if the player is dragging an item around the screen
            if (!m_IsDragging)
            {
                return;
            }

            //Set the new position
            m_GhostIcon.style.top = evt.position.y - m_GhostIcon.layout.height / 2;
            m_GhostIcon.style.left = evt.position.x - m_GhostIcon.layout.width / 2;

        }

        /// <summary>
        /// Finish the drag and compute whether the item should be moved to a new slot
        /// </summary>
        private void OnPointerUp(PointerUpEvent evt)
        {

            if (!m_IsDragging)
            {
                return;
            }

            //Check to see if they are dropping the ghost icon over any inventory slots.
            IEnumerable<InventorySlot> slots = InventoryItems.Where(x => x.worldBound.Overlaps(m_GhostIcon.worldBound));

            //Found at least one
            if (slots.Count() != 0)
            {
                InventorySlot closestSlot = slots.OrderBy(x => Vector2.Distance(x.worldBound.position, m_GhostIcon.worldBound.position)).First();

                //Set the new inventory slot with the data
                closestSlot.HoldItem(inventory.getByName(m_OriginalSlot.ItemGuid));

                //Clear the original slot
                m_OriginalSlot.DropItem();
            }
            //Didn't find any (dragged off the window)
            else
            {
                m_OriginalSlot.Icon.image = Inventory.getIconByName(m_OriginalSlot.ItemGuid);
            }


            //Clear dragging related visuals and data
            m_IsDragging = false;
            m_OriginalSlot = null;
            m_GhostIcon.style.visibility = Visibility.Hidden;
        }

        /// <summary>
        /// Listen for changes to the players inventory and act
        /// </summary>
        /// <param name="itemGuid">Reference ID for the Item Database</param>
        /// <param name="change">Type of change that occurred. This could be extended to handle drop logic.</param>
        private void GameController_OnInventoryChanged(string[] itemGuid, InventoryChangeType change)
        {
            //Loop through each item and if it has been picked up, add it to the next empty slot
            foreach (string item in itemGuid)
            {
                if (change == InventoryChangeType.Pickup)
                {
                    var emptySlot = InventoryItems.FirstOrDefault(x => x.ItemGuid.Equals(""));

                    if (emptySlot != null)
                    {
                        emptySlot.HoldItem(inventory.getByName(item));
                    }
                }
            }
        }
    }
}
