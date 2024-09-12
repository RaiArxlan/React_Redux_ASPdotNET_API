import { useState } from 'react';
import { fetch } from '../services/HttpService';
function RegisterComponent() {

    const [email, setEmail] = useState('arslan.rai@local.com');
    const [password, setPassword] = useState('Password1!');

    return (
        <div className="container mt-5">
            <div className="row justify-content-center">
                <div className="col-md-6">
                    <h2 className="text-center mb-4">Register</h2>
                    <form onSubmit={handleSubmit}>
                        <div className="form-group">
                            <label>Email</label>
                            <input
                                type="text"
                                className="form-control"
                                value={email}
                                onChange={e => setEmail(e.target.value)}
                            />
                        </div>
                        <div className="form-group">
                            <label>Password</label>
                            <input
                                type="password"
                                className="form-control"
                                value={password}
                                onChange={e => setPassword(e.target.value)}
                            />
                        </div>
                        <button type="submit" className="btn btn-primary btn-block">Register</button>
                        <div className="text-center mt-3">
                            <a href="/login">Login</a>
                        </div>
                    </form>
                </div>
            </div>
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
                        alert("Failed to register user");
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