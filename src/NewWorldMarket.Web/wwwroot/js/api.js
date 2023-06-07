$(document).ready(function() {
    //cancel order event register
    $(".btn-cancel-order").click(CancelOrder);
    $(".btn-complete-order").click(CompleteOrder);
    $(".btn-update-order").click(UpdateOrder);
    $(".btn-remove-character").click(RemoveCharacter);
    $(".btn-activate-order").click(ActivateOrder);
    $(".btn-report-order").click(ReportOrder);


});

function ReportOrder(e) {
    var orderGuid = $(e.target).data("order-guid");
    var orderId = $(e.target).data("order-id");
    Swal.fire({
        icon: "warning",
        title: "Order Report",
        html: `
              <h5>You are creating report for order: ${orderId}</h5>
              <div class="text-center">
                  <textarea id="order-report-message" class="m-auto p-auto form-control bg-dark text-light border-0 w-75" style="height:100px" type="text" placeholder="Reason"></textarea>
              </div>
        `,
        showCancelButton: true,
        confirmButtonText: "Confirm",
        preConfirm: (login) => {
            var input = document.getElementById("order-report-message");
            var message = input.value;
            var body = {
                OrderGuid: orderGuid,
                Message: message
            };
            return fetch(`../api/Order/Report`,
                    {
                        method: "POST",
                        body: JSON.stringify(body),
                        headers: {
                            "Content-Type": "application/json"
                            // 'Content-Type': 'application/x-www-form-urlencoded',
                        }
                    })
                .then(response => response.json())
                .then(data => {
                    //console.log(data);
                    if (data.isSuccess) {
                        return;
                    }
                    throw new Error(data.errorCode);

                })
                .catch(error => {
                    Swal.showValidationMessage(
                        `Request failed: ${error.message}`
                    );
                });
        }

    }).then((result) => {
        /* Read more about isConfirmed, isDenied below */
        if (result.isConfirmed) {

            Swal.fire(`You have successfully created report for order: <b>${orderId}`,
                    "",
                    "success")
                .then(() => {
                    //refresh page
                    //location.reload();


                });
        }
    });
}


function ActivateOrder(event) {
    var guid = $(event.target).data("guid");
    const orderId = $(event.target).data("order-id");
    Swal.fire({
        icon: "warning",
        title: "Are you sure ?",
        html: `You are about to activate an expired order.<br> Order id: ${orderId}`,
        showCancelButton: true,
        confirmButtonText: "Save",
        preConfirm: (login) => {
            return fetch(`../api/Order/ActivateOrder?guid=${guid}`)
                .then(response => response.json())
                .then(data => {
                    if (data.isSuccess) {
                        return;
                    }
                    throw new Error(data.errorCode);
                })
                .catch(error => {
                    Swal.showValidationMessage(
                        `Request failed: ${error.message}`
                    );
                });
        }

    }).then((result) => {
        /* Read more about isConfirmed, isDenied below */
        if (result.isConfirmed) {

            Swal.fire("Success!",
                    "You have successfully activated your order! Refresh the page to see changes.",
                    "success")
                .then(() => {
                    //refresh page
                    location.reload();


                });
        }
    });
}

function CompleteOrder(event) {
    var guid = $(event.target).data("guid");
    const orderId = $(event.target).data("order-id");
    Swal.fire({
        icon: "warning",
        title: "Are you sure ?",
        html: `You are about to complete an active order.<br> Order id: ${orderId}`,
        showCancelButton: true,
        confirmButtonText: "Save",
        preConfirm: (login) => {
            return fetch(`../api/Order/CompleteOrder?guid=${guid}`)
                .then(response => response.json())
                .then(data => {
                    if (data.isSuccess) {
                        return;
                    }
                    throw new Error(data.errorCode);

                })
                .catch(error => {
                    Swal.showValidationMessage(
                        `Request failed: ${error.message}`
                    );
                });
        }

    }).then((result) => {
        /* Read more about isConfirmed, isDenied below */
        if (result.isConfirmed) {

            Swal.fire("Success!",
                "You have successfully completed your order! Refresh the page to see changes.",
                "success").then(() => {
                //refresh page
                location.reload();


            });
        }
    });
}

function UpdateOrder(event) {
    var guid = $(event.target).data("guid");
    var orderId = $(event.target).data("order-id");
    const priceText = $(event.target).data("order-price");
    const price = parseFloat(priceText);
    Swal.fire({
        icon: "info",
        title: "Enter new price ",
        html: `
                <h5>Order Id: ${orderId}</h5>
                <div class="text-center">
               <input min="1000" max="500000" id="new-order-price-input" class="m-auto p-auto form-control w-50 bg-dark text-light border-0" type="number" placeholder="New Price" value="${
            price}">

                </div>

        `,
        showCancelButton: true,
        confirmButtonText: "Save",
        preConfirm: (login) => {
            var input = document.getElementById("new-order-price-input");
            var newPrice = input.value;
            return fetch(`../api/Order/UpdateOrderPrice?guid=${guid}&price=${newPrice}`)
                .then(response => response.json())
                .then(data => {
                    if (data.isSuccess) {
                        return;
                    }
                    throw new Error(data.errorCode);

                })
                .catch(error => {
                    Swal.showValidationMessage(
                        `Request failed: ${error.message}`
                    );
                });
        }

    }).then((result) => {
        /* Read more about isConfirmed, isDenied below */
        if (result.isConfirmed) {

            Swal.fire(`You have successfully updated <b>${orderId}`,
                    "",
                    "success")
                .then(() => {
                    //refresh page
                    location.reload();


                });
        }
    });
}

function RemoveCharacter() {
    var guid = $(this).data("guid");
    var characterName = $(this).data("character-name");
    Swal.fire({
        icon: "warning",
        title: "Are you sure ?",
        html: `You are about to remove <b>${characterName
            }</b> character from your account. All of your active orders linked to this character will be canceled automatically.`,
        showCancelButton: true,
        confirmButtonText: "Save",
        preConfirm: (login) => {
            return fetch(`../api/Account/RemoveCharacter?guid=${guid}`)
                .then(response => response.json())
                .then(data => {
                    if (data.isSuccess) {
                        return;
                    }
                    throw new Error(data.errorCode);

                })
                .catch(error => {
                    Swal.showValidationMessage(
                        `Request failed: ${error.message}`
                    );
                });
        }

    }).then((result) => {
        /* Read more about isConfirmed, isDenied below */
        if (result.isConfirmed) {

            Swal.fire(
                    `You have successfully removed <b>${characterName
                    }</b> character from your account! Refresh the page to see changes.`,
                    "",
                    "success")
                .then(() => {
                    //refresh page
                    location.reload();


                });
        }
    });
}

function CancelOrder(event) {
    var guid = $(event.target).data("guid");
    const orderId = $(event.target).data("order-id");
    Swal.fire({
        icon: "warning",
        title: "Are you sure ?",
        html: `You are about to cancel an active order.<br> Order id: ${orderId}`,
        showCancelButton: true,
        confirmButtonText: "Save",
        preConfirm: (login) => {
            return fetch(`../api/Order/CancelOrder?guid=${guid}`)
                .then(response => response.json())
                .then(data => {
                    if (data.isSuccess) {
                        return;
                    }
                    throw new Error(data.errorCode);

                })
                .catch(error => {
                    Swal.showValidationMessage(
                        `Request failed: ${error.message}`
                    );
                });
        }

    }).then((result) => {
        /* Read more about isConfirmed, isDenied below */
        if (result.isConfirmed) {

            Swal.fire("Success!",
                    "You have successfully cancelled your order! Refresh the page to see changes.",
                    "success")
                .then(() => {
                    //refresh page
                    location.reload();


                });
        }
    });
}