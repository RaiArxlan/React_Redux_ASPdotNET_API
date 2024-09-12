import { useState } from 'react';
import { Modal, Button, Form, FormControl, InputGroup } from 'react-bootstrap';
import { fetchWithAuth } from '../services/HttpService';
import { useDispatch } from 'react-redux';

interface AddProductProps {
  show: boolean;
  handleClose: () => void;
}

function AddProduct({ show, handleClose }: AddProductProps) {
  const dispatch = useDispatch();
  
  const [name, setName] = useState('');
  const [price, setPrice] = useState(0);
  const [description, setDescription] = useState('');
  
  const [errors, setErrors] = useState<{ name?: string; price?: string; description?: string }>({});

  const validate = () => {

    console.log('Validating', name, price, description);

    const newErrors: { name?: string; price?: string; description?: string } = {};

    if (!name) newErrors.name = 'Name is required';

    if (!price) newErrors.price = 'Price is required';
    else if (isNaN(Number(price))) newErrors.price = 'Price must be a number';

    if (!description) newErrors.description = 'Description is required';
    
    return newErrors;
  };

  return (
    <Modal show={show} onHide={handleClose}>
      <Modal.Header closeButton>
        <Modal.Title>Add Product</Modal.Title>
      </Modal.Header>
      <Modal.Body>

        <Form>
          <InputGroup className="mb-3">
            <InputGroup.Text id="inputGroup-Name">Name</InputGroup.Text>
            <FormControl
              aria-label="Name"
              aria-describedby="inputGroup-Name"
              value={name}
              onChange={(e) => {setName(e.target.value); validate();}}
              isInvalid={!!errors.name}
            />
            <FormControl.Feedback type="invalid">{errors.name}</FormControl.Feedback>
          </InputGroup>

          <InputGroup className="mb-3">
            <InputGroup.Text id="inputGroup-Price">Price</InputGroup.Text>
            <FormControl
              type="number"
              aria-label="Price"
              aria-describedby="inputGroup-Price"
              value={price}
              onChange={(e) => {setPrice(Number(e.target.value)); validate();}}
              isInvalid={!!errors.price}
            />
            <FormControl.Feedback type="invalid">{errors.price}</FormControl.Feedback>
          </InputGroup>

          <InputGroup className="mb-3">
            <InputGroup.Text id="inputGroup-Description">Description</InputGroup.Text>
            <FormControl
              as="textarea"
              aria-label="Description"
              aria-describedby="inputGroup-Description"
              value={description}
              onChange={(e) => {setDescription(e.target.value); validate();}}
              isInvalid={!!errors.description}
            />
            <FormControl.Feedback type="invalid">{errors.description}</FormControl.Feedback>
          </InputGroup>
        </Form>
      </Modal.Body>
      <Modal.Footer>
        <Button variant="secondary" onClick={handleClose}>
          Close
        </Button>
        <Button variant="primary" onClick={saveProduct}>
          Save Changes
        </Button>
      </Modal.Footer>
    </Modal>
  );

  function saveProduct() {

    const validationErrors = validate();
    if (Object.keys(validationErrors).length > 0) {
      setErrors(validationErrors);
      return;
    }

    fetchWithAuth('/api/products/create', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json'
      },
      body: JSON.stringify({ name, price, description })
    }, dispatch).then(response => {
      if (response.ok) {
        
        setName('');
        setPrice(0);
        setDescription('');

        console.log('Product added');
        alert('Product added');
        handleClose();
      }

    }
    ).catch(error => {
      console.error('Error adding product', error);
    });

    return;
  }
}

export default AddProduct;
