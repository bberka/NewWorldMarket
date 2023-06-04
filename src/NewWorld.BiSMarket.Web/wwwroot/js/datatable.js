$(document).ready(function() {
    var tableElementList = $(".table");
    //add all elements a specific id and then use that id to get the table
    var tableIdList = [];
    var tableList = [];
    tableElementList.each(function() {
        var tableId = $(this).attr("id");
        //add id if null or undefined
        if (tableId == null || tableId == undefined) {
            tableId = `table-${tableElementList.length}`;
            $(this).attr("id", tableId);
        }
        tableIdList.push(tableId);
        const table = new DataTable(`#${tableId}`);
        tableList.push(table);

    });

});