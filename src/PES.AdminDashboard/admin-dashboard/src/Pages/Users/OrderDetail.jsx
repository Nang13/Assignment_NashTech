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
                console.log(data);
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
            <div className=" rounded-lg shadow-lg p-6 mb-8">
                <h1 className="text-4xl font-bold text-green-800 mb-4">Order Details</h1>
                <p className="text-lg text-gray-700 mb-6">Order ID: <span className="font-semibold">{order.id}</span></p>
                <div className="grid grid-cols-1 md:grid-cols-2 gap-8">
                    <div>
                        <h2 className="text-2xl font-semibold text-green-700 mb-4">Order Summary</h2>
                        <div className="bg-white rounded-lg shadow-md p-4 mb-6">
                            <p className="text-gray-800">Total Price: <span className="font-semibold">${order.totalPrice}</span></p>
                            <button className="bg-green-700 text-white px-4 py-2 rounded-md mt-4 hover:bg-green-600">
                                <i className="fas fa-download mr-2"></i>Download Invoice
                            </button>
                        </div>
                    </div>
                    <div>
                        <h2 className="text-2xl font-semibold text-green-700 mb-4">Order Items</h2>
                        {order.ordererDetails.map(detail => (
                            <div key={detail.orderDetailId} className="bg-white rounded-lg shadow-md mb-4 p-4">
                                <div className="flex items-center mb-4">
                                    <img src={`https://firebasestorage.googleapis.com/v0/b/ntassignment-518e1.appspot.com/o/Product%2F${detail.productImage}?alt=media&token=0830e8eb-6d0b-4953-8c5f-49d25819e879`} alt={detail.productName} className="w-16 h-16 object-cover mr-4 rounded-lg" />
                                    <div>
                                        <h3 className="text-lg font-semibold text-green-800">{detail.productName}</h3>
                                        <p className="text-gray-600">Organic and Fresh</p>
                                    </div>
                                </div>
                                <div className="grid grid-cols-3 gap-4">
                                    <p className="text-gray-800">Quantity: <span className="font-semibold">{detail.quantity}</span></p>
                                    <p className="text-gray-800">Price: <span className="font-semibold">${detail.price}</span></p>
                                    <p className="text-gray-800">Total Price: <span className="font-semibold">${detail.totalPrice}</span></p>
                                </div>
                            </div>
                        ))}
                    </div>
                </div>
            </div>
        </div>

    );
};

export default OrderDetail;