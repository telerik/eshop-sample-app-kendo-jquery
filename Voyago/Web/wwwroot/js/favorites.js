function addProductToFavorites(e) {
    var element = e.sender.element;
    var productId = element[0].id.split('_')[1];
    var icon = $(element[0]).find(".k-icon");



    $.get("/Account/ProductIsInFavorites?productId=" + productId, function (data) {
        if (!data) {
            $.post("/Account/AddProductToFavorites?productId=" + productId, function () {
                getFavoritesCount();
                if ($(element[0]).find(".k-button-text")) {
                    $(element[0]).find(".k-button-text").text("Added to favorites");
                }
                icon.removeClass("k-i-heart-outline");
                icon.addClass("k-i-heart");
            });
        }
        else {
            $.post("/Account/RemoveProductFromFavorites?productId=" + productId, function () {
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

	$.post("/Account/RemoveProductFromFavorites?productId=" + productId, function (ev) {
		$("#favoritesListView").data("kendoListView").dataSource.read();		
		getFavoritesCount();
	});
}

function getFavoritesCount() {
	$.get("/Account/GetFavoritesCount", function (data) {
		$("#favourites-btn .k-badge").data('kendoBadge').text(data);
	});
}