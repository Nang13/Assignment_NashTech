import React, { useEffect, useState } from 'react';
import { getCategoryDetail } from '../../API/index';
import { useParams } from 'react-router-dom';
import { Button, Modal } from "antd";
import { PlusOutlined, DeleteOutlined, EditOutlined } from "@ant-design/icons";
function CategoryDetail() {
    const { id } = useParams();
    const [loading, setLoading] = useState(false);
    const [dataSource, setDataSource] = useState([]);
    const [isModalOpen, setIsModalOpen] = useState(false);

    const showModal = () => {
        setIsModalOpen(true);
    };

    const handleOk = () => {
        setIsModalOpen(false);
    };

    const handleCancel = () => {
        setIsModalOpen(false);
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
                                <PlusOutlined onClick={() => showModal()} style={{ color: 'green' }} className='pl-3' />
                                <DeleteOutlined onClick={() => showModal()} style={{ color: 'red' }} className='pl-3' />
                                <EditOutlined onClick={() => showModal()} style={{ color: 'black' }} className='pl-3' />
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
                            <PlusOutlined onClick={() => showModal()} style={{ color: 'green' }} className='pl-3' />
                            <DeleteOutlined onClick={() => showModal()} style={{ color: 'red' }} className='pl-3' />
                            <EditOutlined onClick={() => showModal()} style={{ color: 'black' }} className='pl-3' />
                        </div>
                        <Modal title="Basic Modal" open={isModalOpen} onOk={handleOk} onCancel={handleCancel}>
                            <p>Some contents...</p>
                            <p>Some contents...</p>
                            <p>Some contents...</p>
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
        <div className="font-sans leading-relaxed text-gray-800 bg-gray-100">
            <h2 className="my-8 text-3xl font-bold text-center">Category Detail</h2>
            <p className="mb-8 text-center">ID: {id}</p>
            {loading && <div>Loading...</div>}
            {!loading && renderTopLevelCategories()}
        </div>
    );
}

export default CategoryDetail;