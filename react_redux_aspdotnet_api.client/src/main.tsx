import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';
import App from './App.tsx'
import './index.css'
import LoginComponent from './user/LoginComponent.tsx'
import withAuth from './withAuth.tsx'
import ProductCatalog from './product/ProductCatalog.tsx';
import RegisterComponent from './user/RegisterComponent.tsx';

createRoot(document.getElementById('root')!).render(
    <StrictMode>
        <BrowserRouter>
            <Routes>
                <Route path="/" element={<App />} />
                <Route path="/login" Component={LoginComponent} />
                <Route path="/register" Component={RegisterComponent} />
                <Route path="/products" Component={withAuth(ProductCatalog)} />

                // Redirect to home if no route matches
                <Route path="*" element={<Navigate to="/" />} />
            </Routes>
        </BrowserRouter>
    </StrictMode>,
)
