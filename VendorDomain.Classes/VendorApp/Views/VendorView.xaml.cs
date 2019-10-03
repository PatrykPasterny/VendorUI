using System;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using VendorApp.Transactions;
using VendorApp.Views;
using VendorAppWPF.Trackers;
using VendorAppWPF.ViewModels;
using VendorDomain.Classes;
using VendorDomain.Classes.Enums;
using VendorDomain.DataModel;

namespace VendorAppWPF.Views
{
    public partial class VendorView : Window
    {
        public SelectedItemTracker ItemTracker = new SelectedItemTracker();
        public InventoryView UserInventoryView { get; set; }
        public InventoryView VendorInventoryView { get; set; }
        public VendorView()
        {
            SetInventories();
            InitializeComponent();
            FillGrids();
            ChangeMoneyAmount();
            UserName.Text = UserInventoryView.InventoryName;
            VendorName.Text = VendorInventoryView.InventoryName;
        }

        private void SetInventories()
        {
            UserInventoryView = new InventoryView(InventoryOwner.User)
            {
                InventoryName = "UserInventory"
            };
            VendorInventoryView = new InventoryView(InventoryOwner.Vendor)
            {
                InventoryName = "VendorInventory"
            };
        }


        private void FillGrids()
        {
            UserInventory.ItemsSource = UserInventoryView.FullInventoryView.Select(x => new
            {
                x.Type,
                x.Name,
                x.Price,
                x.Quantity,
                x.TotalPrice
            });
            VendorInventory.ItemsSource = VendorInventoryView.FullInventoryView.Select(x => new
            {
                x.Type,
                x.Name,   
                x.Price,
                x.Quantity,
                x.TotalPrice
            });
        }

        private void ChangeMoneyAmount()
        {
            UserMoney.Text = UserInventoryView.InventoryMoney.ToString("C", CultureInfo.GetCultureInfo("en-US"));
            VendorMoney.Text = VendorInventoryView.InventoryMoney.ToString("C", CultureInfo.GetCultureInfo("en-US"));
        }

        private void DetermineClickedInventory(object sender, MouseButtonEventArgs e)
        {
            if (sender is DataGrid dataGrid)
            {
                ItemTracker.ItemInventoryName = dataGrid.Name;
            }
        }

        private void ShowDataDescription(object sender, RoutedEventArgs e)
        {               
            if (sender is DataGridRow dataGridRow)
            {
                if (ItemTracker.ItemInventoryName.StartsWith("User"))
                {
                    ItemTracker.SelectedItem = UserInventoryView.FullInventoryView[dataGridRow.GetIndex()];
                    SellButton.IsEnabled = true;
                    BuyButton.IsEnabled = false;
                    DisassemblyButton.Visibility = Visibility.Visible;
                    ConsumeButton.Visibility = Visibility.Visible;
                    DetermineEnablityOfConsumeAndDisassemblyButtons();
                }
                else
                {
                    BuyButton.IsEnabled = true;
                    SellButton.IsEnabled = false;
                    DisassemblyButton.Visibility = Visibility.Collapsed;
                    ConsumeButton.Visibility = Visibility.Collapsed;
                    ItemTracker.SelectedItem = VendorInventoryView.FullInventoryView[dataGridRow.GetIndex()];
                }
              
                ItemDescription.Text = $"Item Type: {ItemTracker.SelectedItem.TypeExt} \n\nDescription: {ItemTracker.SelectedItem.Description}";
            }
        }

        private void DetermineEnablityOfConsumeAndDisassemblyButtons()
        {
            switch (ItemTracker.SelectedItem.TypeExt)
            {
                case EquipmentType.QuestItems:
                    DisassemblyButton.IsEnabled = false;
                    ConsumeButton.IsEnabled = false;
                    break;
                case EquipmentType.Weapons:
                case EquipmentType.CraftingMaterials:
                    DisassemblyButton.IsEnabled = ItemTracker.SelectedItem.Disassembable;
                    ConsumeButton.IsEnabled = false;
                    break;
                case EquipmentType.Consumables:
                    DisassemblyButton.IsEnabled = ItemTracker.SelectedItem.Disassembable;
                    ConsumeButton.IsEnabled = true;
                    break;

            }
        }

        private void SellItem(object sender, RoutedEventArgs e)
        {
            TransactionHolder sellTransaction = new TransactionHolder(seller: UserInventoryView, buyer: VendorInventoryView, item: ItemTracker.SelectedItem, stackLimit: ItemTracker.SelectedItem.StackLimit);
            sellTransaction.CompleteTransaction();

            ChangeMoneyAmount();
            FillGrids();
            DisassemblyButton.IsEnabled = false;
            ConsumeButton.IsEnabled = false;
            ItemDescription.Text = string.Empty;
            SellButton.IsEnabled = false;
        }

        private void BuyItem(object sender, RoutedEventArgs e)
        {
            TransactionHolder buyTransaction = new TransactionHolder(seller: VendorInventoryView, buyer: UserInventoryView, item: ItemTracker.SelectedItem, stackLimit: ItemTracker.SelectedItem.StackLimit);

            buyTransaction.CompleteTransaction();

            ChangeMoneyAmount();
            FillGrids();
            DisassemblyButton.IsEnabled = false;
            ConsumeButton.IsEnabled = false;
            ItemDescription.Text = string.Empty;
            BuyButton.IsEnabled = false;
        }

        private void VendorInventoryLostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is DataGrid dataGrid)
            {
                dataGrid.SelectedIndex = -1;
                DisassemblyButton.Visibility = Visibility.Collapsed;
                ConsumeButton.Visibility = Visibility.Collapsed;
                ItemDescription.Text = string.Empty;
            }
        }

        private void ConsumeItem(object sender, RoutedEventArgs e)
        {
            var consumedItem = ItemTracker.SelectedItem;
            if (consumedItem.Quantity > 1)
            {
                consumedItem.Quantity -= 1;
                DbTransactionHolder.UpdateEquipmentItemQuantity(UserInventoryView, consumedItem, consumedItem.Quantity);           
            }
            else
            {
                UserInventoryView.FullInventoryView.Remove(consumedItem);
                DbTransactionHolder.DeleteEquipmentItem(UserInventoryView, consumedItem);
            }
            DisassemblyButton.Visibility = Visibility.Collapsed;
            ConsumeButton.Visibility = Visibility.Collapsed;
            SellButton.IsEnabled = false;
            ItemDescription.Text = string.Empty;
            ChangeMoneyAmount();
            FillGrids();
        }
        private void DisassembleItem(object sender, RoutedEventArgs e)
        {
            var disassemblerWindow = new DisassemblingPopup(ItemTracker);
            disassemblerWindow.ShowDialog();

            if (disassemblerWindow.shouldDisassembleItem)
            {
                DbTransactionHolder.UpdateEquipmentItemQuantity(UserInventoryView, ItemTracker.SelectedItem, ItemTracker.SelectedItem.Quantity - 1);
                ItemTracker.SelectedItem.Quantity -= 1;
                foreach (var product in ItemTracker.SelectedItem.DisassemblyProducts)
                {
                    if (UserInventoryView.FullInventoryView.Find(x => x.ProductId == product.Id && x.Quantity < x.StackLimit) is InventoryViewRow inventoryElement)
                    {
                        inventoryElement.Quantity += 1;
                        DbTransactionHolder.UpdateEquipmentItemQuantity(UserInventoryView, inventoryElement, inventoryElement.Quantity);
                    }
                    else
                    {
                        inventoryElement = new InventoryViewRow(product);
                        UserInventoryView.FullInventoryView.Add(inventoryElement);
                        DbTransactionHolder.AddEquipmentItem(UserInventoryView, inventoryElement, 1);
                    }                                     
                }
            }

            if (ItemTracker.SelectedItem.Quantity <= 0)
            {
                UserInventoryView.FullInventoryView.Remove(ItemTracker.SelectedItem);
                DbTransactionHolder.DeleteEquipmentItem(UserInventoryView, ItemTracker.SelectedItem);
            }
            DisassemblyButton.Visibility = Visibility.Collapsed;
            ConsumeButton.Visibility = Visibility.Collapsed;
            SellButton.IsEnabled = false;
            ItemDescription.Text = string.Empty;
            FillGrids();
        }
        private void InventorySorting(object sender, DataGridSortingEventArgs e)
        {
            if (sender is DataGrid dataGrid)
            {
                var sortingLabel = (string)e.Column.Header;
                if (dataGrid.Name.StartsWith("User"))
                {
                    var sortedInventory = UserInventoryView.FullInventoryView.OrderBy(x => typeof(InventoryViewRow).GetProperty(sortingLabel).GetValue(x)).ToList();
                    UserInventoryView.FullInventoryView = sortedInventory;
                }
                else
                {
                    var sortedInventory = VendorInventoryView.FullInventoryView.OrderBy(x => typeof(InventoryViewRow).GetProperty(sortingLabel).GetValue(x)).ToList();
                    VendorInventoryView.FullInventoryView = sortedInventory;
                }

                dataGrid.SelectedIndex = -1;
                DisassemblyButton.Visibility = Visibility.Collapsed;
                ConsumeButton.Visibility = Visibility.Collapsed;
                SellButton.IsEnabled = false;
                BuyButton.IsEnabled = false;
                ItemDescription.Text = string.Empty;
                FillGrids();
            }
        }
        private void SetDefaultDb(object sender, RoutedEventArgs e)
        {
            DbTransactionHolder.TruncateEquipmentTable();
            DbTransactionHolder.DefaultDbCreator();

            ItemDescription.Text = string.Empty;
            DbTransactionHolder.UpdateDatabase = null;
            SetInventories();
            ChangeMoneyAmount();
            FillGrids();
        }
        private void SetLargeDb(object sender, RoutedEventArgs e)
        {
            DbTransactionHolder.TruncateEquipmentTable();
            DbTransactionHolder.DefaultDbCreator();
            DbTransactionHolder.AddLargeNumOfDefaultEquipmentItem();

            ItemDescription.Text = string.Empty;
            DbTransactionHolder.UpdateDatabase = null;
            ItemDescription.Text = string.Empty;
            SetInventories();
            ChangeMoneyAmount();
            FillGrids();
        }
        private void CloseVendorApp(object sender, RoutedEventArgs e)
        {
            DbTransactionHolder.Context.Database.Log = Console.WriteLine;
            DbTransactionHolder.UpdateInventoryMoney(UserInventoryView);
            DbTransactionHolder.UpdateInventoryMoney(VendorInventoryView);
            DbTransactionHolder.UpdateDatabase += () => Console.WriteLine("Database Updated.");
            try
            {
                DbTransactionHolder.UpdateDatabase.Invoke();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "VendorApp", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            Close();
        }
    }
}
