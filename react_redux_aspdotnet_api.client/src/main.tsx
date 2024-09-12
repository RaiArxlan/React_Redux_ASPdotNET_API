import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';
import WeatherForecast from './WeatherForecast/WeatherForecast.tsx'
import LoginComponent from './user/LoginComponent.tsx'
import withAuth from './withAuth.tsx'
import ProductCatalog from './product/ProductCatalog.tsx';
import RegisterComponent from './user/RegisterComponent.tsx';
import { Provider } from 'react-redux';
import store from './store.ts';
import 'bootstrap/dist/css/bootstrap.css';
import CounterComponent from './counter/CounterComponent.tsx';
import NavComponent from './nav/NavComponent.tsx';

createRoot(document.getElementById('root')!).render(
    <StrictMode>
        <Provider store={store}>
            <BrowserRouter>
                <NavComponent />
                <div className="container mt-2">
                    <Routes>
                        <Route path="/" element={<WeatherForecast />} />
                        <Route path="/login" Component={LoginComponent} />
                        <Route path="/register" Component={RegisterComponent} />
                        <Route path="/products" Component={withAuth(ProductCatalog)} />
                        <Route path="/countercomponent" Component={CounterComponent} />"

                        // Redirect to home if no route matches
                        <Route path="*" element={<Navigate to="/" />} />
                    </Routes>
                </div>
            </BrowserRouter>
        </Provider>
    </StrictMode>,
)