
$(document).ready(function () {
    $('.dropdown').each(function () {
        var $dropdown = $(this);
        var $searchInput = $dropdown.find('#search-dropdown');
        var $dropdownToggle = $dropdown.find('.dropdown-toggle');
        var $dropdownMenu = $dropdown.find('.dropdown-menu');
        $dropdownMenu.on('click',
            '.dropdown-item:not(.disabled)',
            function () {
                var isAlreadySelected = $(this).hasClass('active-dropdown');
                var selectionLimit = $dropdown.data('selection-limit');
                var isAllowMultipleSelection = selectionLimit !== 1;
                var selectedText = $(this).text();
                var toggleOriginalText = $dropdownToggle.text();
                var split = toggleOriginalText.split(':');
                if (split.length !== 0)
                    var originalText = split[0];
                else originalText = toggleOriginalText;
                //disable selection for all elements before selecting
                if (!isAllowMultipleSelection) {
                    $dropdownMenu.find('.dropdown-item').each(function () {
                        $(this).removeClass('active-dropdown');
                    });
                }
                if (isAlreadySelected) {
                    $(this).removeClass('active-dropdown'); //disable selection if already selected
                } else {
                    $(this).addClass('active-dropdown'); //enable selection if not selected
                }
                var activeItemList = $dropdownMenu.find('.active-dropdown');
                var selectedItemCount = activeItemList.length;
                if (selectedItemCount > selectionLimit) {
                    alert("You can only select " + selectionLimit + " number of items");
                    $(this).removeClass('active-dropdown');
                    return;
                }
                var activeItemsText = "";
                activeItemList.each(function () {
                    activeItemsText += $(this).text() + ", ";
                });
                //console.log(activeItemsText2);
                //remove last comma
                activeItemsText = activeItemsText.substring(0, activeItemsText.length - 2);

                if (selectedItemCount > 0)
                    $dropdownToggle.text(originalText + ": " + activeItemsText);
                else $dropdownToggle.text(originalText);


            });
        // Add event listener to the search input
        $searchInput.on('input',
            function () {
                var searchValue = $(this).val().toLowerCase();

                $dropdownMenu.find('.dropdown-item').each(function () {
                    //check if has class disabled and if so, skip
                    if ($(this).hasClass('disabled') && searchValue != "") {
                        //$(this).hide();
                        return;
                    }
                    //var dropDownItemIndex = $(this).index();
                    var itemText = $(this).text().toLowerCase();
                    if (itemText.indexOf(searchValue) > -1) {
                        $(this).show();
                    } else {
                        $(this).hide();
                    }
                });


            });
    });
});

