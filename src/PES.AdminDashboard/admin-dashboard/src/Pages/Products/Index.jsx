import React from 'react'
import { getProduct } from "../../API/index";
import { Avatar, Button, Rate, Space, Table, Typography } from "antd";
import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
function Product() {
  const [loading, setLoading] = useState(false);
  const [dataSource, setDataSource] = useState([]);

  useEffect(() => {

    setLoading(true);
    getProduct().then((res) => {
      setDataSource(res.items);
      setLoading(false);
    });
  }, []);

  return (
    <Space size={20} direction="vertical" >
      <Link to="/add_product">
        <button className="bg-green-500 hover:bg-green-600 text-white font-bold py-2 px-4 rounded">
          Add New
        </button>
      </Link>
      <Typography.Title level={4}>Inventory</Typography.Title>
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

        ]}
        dataSource={dataSource}
        pagination={{
          pageSize: 5,
        }}
      ></Table>
    </Space>
  );
}

export default Product