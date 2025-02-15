import React from 'react'
import { getCategories } from "../../API/index";
import { Link } from "react-router-dom";
import { useEffect, useState } from "react";
import { Avatar, Rate, Space, Table, Typography, Modal, Form, Input, message } from "antd";
import { ClusterOutlined } from "@ant-design/icons";

const { Title } = Typography;
function Category() {
  const [loading, setLoading] = useState(false);
  const [dataSource, setDataSource] = useState([]);
  const [isModalAdding, setIsModalAdding] = useState(false);
  const [currentCategory, setCurrentCategory] = useState(null);
  const [isModalVisible, setIsModalVisible] = useState(false);
  const [form] = Form.useForm();


  useEffect(() => {
    setLoading(true);
    getCategories().then((res) => {
      setDataSource(res);
      setLoading(false);
    });
    console.log(dataSource);
  }, []);


  const showModalAdding = () => {
    setIsModalAdding(true);
  }

  const handleCancel = () => {
    setIsModalAdding(false);
  };


  const handleOk = () => {
    form
      .validateFields()
      .then(values => {
        const payload = {
          categoryName: values.categoryName,
          categoryDescription: values.categoryDescription,
          categoryMain: values.categoryMain || values.categoryName.replace(/\s+/g, ''),
        };

        const requestOptions = {
          method: 'POST',
          headers: { 'Content-Type': 'application/json' },
          body: JSON.stringify(payload),
        };

        fetch("https://localhost:7187/api/v1/Category", requestOptions)
          .then(response => {
            if (!response.ok) {
              throw new Error('Network response was not ok');
            }
            return response.json();
          })
          .then(data => {
            form.resetFields();
            setIsModalVisible(false);
            setCurrentCategory(null);
          })
          .catch(error => {
            console.error('Error:', error);
            message.error('Failed to submit category');
          });
      })
      .catch(info => {
        console.log('Validate Failed:', info);
      });
    //setIsModalAdding(false);

  }
  return (

    <Space size={30} direction="vertical" className="w-full p-4 bg-gray-50 min-h-screen">
    <div className="flex justify-between items-center">
      <Title level={4} className="text-gray-800">Category</Title>
      <button
        className="bg-green-600 hover:bg-green-700 text-white font-semibold py-2 px-4 rounded-lg shadow-md transition duration-300"
        onClick={showModalAdding}
      >
        Add New Category
      </button>
    </div>
  
    <Table
      className="bg-white rounded-lg shadow-md border border-gray-200"
      loading={loading}
      columns={[
        {
          title: "",
          render: () => <ClusterOutlined className="text-lg text-gray-500" />,
        },
        {
          title: "Title",
          dataIndex: "categoryName",
          className: "text-gray-700",
        },
        {
          title: "Action",
          render: (_, record) => (
            <Link to={`/category_detail/${record.categoryId}`} className="text-green-600 hover:text-green-800">
              View Detail
            </Link>
          ),
        },
      ]}
      dataSource={dataSource}
      pagination={{ pageSize: 5 }}
    />
  
    <Modal
      title="Add Parent Category"
      open={isModalAdding}
      onOk={handleOk}
      onCancel={handleCancel}
      className="rounded-lg"
    >
      <Form form={form} layout="vertical" className="space-y-4">
        <Form.Item
          name="categoryName"
          label="Category Name"
          rules={[{ required: true, message: "Please input the category name!" }]}
        >
          <Input placeholder="Enter category name" className="p-2 border border-gray-300 rounded-lg" />
        </Form.Item>
        <Form.Item
          name="categoryDescription"
          label="Category Description"
          rules={[{ required: true, message: "Please input the category description!" }]}
        >
          <Input.TextArea placeholder="Enter category description" className="p-2 border border-gray-300 rounded-lg" />
        </Form.Item>
        <Form.Item
          name="categoryMain"
          label="Category Main Name"
          rules={[{ required: true, message: "Please input the category main!" }]}
        >
          <Input placeholder="Enter category main" className="p-2 border border-gray-300 rounded-lg" />
        </Form.Item>
      </Form>
    </Modal>
  </Space>
  
  )
}

export default Category