using System;
using System.Collections.Generic;
using System.Linq;
using VendorDomain.Classes;
using VendorDomain.Classes.Enums;

namespace VendorAppWPF.ViewModels
{
    public class InventoryViewRow
    {
        public InventoryViewRow() { }
        public InventoryViewRow(Product p)
        {
            ProductId = p.Id;
            Name = p.Name;
            Price = p.Price;
            Quantity = 1;
            TypeExt = p.Type;
            Description = p.Description;
            DisassemblyProducts = p.DisassemblyProducts is null ? new List<Product>() : new List<Product>(p.DisassemblyProducts);
            
        }
        public InventoryViewRow(InventoryViewRow ivr)
        {
            Name = ivr.Name;
            Description = ivr.Description;
            Price = ivr.Price;
            Quantity = ivr.Quantity;
            TypeExt = ivr.TypeExt;
            ProductId = ivr.ProductId;
            DisassemblyProducts = ivr.DisassemblyProducts is null ? new List<Product>() : new List<Product>(ivr.DisassemblyProducts);
        }
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Type { get => new string(TypeExt.ToString().Where(x=> char.IsUpper(x)).ToArray()); }
        public EquipmentType TypeExt { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get => Quantity * Price; }
        public string Description { get; set; }
        public int StackLimit {  get => SetStackLimit(); }
        public List<Product> DisassemblyProducts { get; set; }
        public bool Disassembable { get => !(DisassemblyProducts is null) && DisassemblyProducts.Count() != 0; }

        public int SetStackLimit()
        {
            switch (TypeExt)
            {
                case EquipmentType.Weapons:
                case EquipmentType.QuestItems:
                    return (int)StackLimitForItemType.WeaponsAndQuestItems;

                case EquipmentType.Consumables:
                    return (int)StackLimitForItemType.Consumables;                   
                case EquipmentType.CraftingMaterials:
                    return (int)StackLimitForItemType.CraftingMaterials;
                default:
                    return 1;
            }
        }
    }
}
