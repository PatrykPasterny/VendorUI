using System;
using System.Collections.Generic;
using VendorDomain.Classes.Interfaces;

namespace VendorDomain.Classes
{
    public class Inventory : IModificationHistory
    {
        public Inventory()
        {
            EquipmentElements = new List<EquipmentElement>();
        }
        public int Id { get; set; }
        public List<EquipmentElement> EquipmentElements { get; set; }
        public decimal Money { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public bool IsDirty { get; set; }
    }
}
