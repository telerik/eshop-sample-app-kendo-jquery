# Kendo UI eShop App

The E-shop app is built with <a href="https://www.telerik.com/kendo-ui" target="_blank">Kendo UI for jQuery</a> components within ASP.NET Core application. The sample app demonstrates the most common use cases related to online stores – product categories, product lists and details, shopping cart, similar and recently viewed products, user profile, sophisticated product filters, and so on. Besides that, the demo includes integration with <a href="https://www.telerik.com/products/reporting.aspx" target="_blank">Telerik Reporting</a> to showcase how to generate order invoice, print product catalogue, or export favorite products in PDF. The styling is powered by the new built-in <a href="https://docs.telerik.com/kendo-ui/styles-and-layout/sass-themes/overview" target="_blank">Fluent theme</a>, which is available for all Telerik and Kendo UI components.


----------

## Featured Kendo UI widgets

The sample application showcases some of the most popular Kendo UI for jQuery widgets, such as:

 - [Grid][1]
 - [Scrollview][2]
 - [ListView][3]
 - [AutoComplete][4]
 - [TabStrip][5]
 - [Form][6]
 - [Slider][7]
 - [Rating][8]
 - [CheckBoxGroup][9]
 - [RadioGroup][10]
 - [Map][11]
 - [DropDownList][12]
 - [Captcha][13]
 - [Button][14]


  [1]: https://demos.telerik.com/kendo-ui/grid
  [2]: https://demos.telerik.com/kendo-ui/scrollview
  [3]: https://demos.telerik.com/kendo-ui/listview
  [4]: https://demos.telerik.com/kendo-ui/autocomplete
  [5]: https://demos.telerik.com/kendo-ui/tabstrip
  [6]: https://demos.telerik.com/kendo-ui/form
  [7]: https://demos.telerik.com/kendo-ui/slider
  [8]: https://demos.telerik.com/kendo-ui/rating
  [9]: https://demos.telerik.com/kendo-ui/checkboxgroup
  [10]: https://demos.telerik.com/kendo-ui/radiogroup
  [11]: https://demos.telerik.com/kendo-ui/map
  [12]: https://demos.telerik.com/kendo-ui/dropdownlist
  [13]: https://demos.telerik.com/kendo-ui/captcha
  [14]: https://demos.telerik.com/kendo-ui/button
  
## Prerequisites

 - [.NET 6.0][15]
 - [Visual Studio 2022][16]
 - [Microsoft SQL Server 2019][17]

[15]: https://dotnet.microsoft.com/en-us/download/dotnet/6.0
[16]: https://visualstudio.microsoft.com/downloads/
[17]: https://www.microsoft.com/en-us/sql-server/sql-server-downloads

## Running this app

1. [Add the Telerik Nuget feed as a Package Source](https://docs.telerik.com/kendo-ui/intro/installation/nuget-install).
1. Copy the `.bak` file from the `DatabaseFiles` folder to your SQL Server backup location.
1. Restore the sample database through any of the approaches below:
  * Use SQL Server Management Studio(SSMS)—Follow the steps in [Restore to SQL Server article](https://learn.microsoft.com/en-us/sql/samples/adventureworks-install-configure?view=sql-server-ver16&tabs=ssms#restore-to-sql-server). Note: You need to select the `KendoEShop.bak` file and the name of the database should be KendoEShop. 
  * Use the `Transact-SQL (T-SQL)`—Run the `RESTORE DATABASE` command as described in the article linked [here](https://learn.microsoft.com/en-us/sql/samples/adventureworks-install-configure?view=sql-server-ver16&tabs=tsql#restore-to-sql-server). 
1. Open `EShop.sln` with Visual Studio.
1. Open the terminal and enter the `Web` directory (`...\fluent-eshop-jquery\EShop\Web`).
1. Run `npm install` to install the dependencies from the `package.json` file. This step is required to activate the `gulp tasks` defined in the `gulpfile.js` when running the app. 
1. Run the application (Hit `Ctrl` + `F5`).