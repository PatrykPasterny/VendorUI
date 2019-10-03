using System.Globalization;
using System.Windows;
using VendorApp.Transactions;

namespace VendorAppWPF.Views
{
    public partial class QuantityPopup : Window
    {
        public TransactionHolder CurrentTransaction;

        public QuantityPopup(TransactionHolder currentTransaction)
        {
            CurrentTransaction = currentTransaction;
            InitializeComponent();
            QuantitySlider.Minimum = 1;
            QuantitySlider.Maximum = currentTransaction.TransactionItem.Quantity;
            QuantityPrice.Text = currentTransaction.TransactionItem.Price.ToString("C", CultureInfo.GetCultureInfo("en-US"));
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            CurrentTransaction.TransactionQuantity = (int)QuantitySlider.Value;
            Close();
        }

        private void QuantitySlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            QuantityPrice.Text = ((decimal)QuantitySlider.Value * CurrentTransaction.TransactionItem.Price).ToString("C", CultureInfo.GetCultureInfo("en-US"));
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
