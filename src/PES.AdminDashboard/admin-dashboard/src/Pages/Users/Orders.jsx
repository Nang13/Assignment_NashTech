import React, { useEffect, useState } from 'react';
import { Link, useParams } from 'react-router-dom';

const Orders = () => {
    const { userId } = useParams();
    const [orders, setOrders] = useState([]);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        const fetchOrders = async () => {
            try {
                const response = await fetch(`http://localhost:5046/api/v1/Order/user/${userId}`, {
                    headers: {
                        'accept': '*/*'
                    }
                });
                if (!response.ok) {
                    throw new Error('Network response was not ok');
                }
                const data = await response.json();
                setOrders(data);
            } catch (error) {
                console.error('Error fetching orders:', error);
            } finally {
                setLoading(false);
            }
        };

        fetchOrders();
    }, [userId]);

    if (loading) {
        return <div>Loading...</div>;
    }

    if (orders.length === 0) {
        return <div>No orders found for this user.</div>;
    }

    return (
        <div className="container mx-auto p-4">
            <h1 className="text-3xl font-bold mb-6">Orders for User ID: {userId}</h1>
            <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
                {orders.map(order => (
                    <div key={order.orderId} className="bg-white shadow-md p-6 rounded-lg">
                        <h2 className="text-xl font-semibold mb-2">Order ID: {order.id}</h2>
                        <p className="text-gray-700 mb-2">Total Price: ${order.totalPrice}</p>
                        <p className="text-gray-700 mb-4">Order Date: {new Date(order.orderDate).toLocaleDateString()}</p>
                        <Link to={`/order/${order.orderId}`} className="text-blue-500 hover:underline">
                            View Order Detail
                        </Link>
                    </div>
                ))}
            </div>
        </div>
    );
};

export default Orders;