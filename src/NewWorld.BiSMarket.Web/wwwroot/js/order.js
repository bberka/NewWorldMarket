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

function GetFilter() {
    const elSelectedRarity = $("#dd-rarity").find(".active-dropdown");
    const elSelectedPerks = $("#dd-perks").find(".active-dropdown");
    const elSelectedItem = $("#dd-item").find(".active-dropdown");
    const elSelectedAttr = $("#dd-attr").find(".active-dropdown");
    const elSelectedServer = $("#dd-server").find(".active-dropdown");
    var rarity = elSelectedRarity.data("value");
    var item = elSelectedItem.data("value");
    var attr = elSelectedAttr.data("value");
    var server = elSelectedServer.data("value");
    var perks = [];
    elSelectedPerks.each(function() {
        perks.push($(this).data("value"));
    });
    var perk1 = -1;
    var perk2 = -1;
    var perk3 = -1;
    if (rarity == null || rarity == undefined) rarity = -1;
    if (item == null || item == undefined) item = -1;
    if (attr == null || attr == undefined) attr = -1;
    if (server == null || server == undefined) server = -1;
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
        rarity: rarity,
        item: item,
        attr: attr,
        server: server,
        perk1: perk1,
        perk2: perk2,
        perk3: perk3
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

    //elRarity.addClass("active-dropdown");
    //elItem.addClass("active-dropdown");
    //elAttr.addClass("active-dropdown");
    //elServer.addClass("active-dropdown");
    //elPerk1.addClass("active-dropdown");
    //elPerk2.addClass("active-dropdown");
    //elPerk3.addClass("active-dropdown");
    elRarity.click();
    elItem.click();
    elAttr.click();
    elServer.click();
    elPerk1.click();
    elPerk2.click();
    elPerk3.click();

}

//function name(parameters) {
//    var $dropdownMenu = $dropdown.find('.dropdown-menu');
//    var activeItemList = $dropdownMenu.find('.active-dropdown');
//    var selectedItemCount = activeItemList.length;
//    if (selectedItemCount > selectionLimit) {
//        alert("You can only select " + selectionLimit + " number of items");
//        $(this).removeClass('active-dropdown');
//        return;
//    }
//    var activeItemsText = "";
//    activeItemList.each(function () {
//        activeItemsText += $(this).text() + ", ";
//    });
//    //console.log(activeItemsText2);
//    //remove last comma
//    activeItemsText = activeItemsText.substring(0, activeItemsText.length - 2);

//    if (selectedItemCount > 0)
//        $dropdownToggle.text(originalText + ": " + activeItemsText);
//    else $dropdownToggle.text(originalText);
//}

//var $dropdown = $(this).closest('.dropdown');
//var $dropdownToggle = $dropdown.find('.dropdown-toggle');
//var originalText = $dropdownToggle.text();
//var $dropdownMenu = $dropdown.find('.dropdown-menu');
//var activeItemList = $dropdownMenu.find('.active-dropdown');
//activeItemList.removeClass('active-dropdown');
//$dropdownToggle.text(originalText);
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