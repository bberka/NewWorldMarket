
function LoginForm() {
    Swal.fire({
        title: '<strong>Login</strong>',
        html: `
        <input class="form-control" id="Username" class="pt-2" placeholder="Username"/>
        <br/>
        <input class="form-control" id="Password" class="pt-2" placeholder="Password"/>
        `,
        background: '#fff',
        showCancelButton: true,
        focusConfirm: false,
        confirmButtonText: 'Login',
        cancelButtonText:'Cancel',
    });
}

function RegisterForm() {
    Swal.fire({
        title: 'Submit your Github username',
        input: 'text',
        inputAttributes: {
            autocapitalize: 'off'
        },
        showCancelButton: true,
        confirmButtonText: 'Look up',
        showLoaderOnConfirm: true,
        preConfirm: (login) => {
            return fetch(`//api.github.com/users/${login}`)
                .then(response => {
                    if (!response.ok) {
                        throw new Error(response.statusText)
                    }
                    return response.json()
                })
                .catch(error => {
                    Swal.showValidationMessage(
                        `Request failed: ${error}`
                    )
                })
        },
        allowOutsideClick: () => !Swal.isLoading()
    }).then((result) => {
        if (result.isConfirmed) {
            Swal.fire({
                title: `${result.value.login}'s avatar`,
                imageUrl: result.value.avatar_url
            })
        }
    });
}

function  RemoveCharacter(charGuid, characterName) {
    Swal.fire({
        title: 'Are you sure ?',
        html: 'You are about to remove <b>' + characterName + '</b> character from your account. All of your active orders linked to this character will be canceled automatically.',
        showCancelButton: true,
        confirmButtonText: 'Save',
        preConfirm: (login) => {
            return fetch(`../Account/RemoveCharacter/${charGuid}`)
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
            
            Swal.fire('Success!', '', 'success')
            //refresh page
        } 
    })
}