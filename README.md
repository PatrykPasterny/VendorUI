# VendorUI

*** The Azure based DB version of app named VendorUIAzureBaseRelease.zip works fine, was tested on computer without MS SQL Server 2017, the only need is at least 4.7.2 .Net Framework version installed on your device***



The Microsoft SQL Server 2017 should be installed on the user device when running the default version of app using localDB. For this version of app I added .msi instalation file, it shoild let open app on any other pc device.

VendorUI is a RPG type shop created using C#, EF6 and WPF.
In the ReleaseVendorUI.zip you can find App.exe file with all other files needed to run App.exe on other device. In the VendorDomain.Classes you can find all the project written in VisualStudio2019 and .sln file to set up project on other devices.



4 different types of items can be sold, bought, consumed, disassembled and sorted. The amount of objects in both directories can be very large, without visible performance loss. To test it you can click 'SetLargeDatabase' button on the main window. Set default database brings you back to the first states of User and Vendor's equipment. When you set up new Database using this buttons it is recommended to reload the program to reload the database, because otherwise a primary key conflicts may occur.
