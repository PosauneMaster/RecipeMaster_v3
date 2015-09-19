using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BendSheets
{
    [Serializable]
    public class RecipeTemplateItem : IEquatable<RecipeTemplateItem>
    {
        public string SendName
        { get; set; }

        public string ReceiveName
        { get; set; }

        public RecipeTemplateItemType ItemType
        { get; set; }

        public string CellStart
        { get; set; }

        public string CellEnd
        { get; set; }

        public RecipeTemplateItem() { }

        public RecipeTemplateItem(string sendPrefix, string receivePrefix, string name, RecipeTemplateItemType itemType, string cellStart, string cellEnd)
        {
            SendName = sendPrefix + "." + name;
            ReceiveName = receivePrefix + "." + name;
            ItemType = itemType;
            CellStart = cellStart;
            CellEnd = cellEnd;
        }

        #region IEquatable<RecipeTemplateItem> Members

        public bool Equals(RecipeTemplateItem other)
        {
            return SendName.Equals(other.SendName) && ItemType.Equals(other.ItemType) && CellStart.Equals(other.CellStart) && CellEnd.Equals(other.CellEnd);
        }

        public override string ToString()
        {
            return String.Format("Send Name: {0}, ReceiveName: {1}, Item Type: {2}, Cell Start: {3}, Cell End: {4}", SendName, ReceiveName, ItemType, CellStart, CellEnd);
        }

        #endregion
    }

    public enum RecipeTemplateItemType
    {
        System,
        Cell,
        Range
    }
}
