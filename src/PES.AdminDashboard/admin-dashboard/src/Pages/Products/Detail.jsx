import React, { useState, useEffect } from 'react';
import { useParams } from 'react-router-dom';
import { getProductDetail } from '../../API';

function ProductDetail() {
  const { id } = useParams();
  const [loading, setLoading] = useState(false);
  const [dataSource, setDataSource] = useState(null);

  useEffect(() => {
    setLoading(true);
    getProductDetail({ id })
      .then((res) => {
        setDataSource(res);
        setLoading(false);
      })
      .catch((error) => {
        console.error('Error fetching product detail:', error);
        setLoading(false);
      });
  }, [id]);

  if (loading) {
    return (
      <div className="bg-gray-100 p-6 min-h-screen flex items-center justify-center">
        <div className="text-center">
          <p className="text-xl text-gray-600">Loading...</p>
        </div>
      </div>
    );
  }

  if (!dataSource) {
    return (
      <div className="bg-gray-100 p-6 min-h-screen flex items-center justify-center">
        <div className="text-center">
          <p className="text-xl text-gray-600">No data available</p>
        </div>
      </div>
    );
  }

  return (
    <div className="bg-gray-100 p-6 min-h-screen flex items-center justify-center">
      <div className="max-w-md bg-white shadow-lg rounded-lg overflow-hidden">
        <div className="p-4">
          <h2 className="text-2xl font-bold text-gray-800">{dataSource.productName}</h2>
          <p className="text-xl text-gray-600 mt-2">${dataSource.price}</p>
          <p className="text-sm text-gray-500 mt-2">
            Category: <span className="font-semibold">{dataSource.productCategory.categoryName}</span> ({dataSource.productCategory.categoryMain})
          </p>

          <div className="mt-4">
            <h3 className="text-lg font-semibold text-gray-700">Nutrition Information</h3>
            <ul className="list-disc list-inside ml-2 mt-2 text-gray-600">
              <li>Calories: {dataSource.nutrionInfo.calories}</li>
              <li>Protein: {dataSource.nutrionInfo.protein}g</li>
              <li>Sodium: {dataSource.nutrionInfo.sodium}mg</li>
              <li>Fiber: {dataSource.nutrionInfo.fiber}g</li>
              <li>Sugars: {dataSource.nutrionInfo.sugars}g</li>
            </ul>
          </div>

          <div className="mt-4">
            <h3 className="text-lg font-semibold text-gray-700">Important Information</h3>
            <p className="text-gray-600 mt-2">
              <span className="font-semibold">Ingredients:</span> {dataSource.importantInfo.ingredients}
            </p>
            <p className="text-gray-600 mt-2">
              <span className="font-semibold">Directions:</span> {dataSource.importantInfo.directions || "Not specified"}
            </p>
            <p className="text-gray-600 mt-2">
              <span className="font-semibold">Legal Disclaimer:</span> {dataSource.importantInfo.legalDisclaimer || "Not specified"}
            </p>
          </div>
        </div>
      </div>
    </div>
  );
}

export default ProductDetail;
