function onRecentlyViewedDataBound(e) {
	hideRecentlyViewedIfEmpty(e);
	bindFavouritesAndCartButtones();
	distinguishFavorites();
}

function hideRecentlyViewedIfEmpty(e) {
	var ds = e.sender.dataSource;
	if (ds.total() == 0) {
		$(".recently-viewed").hide();
	}
}

function onCategoryDataBound(e) {
	if (this.dataSource.total() == 0) {
		$(".no-product-msg").show();
	} else $(".no-product-msg").hide();
	bindFavouritesAndCartButtones();
	bindSortByDdl();
	distinguishFavorites();
}

function distinguishFavorites() {
	var favButtons = $("[id^=addToFavoritesButton_]");
	favButtons.each(function () {
		var currentButton = $(this);
		var icon = currentButton.find(".k-icon");
		var productId = this.id.split("_")[1];

		var getUrl = window.location.href.indexOf('fluent-eshop-voyago') > 0 ? "/fluent-eshop-voyago/Account/ProductIsInFavorites?productId=" : "/Account/ProductIsInFavorites?productId=";
		$.get(getUrl + productId, function (data) {
			if (data) {
				if (currentButton.find(".k-button-text")) {
					currentButton.find(".k-button-text").text("Added to favorites");
				}
				icon.removeClass("k-i-heart-outline");
				icon.addClass("k-i-heart");
			}
		});
	});
}

function showResultCount(e) {
	var count = e.sender.dataSource.total();
	$("#resultCount").html(count + " results in ");
}

function addFilterCheckBoxes() {
	$("#discountPicker").kendoRadioGroup({
		change: toggleDiscountPicker,
		items: [{
			"label": "All",
			"value": "1"
		}, {
			"label": "Discounted items only",
			"value": "2"
		}],
		value: "1"
	});

	$("#ratingPicker").kendoCheckBoxGroup({
		"change": filterDataSource,
		"items": [{
			"cssClass": "rating-5",
			"value": "5"
		}, {
			"cssClass": "rating-4",
			"value": "4"
		}, {
			"cssClass": "rating-3",
			"value": "3"
		}, {
			"cssClass": "rating-2",
			"value": "2"
		}, {
			"cssClass": "rating-1",
			"value": "1"
		}]
	});

	//add rating component
	for (i = 1; i <= 5; ++i) {
		$(".rating-" + i).append("<input id='rating" + i + "'></input>");
		$("#rating" + i).kendoRating({
			readonly: true,
			label: false,
			min: 1,
			max: 5,
			value: Number(i) - 0.5
		});
	}

	$("#priceSlider").kendoRangeSlider({
		"change": filterDataSource,
		"largeStep": 1000,
		"max": 4000,
		"min": 0,
		"orientation": "horizontal",
		"selectionEnd": 4000,
		"selectionStart": 0,
		"smallStep": 500,
		"tickPlacement": "both"
	});

	$("#weightSlider").kendoRangeSlider({
		"change": filterDataSource,
		"largeStep": 500,
		"max": 1500,
		"min": 0,
		"orientation": "horizontal",
		"selectionEnd": 1500,
		"selectionStart": 0,
		"smallStep": 250,
		"tickPlacement": "both"
	});
}

function bindFavouritesAndCartButtones() {

	$(".add-to-cart").kendoButton({
		"click": addProductToShoppingCart,
		"themeColor": "primary",
		"icon": "cart"
	});

	$(".add-to-favourites").kendoButton({
		"click": addProductToFavorites,
		"icon": 'heart-outline'
	});

	$(".remove-from-cart").kendoButton({
		"click": removeProductFromFavorites,
		"icon": 'heart'
	});
}

function bindSortByDdl() {

	if ($("#sortBy").data('kendoDropDownList') == undefined) {
		$("#sortBy").kendoDropDownList({
			"change": sortDataSource,
			"dataTextField": "text",
			"dataValueField": "value",
			"optionLabel": "Sort by",
			"dataSource": [
				{ text: "Price - Low to High", value: 1, filterField: "FinalPrice", direction: "asc" },
				{ text: "Price - High to Low", value: 2, filterField: "FinalPrice", direction: "desc" },
				{ text: "Name - A to Z", value: 3, filterField: "Name", direction: "asc" },
				{ text: "Name - Z to A", value: 4, filterField: "Name", direction: "desc" }
			]
		});
	}
}

function onSummaryDataBound(e) {
	showSearchResult(e);
	showCategories(e);
	bindSortByDdl();
	bindFavouritesAndCartButtones();
	distinguishFavorites();

}

function onSimilarDataBound(e) {
	bindFavouritesAndCartButtones();
	distinguishFavorites();
}

function showSearchResult(e) {
	var count = e.sender.dataSource.total();
	var searchParam = $("#searchBar").data("kendoAutoComplete").value();
	if (searchParam != "") {
		$("#searchResultCount").html(count + " results for " + searchParam);
		return;
	}
	$("#searchResultCount").html(count + " results");
}

function showCategories(e) {
	var searchParam = $("#searchBar").data("kendoAutoComplete").value();
	var groupCount = e.sender.dataSource.view()

	$("#availableCategories").html("");
	for (i = 0; i < groupCount.length; ++i) {
		var value = groupCount[i].value;
		var count = groupCount[i].items.length;
		var categoriesElement = $("#availableCategories");
		let getUrl = window.location.href.indexOf('fluent-eshop-voyago') > 0 ? "/fluent-eshop-voyago/Products/Category?subCategory=" : "/Products/Category?subCategory=";
		categoriesElement.append("<a href=' " + getUrl + value + "&searchParam=" + searchParam + "' ><p style='color: black;' ><strong>" + value + "</strong> (" + count + " results)</p></a>");
	}
}

function onAdditionalData() {
	return {
		text: $("#searchBar").val()
	};
}

function onSearchSelect(e) {
	e.preventDefault();
	var product = e.dataItem;
	var productId = product.ProductId;
	location.href = window.location.href.indexOf('fluent-eshop-voyago') > 0 ? "/fluent-eshop-voyago/Products/Details?productId=" + productId : "/Products/Details?productId=" + productId;

}

function searchProducts(e) {
	var params = e.sender.value().split("; ");
	if (params.length == 1) {
		searchByName(params[0]);
	}
	else {
		searchByNameAndCategory(params[0], params[1]);
	}
}

function searchByNameAndCategory(name, subCategory) {
	var productCategoryParam = "/Products/Category?searchParam=" + name + "&subCategory=" + subCategory;
	location.href = window.location.href.indexOf('fluent-eshop-voyago') > 0 ? "/fluent-eshop-voyago" + productCategoryParam : productCategoryParam;

}

function searchByName(name) {
	if (location.pathname.includes("/Products/")) {
		filterDataSource();
	}
	else {
		location.href = window.location.href.indexOf('fluent-eshop-voyago') > 0 ? "/fluent-eshop-voyago/Products/Summary?searchParam=" + name : "/Products/Summary?searchParam=" + name;
	}
}

function dataBoundUserCountry(e) {
	if ($("#shippingAddressForm").data('kendoForm').options.formData.Country != "US") {
		$("#State").data("kendoDropDownList").enable(false)
	}
}

function changeUserCountry(e) {
	let stateDDL = $("#State").data("kendoDropDownList");
	if (this.value() == "US") {
		stateDDL.enable(true);
	} else {
		stateDDL.value("");
		stateDDL.enable(false);
	}
}
