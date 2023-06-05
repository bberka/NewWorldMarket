$(document).ready(function() {
    const tableElementList = $(".table");
    //add all elements a specific id and then use that id to get the table
    var tableIdList = [];
    var tableList = [];
    var count = 0;
    tableElementList.each(function() {
        var tableId = $(this).attr("id");
        //console.log(this);
        //add id if null or undefined
        if (tableId == null || tableId == undefined) {
            tableId = `table-${count}`;
            $(this).attr("id", tableId);
        }
        tableIdList.push(tableId);
        const table = new DataTable(`#${tableId}`);
        tableList.push(table);
        count++;

    });

});