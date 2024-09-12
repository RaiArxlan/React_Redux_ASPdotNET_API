import { useState } from "react";
import { useNavigate, useLocation } from "react-router-dom";
import { useDispatch } from 'react-redux';
import { login } from '../reducers/UserReducer';
import { fetch } from '../services/HttpService';

function LoginComponent() {

    const [email, setEmail] = useState('arslan.rai@local.com');
    const [password, setPassword] = useState('Password1!');

    const navigate = useNavigate();
    const location = useLocation();
    const from = location.state?.from?.pathname || '/';

    const dispatch = useDispatch();

    return (
        <div className="container mt-5">
            <div className="row justify-content-center">
                <div className="col-md-6">
                    <h1 className="text-center mb-4">Login</h1>
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
                        <button type="submit" className="btn btn-primary btn-block">Login</button>
                        <div className="text-center mt-3">
                            <a href="/register">Register</a>
                        </div>
                    </form>
                </div>
            </div>
        </div>
    );


    function handleSubmit(e: React.FormEvent<HTMLFormElement>) {
        e.preventDefault();
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

                        localStorage.setItem("email", email);
                        localStorage.setItem("isAuthenticated", 'true');
                        dispatch(login());

                        // redirect to the page user was trying to access
                        const redirectUrl = localStorage.getItem('redirectAfterLogin');
                        if (redirectUrl) {
                            localStorage.removeItem('redirectAfterLogin');
                            navigate(redirectUrl, { replace: true });
                            return;
                        } else {
                            // Redirect to page from location state or home
                            navigate(from, { replace: true });
                        }
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