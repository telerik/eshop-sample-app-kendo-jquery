function onRecentlyViewedDataBound(e) {
	hideRecentlyViewedIfEmpty(e);
	distinguishFavorites();
	bindFavouritesAndCartButtones()
}

function hideRecentlyViewedIfEmpty(e) {
	var ds = e.sender.dataSource;
	if (ds.total() == 0) {
		$(".recently-viewed").hide();
	}
}

function onCategoryDataBound(e) {
	//showResultCount(e);
	distinguishFavorites();
	bindFavouritesAndCartButtones();
	bindSortByDdl();
}

function distinguishFavorites() {
	var favButtons = $("[id^=addToFavoritesButton_]");
	favButtons.each(function () {
		var currentButton = $(this);
		var productId = this.id.split("_")[1];
		$.get("/Account/ProductIsInFavorites?productId=" + productId, function (data) {
			if (data) {
				currentButton.css("background-color", "#ffdf73");
			}
		});
	});
}

function showResultCount(e) {
	var count = e.sender.dataSource.total();
	$("#resultCount").html(count + " results in ");
}

function addFilterCheckBoxes() {
	$("#discountPicker").kendoCheckBoxGroup({
		change: toggleDiscountPicker,
		items: [{
			"label": "All",
			"value": "1"
		}, {
			"label": "Discounted items only",
			"value": "2"
		}],
		value: ["1"]
	});

	// add checkboxes for the rating
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
			precision: "half",
			value: Number(i) - 0.5
		});
	}

	$("#priceSlider").kendoRangeSlider({
		"change": filterDataSource,
		"largeStep": 2000,
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
		"largeStep": 50,
		"max": 100,
		"min": 0,
		"orientation": "horizontal",
		"selectionEnd": 100,
		"selectionStart": 0,
		"smallStep": 10,
		"tickPlacement": "both"
	});
}


function goToCategoryPage(e) {
	var category = e.sender.value();
	if (category != "") {
		location.href = "/Home/" + category;
	}
}

function bindFavouritesAndCartButtones() {
	$(".add-to-cart").kendoButton({
		"click": addProductToShoppingCart,
		"imageUrl": "/images/shopping_cart.svg"
	});

	$(".add-to-favourites").kendoButton({
		"click": addProductToFavorites,
		"imageUrl": "/images/heart.svg"
	});

	$(".remove-from-cart").kendoButton({
		"click": removeProductFromFavorites,
		"imageUrl": "/images/heart_full.svg"
	});
}

function bindSortByDdl() {

	if ($("#sortBy").data('kendoDropDownList') == undefined ) {
		$("#sortBy").kendoDropDownList({
			"change": sortDataSource,
			"dataTextField": "text",		
			"dataValueField": "value",
			"optionLabel": "Sort by",
			"dataSource": [
				{ text: "Price - Low to High", value: 1, filterField: "Price", direction: "asc" },
				{ text: "Price - High to Low", value: 2, filterField: "Price", direction: "desc" },
				{ text: "Name - A to Z", value: 3, filterField: "Name", direction: "asc" },
				{ text: "Name - Z to A", value: 4, filterField: "Name", direction: "desc" }
			]
		});
    }	
}

function onSummaryDataBound(e) {
	showSearchResult(e);
	showCategories(e);
	distinguishFavorites();
	bindSortByDdl();
	bindFavouritesAndCartButtones();

}

function onSimilarDataBound(e) {	
	distinguishFavorites();
	bindFavouritesAndCartButtones();
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
	//var data = e.sender.dataSource._data;
	var groupCount = e.sender.dataSource.view()
	
	$("#availableCategories").html("");
	for (i = 0; i < groupCount.length; ++i) {		
		var value = groupCount[i].value;
		var count = groupCount[i].items.length;
		var categoriesElement = $("#availableCategories");
		categoriesElement.append("<a href='/Products/Category?subCategory=" + value + "&searchParam=" + searchParam + "' ><p style='color: black;' ><strong>" + value + "</strong> (" + count + " results)</p></a>");
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
	location.href = "/Products/Details?productId=" + product.ProductId;
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
	location.href = "/Products/Category?searchParam=" + name + "&subCategory=" + subCategory;
}

function searchByName(name) {
	if (location.pathname.includes("/Products/")) {
		filterDataSource();
	}
	else {
		location.href = "/Products/Summary?searchParam=" + name;
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