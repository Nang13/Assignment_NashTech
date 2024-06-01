import React, { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';

const OrderDetail = () => {
    const { orderId } = useParams();
    const [order, setOrder] = useState(null);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        const fetchOrder = async () => {
            try {
                const response = await fetch(`http://localhost:5046/api/v1/Order/${orderId}`, {
                    headers: {
                        'accept': '*/*'
                    }
                });
                if (!response.ok) {
                    throw new Error('Network response was not ok');
                }
                const data = await response.json();
                setOrder(data);
            } catch (error) {
                console.error('Error fetching order:', error);
            } finally {
                setLoading(false);
            }
        };

        fetchOrder();
    }, [orderId]);

    if (loading) {
        return <div>Loading...</div>;
    }

    if (!order) {
        return <div>Order not found.</div>;
    }

    return (
        <div className="container mx-auto p-4">
        <h1 className="text-3xl font-bold mb-6">Order Detail for Order ID: {order.orderId}</h1>
        <div className="bg-white shadow-md p-6 rounded-lg mb-6">
            <p className="text-gray-700 mb-2">Total Price: ${order.totalPrice}</p>
            <h2 className="text-xl font-semibold mb-2">Orderer Details</h2>
            {order.ordererDetails.map(detail => (
                <div key={detail.orderDetailId} className="bg-gray-100 border rounded-lg shadow-sm mb-4 p-4">
                    <p className="text-gray-700 mb-2">Order Detail ID: {detail.orderDetailId}</p>
                    <p className="text-gray-700">Price: ${detail.price}</p>
                </div>
            ))}
        </div>
    </div>
    );
};

export default OrderDetail;