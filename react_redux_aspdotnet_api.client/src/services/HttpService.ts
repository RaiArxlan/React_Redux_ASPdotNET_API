import { login, logout } from '../reducers/UserReducer';
import { Dispatch, UnknownAction } from 'redux';

async function refreshToken(): Promise<void> {
    // Implement your token refresh logic here
    const response = await fetch('api/auth/refreshtoken', {
        method: 'POST',
        credentials: 'include', // Include cookies if needed
    });

    if (!response.ok) {
        localStorage.removeItem('isAuthenticated');
        localStorage.removeItem('email');

        throw new Error('Failed to refresh token');
    }

    // no need to store the token + refresh token, it's already stored in the browser cookie
}

async function fetchWithAuth(url: string, options: RequestInit = {}, dispatch : Dispatch<UnknownAction> ): Promise<Response> {
    const response = await fetch(url, { ...options });

    if (response.status === 401) {
        try {
            await refreshToken();
            dispatch(login());

            return fetch(url, { ...options });
        } catch (error) {
            console.error('Token refresh failed', error);
            dispatch(logout());
            
            throw error;
        }
    }

    return response;
}

async function fetch(url: string, options: RequestInit = {}): Promise<Response> {
    try {
        const response = await window.fetch(url, { ...options });
        return response;
    } catch (error) {
        console.error('An error occurred', error);
        throw error;
    }
}

export { fetch, fetchWithAuth };