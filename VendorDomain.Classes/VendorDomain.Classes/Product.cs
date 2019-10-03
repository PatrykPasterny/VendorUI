using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using VendorDomain.Classes.Interfaces;

namespace VendorDomain.Classes
{
    public class Product : IModificationHistory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public EquipmentType Type { get; set; }
        public decimal Price { get; set; }
        public List<Product> DisassemblyProducts { get; set; }
        public bool Disassemblable { get => !(DisassemblyProducts is null) && DisassemblyProducts.Count > 0; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public bool IsDirty { get; set; }
    }
}
