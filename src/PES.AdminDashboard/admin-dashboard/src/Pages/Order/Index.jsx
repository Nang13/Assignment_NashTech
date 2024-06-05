import React, { useState, useEffect } from 'react'
import { Avatar, Rate, Space, Table, Typography, Button, Select, Input } from "antd";
import { getUsers } from '../../API';
import { Link } from 'react-router-dom';
import { CheckOutlined, UserOutlined, ShoppingCartOutlined } from "@ant-design/icons";

function Order() {
  const { Option } = Select;
  const { Search } = Input;
  const [loading, setLoading] = useState(false);
  const [dataSource, setDataSource] = useState([]);
  const [searchText, setSearchText] = useState('');
  const [searchType, setSearchType] = useState('OrderCurrencyCode');
  const [error, setError] = useState(null);

  useEffect(() => {
    fetchOrders();
  }, []);


  const fetchOrders = async (query = '', type = 'OrderCurrencyCode') => {
    console.log(query);
    console.log(type);
    setLoading(true);
    try {
      var response = "";
      if (query == '') {
        response = await fetch('http://localhost:5046/api/v1/Order?pageNumber=0&pageSize=10');
      } else {
        response = await fetch(`http://localhost:5046/api/v1/Order?${type}=${query}&pageNumber=0&pageSize=10`);
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

  const handleStatusFilterChange = (value) => {

    fetchOrders(value , "Status");
};

const handlePaymentTypeFilterChange = value => {
    fetchOrders(value , "PaymentType");
};

  const handleSearch = (value) => {
    setSearchText(value);
    fetchOrders(value, searchType);
  };


  const handleSearchTypeChange = (value) => {
    setSearchType(value);
    fetchOrders(searchText, value); // Call the API with the new search type
  };
  return (
    <Space size={30} direction="vertical" className="w-full p-4">
      <Typography.Title level={4}>Order</Typography.Title>
      <div className="flex space-x-4 mb-6">
        <Select
          defaultValue="OrderCurrencyCode"
          onChange={handleSearchTypeChange}
          className="w-1/4"
        >
          <Option value="OrderCurrencyCode">Currency Code</Option>
          <Option value="UserName">User Name</Option>
        </Select>
        <Search
          placeholder={`Search by ${searchType}`}
          onSearch={handleSearch}
          enterButton
          className="w-64"
        />
        <Select
          placeholder="Filter by Status"
          onChange={handleStatusFilterChange}
          className="w-1/4"
          allowClear
        >
          <Option value="Finish">Finish</Option>
          <Option value="Processing">Processing</Option>
        </Select>
        <Select
          placeholder="Filter by Payment Type"
          onChange={handlePaymentTypeFilterChange}
          className="w-1/4"
          allowClear
        >
          <Option value="COD">COD</Option>
          <Option value="Credit Card">Credit Card</Option>
        </Select>
      </div>

      <Table
        loading={loading}
        columns={[
          {
            title: "",
            render: () => {
              return <ShoppingCartOutlined />;
            },
          },
          {
            title: "Currency Code",
            dataIndex: "orderCurrencyCode",
          },
          {
            title: "User Name",
            dataIndex: "userName",
          },
          {
            title: "Quantity",
            dataIndex: "productCount"
          },
          {
            title: "Total Price",
            dataIndex: "totalPrice"
          },
          {
            title: 'Payment Type',
            dataIndex: "paymentType",
            // Set the width of this column
          },
          {
            title: "Status",
            dataIndex: "status"

          },
        ]}
        dataSource={dataSource}
        pagination={{
          pageSize: 5,
        }}
        className="ant-table-wrapper"
      />
    </Space>

  )
}

export default Order