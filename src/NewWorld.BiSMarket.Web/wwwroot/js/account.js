
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