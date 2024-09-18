import { Link, useLocation, useNavigate } from 'react-router-dom';
import { RootState } from "../store";
import { useSelector, useDispatch } from "react-redux";
import { logout } from '../reducers/UserReducer';
import './nav.css';

const NavComponent = () => {
    const counter = useSelector((state: RootState) => state.counter.counter);
    const user = useSelector((state: RootState) => state.user);
    const dispatch = useDispatch();
    const location = useLocation(); // Get current location for redirect
    const navigate = useNavigate(); // Hook for navigation

    return (
        <nav className="navbar navbar-expand-lg navbar-light bg-light">
            <div className="container-fluid">
                <Link className={`navbar-brand ${location.pathname === '/' ? ' active ' :''}`} to="/">Home</Link>
                {user.isLoggedIn && (
                    <>
                        <Link className={`nav-link ${location.pathname === '/products' ? ' active ' :''}`} to="/products" >Products</Link>
                    </>
                )}
                {!user.isLoggedIn && (
                    <>
                        <Link className={`nav-link ${location.pathname === '/login' ? ' active ' :''}`}  to="/login">Login</Link>
                        <Link className={`nav-link ${location.pathname === '/register' ? ' active ' :''}`}  to="/register">Register</Link>
                    </>
                )}
                <Link className={`nav-link ${location.pathname === '/countercomponent' ? ' active ' :''}`}  to="/countercomponent">Counter '{counter}' </Link>
                {user.isLoggedIn && (
                    <>
                        <div className="nav-item">
                            <span className="navbar-text">Welcome {user.userEmail}</span>
                            <button className="btn btn-link nav-link text-right" onClick={() => {
                                dispatch(logout());
                                
                                return navigate('/login', { state: { from: location } });
                
                            }}>
                                Logout
                            </button>
                        </div>
                    </>
                )}
            </div>
        </nav>
    );
}

export default NavComponent;