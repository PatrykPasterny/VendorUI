using System;
using System.Windows;
using VendorAppWPF.ViewModels;
using VendorAppWPF.Views;
using VendorDomain.Classes;
using VendorDomain.Classes.Enums;
using VendorDomain.DataModel;

namespace VendorApp.Transactions
{
    public class TransactionHolder
    {
        public InventoryView SellerInventory { get; set; }
        public InventoryView BuyerInventory { get; set; }
        public InventoryViewRow TransactionItem { get; set; }
        private int StackLimit { get; set; }
        public int TransactionQuantity { get; set; }
        public VendorContext CurrentContext { get; set; }
        private bool isQuestItem = false;
        public TransactionHolder(InventoryView seller, InventoryView buyer, InventoryViewRow item, int stackLimit)
        {
            SellerInventory = seller;
            BuyerInventory = buyer;
            TransactionItem = item;
            TransactionQuantity = 0;
            StackLimit = stackLimit;

        }
        public void CompleteTransaction()
        {
            SetTransactionQuantity();
            if (TransactionQuantity < 1)
            {
                return;
            }
            if (isQuestItem)
            {
                MessageBox.Show($"You can't sell {TransactionItem.Name}, because it is Quest Item.", "VendorApp", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (BuyerInventory.InventoryMoney - TransactionQuantity * TransactionItem.Price >= 0)
            {
                CreateTransactionWithTreshold();
            }
            else
            {
                MessageBox.Show($"{BuyerInventory.InventoryName} don't have enough money to buy {TransactionQuantity} {TransactionItem.Name}s.", "VendorApp", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

        }
        private void SetTransactionQuantity()
        {
            switch (TransactionItem.TypeExt)
            {
                case EquipmentType.QuestItems:
                    TransactionQuantity = 1;
                    isQuestItem = SellerInventory.Owner == InventoryOwner.Vendor ? false : true;
                    break;
                case EquipmentType.Weapons:
                    TransactionQuantity = 1;
                    break;
                case EquipmentType.Consumables:
                    SetTransactionQuantityForStackable();
                    break;
                case EquipmentType.CraftingMaterials:
                    SetTransactionQuantityForStackable();
                    break;
            }
        }
        private void SetTransactionQuantityForStackable()
        {

            if (TransactionItem.Quantity > 1)
            {
                var quantitySetterWindow = new QuantityPopup(this);
                quantitySetterWindow.ShowDialog();
            }
            else
            {
                TransactionQuantity = 1;
            }
        }
        public void CreateTransactionWithTreshold()
        {
            SellerInventory.InventoryMoney += TransactionItem.Price * TransactionQuantity;
            BuyerInventory.InventoryMoney -= TransactionItem.Price * TransactionQuantity;
            if (BuyerInventory.FullInventoryView.Find(x => x.ProductId == TransactionItem.ProductId && x.Quantity < StackLimit) is InventoryViewRow buyerItemsOfSoldKind)
            {
                if (buyerItemsOfSoldKind.Quantity + TransactionQuantity > StackLimit)
                {
                    var amountOverStackLimit = buyerItemsOfSoldKind.Quantity + TransactionQuantity - StackLimit;
                    buyerItemsOfSoldKind.Quantity = StackLimit;
                    BuyerInventory.FullInventoryView.Add(new InventoryViewRow(TransactionItem) { Quantity = amountOverStackLimit });
                    DbTransactionHolder.UpdateEquipmentItemQuantity(SellerInventory, buyerItemsOfSoldKind, TransactionItem.Quantity - TransactionQuantity);
                    DbTransactionHolder.UpdateEquipmentItemQuantity(BuyerInventory, buyerItemsOfSoldKind, StackLimit);
                    DbTransactionHolder.AddEquipmentItem(BuyerInventory, TransactionItem, amountOverStackLimit);
                }
                else
                {
                    int newQuantity = buyerItemsOfSoldKind.Quantity + TransactionQuantity;
                    buyerItemsOfSoldKind.Quantity = newQuantity;
                    DbTransactionHolder.UpdateEquipmentItemQuantity(SellerInventory, buyerItemsOfSoldKind, TransactionItem.Quantity - TransactionQuantity);
                    DbTransactionHolder.UpdateEquipmentItemQuantity(BuyerInventory, buyerItemsOfSoldKind, newQuantity);
                }
            }
            else
            {
                BuyerInventory.FullInventoryView.Add(new InventoryViewRow(TransactionItem) { Quantity = TransactionQuantity });
                DbTransactionHolder.UpdateEquipmentItemQuantity(SellerInventory, TransactionItem, TransactionItem.Quantity-TransactionQuantity);
                DbTransactionHolder.AddEquipmentItem(BuyerInventory, TransactionItem, TransactionQuantity);
            }
            TransactionItem.Quantity -= TransactionQuantity;
            if (TransactionItem.Quantity <= 0)
            {
                SellerInventory.FullInventoryView.Remove(TransactionItem);
                DbTransactionHolder.DeleteEquipmentItem(SellerInventory, TransactionItem);
            }
        }
    }
}
