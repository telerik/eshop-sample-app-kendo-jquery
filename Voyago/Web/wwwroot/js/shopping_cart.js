function getShoppingCartGrid() {
	
	return $("#shoppingCartGrid").data("kendoGrid");
}

function shoppingCartGridOnDataBoundInitial(e) {	
	
}

function shoppingCartGridOnDataBound(e) {
	//$(".templateCell").each(function () {
	//	eval($(this).children("script").last().html());
	//});	

	var grid = e.sender;
	var items = e.sender.items();

	items.each(function (e) {
		var dataItem = grid.dataItem(this);
		var ntb = $(this).find('.quantity-editor');

		$(ntb).kendoNumericTextBox({
			change: updateShoppingCartChanges,
			value: dataItem.Quantity,
			"decimals": 0,
			"format": "\\#",
			"max": 100,
			"min": 1,
			"rounded": "none"
		});
	});
	
	//var rows = this.tbody.children();
	//var dataItems = this.dataSource.view();
	//for (var i = 0; i < dataItems.length; i++) {
	//	kendo.bind(rows[i], dataItems[i]);
	//}

	calculateShoppingCartTotal();
	getShoppingCartItemsCount();
}

function removeItemFromShoppingCart(itemId) {
	var rowToRemove = $("#remove_" + itemId).closest('tr') ;
	var grid = getShoppingCartGrid();
	var refresh = grid.dataSource.data().length == 1;

	grid.removeRow(rowToRemove);
	grid.dataSource.sync();

	if (refresh) {
		location.href = "/Account/ShoppingCart";
    }
}

function updateShoppingCartChanges(e) {	
	var shoppingCartGrid = $('#shoppingCartGrid').data('kendoGrid')
	var currentRow = $(e.sender.element).closest('tr')
	var editDataItem = shoppingCartGrid.dataItem(currentRow);
	var newQtyValue = e.sender.value()
	editDataItem.set('Quantity', newQtyValue)
	editDataItem.set('Total', newQtyValue * editDataItem.ProductPrice)
	getShoppingCartGrid().dataSource.sync();
	debugger;
}

function checkoutShoppingCart() {
	kendo.ui.progress($("#checkoutButton"), true);	
	location.href = "/Account/CheckoutShoppingCart";
}

function calculateShoppingCartTotal() {
	var data = getShoppingCartGrid().dataSource.data();

	var total = 0;
	for (i = 0; i < data.length; ++i) {
		total += data[i].Total;
	}

	total = parseFloat(total).toFixed(2);
	$("#subTotalValue").html("$ " + total);
}

function addProductToShoppingCart(e) {	
	var productId = e.sender.element[0].id.split('_')[1];
	$.post("/Account/AddProductToShoppingCart?productId=" + productId, function () {
		getShoppingCartItemsCount();
	});
}

function getShoppingCartItemsCount() {
	$.get("/Account/GetShoppingCartItemsCount", function (data) {
		$("#shopping-cart .k-badge").data('kendoBadge').text(data);
	});
}