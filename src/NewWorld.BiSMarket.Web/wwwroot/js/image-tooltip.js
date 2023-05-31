
$(document).ready(function () {
    // Attach event listeners to each icon
    $('.icon').on('mouseenter', showFullPicture);
    $('.icon').on('mouseleave', hideFullPicture);
});

// Event handler for showing the full picture
function showFullPicture() {
    const icon = $(this);
    const fullPicture = icon.siblings('.full-picture');
    const guid = fullPicture.data('image-guid'); // Get the GUID from the data-image-guid attribute

    // Update the image source dynamically using the GUID
    fullPicture.attr('src', '/api/Image/Get/' + guid);

    // Show the full picture
    fullPicture.show();

    // Update the position of the full picture
    $(document).on('mousemove', updateFullPicturePosition);
}

// Event handler for hiding the full picture
function hideFullPicture() {
    const icon = $(this);
    const fullPicture = icon.siblings('.full-picture');

    // Hide the full picture
    fullPicture.hide();

    // Remove the event listener for updating the position
    $(document).off('mousemove', updateFullPicturePosition);
}

// Event handler for updating the position of the full picture
function updateFullPicturePosition(event) {
    const fullPicture = $('.full-picture');
    const offsetX = 50; // Adjust as needed to offset the full picture from the cursor horizontally
    const offsetY = 0; // Adjust as needed to offset the full picture from the cursor vertically

    // Get the position of the icon relative to the page
    const icon = $('.icon');
    const iconOffset = icon.offset();
    const iconX = iconOffset.left;
    const iconY = iconOffset.top;

    // Calculate the position relative to the mouse coordinates and the icon's position
    const leftPosition = icon.outerWidth() + offsetX;
    let topPosition = offsetY;

    // Adjust the top position if the full picture exceeds the viewport height
    const viewportHeight = $(window).height();
    const fullPictureHeight = fullPicture.outerHeight();
    //if (iconY + topPosition + fullPictureHeight > viewportHeight) {
    //    console.log(topPosition);

    //    //topPosition = viewportHeight - iconY - fullPictureHeight - offsetY;
    //    console.log(topPosition);

    //}

    // Update the position based on the calculated coordinates
    fullPicture.css({
        left: leftPosition,
        top: topPosition
    });
}

