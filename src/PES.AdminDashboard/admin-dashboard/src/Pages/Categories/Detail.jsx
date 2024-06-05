import React, { useEffect, useState } from 'react';
import { getCategoryDetail } from '../../API/index';
import { useParams } from 'react-router-dom';
import { Button, Modal, Form, Select, Input, message } from "antd";
import { PlusOutlined, DeleteOutlined, EditOutlined } from "@ant-design/icons";
import { Bounce, ToastContainer, toast } from 'react-toastify';



function CategoryDetail() {
    const { id } = useParams();
    const [loading, setLoading] = useState(false);
    const [dataSource, setDataSource] = useState([]);
    const [isModalOpen, setIsModalOpen] = useState(false);
    const [isModalAdding, setIsModalAdding] = useState(false);
    const [isModalUpdate, setIsModalUpdate] = useState(false);

    //? cacheData Transfer
    const [currentCategory, setCurrentCategory] = useState(null);
    const [form] = Form.useForm();


    const showModal = (category = null) => {
        console.log(category);
        setIsModalOpen(true);
        setCurrentCategory(category);
        // if (category) {
        //     form.setFieldsValue({
        //         categoryName: category.name,
        //         categoryDescription: category.description,
        //     });
        // } else {
        //     form.resetFields();
        // }
        //   setIsModalVisible(true);
        deleteCategory(category.categoryId)
    };

  
    const deleteCategory = (categoryId) => {
        const url = `http://localhost:5046/api/v1/Category/${categoryId}`;

        const requestOptions = {
            method: 'DELETE',
            headers: {
                'Content-Type': 'application/json',
            }
        };

        fetch(url, requestOptions)
            .then(response => {
                if (!response.ok) {
                    throw new Error('Network response was not ok');
                }
                window.location.reload()
                message.success('Category deleted successfully');
                // You can perform additional actions here if needed
            })
            .catch(error => {
                console.error('Error:', error);
                // Handle error here
            });
    };

    const handleCancel = () => {
        // setIsModalVisible(false);
        setCurrentCategory(null);
    };

    const showModalAdding = (categoryId) => {
        form.setFieldsValue({
            categoryId: categoryId,
            categoryName: '',
            categoryDescription: '',
        });
        setIsModalAdding(true);
    }
    const showModalUpdate = (category) => {
        setCurrentCategory(category);
        form.setFieldsValue({
            categoryId: category.id,
            categoryName: category.name,
            categoryDescription: category.description,
        });

        setIsModalUpdate(true);
        // setIsUpdating(true);
        //  setIsModalVisible(true);
    };


    const handleOKAdding = () => {
        form
            .validateFields()
            .then(values => {
                const payload = {
                    categoryName: values.categoryName,
                    categoryDescription: values.categoryDescription,
                    categoryParentId: values.categoryId
                };

                console.log(payload);
                const requestOptions = {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json' },
                    body: JSON.stringify(payload),
                };

                fetch("http://localhost:5046/api/v1/Category", requestOptions)
                    .then(response => {
                        if (!response.ok) {
                            toast(response.message, {
                                position: "top-right",
                                autoClose: 5000,
                                hideProgressBar: false,
                                closeOnClick: true,
                                pauseOnHover: true,
                                draggable: true,
                                progress: undefined,
                                theme: "light",
                                transition: Bounce,
                            });
                        }
                        return response.json();
                    })
                    .then(data => {
                        debugger
                        if (data.status == 400) {
                            message.error("Name have been duplicated or the length of description");
                        } else {
                            message.success("Add Category Successfully")
                            console.log(data);
                            form.resetFields();
                            setCurrentCategory(null);
                            window.location.reload()
                        }
                        // Reload the page to get new elements
                    })
                    .catch(error => {
                        console.error('Error:', error);
                        // message.error('Failed to submit category');
                    });
            })
            .catch(info => {
                console.log('Validate Failed:', info);
            });

        setIsModalAdding(false);
    }

    const handleOkUpdate = () => {
        form
            .validateFields()
            .then(values => {
                const payload = {
                    categoryName: values.categoryName,
                    categoryDescription: values.categoryDescription,
                };

                const requestOptions = {
                    method: "PATCH",
                    headers: { 'Content-Type': 'application/json' },
                    body: JSON.stringify(payload),
                };

                fetch(`http://localhost:5046/api/v1/Category/${currentCategory.id}`, requestOptions)
                    .then(response => {
                        debugger
                        console.log(response);
                        if (response.status != 200) {
                            message.error("Check your data again")
                        } else {
                            message.success('Category updated successfully');
                            form.resetFields();
                            setIsModalAdding(false);
                            setIsModalUpdate(false);
                            setCurrentCategory(null);
                            window.location.reload()
                        }
                        return response.json();
                    })
                    .catch(error => {
                        console.error('Error:', error);
                        message.error('Failed to submit category');
                    });

            }).catch(info => {
                console.log('Validate Failed:', info);
            });
    }




    const handleOk = () => {
        setIsModalOpen(false);
        setIsModalAdding(false);
        setIsModalUpdate(false);

        form
            .validateFields()
            .then(values => {
                // Handle form submission logic here
                console.log('Category Data:', values);
                form.resetFields();
                setIsModalAdding(false);
            })
            .catch(info => {
                console.log('Validate Failed:', info);
            });
    };


    useEffect(() => {
        setLoading(true);
        getCategoryDetail({ id }).then((res) => {
            setDataSource(res);
            setLoading(false);
        }).catch(error => {
            console.error('Error fetching category detail:', error);
            setLoading(false);
        });

        console.log(dataSource);
    }, [id]);

    const renderSubcategories = (parentId) => {
        const subcategories = dataSource.filter(c => c.parentId === parentId);
        if (subcategories.length === 0) return null;
        return (
            <ul className="pl-5 list-disc">
                {subcategories.map(subcategory => (
                    <li key={subcategory.categoryId}>
                        <div className="mb-5">
                            <div className="flex items-center text-base">
                                <div className="ml-2 text-gray-700">{subcategory.categoryName}</div>
                                <PlusOutlined onClick={() => showModalAdding(subcategory.categoryId)} style={{ color: 'green' }} className='pl-3' />
                                <DeleteOutlined onClick={() => showModal(subcategory)} style={{ color: 'red' }} className='pl-3' />
                                <EditOutlined onClick={() => showModalUpdate({ id: subcategory.categoryId, name: subcategory.categoryName, description: subcategory.categoryDescription })} style={{ color: 'black' }} className='pl-3' />
                            </div>

                            <div className="pl-5 border-t border-gray-300">
                                {renderSubcategories(subcategory.categoryId)}
                            </div>
                        </div>
                    </li>
                ))}
            </ul>
        );
    };

    const renderTopLevelCategories = () => {
        const topLevelCategories = dataSource.filter(category => category.parentId === "00000000-0000-0000-0000-000000000000");
        return (
            <ul className="w-full max-w-3xl mx-auto bg-white border rounded shadow-lg">
                {topLevelCategories.map(category => (
                    <li key={category.categoryId} className="mb-5">
                        <div className="flex items-center text-base">
                            <div className="ml-2 text-gray-700">{category.categoryName}</div>
                            <PlusOutlined onClick={() => showModalAdding(category.categoryId)} style={{ color: 'green' }} className='pl-3' />
                            <DeleteOutlined onClick={() => showModal(category)} style={{ color: 'red' }} className='pl-3' />
                            <EditOutlined onClick={() => showModalUpdate({ id: category.categoryId, name: category.categoryName, description: category.categoryDescription })} style={{ color: 'black' }} className='pl-3' />
                        </div>


                        <Modal title="Basic Modal" open={isModalOpen} onOk={handleOk} onCancel={handleCancel}>
                            <p>Is Delete Category ?</p>
                        </Modal>
                        <Modal title="Basic Modal" open={isModalAdding} onOk={handleOKAdding} onCancel={handleCancel}>
                            <Form
                                form={form}
                                layout="vertical"
                                name="add_category_form"
                            >
                                <Form.Item
                                    name="categoryName"
                                    label="Category Name"
                                    rules={[
                                        {
                                            required: true,
                                            message: 'Please input the category name!',
                                        },
                                    ]}
                                >
                                    <Input placeholder="Enter category name" />
                                </Form.Item>
                                <Form.Item
                                    name="categoryDescription"
                                    label="Category Description"
                                    rules={[
                                        {
                                            required: true,
                                            message: 'Please input the category description!',
                                        },
                                    ]}
                                >
                                    <Input.TextArea placeholder="Enter category description" />
                                </Form.Item>
                                <Form.Item
                                    name="categoryId"
                                    hidden={true}
                                >
                                    <Input type="hidden" />
                                </Form.Item>
                            </Form>
                        </Modal>
                        <Modal title="Basic Modal" open={isModalUpdate} onOk={handleOkUpdate} onCancel={handleCancel}>
                            <Form
                                form={form}
                                layout="vertical"
                                name="category_form"
                            >
                                <Form.Item
                                    name="categoryName"
                                    label="Category Name"
                                    rules={[
                                        {
                                            required: true,
                                            message: 'Please input the category name!',
                                        },
                                    ]}
                                >
                                    <Input placeholder="Enter category name" />
                                </Form.Item>
                                <Form.Item
                                    name="categoryDescription"
                                    label="Category Description"
                                    rules={[
                                        {
                                            required: true,
                                            message: 'Please input the category description!',
                                        },
                                    ]}
                                >
                                    <Input.TextArea placeholder="Enter category description" />
                                </Form.Item>
                            </Form>
                        </Modal>
                        <div className="pl-5 border-t border-gray-300">
                            {renderSubcategories(category.categoryId)}
                        </div>
                    </li>
                ))}
            </ul>
        );
    };

    return (
        <div className="font-sans leading-relaxed text-gray-800 bg-gray-50">
            <h2 className="my-8 text-3xl font-bold text-center">Category Tree</h2>
            <p className="mb-8 text-center"></p>
            {loading && <div>Loading...</div>}
            {!loading && renderTopLevelCategories()}
            <ToastContainer
                position="top-right"
                autoClose={5000}
                hideProgressBar={false}
                newestOnTop={false}
                closeOnClick
                rtl={false}
                pauseOnFocusLoss
                draggable
                pauseOnHover
                theme="light"

            />
            {/* Same as */}
            <ToastContainer />
        </div>
    );
}

export default CategoryDetail;