using System;

namespace VendorDomain.Classes.Interfaces
{
    public interface IModificationHistory
    {
        DateTime DateCreated { get; set; }
        DateTime DateModified { get; set; }
        bool IsDirty { get; set; }
    }
}
