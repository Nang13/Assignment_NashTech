import React, { useState, useEffect } from 'react';
import { Form, Checkbox, Input, Button, Upload, Select, message, Typography } from "antd";
import { UploadOutlined } from "@ant-design/icons";

function AddProductForm() {
    const { Option } = Select;
    const [productData, setProductData] = useState({
        productName: '',
        price: '',
        quantity: '',
        description: '',
        informationRequest: {
            ingredients: '',
            directions: '',
            legalDisclaimer: ''
        },
        nutritionInformationRequest: {
            calories: '',
            protein: '',
            sodium: '',
            fiber: '',
            sugars: ''
        },
        listImages: [],
        mainImage: '',
        categoryId: ''
    });

    const [form] = Form.useForm();
    const [selectedCategory, setSelectedCategory] = useState(null);
    const [selectedSubCategory, setSelectedSubCategory] = useState(null);
    const [subcategories, setSubcategories] = useState([]);
    const [categories, setCategories] = useState([]);
    const [showOptionalFields, setShowOptionalFields] = useState(false);
    const [showImage, setShowImage] = useState(false);
    const [showNutrionAdd, setshowNutrionAdd] = useState(false);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState(null);

    const handleChange = (e) => {
        const { name, value } = e.target;
        setProductData({
            ...productData,
            [name]: value
        });
    };
    const onFinish = async (values) => {
        console.log('Received values:', values);
        console.log(values.productName)

        await values.listImages.fileList.forEach(element => {
            console.log(element);
            uploadImage(values.productName, element.originFileObj);
        });


        try {
            // Construct the data payload
            const data = {
                productName: values.productName,
                price: values.price,
                quantity: values.quantity,
                description: values.description,
                imformationRequest: showOptionalFields ? {
                    ingredients: values.ingredients_optional,
                    directions: values.directions_optional,
                    legalDisclaimer: values.legalDisclaimer_optional
                } : null,
                nutrionInforrmationRequest: showNutrionAdd ? {
                    calories: values.calories,
                    protein: values.protein,
                    sodium: values.sodium,
                    fiber: values.fiber,
                    sugars: values.sugars
                } : null,
                listImages: values.listImages.fileList.map(file => file.name), // Assuming listImages is an array of files
                categoryId: selectedSubCategory// Replace with actual category ID
            };

            console.log(data);
            // Make API request to add product with image data
            const response = await fetch('http://localhost:5046/api/v1/Product', {
                method: 'Post',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(data)
            });

            if (!response.ok) {
                console.log(response)
                throw new Error('Failed to add product');
            }

            const responseData = await response.json();
            console.log('Product added successfully:', responseData);
            // Optionally, do something with the response data

            // Reset form after submission
            form.resetFields();
        } catch (error) {
            console.error('Error adding product:', error);
            // Handle error here
        }
        // form.resetFields();
    };

    const onFinishFailed = (errorInfo) => {
        console.log('Failed:', errorInfo);
    };

    const handleCategoryChange = (value) => {
        setSelectedCategory(value);
        setSelectedSubCategory(null); // Reset subcategory selection
        fetchSubcategories(value); // Fetch subcategories
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

    const handleSubCategoryChange = (value) => {
        setSelectedSubCategory(value);
    };
    const handleCheckboxChange = () => {
        setShowOptionalFields(!showOptionalFields);
    };
    const handleCheckboxChangeShowImage = () => {
        setShowImage(!showImage);
    };
    const handleCheckboxChangeNutrionInformation = () => {
        setshowNutrionAdd(!showNutrionAdd);
    };

    //? upload Image 
    const uploadImage = async (productName, file) => {
        try {
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


    return (
        <div className="max-w-md mx-auto bg-white p-6 rounded-md shadow-md">
            <h2 className="text-lg font-semibold mb-4">Add New Product</h2>
            <Form
                form={form}
                name="basic"
                initialValues={{ remember: true }}
                onFinish={onFinish}
                onFinishFailed={onFinishFailed}
                layout="vertical"
            >
                <Form.Item
                    label="Product Name"
                    name="productName"
                    rules={[{ required: true, message: 'Please input the product name!' }]}
                >
                    <Input onChange={handleChange} />
                </Form.Item>

                <Form.Item
                    label="Price"
                    name="price"
                    rules={[{ required: true, message: 'Please input the price!' }]}
                >
                    <Input type="number" onChange={handleChange} />
                </Form.Item>

                <Form.Item label="Quantity" name="quantity">
                    <Input type="number" onChange={handleChange} />
                </Form.Item>

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
                        <Typography.Title level={4} className="text-gray-800">Subcategory</Typography.Title>
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
                <Form.Item
                    label="Description"
                    name="description"
                    rules={[{ required: true, message: 'Please input the description!' }]}
                >
                    <Input.TextArea onChange={handleChange} />
                </Form.Item>

                <Form.Item>
                    <Checkbox checked={showImage} onChange={handleCheckboxChangeShowImage}>
                        Add Image Options
                    </Checkbox>
                </Form.Item>

                {showImage && (
                    <div>
                        <Form.Item label="Images" name="listImages">
                            <Upload
                                beforeUpload={() => false}
                                listType="picture"
                                maxCount={10} // Maximum number of images user can upload
                                accept="image/*"
                            >
                                <Button icon={<UploadOutlined />}>Upload</Button>
                            </Upload>
                        </Form.Item>
                    </div>
                )}

                <Form.Item>
                    <Checkbox checked={showOptionalFields} onChange={handleCheckboxChange}>
                        Add Important Information
                    </Checkbox>
                </Form.Item>

                {showOptionalFields && (
                    <div>
                        <Form.Item label="Ingredients" name="ingredients_optional">
                            <Input onChange={handleChange} />
                        </Form.Item>
                        <Form.Item label="Directions" name="directions_optional">
                            <Input onChange={handleChange} />
                        </Form.Item>
                        <Form.Item label="Legal Disclaimer" name="legalDisclaimer_optional">
                            <Input onChange={handleChange} />
                        </Form.Item>
                    </div>
                )}

                <Form.Item>
                    <Checkbox checked={showNutrionAdd} onChange={handleCheckboxChangeNutrionInformation}>
                        Add Nutrion Information
                    </Checkbox>
                </Form.Item>

                {showNutrionAdd && (
                    <div>
                        <Form.Item label="Calories" name="calories">
                            <Input type="number" onChange={handleChange} />
                        </Form.Item>
                        <Form.Item label="Protein" name="protein">
                            <Input type="number" onChange={handleChange} />
                        </Form.Item>
                        <Form.Item label="Sodium" name="sodium">
                            <Input type="number" onChange={handleChange} />
                        </Form.Item>
                        <Form.Item label="Fiber" name="fiber">
                            <Input type="number" onChange={handleChange} />
                        </Form.Item>
                        <Form.Item label="Sugars" name="sugars">
                            <Input type="number" onChange={handleChange} />
                        </Form.Item>
                    </div>
                )}

                <Form.Item>
                    <Button type="primary" htmlType="submit">
                        Add Product
                    </Button>
                </Form.Item>
            </Form>
        </div>
    );
}

export default AddProductForm;