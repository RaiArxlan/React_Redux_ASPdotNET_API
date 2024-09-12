import { useEffect, useState } from 'react';
import { fetchWithAuth } from '../services/HttpService.ts';
import { useDispatch } from 'react-redux';

interface Product {
    id: number,
    name: string,
    price: number,
    description: string
}

function ProductCatalog() {
    const [products, setProducts] = useState<Product[]>();
    const dispatch = useDispatch();

    useEffect(() => {
        populateProducts();
    }, []);

    const contents = products === undefined
        ? <p><em>Loading... </em></p>
        : <table className="table table-striped" aria-labelledby="tableLabel">
            <thead>
                <tr>
                    <th>Id</th>
                    <th>Name</th>
                    <th>Price</th>
                    <th>Description</th>
                </tr>
            </thead>
            <tbody>
                {products.map(forecast =>
                    <tr key={forecast.id}>
                        <td>{forecast.id}</td>
                        <td>{forecast.name}</td>
                        <td>{forecast.price}</td>
                        <td>{forecast.description}</td>
                    </tr>
                )}
            </tbody>
        </table>;

    return (
        <div>
            <h1 id="tableLabel">Products</h1>
            <p>This component demonstrates fetching data from the server.</p>
            {contents}
        </div>
    );

    async function populateProducts() {
        try {
            const response = await fetchWithAuth('/api/products/list', {}, dispatch);
            if (!response.ok) {
                setProducts([]);
                throw new Error(`HTTP error! status: ${response.status}`);
            }
            const data = await response.json();

            setProducts(data);
        } catch (error) {
            console.error(error);
        }
    }
}

export default ProductCatalog;