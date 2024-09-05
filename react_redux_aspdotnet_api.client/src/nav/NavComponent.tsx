import { Link } from 'react-router-dom';
import { RootState } from "../store";
import { useSelector } from "react-redux";

const NavComponent = () => {
    const counter = useSelector((state: RootState) => state.counter.counter);

    return (
        <nav className="navbar navbar-expand-lg navbar-light bg-light">
            <div className="container-fluid">
                <Link className="navbar-brand" to="/">Home</Link>
                <Link className="nav-link" to="/products">Products</Link>
                <Link className="nav-link" to="/login">Login</Link>
                <Link className="nav-link" to="/register">Register</Link>
                <Link className="nav-link" to="/countercomponent">Counter '{counter}' </Link>
            </div>
        </nav>
    );
}

export default NavComponent;