using System;
using System.ComponentModel.DataAnnotations;
using VendorDomain.Classes.Interfaces;

namespace VendorDomain.Classes
{
    public class EquipmentElement : IModificationHistory
    {
        public int Id { get; set; }
        [Required]
        public Inventory Inventory { get; set; }
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get => Quantity * Product.Price; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public bool IsDirty { get; set; }

    }
}
