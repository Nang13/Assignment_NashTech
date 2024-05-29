import React from 'react'
import { getCategories } from "../../API/index";
import { Link } from "react-router-dom";
import { useEffect, useState } from "react";
import { Avatar, Rate, Space, Table, Typography, Modal, Form, Input, message } from "antd";


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

        fetch("http://localhost:5046/api/v1/Category", requestOptions)
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

    <Space size={20} direction="vertical" >
      <Typography.Title level={4}>Category</Typography.Title>
      <Link className="mb-4">
        <button className="bg-green-500 hover:bg-green-600 text-white font-bold py-2 px-4 rounded" onClick={() => showModalAdding()}>
          Add New Category
        </button>
      </Link>
      <Table
        loading={loading}
        columns={[
          {
            title: "Image",
            render: () => {
              return <Avatar src="https://firebasestorage.googleapis.com/v0/b/ntassignment-518e1.appspot.com/o/OIP%20(4).jpg?alt=media&token=d30f5a2d-3598-430e-a7ad-28708271a927" />;
            },
          },
          {
            title: "Title",
            dataIndex: "categoryName",
          },
          {
            title: "Action",
            render: (text, record) => {
              return (
                <Link to={`/category_detail/${record.categoryId}`} style={{ color: 'green' }}>
                  View Detail
                </Link>
              );
            },
          },
        ]}
        dataSource={dataSource}
        pagination={{
          pageSize: 5,
        }}
      ></Table>
      <Modal title="Add Parent Category" open={isModalAdding} onOk={handleOk} onCancel={handleCancel}>
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
            name="categoryMain"
            label="Category Main Name"
            rules={[
              {
                required: true,
                message: 'Please input the category main!',
              },
            ]}
          >
            <Input placeholder="Enter category main" />
          </Form.Item>
        </Form>
      </Modal>
    </Space>
  )
}

export default Category