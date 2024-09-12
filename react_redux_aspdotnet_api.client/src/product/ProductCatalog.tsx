import { useEffect, useState } from 'react';
import { fetchWithAuth } from '../services/HttpService.ts';
import { useDispatch } from 'react-redux';
import AddProduct from './AddProduct.tsx';
import { Button } from 'react-bootstrap';

interface Product {
    id: number,
    name: string,
    price: number,
    description: string
}

function ProductCatalog() {
    const [products, setProducts] = useState<Product[]>();
    const [showModal, setShowModal] = useState(false);

    const handleShow = () => {
        setShowModal(true);
    }

    const handleClose = () => {
        setShowModal(false);
    }

    const dispatch = useDispatch();

    useEffect(() => {
        if (!showModal) {
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
            populateProducts();
        }
    }, [showModal, dispatch]);

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
        <>
            <div className="row">
                <div className="col-md-9 col-sm-12">
                    <h1>Products</h1>
                </div>
                <div className="col-md-3 col-sm-12">
                    <Button type="button" className="btn btn-primary" data-toggle="modal" onClick={handleShow}>
                        Add Product
                    </Button>
                </div>
            </div>
            {contents}
            <AddProduct show={showModal} handleClose={handleClose} />
        </>
    );
}

export default ProductCatalog;
export type { Product };