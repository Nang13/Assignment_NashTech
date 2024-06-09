import { ShoppingCartOutlined } from '@ant-design/icons'
import { Typography, Space, Card, Table } from 'antd/es'
import Statistic from 'antd/es/statistic/Statistic'
import { AppstoreOutlined, UserOutlined, ShoppingOutlined, ApartmentOutlined, DollarCircleOutlined } from "@ant-design/icons";
import React, { useEffect, useState } from 'react'
import { getOrders ,getRevenue } from "../API/index";

import {
  Chart as ChartJS,
  CategoryScale,
  LinearScale,
  BarElement,
  Title,
  Tooltip,
  Legend,
} from "chart.js";
import { Bar } from "react-chartjs-2";

ChartJS.register(
  CategoryScale,
  LinearScale,
  BarElement,
  Title,
  Tooltip,
  Legend
);

function Dashboard() {
  return (
    <div className='pl-2'>
      <Typography.Title level={4}>Dashboard</Typography.Title>
      <Space direction='horizontal'>
        <CardFunction
          icon={<ShoppingCartOutlined style={{
            color: "green",
            backgroundColor: "rgba(0,255,0,0.25)",
            borderRadius: 20,
            fontSize: 24,
            padding: 8,
          }} />} title={'Order'} value={1234} />
        <CardFunction
          icon={<ShoppingOutlined
            style={{
              color: "blue",
              backgroundColor: "rgba(0,0,255,0.25)",
              borderRadius: 20,
              fontSize: 24,
              padding: 8,
            }} />} title={'Product'} value={1234} />
        <CardFunction
          icon={<UserOutlined style={{
            color: "purple",
            backgroundColor: "rgba(0,255,255,0.25)",
            borderRadius: 20,
            fontSize: 24,
            padding: 8,
          }} />} title={'User'} value={1234} />
        <CardFunction
          icon={<DollarCircleOutlined style={{
            color: "red",
            backgroundColor: "rgba(255,0,0,0.25)",
            borderRadius: 20,
            fontSize: 24,
            padding: 8,
          }} />} title={'Revenue'} value={1234} />
      </Space>
      <Space>
        <RecentOrder />
        <DashboardChart />
      </Space>
    </div>
  )
}

function CardFunction({ title, value, icon }) {
  return (
    <Card>
      <Space direction='horizontal'>
        {icon}
        <Statistic title={title} value={value}></Statistic>
      </Space>
    </Card>
  )
}

function RecentOrder() {
  const [dataSource, setDataSource] = useState([]);
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    setLoading(true);
    getOrders().then((res) => {
      console.log(res)
      setDataSource(res.items);
      setLoading(false);
    });
  }, []);
  return (
    <Table
      columns={[
        { title: 'title', dataIndex: 'productName' },
        { title: "Quantity", dataIndex: "price" },
        { title: "Price", dataIndex: "price" }]}
        loading={loading}
        dataSource={dataSource}>
    </Table>
  )
}

function DashboardChart() {
  const [reveneuData, setReveneuData] = useState({
    labels: [],
    datasets: [],
  });

  useEffect(() => {
    getRevenue().then((res) => {
      const labels = res.carts.map((cart) => {
        return `User-${cart.userId}`;
      });
      const data = res.carts.map((cart) => {
        return cart.discountedTotal;
      });

      const dataSource = {
        labels,
        datasets: [
          {
            label: "Revenue",
            data: data,
            backgroundColor: "rgba(255, 0, 0, 1)",
          },
        ],
      };

      setReveneuData(dataSource);
    });
  }, []);

  const options = {
    responsive: true,
    plugins: {
      legend: {
        position: "bottom",
      },
      title: {
        display: true,
        text: "Order Revenue",
      },
    },
  };

  return (
    <Card style={{ width: 500, height: 250 }}>
      <Bar options={options} data={reveneuData} />
    </Card>
  );
}


export default Dashboard