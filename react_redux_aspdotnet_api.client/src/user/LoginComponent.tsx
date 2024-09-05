import { useState } from "react";
import { useNavigate, useLocation } from "react-router-dom";

function LoginComponent() {

    const [email, setEmail] = useState('arslan.rai@local.com');
    const [password, setPassword] = useState('Password1!');

    const navigate = useNavigate();
    const location = useLocation();
    const from = location.state?.from?.pathname || '/';
    return (
        <div>
            <h1>Login</h1>
            <form onSubmit={handleSubmit}>
                <div>
                    <label>Email</label>
                    <input type="text" value={email} onChange={e => setEmail(e.target.value)} />
                </div>
                <div>
                    <label>Password</label>
                    <input type="password" value={password} onChange={e => setPassword(e.target.value)} />
                </div>
                <button type="submit">Login</button>
                <a href="/register">Register</a>
            </form>
        </div>
    );


     function handleSubmit(e: React.FormEvent<HTMLFormElement>) {
        e.preventDefault();
        console.log("login user...");

        // Call the API to register the user
        try {

            const formData = new FormData();
            formData.append('Email', email);
            formData.append('Password', password);

            fetch('/api/auth/login', {
                method: 'POST',
                headers: {
                    'accept': 'application/json'
                },
                body: formData
            })
                .then(response => {
                    if (response.ok) {
                        console.log("User login successfully");

                        localStorage.setItem("email", email);
                        localStorage.setItem("isAuthenticated", 'true');

                        // Redirect to home page
                        navigate(from, { replace: true });
                    } else {
                        console.log(response.statusText);
                        console.log("Failed to login user");
                        alert("Failed to login user");
                    }
                })
                .catch(error => {
                    console.log(error);
                });
        } catch (error) {
            console.log(error);
            console.log("Failed to login user");
            alert("Failed to login user");
        };
    }
}

export default LoginComponent;