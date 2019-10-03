using System;
using System.Text;
using System.Windows;
using VendorAppWPF.Trackers;
using VendorAppWPF.ViewModels;

namespace VendorApp.Views
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class DisassemblingPopup : Window
    {
        public SelectedItemTracker ItemToDisassemble { get; set; }
        public bool shouldDisassembleItem { get; set; }
        public DisassemblingPopup(SelectedItemTracker item)
        {
            InitializeComponent();
            ItemToDisassemble = item;
            SetTextBoxes();
        }

        private void SetTextBoxes()
        {
            InfoBox.Text = $"You are trying to disassemble {ItemToDisassemble.SelectedItem.Name}. Below the product of disassembly are listed. Are you sure you want to complete the disassemble process?";

            StringBuilder disassemblyProductsInfo = new StringBuilder();
            for (int i = 0; i < ItemToDisassemble.SelectedItem.DisassemblyProducts.Count; i++)
            {
                disassemblyProductsInfo.Append($"- {ItemToDisassemble.SelectedItem.DisassemblyProducts[i].Name}\n");
            }
            ItemComponents.Text = disassemblyProductsInfo.ToString();
        }

        private void NoButtonClick(object sender, RoutedEventArgs e)
        {
            shouldDisassembleItem = false;
            Close();
        }

        private void YesButtonClick(object sender, RoutedEventArgs e)
        {
            shouldDisassembleItem = true;
            Close();
        }
    }
}
