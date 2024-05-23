import { Menu } from 'antd/es'
import React from 'react'
import { AppstoreOutlined, UserOutlined, ShoppingOutlined, ApartmentOutlined } from "@ant-design/icons";
import { useNavigate } from "react-router-dom";


function SideMenu() {
  const navigate = useNavigate();
  return (
    <div className=' h-full' >
      <Menu
        onClick={(item) => {
          navigate(item.key)
        }}
        items={[{
          label: "Dashboard",
          icon: <AppstoreOutlined />,
          key: '/'
        }, {
          label: 'User',
          icon: <UserOutlined />,
          key: '/user'
        }, {
          label: 'Product',
          icon: <ShoppingOutlined />,
          key: '/product'
        }, {
          label: 'Category',
          icon: <ApartmentOutlined />,
          key: '/category'
        }]}>
      </Menu></div>
  )
}

export default SideMenu