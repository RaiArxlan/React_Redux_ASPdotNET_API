import React, { useState, useEffect } from 'react';
import { Navigate, useLocation } from 'react-router-dom';

// The HOC as a function that returns a functional component
const withAuth = <P extends object>(WrappedComponent: React.ComponentType<P>) => {
    return (props: P) => {
        const [isAuthenticated, setIsAuthenticated] = useState<boolean | null>(null);
        const location = useLocation(); // Get current location for redirect

        useEffect(() => {
            const login = localStorage.getItem('isAuthenticated') === 'true';

            if (login)
                setIsAuthenticated(true);
            else
                setIsAuthenticated(false);
        }, []);

        if (isAuthenticated === null) {
            return <div>Loading...</div>;
        }

        if (!isAuthenticated) {
            // Redirect to login with the current location state
            return <Navigate to="/login" state={{ from: location }} replace />;
        }

        if (isAuthenticated) {
            return <WrappedComponent {...props} />;
        }
    };
};

export default withAuth;
