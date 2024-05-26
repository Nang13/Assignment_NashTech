import React from 'react'
import { getProduct } from "../../API/index";
import { Avatar, Button, Rate, Space, Table, Typography , Select , Input } from "antd";
import { useEffect, useState } from "react";
import { Link } from "react-router-dom";


const { Title } = Typography;
const { Search } = Input;
const { Option } = Select;
function Product() {
  const [loading, setLoading] = useState(false);
  const [dataSource, setDataSource] = useState([]);
  const [searchText, setSearchText] = useState('');
  const [searchType, setSearchType] = useState('productName'); 

  useEffect(() => {
    fetchProducts();
  }, []);

  const fetchProducts = async (query = '', type = 'productName') => {
    setLoading(true);
    try {
      const response = await fetch(`http://localhost:5046/api/v1/Product?pageNumber=0&pageSize=10`);
    //  const response = await fetch(`http://localhost:5046/api/v1/Product?${type}=${query}`);
      const data = await response.json();
      setDataSource(data["items"]);
    } catch (error) {
      console.error('Error fetching products:', error);
    } finally {
      setLoading(false);
    }
  };

  const handleSearch = (value) => {
    setSearchText(value);
    fetchProducts(value, searchType);
  };

  const handleSearchTypeChange = (value) => {
    setSearchType(value);
  };
  return (
    <Space size={20} direction="vertical" className="p-4">
      <Title level={4}>Product</Title>
      <Space>
        <Select defaultValue={searchType} onChange={handleSearchTypeChange} style={{ width: 120 }}>
          <Option value="productName">Product Name</Option>
          <Option value="categoryMain">Category Main</Option>
          <Option value="CategoryName">Category Name</Option>
          {/* Add more options as needed */}
        </Select>
        <Search
          placeholder={`Search by ${searchType}`}
          onSearch={handleSearch}
          enterButton
          style={{ width: 200 }}
        />
      </Space>
      <Link to="/add_product" className="mb-4">
        <button className="bg-green-500 hover:bg-green-600 text-white font-bold py-2 px-4 rounded">
          Add New
        </button>
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
                <Link to={`/product_detail/${record.id}`} style={{ color: 'green' }}>
                  View Detail
                </Link>
              );
            },
          },
          {
            title: "Update",
            render: (text, record) => {
              return (
                <Link to={`/product_update/${record.id}`} style={{ color: 'green' }}>
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
      />
    </Space>
  );
}

export default Product