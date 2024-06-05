import React, { useEffect, useState } from 'react';
import { Link, useParams ,useLocation} from 'react-router-dom';

const Orders = () => {
    const location = useLocation();
    const searchParams = new URLSearchParams(location.search);
    const userId = location.pathname.split('/')[2];
    const userName = searchParams.get('userName');
    //const { userId } = useParams();
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
                console.log(data);
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
        <div className="container mx-auto p-6 min-h-screen bg-gradient-to-r  ">
        <h1 className="text-3xl font-bold text-green-800 mb-8">Orders Of User: {userName}</h1>
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-8">
            {orders.map(order => (
                <div key={order.orderId} className="bg-white shadow-lg p-6 rounded-lg hover:shadow-xl transition duration-300 transform hover:scale-105">
                    <h2 className="text-xl font-semibold text-green-700 mb-2">{order.orderCurrencyCode}</h2>
                    <p className="text-gray-700 mb-2">Total Price: ${order.totalPrice}</p>
                    <p className="text-gray-700 mb-2">Payment Type: {order.paymentType}</p>
                    <p className="text-gray-700 mb-2">Product Count: {order.productCount}</p>
                    <p className="text-gray-700 mb-4">Status: {order.status}</p>
                    <Link to={`/order/${order.orderId}`} className="block text-center bg-green-600 hover:bg-green-700 text-white font-semibold py-2 px-4 rounded hover:shadow-md transition duration-300">
                        View Order Detail
                    </Link>
                    {/* Integrate the order detail template here */}
                    <div className="mt-4">
                        {/* Replace this div with the order detail template */}
                        {/* You can paste the order detail template here */}
                        {/* Add dynamic props such as 'order={order}' and 'userName={userName}' */}
                    </div>
                </div>
            ))}
        </div>
    </div>
    );
};

export default Orders;