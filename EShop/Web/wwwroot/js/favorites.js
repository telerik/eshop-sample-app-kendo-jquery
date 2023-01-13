function addProductToFavorites(e) {
    var element = e.sender.element;
    var productId = element[0].id.split('_')[1];
    var icon = $(element[0]).find(".k-icon");

    var getUrl = window.location.href.indexOf('eshop') > 0 ? "/kendo-ui/eshop/Account/ProductIsInFavorites?productId=" : window.location.origin + "/Account/ProductIsInFavorites?productId=";
    var addUrl = window.location.href.indexOf('eshop') > 0 ? "/kendo-ui/eshop/Account/AddProductToFavorites?productId=" : window.location.origin + "/Account/AddProductToFavorites?productId=";
    var removeUrl = window.location.href.indexOf('eshop') > 0 ? "/kendo-ui/eshop/Account/RemoveProductFromFavorites?productId=" : window.location.origin + "/Account/RemoveProductFromFavorites?productId=";

    $.get(getUrl + productId, function (data) { 
        if (!data) {
            $.post(addUrl + productId, function () {
                getFavoritesCount();
                if ($(element[0]).find(".k-button-text")) {
                    $(element[0]).find(".k-button-text").text("Added to favorites");
                }
                icon.removeClass("k-i-heart-outline");
                icon.addClass("k-i-heart");
            });
        }
        else {
            $.post(removeUrl + productId, function () {
                getFavoritesCount();
                if ($(element[0]).find(".k-button-text")) {
                    $(element[0]).find(".k-button-text").text("Add to favorites");
                }
                icon.removeClass("k-i-heart");
                icon.addClass("k-i-heart-outline");
            });
        }
    });
}

function removeProductFromFavorites(e) {
	var productId = e.sender.element[0].id.split('_')[1];	
    var removeUrl = window.location.href.indexOf('eshop') > 0 ? "/kendo-ui/eshop/Account/RemoveProductFromFavorites?productId=" : window.location.origin + "/Account/RemoveProductFromFavorites?productId=";

    $.post(removeUrl + productId, function () {
		$("#favoritesListView").data("kendoListView").dataSource.read();		
		getFavoritesCount();
	});
}

function getFavoritesCount() {
    var getUrl = window.location.href.indexOf('eshop') > 0 ? "/kendo-ui/eshop/Account/GetFavoritesCount" : window.location.origin + "/Account/GetFavoritesCount";
    $.get(getUrl, function (data) {        
        $("#favourites-badge").data("kendoBadge").text(data);
    });
}
