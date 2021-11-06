import axios from 'axios';
import React, {useEffect, useState} from 'react';
import { Product } from '../../app/models/product';
import ProductList from './ProductList';

const Catalog = () => {
  const [products, setProducts] = useState<Product[]>([]);

  useEffect(() => {
    axios.get('https://localhost:5001/api/products')
      .then(response => setProducts(response.data))
      .catch(error => console.log(error));
  }, []);
  
  return (
    <>
      <ProductList products={products} />
    </>
  );
};

export default Catalog;
