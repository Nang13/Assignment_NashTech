import React, { useState, useEffect } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { getProductDetail } from '../../API';
import { Card, Row, Col, Carousel, Button } from 'antd';

function ProductDetail() {
  const navigate = useNavigate();
  const { id } = useParams();
  const [loading, setLoading] = useState(false);
  const [product, setProduct] = useState(null);



  useEffect(() => {
    setLoading(true);
    getProductDetail({ id })
      .then((res) => {
        setProduct(res);
        setLoading(false);
      })
      .catch((error) => {
        console.error('Error fetching product detail:', error);
        setLoading(false);
      });
    console.log(product);
  }, [id]);

  if (loading) {
    return (
      <div className="bg-gray-100 p-6 min-h-screen flex items-center justify-center">
        <div className="text-center">
          <p className="text-xl text-gray-600">Loading...</p>
        </div>
      </div>
    );
  }

  if (!product) {
    return (
      <div className="bg-gray-100 p-6 min-h-screen flex items-center justify-center">
        <div className="text-center">
          <p className="text-xl text-gray-600">No data available</p>
        </div>
      </div>
    );
  }

  const toggleProductStatus = async (productId, isDeleted) => {
    const url = `https://localhost:7187/api/v1/Product/${productId}/${isDeleted ? 'active' : 'inactive'}`;

    try {
      const response = await fetch(url, {
        method: 'POST',
        headers: {
          'Accept': '*/*',
        },
        body: ''
      });

      if (!response.ok) {
        throw new Error('Network response was not ok');
      }

      const result = await response.json();
      console.log(`Toggled product status for productId: ${productId}`, result);

      navigate("/product");
    } catch (error) {
      console.error(`There was a problem toggling the status for productId: ${productId}`, error);
    }
  };

  return (
    <div className="container mx-auto p-4">
      <Row gutter={[16, 16]} justify="center">
        <Col span={12}>
          <Card>
            <div className="max-w-md mx-auto bg-white shadow-lg rounded-lg overflow-hidden mb-6">
              <Carousel autoplay>
                {product.productImages.map((image, index) => (
                  <div key={index}>
                    <img
                      src={`https://firebasestorage.googleapis.com/v0/b/ntassignment-518e1.appspot.com/o/Product%2F${image.url}?alt=media&token=c49c50eb-df74-4ace-a5cf-6c52badf4074`}
                      alt={`${product.productName} image ${index + 1}`}
                      className="object-cover w-full h-64"
                    />
                  </div>
                ))}
              </Carousel>
            </div>
            <div className="p-4">
              <h1 className="text-2xl font-bold mb-2">{product.productName}</h1>
              <p className="text-gray-600 text-lg mb-4">${product.price}</p>
              <h2 className="text-lg font-semibold">Category</h2>
              <p className="text-gray-600 text-lg">
                {product.productCategory.categoryName} ({product.productCategory.categoryMain})
              </p>
              <p className="text-gray-600 text-lg mb-4">
                Created: {new Date(product.createdDate).toLocaleString()}
              </p>
              <p className="text-gray-600 text-lg mb-4">
                Last Modified: {new Date(product.lastModifiedDate).toLocaleString()}
              </p>
              <Button
                onClick={() => toggleProductStatus(product.id, product.isDeleted)}
                type="primary"
                className={`${product.isDeleted ? 'bg-green-500' : 'bg-red-500'} text-white mt-4`}
              >
                {product.isDeleted ? 'Active' : 'Deactivate'}
              </Button>
            </div>
          </Card>
        </Col>
        <Col span={12}>
          <Card>
            <div className="p-4">
              <h2 className="text-xl font-semibold mb-4">Nutritional Information</h2>
              <ul className="text-gray-600 text-lg mb-6">
                <li>Calories: {product.nutrionInfo ? `${product.nutrionInfo.calories} kcal` : 'N/A'}</li>
                <li>Protein: {product.nutrionInfo ? `${product.nutrionInfo.protein} g` : 'N/A'}</li>
                <li>Sodium: {product.nutrionInfo ? `${product.nutrionInfo.sodium} mg` : 'N/A'}</li>
                <li>Fiber: {product.nutrionInfo ? `${product.nutrionInfo.fiber} g` : 'N/A'}</li>
                <li>Sugars: {product.nutrionInfo ? `${product.nutrionInfo.sugars} g` : 'N/A'}</li>
              </ul>
              <h2 className="text-xl font-semibold mb-4">Ingredients</h2>
              <p className="text-gray-600 text-lg mb-6">{product.importantInfo && product.importantInfo.ingredients ? product.importantInfo.ingredients : 'N/A'}</p>
              <h2 className="text-xl font-semibold mb-4">Directions</h2>
              <p className="text-gray-600 text-lg mb-6">{product.importantInfo && product.importantInfo.directions ? product.importantInfo.directions : 'N/A'}</p>
              <h2 className="text-xl font-semibold mb-4">Legal Disclaimer</h2>
              <p className="text-gray-600 text-lg">{product.importantInfo && product.importantInfo.legalDisclaimer ? product.importantInfo.legalDisclaimer : 'N/A'}</p>
            </div>
          </Card>
        </Col>
      </Row>
    </div >
  );
}

export default ProductDetail;
