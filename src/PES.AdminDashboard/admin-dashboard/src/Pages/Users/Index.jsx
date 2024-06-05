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
      var response = await fetch(`http://localhost:5046/api/v1/User?pageNumber=0&pageSize=10`);
      if (query != '') {
        response = await fetch(`http://localhost:5046/api/v1/User?${type}=${query}&pageNumber=0&pageSize=10`);
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
        await fetch(`http://localhost:5046/api/v1/User/${userId}/enable`, {
          method: 'POST',
          headers: {
            'accept': '*/*',
          },
          body: '', // The request body can be empty
        });
      } else {
        // Call disable API endpoint for deactivating the user (assuming you have one)
        await fetch(`http://localhost:5046/api/v1/User/${userId}`, {
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
    <Space size={30} direction="vertical" className="w-full p-4">
      <Typography.Title level={4}>User</Typography.Title>
      <div className="flex space-x-4 mb-6">
        <Select
          defaultValue="Name"
          onChange={handleSearchTypeChange}
          className="w-1/4"
        >
          <Option value="Name">Name</Option>
          <Option value="Email">Email</Option>
        </Select>
        <Search
          placeholder={`Search by ${searchType}`}
          onSearch={handleSearch}
          enterButton
          className="w-64"
        />
      </div>
      <Table
        loading={loading}
        columns={[
          {
            title: "Icon",
            render: () => {
              return <UserOutlined />;
            },
          },
          {
            title: "Name",
            dataIndex: "name",
          },
          {
            title: "Email",
            dataIndex: "email",
          },
          {
            title: 'Action',
            render: (text, record) => {
              const buttonClass = record.isInactive ? 'bg-green-500' : 'bg-red-500';
              const buttonText = record.isInactive ? 'Activate' : 'Deactivate';
      
              return (
                <Button
                  onClick={() => toggleUserStatus(record.userId, record.isInactive)}
                  type="primary"
                  className={`${buttonClass} text-white`}
                >
                  {buttonText}
                </Button>
              );
            },
            fixed: 'right', // Ensures this column maintains a fixed width
            width: 150, // Set the width of this column
          },
          {
            title: "Order Detail",
            render: (text, record) => {
              return (
                <Link
                to={`/orders/${record.userId}?userName=${encodeURIComponent(record.name)}`}
                className="text-green-500 hover:underline"
              >
                  View Detail
                </Link>
              );
            },
          },
        ]}
        dataSource={users}
        pagination={{
          pageSize: 5,
        }}
        className="ant-table-wrapper"
      />
    </Space>

  )
}

export default User