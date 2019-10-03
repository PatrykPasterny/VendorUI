using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendorAppWPF.ViewModels;
using VendorDomain.Classes;
using VendorDomain.Classes.Enums;

namespace VendorAppWPF.Trackers
{
    public class SelectedItemTracker
    {
        public InventoryViewRow SelectedItem { get; set; }
        public string ItemInventoryName { get; set; }
        public SelectedItemTracker()
        {
            SelectedItem = new InventoryViewRow();
        }
    }
}
