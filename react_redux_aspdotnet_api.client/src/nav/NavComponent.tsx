import { Link, useLocation, useNavigate } from 'react-router-dom';
import { RootState } from "../store";
import { useSelector, useDispatch } from "react-redux";
import { logout } from '../reducers/UserReducer';

const NavComponent = () => {
    const counter = useSelector((state: RootState) => state.counter.counter);
    const user = useSelector((state: RootState) => state.user);
    const dispatch = useDispatch();
    const location = useLocation(); // Get current location for redirect
    const navigate = useNavigate(); // Hook for navigation

    return (
        <nav className="navbar navbar-expand-lg navbar-light bg-light">
            <div className="container-fluid">
                <Link className="navbar-brand" to="/">Home</Link>
                {user.isLoggedIn && (
                    <>
                        <Link className="nav-link" to="/products">Products</Link>
                    </>
                )}
                {!user.isLoggedIn && (
                    <>
                        <Link className="nav-link" to="/login">Login</Link>
                        <Link className="nav-link" to="/register">Register</Link>
                    </>
                )}
                <Link className="nav-link" to="/countercomponent">Counter '{counter}' </Link>
                {user.isLoggedIn && (
                    <>
                        <button className="nav-link" onClick={() => {
                            dispatch(logout());

                           return navigate('/login', { state: { from: location } });

                        }} >Logout</button>
                    </>
                )}
            </div>
        </nav>
    );
}

export default NavComponent;