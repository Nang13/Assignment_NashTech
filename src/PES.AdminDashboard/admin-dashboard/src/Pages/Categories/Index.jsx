import React from 'react'
import { getCategories } from "../../API/index";
import { Link } from "react-router-dom";
import { useEffect, useState } from "react";
import { Avatar, Rate, Space, Table, Typography } from "antd";
function Category() {
  const [loading, setLoading] = useState(false);
  const [dataSource, setDataSource] = useState([]);

  useEffect(() => {
    setLoading(true);
    getCategories().then((res) => {
      setDataSource(res);
      setLoading(false);
    });
    console.log(dataSource);
  }, []);

 
  return (
    <Space size={20} direction="vertical" >
    <Typography.Title level={4}>Inventory</Typography.Title>
    <Table
      loading={loading}
      columns={[
        {
          title: "Image",
          render: () => {
            return <Avatar src="https://firebasestorage.googleapis.com/v0/b/ntassignment-518e1.appspot.com/o/OIP%20(4).jpg?alt=media&token=d30f5a2d-3598-430e-a7ad-28708271a927" />;
          },
        },
        {
          title: "Title",
          dataIndex: "categoryName",
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
      dataSource={dataSource}
      pagination={{
        pageSize: 5,
      }}
    ></Table>
  </Space>
  )
}

export default Category