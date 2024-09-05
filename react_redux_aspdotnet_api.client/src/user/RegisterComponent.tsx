import { useState } from 'react';
function RegisterComponent() {

    const [email, setEmail] = useState('arslan.rai@local.com');
    const [password, setPassword] = useState('Password1!');

    return (
        // Form to register a new user
        <div>
            <h2>Register</h2>
            <form onSubmit={handleSubmit}>
                <div>
                    <label>Email</label>
                    <input type="text" value={email} onChange={e => setEmail(e.target.value)} />
                </div>
                <div>
                    <label>Password</label>
                    <input type="password" value={password} onChange={e => setPassword(e.target.value)} />
                </div>
                <button type="submit">Register</button>
            </form>
        </div>
    );


    function handleSubmit(e: React.FormEvent<HTMLFormElement>) {
        e.preventDefault();
        console.log("Registering user...");

        // Call the API to register the user
        try {

            const formData = new FormData();
            formData.append('Email', email);
            formData.append('Password', password);

            fetch('/api/user/register', {
                method: 'POST',
                headers: {
                    'accept': 'application/json'
                },
                body: formData
            })
                .then(response => {
                    if (response.ok) {
                        console.log("User registered successfully");

                        localStorage.setItem("email", email);
                        localStorage.setItem("isAuthenticated", 'true');
                        
                        // Redirect to home page
                        window.location.href = '/';

                    } else {
                        console.log(response.statusText);
                        console.log("Failed to register user");
                        //alert("Failed to register user");
                    }
                })
                .catch(error => {
                    console.log(error);
                });
        } catch (error) {
            console.log(error);
            console.log("Failed to register user");
            alert("Failed to register user");
        };
    }
}

export default RegisterComponent;