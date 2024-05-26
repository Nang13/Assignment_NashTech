import React, { useState, useEffect } from 'react'
import { Avatar, Rate, Space, Table, Typography, Button, Select, Input } from "antd";
import { getUsers } from '../../API';
import { Link } from 'react-router-dom';
import { CheckOutlined, UserOutlined } from "@ant-design/icons";
import Search from 'antd/es/transfer/search';


function User() {
  const { Option } = Select;
  const [users, setUsers] = useState([]);
  const [loading, setLoading] = useState(false);
  const [searchText, setSearchText] = useState('');
  const [searchType, setSearchType] = useState('UserName');

  useEffect(() => {
    fetchUsers();
  }, []);

  const fetchUsers = async (query = '', type = 'name') => {
    setLoading(true);
    try {
      const response = await fetch(`http://localhost:5046/api/v1/User?${type}=${query}`);
      const data = await response.json();
      setUsers(data);
    } catch (error) {
      console.error('Error fetching users:', error);
    } finally {
      setLoading(false);
    }
  };


  const toggleUserStatus = async (userId, isInactive) => {
    try {

      console.log(isInactive)
      console.log(userId)
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
        if (user.id === userId) {
          return { ...user, isInactive: !user.isInactive };
        }
        return user;
      });
      setUsers(updatedUsers);
    } catch (error) {
      console.error('Error toggling user status:', error);
    }
  };

  const handleSearch = (e) => {
    const searchText = e.target.value.toLowerCase();
    setSearchText(searchText);
    fetchUsers(searchText, searchType); // Call the API with the search query
  };

  const handleSearchTypeChange = (value) => {
    setSearchType(value);
    fetchUsers(searchText, value); // Call the API with the new search type
  };

  const handleSearchInputChange = (e) => {
    setSearchText(e.target.value.toLowerCase());
  };

  return (
    <Space size={30} direction="vertical" >
      <Typography.Title level={4}>User</Typography.Title>
      <div className="flex space-x-4 mb-6">
        <Select
          defaultValue="UserName"
          onChange={handleSearchTypeChange}
          className="w-1/4"
        >
          <Option value="UserName">Name</Option>
          <Option value="Email">Email</Option>
        </Select>
        <Search
          placeholder={`Search by ${searchType}`}
          value={searchText}
          onChange={handleSearchInputChange}
          onPressEnter={handleSearch}
          className="w-3/4"
        />
      </div>
      <Table
        loading={loading}
        columns={[
          {
            title: "  ",
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
            title: "Action",
            render: (text, record) => {
              return (
                <Button
                  onClick={() => toggleUserStatus(record.userId, record.isInactive)}
                  type="primary"
                  style={{ backgroundColor: record.isInactive ? 'green' : 'red' }}
                >
                  {record.isInactive ? "Activate" : "Deactivate"}
                </Button>
              );
            },
            fixed: 'right', // Ensures this column maintains a fixed width
            width: 150, // Set the width of this column
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
        dataSource={users}
        pagination={{
          pageSize: 5,
        }}
      ></Table>
    </Space>
  )
}

export default User