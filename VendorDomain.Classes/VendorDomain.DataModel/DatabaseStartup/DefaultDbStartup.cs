using System;
using System.Collections.Generic;
using System.Linq;
using VendorDomain.Classes;
using VendorDomain.Classes.Enums;

namespace VendorDomain.DataModel
{
    public static class DefaultDbStartup
    {
        public static void CreateDefaultDb(VendorContext db)
        {
            db.Database.Log = Console.WriteLine;
            var inventory1 = new Inventory
            {
                Money = 500
            };
            var inventory2 = new Inventory
            {
                Money = 1500
            };

            var product0 = new Product
            {
                Name = "Dragon's blood",
                Description = "The blood of a red dragon. Can be used to bless weapons.",
                Type = EquipmentType.CraftingMaterials,
                Price = 250
            };
            var product1 = new Product
            {
                Name = "Iron ore",
                Description = "Metallic iron can be extracted from this rocks.",
                Type = EquipmentType.CraftingMaterials,
                Price = 5
            };
            var product2 = new Product
            {
                Name = "Iron bar",
                Description = "Melted from iron ore. Can be forged into a weapon by gifted blacksmith.",
                Type = EquipmentType.CraftingMaterials,
                Price = 20,
                DisassemblyProducts = new List<Product>() { product1 }
            };
            var product14 = new Product
            {
                Name = "Dragon ore",
                Description = "Dragon's metal can be extracted from this rocks.",
                Type = EquipmentType.CraftingMaterials,
                Price = 15
            };
            var product15 = new Product
            {
                Name = "Dragon bar",
                Description = "Melted from dragon ore. Can be forged into a dragon sword by gifted blacksmith.",
                Type = EquipmentType.CraftingMaterials,
                Price = 50,
                DisassemblyProducts = new List<Product>() { product14 }
            };
            var product3 = new Product
            {
                Name = "Dragon Sword",
                Description = "One-handed sword blessed with dragon's blood.",
                Type = EquipmentType.Weapons,
                Price = 400,
                DisassemblyProducts = new List<Product>() { product15 }
            };
            var product4 = new Product
            {
                Name = "Snake's venom",
                Description = "Weakens human and makes running impossible.",
                Type = EquipmentType.CraftingMaterials,
                Price = 175
            };
            var product5 = new Product
            {
                Name = "Poisoned Claws",
                Description = "Daggers created to kill enemies from the shadows. If spotted, the poison will slow down the opponent and let you run away.",
                Type = EquipmentType.Weapons,
                Price = 300,
                DisassemblyProducts = new List<Product>() { product4, product2 }
            };
            var product6 = new Product
            {
                Name = "Water",
                Description = "Well known liquid.",
                Type = EquipmentType.Consumables,
                Price = 1
            };
            var product7 = new Product
            {
                Name = "Flour",
                Description = "Wheat flour.",
                Type = EquipmentType.Consumables,
                Price = 2
            };
            var product8 = new Product
            {
                Name = "Bread",
                Description = "Wheat, healthy bread from the local bakery.",
                Type = EquipmentType.Consumables,
                Price = 5,
                DisassemblyProducts = new List<Product>() { product6, product7 }
            };
            var product9 = new Product
            {
                Name = "Bandage",
                Description = "Can be used to bandage scratches.",
                Type = EquipmentType.CraftingMaterials,
                Price = 10
            };
            var product10 = new Product
            {
                Name = "First aid",
                Description = "Some bandages and painkillers for light wounds.",
                Type = EquipmentType.Consumables,
                Price = 50,
                DisassemblyProducts = new List<Product>() { product9 }
            };

            var product11 = new Product
            {
                Name = "Leather",
                Description = "Tanned cow skin.",
                Type = EquipmentType.CraftingMaterials,
                Price = 35
            };
            var product12 = new Product
            {
                Name = "Armour of Destiny",
                Description = "Armour created by gods for The Choosen One.",
                Type = EquipmentType.QuestItems,
                Price = 1
            };
            var product13 = new Product
            {
                Name = "Mysterious book",
                Description = "Contains text written in a forgotten language.",
                Type = EquipmentType.QuestItems,
                Price = 1
            };

            db.Inventories.AddRange(new List<Inventory>() { inventory1, inventory2 });
            db.Products.AddRange(new List<Product>() { product0, product1, product2, product14, product15, product3, product4, product5, product6, product7, product8, product9, product10, product11, product12, product13, });
            db.SaveChanges();

            var equipment0 = new EquipmentElement()
            {
                Inventory = inventory1,
                Product = product3,
                Quantity = 1
            };
            var equipment1 = new EquipmentElement()
            {
                Inventory = inventory1,
                Product = product8,
                Quantity = 5
            };
            var equipment2 = new EquipmentElement()
            {
                Inventory = inventory1,
                Product = product10,
                Quantity = 3
            };
            var equipment3 = new EquipmentElement()
            {
                Inventory = inventory1,
                Product = product2,
                Quantity = 5
            };
            var equipment4 = new EquipmentElement()
            {
                Inventory = inventory1,
                Product = product11,
                Quantity = 4
            };
            var equipment5 = new EquipmentElement()
            {
                Inventory = inventory1,
                Product = product0,
                Quantity = 2
            };
            var equipment6 = new EquipmentElement()
            {
                Inventory = inventory2,
                Product = product5,
                Quantity = 1
            };
            var equipment7 = new EquipmentElement()
            {
                Inventory = inventory2,
                Product = product8,
                Quantity = 20
            };
            var equipment8 = new EquipmentElement()
            {
                Inventory = inventory2,
                Product = product10,
                Quantity = 20
            };
            var equipment9 = new EquipmentElement()
            {
                Inventory = inventory2,
                Product = product2,
                Quantity = 10
            };
            var equipment10 = new EquipmentElement()
            {
                Inventory = inventory2,
                Product = product11,
                Quantity = 7
            };
            var equipment11 = new EquipmentElement()
            {
                Inventory = inventory2,
                Product = product4,
                Quantity = 2
            };
            var equipment12 = new EquipmentElement()
            {
                Inventory = inventory2,
                Product = product0,
                Quantity = 3
            };
            var equipment13 = new EquipmentElement()
            {
                Inventory = inventory2,
                Product = product12,
                Quantity = 1
            };
            var equipment14 = new EquipmentElement()
            {
                Inventory = inventory2,
                Product = product13,
                Quantity = 1
            };
            db.Equipment.AddRange(new List<EquipmentElement>() { equipment0, equipment1, equipment2, equipment3, equipment4, equipment5,
                                                                     equipment6, equipment7, equipment8, equipment9, equipment10, equipment11, equipment12, equipment13, equipment14 });
            db.SaveChanges();

        }
    }
}
