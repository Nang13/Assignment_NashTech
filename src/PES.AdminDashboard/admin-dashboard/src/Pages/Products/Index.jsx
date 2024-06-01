import React from 'react'
import { getProduct } from "../../API/index";
import { Avatar, Button, Rate, Space, Table, Typography, Select, Input } from "antd";
import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import { PlusOutlined } from '@ant-design/icons';


const { Title } = Typography;
const { Search } = Input;
const { Option } = Select;
function Product() {
  const [loading, setLoading] = useState(false);
  const [dataSource, setDataSource] = useState([]);
  const [searchText, setSearchText] = useState('');
  const [searchType, setSearchType] = useState('ProductName');
  const [error, setError] = useState(null);

  useEffect(() => {
    fetchProducts();
  }, []);

  const fetchProducts = async (query = '', type = 'ProductName') => {
    setLoading(true);
    try {
      var response = "";
      if (query == '') {
        response = await fetch('http://localhost:5046/api/v1/Product?pageNumber=0&pageSize=10');
      } else {
        response = await fetch(`http://localhost:5046/api/v1/Product?${type}=${query}&pageNumber=0&pageSize=10`);
      }

      const data = await response.json();
      console.log(data); // Logging data after it has been assigned
      setDataSource(data.items); // Assuming data has an "items" array
    } catch (error) {
      console.error('Error fetching products:', error);
      setError(error);
    } finally {
      setLoading(false);
    }
  };

  const handleSearch = (value) => {
    setSearchText(value);
    fetchProducts(value, searchType);
    console.log(value)
    console.log(searchType)
  };

  const handleSearchTypeChange = (value) => {
    setSearchType(value);
  };
  return (
    <div className="container mx-auto p-4">
      <Title level={4}>Product</Title>
      <div className="flex space-x-4 mb-4">
        <Select defaultValue={searchType} onChange={handleSearchTypeChange} style={{ width: 150 }} className="border border-gray-300 rounded-md px-2 py-1 focus:outline-none">
          <Option value="ProductName">Product Name</Option>
          <Option value="CategoryMain">Category Main</Option>
          <Option value="CategoryName">Category Name</Option>
        </Select>
        <Search
          placeholder={`Search by ${searchType}`}
          onSearch={handleSearch}
          enterButton
          style={{ width: 250 }}
          className="border border-gray-300 rounded-md px-2 py-1 focus:outline-none"
        />
      </div>
      <Link to="/add_product" className="mb-4">
        <Button type="primary" className="rounded-md">
          <PlusOutlined /> Add New
        </Button>
      </Link>
      <Table
        loading={loading}
        columns={[
          {
            title: "Thumbnail",
            dataIndex: "imageMain",
            render: (link) => {
              return (
                <Avatar
                  src={`https://firebasestorage.googleapis.com/v0/b/ntassignment-518e1.appspot.com/o/product%2F${link}?alt=media&token=0830e8eb-6d0b-4953-8c5f-49d25819e879`}
                />
              );
            },
          },
          {
            title: "Title",
            dataIndex: "productName",
          },
          {
            title: "Price",
            dataIndex: "price",
            render: (value) => <span>${value}</span>,
          },
          {
            title: "Rating",
            dataIndex: "rating",
            render: (rating) => {
              return <Rate value={rating} allowHalf disabled />;
            },
          },
          {
            title: "Category Main",
            dataIndex: "categoryMain",
          },
          {
            title: "Category Name",
            dataIndex: "categoryName",
          },
          {
            title: "Action",
            render: (text, record) => {
              return (
                <Link to={`/product_detail/${record.id}`} className="text-green-500 hover:underline">
                  View Detail
                </Link>
              );
            },
          },
          {
            title: "Update",
            render: (text, record) => {
              return (
                <Link to={`/product_update/${record.id}`} className="text-green-500 hover:underline">
                  Update
                </Link>
              );
            },
          },
        ]}
        dataSource={dataSource}
        pagination={{
          pageSize: 5,
        }}
        className="w-full"
      />
    </div>
  );
}

export default Product