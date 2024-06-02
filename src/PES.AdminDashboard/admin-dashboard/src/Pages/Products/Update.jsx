import React, { useState, useEffect } from 'react';
import { Row, Col, Card, Carousel, Input, Button, Typography, Form, Checkbox, Select , message } from 'antd';
import { getProductDetail } from '../../API';
import { CloseOutlined } from '@ant-design/icons'
import { useParams } from 'react-router';

const UpdateProduct = () => {
    const { id } = useParams();
    const { Option } = Select;
    const { Title } = Typography;
    const [loading, setLoading] = useState(false);    
    const [subcategories, setSubcategories] = useState([]);
    const [changedFields, setChangedFields] = useState({});
    const [newImage, setNewImage] = useState(null);
    const [showOptionalFields, setShowOptionalFields] = useState(false);
    const [showNutrionAdd, setshowNutrionAdd] = useState(false);
    const [selectedCategory, setSelectedCategory] = useState(null);
    const [selectedSubCategory, setSelectedSubCategory] = useState(null);
    const [categories, setCategories] = useState([]);
    const [error, setError] = useState(null);
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

    useEffect(() => {
        fetch("http://localhost:5046/api/v1/Category")
            .then(response => {
                if (!response.ok) {
                    throw new Error('Network response was not ok');
                }
                return response.json();
            })
            .then(data => {
                setCategories(data);
                setLoading(false);
            })
            .catch(error => {
                setError(error.message);
                setLoading(false);
            });
    }, []);

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


    const handleCategoryChange = (value) => {
        setSelectedCategory(value);
        setSelectedSubCategory(null); // Reset subcategory selection
        fetchSubcategories(value); // Fetch subcategories
    };

    const handleSubCategoryChange = (value) => {
        setSelectedSubCategory(value);
    };

    const fetchSubcategories = (categoryId) => {
        fetch(`http://localhost:5046/api/v1/Category/${categoryId}`)
            .then(response => {
                if (!response.ok) {
                    throw new Error('Network response was not ok');
                }
                return response.json();
            })
            .then(data => {
                setSubcategories(data);
            })
            .catch(error => {
                message.error('Failed to fetch subcategories');
                console.error('Error:', error);
            });
    };

    return (
        <div className="container mx-auto p-6">
            <Row gutter={16}>
                <Col span={8}>
                    <Card className="shadow-lg rounded-lg">
                        <div className="relative mb-6">
                            <Carousel autoplay>
                                {updatedProduct.productImages.map((image, index) => (
                                    <div key={index} className="relative">
                                        <img
                                            src={image.isLocal ? URL.createObjectURL(image.url) : `https://firebasestorage.googleapis.com/v0/b/ntassignment-518e1.appspot.com/o/Product%2F${image.url}?alt=media&token=c49c50eb-df74-4ace-a5cf-6c52badf4074`}
                                            alt={`${updatedProduct.productName} image ${index + 1}`}
                                            className="object-cover w-full h-64 rounded-t-lg"
                                        />
                                        <Button
                                            type="text"
                                            icon={<CloseOutlined />}
                                            onClick={() => handleDeleteImage(index)}
                                            className="absolute top-2 right-2 bg-white rounded-full shadow"
                                        />
                                    </div>
                                ))}
                            </Carousel>
                        </div>
                        <input type="file" onChange={handleImageChange} className="mt-2" />
                        <Button onClick={handleAddImage} disabled={!newImage} className="mt-2 bg-green-500 hover:bg-green-600 text-white">Add Image</Button>
                        <div className="p-4">
                            <Input
                                type="text"
                                value={updatedProduct.productName}
                                onChange={e => handleInputChange('productName', e.target.value)}
                                className="text-xl font-bold w-full mb-4"
                                placeholder="Product Name"
                            />
                            <Input
                                type="number"
                                value={updatedProduct.price}
                                onChange={e => handleInputChange('price', e.target.value)}
                                className="text-gray-600 w-full mb-4"
                                placeholder="Price"
                            />
                            <h2 className="text-lg font-semibold mb-2">Category</h2>
                            <Select
                                placeholder="Select Category"
                                value={selectedCategory}
                                onChange={handleCategoryChange}
                                className="w-full mb-4"
                            >
                                {categories.map(category => (
                                    <Option key={category.categoryId} value={category.categoryId}>
                                        {category.categoryName}
                                    </Option>
                                ))}
                            </Select>
                            {selectedCategory && (
                                <>
                                    <h2 className="text-lg font-semibold mb-2">Subcategory</h2>
                                    <Select
                                        placeholder="Select Subcategory"
                                        value={selectedSubCategory}
                                        onChange={handleSubCategoryChange}
                                        className="w-full mb-4"
                                    >
                                        {subcategories.map(subcategory => (
                                            <Option key={subcategory.categoryId} value={subcategory.categoryId}>
                                                {subcategory.categoryName}
                                            </Option>
                                        ))}
                                    </Select>
                                </>
                            )}
                        </div>
                        <Form.Item className="p-4">
                            <Checkbox checked={showOptionalFields} onChange={handleCheckboxChange}>
                                Add Important Information
                            </Checkbox>
                        </Form.Item>
                        <Form.Item className="p-4">
                            <Checkbox checked={showNutrionAdd} onChange={handleCheckboxChangeNutrionInformation}>
                                Add Nutrition Information
                            </Checkbox>
                        </Form.Item>
                    </Card>
                </Col>

                <Col span={8}>
                    <Card className="shadow-lg rounded-lg">
                        <div className="p-4" style={{ visibility: showOptionalFields ? 'visible' : 'hidden', height: showOptionalFields ? 'auto' : 0 }}>
                            <h2 className="text-lg font-semibold mb-4">Important Information</h2>
                            <div className="mb-4">
                                <h3 className="text-lg font-semibold mb-2">Ingredients</h3>
                                <textarea
                                    value={updatedProduct.importantInfo?.ingredients || ''}
                                    onChange={e => handleInputChange('importantInfo.ingredients', e.target.value)}
                                    className="text-gray-600 w-full p-2 border rounded"
                                    placeholder="Ingredients"
                                />
                            </div>
                            <div className="mb-4">
                                <h3 className="text-lg font-semibold mb-2">Directions</h3>
                                <textarea
                                    value={updatedProduct.importantInfo?.directions || ''}
                                    onChange={e => handleInputChange('importantInfo.directions', e.target.value)}
                                    className="text-gray-600 w-full p-2 border rounded"
                                    placeholder="Directions"
                                />
                            </div>
                            <div className="mb-4">
                                <h3 className="text-lg font-semibold mb-2">Legal Disclaimer</h3>
                                <textarea
                                    value={updatedProduct.importantInfo?.legalDisclaimer || ''}
                                    onChange={e => handleInputChange('importantInfo.legalDisclaimer', e.target.value)}
                                    className="text-gray-600 w-full p-2 border rounded"
                                    placeholder="Legal Disclaimer"
                                />
                            </div>
                        </div>
                    </Card>
                </Col>

                <Col span={8}>
                    <Card className="shadow-lg rounded-lg">
                        <div className="p-4" style={{ visibility: showNutrionAdd ? 'visible' : 'hidden', height: showNutrionAdd ? 'auto' : 0 }}>
                            <h2 className="text-lg font-semibold mb-4">Nutritional Information</h2>
                            <ul className="text-gray-600">
                                <li className="mb-4">
                                    <Title level={5}>Calories: (kcal)</Title>
                                    <Input
                                        type="number"
                                        value={updatedProduct.nutrionInfo?.calories || ''}
                                        onChange={e => handleInputChange('nutrionInfo.calories', e.target.value)}
                                        className="w-full mb-2"
                                        placeholder="Calories"
                                    />
                                </li>
                                <li className="mb-4">
                                    <Title level={5}>Protein: (g)</Title>
                                    <Input
                                        type="number"
                                        value={updatedProduct.nutrionInfo?.protein || ''}
                                        onChange={e => handleInputChange('nutrionInfo.protein', e.target.value)}
                                        className="w-full mb-2"
                                        placeholder="Protein"
                                    />
                                </li>
                                <li className="mb-4">
                                    <Title level={5}>Sodium: (mg)</Title>
                                    <Input
                                        type="number"
                                        value={updatedProduct.nutrionInfo?.sodium || ''}
                                        onChange={e => handleInputChange('nutrionInfo.sodium', e.target.value)}
                                        className="w-full mb-2"
                                        placeholder="Sodium"
                                    />
                                </li>
                                <li className="mb-4">
                                    <Title level={5}>Fiber: (g)</Title>
                                    <Input
                                        type="number"
                                        value={updatedProduct.nutrionInfo?.fiber || ''}
                                        onChange={e => handleInputChange('nutrionInfo.fiber', e.target.value)}
                                        className="w-full mb-2"
                                        placeholder="Fiber"
                                    />
                                </li>
                                <li className="mb-4">
                                    <Title level={5}>Sugars: (g)</Title>
                                    <Input
                                        type="number"
                                        value={updatedProduct.nutrionInfo?.sugars || ''}
                                        onChange={e => handleInputChange('nutrionInfo.sugars', e.target.value)}
                                        className="w-full mb-2"
                                        placeholder="Sugars"
                                    />
                                </li>
                            </ul>
                        </div>
                    </Card>
                </Col>
            </Row>
            <div className="flex justify-center mt-6">
                <Button onClick={handleUpdate} className="bg-blue-500 hover:bg-blue-600 text-white font-bold py-2 px-4 rounded">Update Product</Button>
            </div>
        </div>
    );
};

export default UpdateProduct;