$(document).ready(function () {
    //cancel order event register
    $(".btn-cancel-order").click(CancelOrder);
    $(".btn-remove-character").click(RemoveCharacter);


});

//function LoginForm() {
//    Swal.fire({
//        title: '<strong>Login</strong>',
//        html: `
//        <input class="form-control" id="Username" class="pt-2" placeholder="Username"/>
//        <br/>
//        <input class="form-control" id="Password" class="pt-2" placeholder="Password"/>
//        `,
//        background: '#fff',
//        showCancelButton: true,
//        focusConfirm: false,
//        confirmButtonText: 'Login',
//        cancelButtonText: 'Cancel',
//    });
//}

//function RegisterForm() {
//    Swal.fire({
//        title: 'Submit your Github username',
//        input: 'text',
//        inputAttributes: {
//            autocapitalize: 'off'
//        },
//        showCancelButton: true,
//        confirmButtonText: 'Look up',
//        showLoaderOnConfirm: true,
//        preConfirm: (login) => {
//            return fetch(`//api.github.com/users/${login}`)
//                .then(response => {
//                    if (!response.ok) {
//                        throw new Error(response.statusText)
//                    }
//                    return response.json()
//                })
//                .catch(error => {
//                    Swal.showValidationMessage(
//                        `Request failed: ${error}`
//                    )
//                })
//        },
//        allowOutsideClick: () => !Swal.isLoading()
//    }).then((result) => {
//        if (result.isConfirmed) {
//            Swal.fire({
//                title: `${result.value.login}'s avatar`,
//                imageUrl: result.value.avatar_url
//            })
//        }
//    });
//}

function RemoveCharacter() {
    var guid = $(this).data('guid');
    var characterName = $(this).data('character-name');
    Swal.fire({
        icon: 'warning',
        title: 'Are you sure ?',
        html: 'You are about to remove <b>' + characterName + '</b> character from your account. All of your active orders linked to this character will be canceled automatically.',
        showCancelButton: true,
        confirmButtonText: 'Save',
        preConfirm: (login) => {
            return fetch(`../api/Account/RemoveCharacter?guid=${guid}`)
                .then(response => response.json())
                .then(data => {
                    console.log(data);
                    if (data.isSuccess) {
                        return;
                    }
                    throw new Error(data.errorCode);

                })
                .catch(error => {
                    Swal.showValidationMessage(
                        `Request failed: ${error.message}`
                    )
                })
        },

    }).then((result) => {
        /* Read more about isConfirmed, isDenied below */
        if (result.isConfirmed) {

            Swal.fire('You have successfully removed <b>' + characterName + '</b> character from your account! Refresh the page to see changes.', '', 'success')
            //refresh page
        }
    })
}




function CancelOrder(event) {
    var guid = $(event.target).data("guid");
    console.log(guid);
    var orderNo = $(event.target).data("order-no");
    Swal.fire({
        icon: 'warning',
        title: 'Are you sure ?',
        html: 'You are about to cancel an active order.',
        showCancelButton: true,
        confirmButtonText: 'Save',
        preConfirm: (login) => {
            return fetch(`../api/Order/CancelOrder?guid=${guid}`)
                .then(response => response.json())
                .then(data => {
                    console.log(data);
                    if (data.isSuccess) {
                        return;
                    }
                    throw new Error(data.errorCode);

                })
                .catch(error => {
                    Swal.showValidationMessage(
                        `Request failed: ${error.message}`
                    )
                })
        },

    }).then((result) => {
        /* Read more about isConfirmed, isDenied below */
        if (result.isConfirmed) {

            Swal.fire('You have successfully cancelled your order! Refresh the page to see changes.', 'asd', 'success');
            //TODO : refresh page
        }
    })
}
