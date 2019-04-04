// ***********************************************************************
// Assembly         : Zeroit.Framework.PictureBox
// Author           : ZEROIT
// Created          : 12-20-2018
//
// Last Modified By : ZEROIT
// Last Modified On : 12-20-2018
// ***********************************************************************
// <copyright file="FlagCheckedList.cs" company="Zeroit Dev Technologies">
//     Copyright © Zeroit Dev Technologies  2017. All Rights Reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
#region Imports

using System;
using System.ComponentModel;
//using System.Windows.Forms.VisualStyles;
using System.Windows.Forms;

#endregion

namespace Zeroit.Framework.PictureBox
{

    #region FlagCheckedListBox
    /// <summary>
    /// Control inheriting from <see cref="CheckedListBox" /> to
    /// show and select values from a Flags enumeration.
    /// </summary>
    /// <seealso cref="System.Windows.Forms.CheckedListBox" />
	public class ZeroitFlagCheckedListBox : CheckedListBox
    {
        #region Fields

        /// <summary>
        /// The components
        /// </summary>
        private System.ComponentModel.Container components = null;
        /// <summary>
        /// The is updating check states
        /// </summary>
        private bool _isUpdatingCheckStates = false;
        /// <summary>
        /// The enum type
        /// </summary>
        private Type _enumType;
        /// <summary>
        /// The enum value
        /// </summary>
        private Enum _enumValue;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        public ZeroitFlagCheckedListBox()
        {
            this.CheckOnClick = true;
        }

        #endregion

        #region Overridden from CheckedListBox

        /// <summary>
        /// Frees used resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                    components.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Handles the <see cref="CheckedListBox.ItemCheck" /> event.
        /// </summary>
        /// <param name="e">Event arguments</param>
        protected override void OnItemCheck(ItemCheckEventArgs e)
        {
            base.OnItemCheck(e);

            if (_isUpdatingCheckStates)
                return;

            // Get the checked/unchecked item
            ZeroitFlagCheckedListBoxItem item = Items[e.Index] as ZeroitFlagCheckedListBoxItem;
            // Update other items
            UpdateCheckedItems(item, e.NewValue);
        }

        #endregion

        #region Public interface

        /// <summary>
        /// Adds a new item to the list.
        /// </summary>
        /// <param name="value">Value of the new item.</param>
        /// <param name="caption">Caption of the new item.</param>
        /// <returns>The new item.</returns>
        public ZeroitFlagCheckedListBoxItem Add(int value, string caption)
        {
            ZeroitFlagCheckedListBoxItem item = new ZeroitFlagCheckedListBoxItem(value, caption);
            Items.Add(item);
            return item;
        }

        /// <summary>
        /// Adds an item to the list.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>The item.</returns>
        public ZeroitFlagCheckedListBoxItem Add(ZeroitFlagCheckedListBoxItem item)
        {
            Items.Add(item);
            return item;
        }

        /// <summary>
        /// Gets the current bit value corresponding to all checked items
        /// </summary>
        /// <returns>System.Int32.</returns>
        public int GetCurrentValue()
        {
            int sum = 0;

            for (int i = 0; i < Items.Count; i++)
            {
                ZeroitFlagCheckedListBoxItem item = Items[i] as ZeroitFlagCheckedListBoxItem;

                if (GetItemChecked(i))
                    sum |= item.Value;
            }

            return sum;
        }

        /// <summary>
        /// Gets or sets the current enumeration value.
        /// </summary>
        /// <value>The enum value.</value>
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Hidden)]
        public Enum EnumValue
        {
            get
            {
                object e = Enum.ToObject(_enumType, GetCurrentValue());
                return (Enum)e;
            }
            set
            {

                Items.Clear();
                _enumValue = value; // Store the current enum value
                _enumType = value.GetType(); // Store enum type
                FillEnumMembers(); // Add items for enum members
                ApplyEnumValue(); // Check/uncheck items depending on enum value
            }
        }

        #endregion

        #region Privates

        /// <summary>
        /// Updates the checked items.
        /// </summary>
        /// <param name="value">The value.</param>
        private void UpdateCheckedItems(int value)
        {
            _isUpdatingCheckStates = true;

            // Iterate over all items
            for (int i = 0; i < Items.Count; i++)
            {
                ZeroitFlagCheckedListBoxItem item = Items[i] as ZeroitFlagCheckedListBoxItem;

                if (item.Value == 0)
                {
                    SetItemChecked(i, value == 0);
                }
                else
                {
                    // If the bit for the current item is on in the bitvalue, check it
                    if ((item.Value & value) == item.Value && item.Value != 0)
                        SetItemChecked(i, true);
                    else // Otherwise uncheck it
                        SetItemChecked(i, false);
                }
            }

            _isUpdatingCheckStates = false;
        }

        /// <summary>
        /// Updates the checked items.
        /// </summary>
        /// <param name="composite">The composite.</param>
        /// <param name="cs">The cs.</param>
        private void UpdateCheckedItems(ZeroitFlagCheckedListBoxItem composite, CheckState cs)
        {
            // If the value of the item is 0, call directly.
            if (composite.Value == 0)
                UpdateCheckedItems(0);

            // Get the total value of all checked items
            int sum = 0;
            for (int i = 0; i < Items.Count; i++)
            {
                ZeroitFlagCheckedListBoxItem item = Items[i] as ZeroitFlagCheckedListBoxItem;

                // If item is checked, add its value to the sum.
                if (GetItemChecked(i))
                    sum |= item.Value;
            }

            // If the item has been unchecked, remove its bits from the sum
            if (cs == CheckState.Unchecked)
                sum = sum & (~composite.Value);
            else // If the item has been checked, combine its bits with the sum
                sum |= composite.Value;

            // Update all items in the checklistbox based on the final bit value
            UpdateCheckedItems(sum);
        }

        // Adds items to the checklistbox based on the members of the enum
        /// <summary>
        /// Fills the enum members.
        /// </summary>
        private void FillEnumMembers()
        {
            foreach (string name in Enum.GetNames(_enumType))
            {
                object val = Enum.Parse(_enumType, name);
                int intVal = (int)Convert.ChangeType(val, typeof(int));

                Add(intVal, name);
            }
        }

        /// <summary>
        /// Applies the enum value.
        /// </summary>
        private void ApplyEnumValue()
        {
            int intVal = (int)Convert.ChangeType(_enumValue, typeof(int));
            UpdateCheckedItems(intVal);
        }

        #endregion
    }
    #endregion


}
