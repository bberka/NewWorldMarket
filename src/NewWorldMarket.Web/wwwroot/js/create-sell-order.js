$(document).ready(function () {
    $("select").each(ChangeElementTextColor_ForSelect);
    //on selection change
    $("select").change(ChangeElementTextColor_ForSelect);
    $("input").each(ChangeElementTextColor_ForInput);
    //on input change
    $("input").change(ChangeElementTextColor_ForInput);



});

function ChangeElementTextColor_ForSelect() {
    var selectedValue = $(this).val();
    var id = $(this).attr("id");
    console.log(id + ":" + selectedValue);
    if (selectedValue == -1 || selectedValue == null) {
        //remove text-light class
        $(this).removeClass("text-light");
        //add text-danger class
        $(this).addClass("text-warning");
    } else {
        $(this).addClass("text-light");
        $(this).removeClass("text-warning");
    }
};
function ChangeElementTextColor_ForInput() {
    var inputValue = $(this).val();
    //var inputValue = $(this).attr("value");
    var id = $(this).attr("id");
    console.log(id + ":" + inputValue);
    var isGearScoreInput = $(this).attr("id") == "GearScore";
    var isPriceInput = $(this).attr("id") == "Price";
    if (isGearScoreInput) {
        if (inputValue < 590 || inputValue > 600) {
            //remove text-light class
            $(this).removeClass("text-light");
            //add text-danger class
            $(this).addClass("text-warning");
        } else {
            $(this).addClass("text-light");
            $(this).removeClass("text-warning");
        }
        return;
    }
    if (isPriceInput) {
        if (inputValue < 1000 || inputValue > 500000) {
            //remove text-light class
            $(this).removeClass("text-light");
            //add text-danger class
            $(this).addClass("text-warning");
        } else {
            $(this).addClass("text-light");
            $(this).removeClass("text-warning");
        }
        return;
    }
    if (inputValue == -1 || (isGearScoreInput && (inputValue < 590 || inputValue > 600))) {
        //remove text-light class
        $(this).removeClass("text-light");
        //add text-danger class
        $(this).addClass("text-warning");
    } else {
        $(this).addClass("text-light");
        $(this).removeClass("text-warning");
    }
};