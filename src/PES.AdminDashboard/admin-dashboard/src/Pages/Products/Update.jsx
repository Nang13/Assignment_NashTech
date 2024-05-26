import React, { useState, useEffect } from 'react';
import { Row, Col, Card, Carousel, Input, Button, Typography, Form, Checkbox, } from 'antd';
import { getProductDetail } from '../../API';
import { CloseOutlined } from '@ant-design/icons'
import { useParams } from 'react-router';

const UpdateProduct = () => {
    const { id } = useParams();
    const [loading, setLoading] = useState(false);
    const [newImage, setNewImage] = useState(null);
    const [showOptionalFields, setShowOptionalFields] = useState(false);
    const [showNutrionAdd, setshowNutrionAdd] = useState(false);
    const [updatedProduct, setUpdatedProduct] = useState({
        productName: '',
        price: 0,
        description: '',
        quantity: 0,
        categoryId: '',
        nutrionInfo: {
            calories: 0,
            protein: 0,
            sodium: 0,
            fiber: 0,
            sugars: 0
        },
        importantInfo: {
            ingredients: '',
            directions: '',
            legalDisclaimer: ''
        },
        productImages: []
    });

    const [changedFields, setChangedFields] = useState([]);

    useEffect(() => {
        setLoading(true);
        getProductDetail({ id })
            .then((res) => {
                console.log(res)
                setUpdatedProduct(res);
                setLoading(false);
            })
            .catch((error) => {
                console.error('Error fetching product detail:', error);
                setLoading(false);
            });
    }, [id]);

    const handleCheckboxChange = () => {
        setShowOptionalFields(!showOptionalFields);
    };

    const handleCheckboxChangeNutrionInformation = () => {
        setshowNutrionAdd(!showNutrionAdd);
    };

    if (loading) {
        return (
            <div className="bg-gray-100 p-6 min-h-screen flex items-center justify-center">
                <div className="text-center">
                    <p className="text-xl text-gray-600">Loading...</p>
                </div>
            </div>
        );
    }

    const handleAddImage = () => {
        if (newImage) {
            const tempUrl = URL.createObjectURL(newImage);
            setUpdatedProduct(prevState => ({
                ...prevState,
                productImages: [...prevState.productImages, { url: newImage, isLocal: true }] // Set isLocal to true for local images
            }));
            setNewImage(null);
        }
    };

    const handleDeleteImage = (index) => {
        setUpdatedProduct(prevState => ({
            ...prevState,
            productImages: prevState.productImages.filter((_, i) => i !== index)
        }));
    };
    const handleChange = (fieldName, value) => {
        setUpdatedProduct({
            ...updatedProduct,
            [fieldName]: value
        });
        if (!changedFields.includes(fieldName)) {
            setChangedFields([...changedFields, fieldName]);
        }
    };

    const handleNutritionalInfoChange = (fieldName, value) => {
        setUpdatedProduct({
            ...updatedProduct,
            nutrionInforrmationRequest: {
                ...updatedProduct.nutrionInforrmationRequest,
                [fieldName]: value
            }
        });
        if (!changedFields.includes(fieldName)) {
            setChangedFields([...changedFields, fieldName]);
        }
    };
    const handleInputChange = (field, value) => {
        setUpdatedProduct({
            ...updatedProduct,
            [field]: value,
        });
    };

    const handleImportantInfoChange = (fieldName, value) => {
        setUpdatedProduct({
            ...updatedProduct,
            importantImformationRequest: {
                ...updatedProduct.importantImformationRequest,
                [fieldName]: value
            }
        });
        if (!changedFields.includes(fieldName)) {
            setChangedFields([...changedFields, fieldName]);
        }
    };

    const  handleUpdate = () => {
        // Prepare data to be sent in the patch request
        const dataToSend = {};
        changedFields.forEach(fieldName => {
            dataToSend[fieldName] = updatedProduct[fieldName];
        });

        // Send patch request to API with dataToSend
        console.log('Fields changed:', changedFields);
        console.log('Data to send:', dataToSend);
    };
    const handleImageChange = (e) => {
        setNewImage(e.target.files[0]);
    };


    return (
        <div className='w-full'>
            <Row gutter={16}>
                <Col span={8}>
                    <Card>
                        <div className="max-w-md mx-auto bg-white shadow-lg rounded-lg overflow-hidden mb-6">
                            <div className="relative h-64 w-full">
                                <Carousel autoplay>
                                    {updatedProduct.productImages.map((image, index) => (
                                        <div key={index} className="relative">
                                            <img
                                                src={image.isLocal ? URL.createObjectURL(image.url) : `https://firebasestorage.googleapis.com/v0/b/ntassignment-518e1.appspot.com/o/product%2F${image.url}?alt=media&token=c49c50eb-df74-4ace-a5cf-6c52badf4074`}
                                                alt={`${updatedProduct.productName} image ${index + 1}`}
                                                className="object-cover w-full h-64"
                                            />
                                            {image.isLocal && (
                                                <Button
                                                    type="text"
                                                    icon={<CloseOutlined />}
                                                    onClick={() => handleDeleteImage(index)}
                                                    className="absolute top-2 right-2 bg-white rounded-full"
                                                />
                                            )}
                                        </div>
                                    ))}
                                </Carousel>
                            </div>
                            <input type="file" onChange={handleImageChange} className="mt-2" />
                            <Button onClick={handleAddImage} disabled={!newImage}>Add Image</Button>
                        </div>
                        <div className="p-4">
                            <Input
                                type="text"
                                value={updatedProduct.productName}
                                onChange={e => handleInputChange('productName', e.target.value)}
                                className="text-xl font-bold w-full mb-2"
                            />
                            <Input
                                type="number"
                                value={updatedProduct.price}
                                onChange={e => handleInputChange('price', e.target.value)}
                                className="text-gray-600 w-full mb-2"
                            />
                            <h2 className="text-lg font-semibold">Category</h2>
                            <Input
                                type="text"
                                value={updatedProduct.categoryId}
                                onChange={e => handleInputChange('categoryId', e.target.value)}
                                className="text-gray-600 w-full mb-2"
                            />
                        </div>

                        <Form.Item>
                            <Checkbox checked={showOptionalFields} onChange={handleCheckboxChange}>
                                Add Important Information
                            </Checkbox>
                        </Form.Item>
                        <Form.Item>
                            <Checkbox checked={showNutrionAdd} onChange={handleCheckboxChangeNutrionInformation}>
                                Add Nutrion Information
                            </Checkbox>
                        </Form.Item>
                    </Card>
                </Col>

                <Col span={8}>
                    <Card>
                        <div className="mt-4" style={{ visibility: showOptionalFields ? 'visible' : 'hidden', height: showOptionalFields ? 'auto' : 0 }}>
                            <h2 className="text-lg font-semibold">Important Information</h2>
                            <div className="mt-4">
                                <h2 className="text-lg font-semibold">Ingredients</h2>
                                <textarea
                                    value={updatedProduct.importantInfo?.ingredients || ''}
                                    onChange={e => handleInputChange('importantInfo.ingredients', e.target.value)}
                                    className="text-gray-600 w-full mb-2"
                                />
                            </div>
                            <div className="mt-4">
                                <h2 className="text-lg font-semibold">Directions</h2>
                                <textarea
                                    value={updatedProduct.importantInfo?.directions || ''}
                                    onChange={e => handleInputChange('importantInfo.directions', e.target.value)}
                                    className="text-gray-600 w-full mb-2"
                                />
                            </div>
                            <div className="mt-4">
                                <h2 className="text-lg font-semibold">Legal Disclaimer</h2>
                                <textarea
                                    value={updatedProduct.importantInfo?.legalDisclaimer || ''}
                                    onChange={e => handleInputChange('importantInfo.legalDisclaimer', e.target.value)}
                                    className="text-gray-600 w-full mb-2"
                                />
                            </div>
                        </div>
                    </Card>
                </Col>

                <Col span={8}>
                    <Card>
                        <div className="mt-4" style={{ visibility: showNutrionAdd ? 'visible' : 'hidden', height: showNutrionAdd ? 'auto' : 0 }}>
                            <h2 className="text-lg font-semibold">Nutritional Information</h2>
                            <ul className="mt-2 text-gray-600">
                                <li>
                                    <Typography.Title level={5}>Calories: (kcal)</Typography.Title>
                                    <Input
                                        type="number"
                                        value={updatedProduct.nutrionInfo?.calories || ''}
                                        onChange={e => handleInputChange('nutrionInfo.calories', e.target.value)}
                                        className="w-full mb-2"
                                    />
                                </li>
                                <li>
                                    <Typography.Title level={5}>Protein: (g)</Typography.Title>
                                    <Input
                                        type="number"
                                        value={updatedProduct.nutrionInfo?.protein || ''}
                                        onChange={e => handleInputChange('nutrionInfo.protein', e.target.value)}
                                        className="w-full mb-2"
                                    />
                                </li>
                                <li>
                                    <Typography.Title level={5}>Sodium: (mg)</Typography.Title>
                                    <Input
                                        type="number"
                                        value={updatedProduct.nutrionInfo?.sodium || ''}
                                        onChange={e => handleInputChange('nutrionInfo.sodium', e.target.value)}
                                        className="w-full mb-2"
                                    />
                                </li>
                                <li>
                                    <Typography.Title level={5}>Fiber: (g)</Typography.Title>
                                    <Input
                                        type="number"
                                        value={updatedProduct.nutrionInfo?.fiber || ''}
                                        onChange={e => handleInputChange('nutrionInfo.fiber', e.target.value)}
                                        className="w-full mb-2"
                                    />
                                </li>
                                <li>
                                    <Typography.Title level={5}>Sugars: (g)</Typography.Title>
                                    <Input
                                        type="number"
                                        value={updatedProduct.nutrionInfo?.sugars || ''}
                                        onChange={e => handleInputChange('nutrionInfo.sugars', e.target.value)}
                                        className="w-full mb-2"
                                    />
                                </li>
                            </ul>
                        </div>
                    </Card>
                </Col>
            </Row>
            <Button onClick={handleUpdate}>Update Product</Button>
        </div>
    );
};

export default UpdateProduct;