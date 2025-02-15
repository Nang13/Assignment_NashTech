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
      const token = localStorage.getItem('authToken');
      var response = "";
      const headers = {
        'Authorization': `Bearer ${token}`, // Add Authorization header
        'Content-Type': 'application/json' // Optional: if your API expects JSON
      };

      if (query === '') {
        response = await fetch('https://localhost:7187/api/v1/Product?pageNumber=0&pageSize=50', {
          headers
        });
      } else {
        response = await fetch(`https://localhost:7187/api/v1/Product?${type}=${query}&pageNumber=0&pageSize=50`, {
          headers
        });
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
    <Space size={30} direction="vertical" className="w-full p-4">
  <Typography.Title level={4}>Product</Typography.Title>

  {/* Search & Add Section */}
  <div className="flex flex-col lg:flex-row lg:items-center lg:justify-start gap-4 mb-4">
    {/* Search Filters */}
    <div className="flex flex-wrap items-center gap-3">
      <Select
        defaultValue={searchType}
        onChange={handleSearchTypeChange}
        className="border border-gray-300 rounded-md px-3 py-2 focus:outline-none w-[160px] shadow-sm"
      >
        <Option value="ProductName">Product Name</Option>
        <Option value="CategoryMain">Category Main</Option>
        <Option value="CategoryName">Category Name</Option>
      </Select>

      <Search
        placeholder={`Search by ${searchType}`}
        onSearch={handleSearch}
        enterButton
        className="border border-gray-300 rounded-md px-3 py-2 focus:outline-none w-[250px] shadow-sm"
      />
    </div>

    {/* Add Product Button */}
    <Link to="/add_product">
      <Button
        type="primary"
        className="rounded-md px-4 py-2 flex items-center gap-2 bg-blue-600 hover:bg-blue-700 transition-all shadow-md"
      >
        <PlusOutlined /> Add New Product
      </Button>
    </Link>
  </div>

  {/* Product Table */}
  <Table
    loading={loading}
    columns={[
      {
        title: "Thumbnail",
        dataIndex: "imageMain",
        render: (link) => (
          <Avatar
            src={`https://firebasestorage.googleapis.com/v0/b/ntassignment-518e1.appspot.com/o/Product%2F${link}?alt=media&token=0830e8eb-6d0b-4953-8c5f-49d25819e879`}
            className="w-12 h-12"
          />
        ),
      },
      {
        title: "Title",
        dataIndex: "productName",
        className: "text-gray-700 font-medium",
      },
      {
        title: "Price",
        dataIndex: "price",
        render: (value) => <span className="font-medium">${value}</span>,
      },
      {
        title: "Rating",
        dataIndex: "rating",
        render: (rating) => <Rate value={rating} allowHalf disabled />,
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
        render: (text, record) => (
          <Link
            to={`/product_detail/${record.id}`}
            className="text-green-500 hover:underline"
          >
            View Detail
          </Link>
        ),
      },
      {
        title: "Update",
        render: (text, record) => (
          <Link
            to={`/product_update/${record.id}`}
            className="text-blue-500 hover:underline"
          >
            Update
          </Link>
        ),
      },
    ]}
    dataSource={dataSource}
    pagination={{ pageSize: 5 }}
    className="w-full rounded-lg shadow"
  />
</Space>
  );
}

export default Product