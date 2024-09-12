import { createSlice } from '@reduxjs/toolkit';

interface UserState {
    isLoggedIn: boolean;
}

const initialState: UserState = {
    isLoggedIn: localStorage.getItem('isAuthenticated') === 'true',
};

const UserReducer = createSlice({
    name: 'user',
    initialState,
    reducers: {
        login(state) {
            localStorage.setItem('isAuthenticated', 'true');
            state.isLoggedIn = true;
        },
        logout(state) {
            localStorage.removeItem('isAuthenticated');
            localStorage.removeItem('email');
            state.isLoggedIn = false;

            const currentUrl = window.location.href;
            localStorage.setItem('redirectAfterLogin', currentUrl);
            window.location.href = '/login';
        }
    },
});

export const { login, logout } = UserReducer.actions;
export default UserReducer.reducer;
