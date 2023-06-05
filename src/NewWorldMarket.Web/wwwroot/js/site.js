
$(document).ready(function() {
    $(".dropdown").each(function() {
        var $dropdown = $(this);
        if ($dropdown.hasClass("dd-menu-ignore"))
            return;
        const $searchInput = $dropdown.find("#search-dropdown");
        var $dropdownToggle = $dropdown.find(".dropdown-toggle");
        var $dropdownMenu = $dropdown.find(".dropdown-menu");
        $dropdownMenu.on("click",
            ".dropdown-item:not(.disabled)",
            function () {
                //if has class ignore, skip dd-menu-ignore
                
                const isAlreadySelected = $(this).hasClass("active-dropdown");
                const selectionLimit = $dropdown.data("selection-limit");
                const isAllowMultipleSelection = selectionLimit !== 1;
                const selectedText = $(this).text();
                const toggleOriginalText = $dropdownToggle.text();
                const split = toggleOriginalText.split(":");
                if (split.length !== 0)
                    var originalText = split[0];
                else originalText = toggleOriginalText;
                //disable selection for all elements before selecting
                if (!isAllowMultipleSelection) {
                    $dropdownMenu.find(".dropdown-item").each(function() {
                        $(this).removeClass("active-dropdown");
                    });
                }
                if (isAlreadySelected) {
                    $(this).removeClass("active-dropdown"); //disable selection if already selected
                } else {
                    $(this).addClass("active-dropdown"); //enable selection if not selected
                }
                const activeItemList = $dropdownMenu.find(".active-dropdown");
                const selectedItemCount = activeItemList.length;
                if (selectedItemCount > selectionLimit) {
                    alert(`You can only select ${selectionLimit} number of items`);
                    $(this).removeClass("active-dropdown");
                    return;
                }
                var activeItemsText = "";
                activeItemList.each(function() {
                    activeItemsText += $(this).text() + ", ";
                });
                //console.log(activeItemsText2);
                //remove last comma
                activeItemsText = activeItemsText.substring(0, activeItemsText.length - 2);

                if (selectedItemCount > 0)
                    $dropdownToggle.text(originalText + ": " + activeItemsText);
                else $dropdownToggle.text(originalText);

                UpdatePerkInput();
            });
        // Add event listener to the search input
        $searchInput.on("input",
            function() {
                var searchValue = $(this).val().toLowerCase();

                $dropdownMenu.find(".dropdown-item").each(function() {
                    //check if has class disabled and if so, skip
                    if ($(this).hasClass("disabled") && searchValue != "") {
                        //$(this).hide();
                        return;
                    }
                    //var dropDownItemIndex = $(this).index();
                    const itemText = $(this).text().toLowerCase();
                    if (itemText.indexOf(searchValue) > -1) {
                        $(this).show();
                    } else {
                        $(this).hide();
                    }
                });


            });
    });
});


$(document).ready(function() {
    //cancel order event register
    $(".btn-apply-filter-search").click(ApplyFilterAndSearch);
    $(".btn-clear-filter").click(ClearFilter);
    SelectFilterElementsInDropdownByQuery();


});

function ApplyFilterAndSearch() {
    const filter = GetFilter();
    const queryArray = [];
    if (filter.rarity != -1)
        queryArray.push(`rarity=${filter.rarity}`);
    if (filter.item != -1)
        queryArray.push(`item=${filter.item}`);
    if (filter.attr != -1)
        queryArray.push(`attr=${filter.attr}`);
    if (filter.server != -1)
        queryArray.push(`server=${filter.server}`);
    if (filter.perk1 != -1)
        queryArray.push(`perk1=${filter.perk1}`);
    if (filter.perk2 != -1)
        queryArray.push(`perk2=${filter.perk2}`);
    if (filter.perk3 != -1)
        queryArray.push(`perk3=${filter.perk3}`);
    //append query array to url 
    const query = queryArray.join("&");
    const url = `/list?${query}`;
    //redirect to url
    window.location.href = url;


}

function UpdatePerkInput() {
    const input = $(".input-perk-string");
    if (input == undefined || input == null)
        return;
    var perk1Value = -1;
    const perk2Value = -1;
    const perk3Value = -1;
    const perk1 = $("#dd-perks").find(".active-dropdown").eq(0);
    if (perk1 != undefined || perk1 != null) {
        const perk1ValueStr = perk1.data("value");
        perk1Value = parseInt(perk1ValueStr);
    }
    const perk2 = $("#dd-perks").find(".active-dropdown").eq(1);
    if (perk2 != undefined || perk2 != null) {
        const perk2ValueStr = perk2.data("value");
        perk1Value = parseInt(perk2ValueStr);
    }
    const perk3 = $("#dd-perks").find(".active-dropdown").eq(2);
    if (perk3 != undefined || perk3 != null) {
        const perk3ValueStr = perk3.data("value");
        perk1Value = parseInt(perk3ValueStr);
    }
    const perkString = `${perk1Value},${perk2Value},${perk3Value}`;
    input.val(perkString);
}

function Filter_GetPerks() {
    const elSelectedPerks = $("#dd-perks").find(".active-dropdown");
    var perks = [];
    elSelectedPerks.each(function() {
        perks.push($(this).data("value"));
    });
    var perk1 = -1;
    var perk2 = -1;
    var perk3 = -1;
    try {
        perk1 = perks[0];
        perk2 = perks[1];
        perk3 = perks[2];

    } catch (e) {
    }
    if (perk1 == null || perk1 == undefined) perk1 = -1;
    if (perk2 == null || perk2 == undefined) perk2 = -1;
    if (perk3 == null || perk3 == undefined) perk3 = -1;
    return {
        perk1: perk1,
        perk2: perk2,
        perk3: perk3
    };
}

function Filter_GetAttribute() {
    const elSelectedAttr = $("#dd-attr").find(".active-dropdown");
    var attr = elSelectedAttr.data("value");
    if (attr == null || attr == undefined) attr = -1;
    return attr;
}

function Filter_GetRarity() {
    const elSelectedRarity = $("#dd-rarity").find(".active-dropdown");
    var rarity = elSelectedRarity.data("value");
    if (rarity == null || rarity == undefined) rarity = -1;
    return rarity;

}

function Filter_GetItem() {
    const elSelectedItem = $("#dd-item").find(".active-dropdown");
    var item = elSelectedItem.data("value");
    if (item == null || item == undefined) item = -1;
    return item;
}

function Filter_GetServer() {
    const elSelectedServer = $("#dd-server").find(".active-dropdown");
    var server = elSelectedServer.data("value");
    if (server == null || server == undefined) server = -1;
    return server;
}

function GetFilter() {
    const rarity = Filter_GetRarity();
    const item = Filter_GetItem();
    const attr = Filter_GetAttribute();
    const server = Filter_GetServer();
    const perks = Filter_GetPerks();
    return {
        rarity: rarity,
        item: item,
        attr: attr,
        server: server,
        perk1: perks.perk1,
        perk2: perks.perk2,
        perk3: perks.perk3
    };


}


function SelectFilterElementsInDropdownByQuery() {
    const query = window.location.search;
    const params = new URLSearchParams(query);
    const rarity = params.get("rarity");
    const item = params.get("item");
    const attr = params.get("attr");
    const server = params.get("server");
    const perk1 = params.get("perk1");
    const perk2 = params.get("perk2");
    const perk3 = params.get("perk3");
    const elRarity = $("#dd-rarity").find(`[data-value='${rarity}']`);
    const elItem = $("#dd-item").find(`[data-value='${item}']`);
    const elAttr = $("#dd-attr").find(`[data-value='${attr}']`);
    const elServer = $("#dd-server").find(`[data-value='${server}']`);
    const elPerk1 = $("#dd-perks").find(`[data-value='${perk1}']`);
    const elPerk2 = $("#dd-perks").find(`[data-value='${perk2}']`);
    const elPerk3 = $("#dd-perks").find(`[data-value='${perk3}']`);

    if (elRarity != undefined)
        elRarity.click();
    if (elItem != undefined)
        elItem.click();
    if (elAttr != undefined)
        elAttr.click();
    if (elServer != undefined)
        elServer.click();
    if (elPerk1 != undefined)
        elPerk1.click();
    if (elPerk2 != undefined)
        elPerk2.click();
    if (elPerk3 != undefined)
        elPerk3.click();


}

function ClearFilter() {
    const dropdownList = $(".dropdown-menu");
    dropdownList.each(function() {
        const activeDropdownItems = $(this).find(".active-dropdown");
        activeDropdownItems.each(function() {
            //trigger click
            $(this).click();
        });
    });


}