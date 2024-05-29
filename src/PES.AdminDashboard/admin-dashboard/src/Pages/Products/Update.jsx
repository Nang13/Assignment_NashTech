import React, { useState, useEffect } from 'react';
import { Row, Col, Card, Carousel, Input, Button, Typography, Form, Checkbox, } from 'antd';
import { getProductDetail } from '../../API';
import { CloseOutlined } from '@ant-design/icons'
import { useParams } from 'react-router';

const UpdateProduct = () => {
    const { id } = useParams();
    const [loading, setLoading] = useState(false);
    const [changedFields, setChangedFields] = useState({});
    const [newImage, setNewImage] = useState(null);
    const [showOptionalFields, setShowOptionalFields] = useState(false);
    const [showNutrionAdd, setshowNutrionAdd] = useState(false);
    const [updatedProduct, setUpdatedProduct] = useState({
        id: '',
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
            setChangedFields(prev => ({
                ...prev,
                productImages: true
            }));
            setNewImage(null);
        }

    };

    const handleDeleteImage = (index) => {
        setUpdatedProduct(prevState => ({
            ...prevState,
            productImages: prevState.productImages.filter((_, i) => i !== index)
        }));
        setChangedFields(prev => ({
            ...prev,
            productImages: true
        }));

    };

    const handleInputChange = (field, value) => {
        const keys = field.split('.');
        setUpdatedProduct(prevState => {
            let updated = { ...prevState };
            let current = updated;

            // Iterate through keys to reach the target field
            for (let i = 0; i < keys.length - 1; i++) {
                if (!current[keys[i]]) {
                    current[keys[i]] = {};
                }
                current = current[keys[i]];
            }

            // Update the value of the target field
            current[keys[keys.length - 1]] = value;

            // Mark the field as changed
            setChangedFields(prev => ({
                ...prev,
                [field]: true
            }));

            return updated;
        });
    };


    const handleUpdate = async () => {
        const payload = {};
        Object.keys(changedFields).forEach(field => {
            const keys = field.split('.');
            let current = payload;
            let productCurrent = updatedProduct;

            // Iterate through keys to construct the payload
            for (let i = 0; i < keys.length; i++) {
                if (i === keys.length - 1) {
                    current[keys[i]] = productCurrent[keys[i]];
                } else {
                    if (!current[keys[i]]) {
                        current[keys[i]] = {};
                    }
                    current = current[keys[i]];
                    productCurrent = productCurrent[keys[i]];
                }
            }
        });




        console.log(payload);
        const imageUploadPromises = updatedProduct.productImages.map(async (image) => {
            console.log(image)
            if (image.isLocal) {
                const uploadedUrl = await uploadImage(updatedProduct.productName, image.url);
                return { ...image, url: uploadedUrl, isLocal: false };
            }
            return image;
        });

        await Promise.all(imageUploadPromises);
        if (payload.productImages) {

            const transformedArray = payload.productImages.map((item, index) => {
                const isMain = item.isMain !== undefined ? item.isMain : false; // Default value if isMain is not provided
                const fileName = item.url instanceof File ? item.url.name : item.url; // Get the fileName from url if it's a File object
                return fileName; // Return the transformed object
            });
            payload.productImages = transformedArray;
        }


        // fetch(`http://localhost:5046/api/v1/Product/${updatedProduct.id}`, {
        //     method: 'PATCH',
        //     headers: { 'Content-Type': 'application/json' },
        //     body: JSON.stringify(payload)
        // })
        //     .then(response => response.json())
        //     .then(data => {
        //         console.log('Success:', data);
        //     })
        //     .catch(error => {
        //         console.error('Error:', error);
        //     });

    };

    const uploadImage = async (productName, file) => {
        try {
            console.log(productName);
            const formData = new FormData();
            formData.append("imageFile", file);
            
            const response = await fetch(`http://localhost:5046/api/v1/Product/upload?productName=${encodeURIComponent(productName)}`, {
                method: 'POST',
                body: formData
            });
    
            if (!response.ok) {
                throw new Error('Failed to upload image');
            }
    
            const data = await response.json();
            console.log('Image uploaded successfully:', data);
            return data; // Return any response data if needed
        } catch (error) {
            console.error('Error uploading image:', error);
            throw error;
        }
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
                                            <Button
                                                type="text"
                                                icon={<CloseOutlined />}
                                                onClick={() => handleDeleteImage(index)}
                                                className="absolute top-2 right-2 bg-white rounded-full"
                                            />

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
                                        placeholder='Nutrion Info'
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