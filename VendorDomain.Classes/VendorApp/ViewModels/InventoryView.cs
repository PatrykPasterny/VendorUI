using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendorDomain.Classes.Enums;
using VendorDomain.DataModel;
using System.Data.Entity;
using VendorApp.Transactions;
using VendorDomain.Classes;

namespace VendorAppWPF.ViewModels
{
    public class InventoryView
    {
        public InventoryView()
        {
            FullInventoryView = new List<InventoryViewRow>();
        }
        public InventoryView(InventoryOwner owner)
        {

            FullInventoryView = DbTransactionHolder.Context.Equipment.Include(x => x.Product).Include(x => x.Product.DisassemblyProducts).Include(x => x.Inventory)
                                   .Where(x => x.Inventory.Id == (int)owner).Select(x => new InventoryViewRow()
                                   {
                                       ProductId = x.Product.Id,
                                       Price = x.Product.Price,
                                       Name = x.Product.Name,
                                       Quantity = x.Quantity,
                                       Description = x.Product.Description,
                                       TypeExt = x.Product.Type,
                                       DisassemblyProducts = x.Product.DisassemblyProducts                        
                                   }).ToList();

            var a = DbTransactionHolder.Context.Products.Include(x => x.DisassemblyProducts).ToList();
            InventoryMoney = DbTransactionHolder.Context.Inventories.Where(x => x.Id == (int)owner).Select(x => x.Money).FirstOrDefault();
            Owner = owner;
        }
        public List<InventoryViewRow> FullInventoryView;
        public string InventoryName { get; set; }
        public decimal InventoryMoney { get; set; }
        public InventoryOwner Owner { get; set; }
    }
}
