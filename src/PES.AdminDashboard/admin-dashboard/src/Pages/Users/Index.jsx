import React, { useState, useEffect } from 'react'
import { Avatar, Rate, Space, Table, Typography, Button, Select, Input } from "antd";
import { getUsers } from '../../API';
import { Link } from 'react-router-dom';
import { CheckOutlined, UserOutlined } from "@ant-design/icons";


function User() {
  const { Option } = Select;
  const { Search } = Input;
  const [users, setUsers] = useState([]);
  const [loading, setLoading] = useState(false);
  const [searchText, setSearchText] = useState('');
  const [searchType, setSearchType] = useState('Name');
  useEffect(() => {
    fetchUsers();
  }, []);

  const fetchUsers = async (query = '', type = 'Name') => {
    console.log(query)
    console.log(type)
    setLoading(true);
    try {
      var response = await fetch(`https://localhost:7187/api/v1/User?pageNumber=0&pageSize=50`);
      if (query != '') {
        response = await fetch(`https://localhost:7187/api/v1/User?${type}=${query}&pageNumber=0&pageSize=50`);
      }
      const data = await response.json();
console.log(data)
      setUsers(data["items"]);
    } catch (error) {
      console.error('Error fetching users:', error);
    } finally {
      setLoading(false);
    }
  };


  const toggleUserStatus = async (userId, isInactive) => {
    try {

      if (isInactive) {
        // Call enable API endpoint for activating the user
        await fetch(`https://localhost:7187/api/v1/User/${userId}/enable`, {
          method: 'POST',
          headers: {
            'accept': '*/*',
          },
          body: '', // The request body can be empty
        });
      } else {
        // Call disable API endpoint for deactivating the user (assuming you have one)
        await fetch(`https://localhost:7187/api/v1/User/${userId}`, {
          method: 'DELETE',
          headers: {
            'accept': '*/*',
          },
          body: '', // The request body can be empty
        });
      }
      //Update the user status in the state
      const updatedUsers = users.map(user => {
        if (user.userId === userId) {
          return { ...user, isInactive: !user.isInactive };
        }
        return user;
      });
      setUsers(updatedUsers);
    } catch (error) {
      console.error('Error toggling user status:', error);
    }
  };



  const handleSearch = (value) => {
    setSearchText(value);
    fetchUsers(value, searchType);
  };


  const handleSearchTypeChange = (value) => {
    setSearchType(value);
    fetchUsers(searchText, value); // Call the API with the new search type
  };

  const handleSearchInputChange = (e) => {
    setSearchText(e.target.value.toLowerCase());
  };

  return (
    <Space size={30} direction="vertical" className="w-full p-6">
    <Typography.Title level={3} className="text-gray-800">User Management</Typography.Title>
  
    {/* Search Bar */}
    <div className="flex flex-wrap gap-4 items-center">
      <Select
        defaultValue="Name"
        onChange={handleSearchTypeChange}
        className="w-1/4 min-w-[150px]"
      >
        <Option value="Name">Name</Option>
        <Option value="Email">Email</Option>
      </Select>
      <Search
        placeholder={`Search by ${searchType}`}
        onSearch={handleSearch}
        enterButton
        className="w-72 min-w-[200px]"
      />
    </div>
  
    {/* User Table */}
    <Table
      loading={loading}
      columns={[
        {
          title: "Avatar",
          render: () => <UserOutlined className="text-2xl text-gray-600" />,
        },
        {
          title: "Name",
          dataIndex: "name",
          className: "text-gray-700 font-medium",
        },
        {
          title: "Email",
          dataIndex: "email",
          className: "text-gray-600",
        },
        {
          title: "Action",
          render: (text, record) => {
            const isActive = !record.isInactive;
            return (
              <div className="flex justify-center">
                <Button
                  onClick={() => toggleUserStatus(record.userId, record.isInactive)}
                  type="primary"
                  className={`w-[100px] py-1 text-sm font-medium text-white rounded-md transition-all duration-200
                    ${isActive ? 'bg-red-500 hover:bg-red-600' : 'bg-green-500 hover:bg-green-600'}`}
                >
                  {isActive ? "Deactivate" : "Activate"}
                </Button>
              </div>
            );
          },
          fixed: "right",
          width: 150,
        },        
        {
          title: "Order Detail",
          render: (text, record) => (
            <Link
              to={`/orders/${record.userId}?userName=${encodeURIComponent(record.name)}`}
              className="text-blue-500 hover:text-blue-600 underline transition-all duration-200"
            >
              View Detail
            </Link>
          ),
        },
      ]}
      dataSource={users}
      pagination={{ pageSize: 5 }}
      className="shadow-lg rounded-lg overflow-hidden border"
    />
  </Space>
  )
}

export default User