using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;
using VendorAppWPF.ViewModels;
using VendorDomain.Classes;
using VendorDomain.Classes.Enums;
using VendorDomain.DataModel;

namespace VendorApp.Transactions
{
    public static class DbTransactionHolder
    {
        public delegate void UpdateDatabaseDelegate();
        public static VendorContext Context { get; set; }
        public static UpdateDatabaseDelegate UpdateDatabase { get; set; }
        private static List<EquipmentElement> DefaultDbEquipment { get; set; }
        private static List<EquipmentElement> LargeDbEquipment { get; set; }
        static DbTransactionHolder()
        {
            Context = new VendorContext();
            DefaultDbEquipment = new List<EquipmentElement>();
            LargeDbEquipment = new List<EquipmentElement>();
            SetDefaultEquipmentLists();
            UpdateDatabase += () => Console.WriteLine("Begin Updating Database");
        }

        public static void AddEquipmentItem(InventoryView inventory, InventoryViewRow item, int quantity)
        {
            UpdateDatabase += () => {
                Context.Equipment.Add(new EquipmentElement()
                {
                    Inventory = Context.Inventories.Where(x => x.Id == (int)inventory.Owner).FirstOrDefault(),
                    Product = Context.Products.Where(x => x.Id == item.ProductId).FirstOrDefault(),
                    Quantity = quantity
                });
                Context.SaveChanges();
            };
        }
        public static void UpdateEquipmentItemQuantity(InventoryView inventory, InventoryViewRow item, int newQuantity)
        {
            UpdateDatabase += () =>
            {
                var itemToUpdate = Context.Equipment.Where(x => x.Inventory.Id == (int)inventory.Owner && x.Product.Id == item.ProductId).FirstOrDefault();
                itemToUpdate.Quantity = newQuantity;
                Context.SaveChanges();
            };
        }
        public static void DeleteEquipmentItem(InventoryView inventory, InventoryViewRow item)
        {
            UpdateDatabase += () =>
            {
                var itemToRemove = Context.Equipment.Where(x => x.Inventory.Id == (int)inventory.Owner && x.Inventory.Id == (int)inventory.Owner && x.Product.Id == item.ProductId).FirstOrDefault();
                Context.Equipment.Remove(itemToRemove);
                Context.SaveChanges();
            };
        }
        public static void UpdateInventoryMoney(InventoryView inventory)
        {
            UpdateDatabase += () =>
            {
                var inventoryToUpdate = Context.Inventories.Where(x => x.Id == (int)inventory.Owner).FirstOrDefault();
                inventoryToUpdate.Money = inventory.InventoryMoney;
                Context.SaveChanges();
            };
        }
        static public void TruncateEquipmentTable()
        {
            Context.Database.ExecuteSqlCommand("TRUNCATE TABLE dbo.EquipmentElements");
        }
        static public void DefaultDbCreator()
        {

            Context.Database.Log = Console.WriteLine;
            Context.Equipment.AddRange(DefaultDbEquipment);

            Context.Inventories.Where(x => x.Id == (int)InventoryOwner.User).FirstOrDefault().Money = 500;
            Context.Inventories.Where(x => x.Id == (int)InventoryOwner.Vendor).FirstOrDefault().Money = 1500;
            Context.SaveChanges();

        }
        public static void AddLargeNumOfDefaultEquipmentItem()
        {
            Context.Equipment.AddRange(LargeDbEquipment);
            Context.SaveChanges();

        }

        private static void SetDefaultEquipmentLists()
        {
                var equipment0 = new EquipmentElement()
                {
                    Inventory = Context.Inventories.Where(x => x.Id == (int)InventoryOwner.User).FirstOrDefault(),
                    Product = Context.Products.Where(x => x.Id == 1).FirstOrDefault(),
                    Quantity = 1
                };
                var equipment1 = new EquipmentElement()
                {
                    Inventory = Context.Inventories.Where(x => x.Id == (int)InventoryOwner.User).FirstOrDefault(),
                    Product = Context.Products.Where(x => x.Id == 8).FirstOrDefault(),
                    Quantity = 5
                };
                var equipment2 = new EquipmentElement()
                {
                    Inventory = Context.Inventories.Where(x => x.Id == (int)InventoryOwner.User).FirstOrDefault(),
                    Product = Context.Products.Where(x => x.Id == 11).FirstOrDefault(),
                    Quantity = 3
                };
                var equipment3 = new EquipmentElement()
                {
                    Inventory = Context.Inventories.Where(x => x.Id == (int)InventoryOwner.User).FirstOrDefault(),
                    Product = Context.Products.Where(x => x.Id == 5).FirstOrDefault(),
                    Quantity = 5
                };
                var equipment4 = new EquipmentElement()
                {
                    Inventory = Context.Inventories.Where(x => x.Id == (int)InventoryOwner.User).FirstOrDefault(),
                    Product = Context.Products.Where(x => x.Id == 14).FirstOrDefault(),
                    Quantity = 4
                };
                var equipment5 = new EquipmentElement()
                {
                    Inventory = Context.Inventories.Where(x => x.Id == (int)InventoryOwner.User).FirstOrDefault(),
                    Product = Context.Products.Where(x => x.Id == 13).FirstOrDefault(),
                    Quantity = 2
                };
                var equipment6 = new EquipmentElement()
                {
                    Inventory = Context.Inventories.Where(x => x.Id == (int)InventoryOwner.Vendor).FirstOrDefault(),
                    Product = Context.Products.Where(x => x.Id == 4).FirstOrDefault(),
                    Quantity = 1
                };
                var equipment7 = new EquipmentElement()
                {
                    Inventory = Context.Inventories.Where(x => x.Id == (int)InventoryOwner.Vendor).FirstOrDefault(),
                    Product = Context.Products.Where(x => x.Id == 8).FirstOrDefault(),
                    Quantity = 20
                };
                var equipment8 = new EquipmentElement()
                {
                    Inventory = Context.Inventories.Where(x => x.Id == (int)InventoryOwner.Vendor).FirstOrDefault(),
                    Product = Context.Products.Where(x => x.Id == 11).FirstOrDefault(),
                    Quantity = 20
                };
                var equipment9 = new EquipmentElement()
                {
                    Inventory = Context.Inventories.Where(x => x.Id == (int)InventoryOwner.Vendor).FirstOrDefault(),
                    Product = Context.Products.Where(x => x.Id == 14).FirstOrDefault(),
                    Quantity = 10
                };
                var equipment10 = new EquipmentElement()
                {
                    Inventory = Context.Inventories.Where(x => x.Id == (int)InventoryOwner.Vendor).FirstOrDefault(),
                    Product = Context.Products.Where(x => x.Id == 5).FirstOrDefault(),
                    Quantity = 7
                };
                var equipment11 = new EquipmentElement()
                {
                    Inventory = Context.Inventories.Where(x => x.Id == (int)InventoryOwner.Vendor).FirstOrDefault(),
                    Product = Context.Products.Where(x => x.Id == 13).FirstOrDefault(),
                    Quantity = 2
                };
                var equipment12 = new EquipmentElement()
                {
                    Inventory = Context.Inventories.Where(x => x.Id == (int)InventoryOwner.Vendor).FirstOrDefault(),
                    Product = Context.Products.Where(x => x.Id == 7).FirstOrDefault(),
                    Quantity = 3
                };
                var equipment13 = new EquipmentElement()
                {
                    Inventory = Context.Inventories.Where(x => x.Id == (int)InventoryOwner.Vendor).FirstOrDefault(),
                    Product = Context.Products.Where(x => x.Id == 15).FirstOrDefault(),
                    Quantity = 1
                };
                var equipment14 = new EquipmentElement()
                {
                    Inventory = Context.Inventories.Where(x => x.Id == (int)InventoryOwner.Vendor).FirstOrDefault(),
                    Product = Context.Products.Where(x => x.Id == 16).FirstOrDefault(),
                    Quantity = 1
                };
                DefaultDbEquipment.AddRange(new List<EquipmentElement>() { equipment0, equipment1, equipment2, equipment3, equipment4, equipment5,
                                                                     equipment6, equipment7, equipment8, equipment9, equipment10, equipment11, equipment12, equipment13, equipment14 });
            for (int i = 0; i < 1000; i++)
            {
                LargeDbEquipment.Add(new EquipmentElement()
                {
                    Inventory = Context.Inventories.Where(x => x.Id == (int)InventoryOwner.User).FirstOrDefault(),
                    Product = Context.Products.Where(x => x.Id == 1).FirstOrDefault(),
                    Quantity = 1
                });
            }
        }
    }
}
