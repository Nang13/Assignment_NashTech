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
        response = await fetch('https://localhost:7187/api/v1/Order?pageNumber=0&pageSize=50');
      } else {
        response = await fetch(`https://localhost:7187/api/v1/Order?${type}=${query}&pageNumber=0&pageSize=50`);
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

    fetchOrders(value, "Status");
  };

  const handlePaymentTypeFilterChange = value => {
    fetchOrders(value, "PaymentType");
  };

  const handleSearch = (value) => {
    setSearchText(value);
    fetchOrders(value, searchType);
  };

  // Function to toggle user status
  const toggleUserStatus = async (orderId) => {
    const url = `https://localhost:7187/api/v1/Order/${orderId}/finish`;

    try {
      const response = await fetch(url, {
        method: 'POST',
        headers: {
          'Accept': '*/*',
        },
        body: ''
      });

      if (!response.ok) {
        throw new Error('Network response was not ok');
      }

      const result = await response.json();
      fetchOrders();
      console.log(`Finished processing for userId: ${orderId}`, result);

      // Update the state or UI accordingly
    } catch (error) {
      console.error(`There was a problem finishing the processing for userId: ${orderId}`, error);
    }
  };
  const handleSearchTypeChange = (value) => {
    setSearchType(value);
    fetchOrders(searchText, value); // Call the API with the new search type
  };
  return (
    <Space size={30} direction="vertical" className="w-full p-4">
    <Typography.Title level={4} className="text-gray-800">Order</Typography.Title>
  
    {/* Search & Filter Controls */}
    <div className="flex flex-wrap gap-4 mb-6">
      <Select
        defaultValue="OrderCurrencyCode"
        onChange={handleSearchTypeChange}
        className="w-1/4 min-w-[200px]"
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
        className="w-1/4 min-w-[200px]"
        allowClear
      >
        <Option value="Finish">Finish</Option>
        <Option value="Processing">Processing</Option>
      </Select>
  
      <Select
        placeholder="Filter by Payment Type"
        onChange={handlePaymentTypeFilterChange}
        className="w-1/4 min-w-[200px]"
        allowClear
      >
        <Option value="COD">COD</Option>
        <Option value="Credit Card">Credit Card</Option>
      </Select>
    </div>
  
    {/* Orders Table */}
    <Table
      loading={loading}
      columns={[
        {
          title: "",
          render: () => <ShoppingCartOutlined className="text-lg text-gray-500" />,
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
          dataIndex: "productCount",
        },
        {
          title: "Total Price",
          dataIndex: "totalPrice",
          render: (price) => <span className="font-semibold">${price.toFixed(2)}</span>,
        },
        {
          title: "Payment Type",
          dataIndex: "paymentType",
        },
        {
          title: "Status",
          dataIndex: "status",
          render: (status) => (
            <span
              className={`px-3 py-1 rounded-md text-white ${
                status === "Processing" ? "bg-yellow-500" : "bg-green-500"
              }`}
            >
              {status}
            </span>
          ),
        },
        {
          title: "Action",
          render: (text, record) => {
            if (record.status !== "Processing") return null;
  
            const buttonClass = record.paymentType ? "bg-green-500" : "bg-red-500";
            const buttonText = record.isInactive ? "Activate" : "Finish";
  
            return (
              <Button
                onClick={() => toggleUserStatus(record.orderId)}
                type="primary"
                className={`${buttonClass} text-white`}
              >
                {buttonText}
              </Button>
            );
          },
        },
      ]}
      dataSource={dataSource}
      pagination={{ pageSize: 5 }}
      className="bg-white rounded-lg shadow-md"
    />
  </Space>
  

  )
}

export default Order