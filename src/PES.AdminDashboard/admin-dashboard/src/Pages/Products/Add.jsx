import React, { useState } from 'react';

function AddProductForm() {
    const [productData, setProductData] = useState({
        productName: '',
        price: '',
        quantity: '',
        description: '',
        informationRequest: {
            ingredients: '',
            directions: '',
            legalDisclaimer: ''
        },
        nutritionInformationRequest: {
            calories: '',
            protein: '',
            sodium: '',
            fiber: '',
            sugars: ''
        },
        listImages: [],
        mainImage: '',
        categoryId: ''
    });
    const [showOptionalFields, setShowOptionalFields] = useState(false);
    const [showImage,setShowImage ] = useState(false);
    const [showNutrionAdd, setshowNutrionAdd] = useState(false);

    const handleChange = (e) => {
        const { name, value } = e.target;
        setProductData({
            ...productData,
            [name]: value
        });
    };

    const handleCheckboxChange = () => {
        setShowOptionalFields(!showOptionalFields);
    };
    const handleCheckboxChangeShowImage = () => {
        setShowImage(!showImage);
    };
    const handleCheckboxChangeNutrionInformation = () => {
        setshowNutrionAdd(!showNutrionAdd);
    };

    const handleSubmit = (e) => {
        e.preventDefault();
        // Send productData to your API endpoint for adding a new product
        console.log('Product data:', productData);
        // Clear the form after submission
        setProductData({
            productName: '',
            price: '',
            quantity: '',
            description: '',
            informationRequest: {
                ingredients: '',
                directions: '',
                legalDisclaimer: ''
            },
            nutritionInformationRequest: {
                calories: '',
                protein: '',
                sodium: '',
                fiber: '',
                sugars: ''
            },
            listImages: [],
            mainImage: '',
            categoryId: ''
        });
        setShowOptionalFields(false);
    };

    return (
        <div className="max-w-md mx-auto bg-white p-6 rounded-md shadow-md">
            <h2 className="text-lg font-semibold mb-4">Add New Product</h2>
            <form onSubmit={handleSubmit} className="space-y-4">
                <div>
                    <label className="block">
                        Product Name:
                        <input
                            type="text"
                            name="productName"
                            value={productData.productName}
                            onChange={handleChange}
                            className="form-input mt-1 block w-full"
                            required
                        />
                    </label>
                </div>
                <div>
                    <label className="block">
                        Price:
                        <input
                            type="number"
                            name="price"
                            value={productData.price}
                            onChange={handleChange}
                            className="form-input mt-1 block w-full"
                            required
                        />
                    </label>
                </div>
                <div>
                    <label className="block">
                        Quantity:
                        <input
                            type="number"
                            name="quantity"
                            value={productData.quantity}
                            onChange={handleChange}
                            className="form-input mt-1 block w-full"
                        />
                    </label>
                </div>
                <div>
                    <label className="block">
                        Description:
                        <textarea
                            name="description"
                            value={productData.description}
                            onChange={handleChange}
                            className="form-textarea mt-1 block w-full"
                            required
                        ></textarea>
                    </label>
                </div>
                <div className="mb-4">
                    <label className="flex items-center">
                        <input
                            type="checkbox"
                            className="form-checkbox"
                            checked={showImage}
                            onChange={handleCheckboxChangeShowImage}
                        />
                        <span className="ml-2">Add Image Options</span>
                    </label>
                </div>
                {showImage && (
                    <div>
                        {/* Optional fields */}
                        <div>
                            <label className="block">
                                Ingredients:
                                <input
                                    type="text"
                                    name="ingredients"
                                    value={productData.informationRequest.ingredients}
                                    onChange={(e) => handleChange({ target: { name: 'ingredients', value: e.target.value } })}
                                    className="form-input mt-1 block w-full"
                                />
                            </label>
                        </div>
                        <div>
                            <label className="block">
                                Directions:
                                <input
                                    type="text"
                                    name="directions"
                                    value={productData.informationRequest.directions}
                                    onChange={(e) => handleChange({ target: { name: 'directions', value: e.target.value } })}
                                    className="form-input mt-1 block w-full"
                                />
                            </label>
                        </div>
                        <div>
                            <label className="block">
                                Legal Disclaimer:
                                <input
                                    type="text"
                                    name="legalDisclaimer"
                                    value={productData.informationRequest.legalDisclaimer}
                                    onChange={(e) => handleChange({ target: { name: 'legalDisclaimer', value: e.target.value } })}
                                    className="form-input mt-1 block w-full"
                                />
                            </label>
                        </div>
                    </div>
                )}
                <div className="mb-4">
                    <label className="flex items-center">
                        <input
                            type="checkbox"
                            className="form-checkbox"
                            checked={showOptionalFields}
                            onChange={handleCheckboxChange}
                        />
                        <span className="ml-2">Show Optional 1</span>
                    </label>
                </div>

                {showOptionalFields && (
                    <div>
                        {/* Optional fields */}
                        <div>
                            <label className="block">
                                Ingredients:
                                <input
                                    type="text"
                                    name="ingredients"
                                    value={productData.informationRequest.ingredients}
                                    onChange={(e) => handleChange({ target: { name: 'ingredients', value: e.target.value } })}
                                    className="form-input mt-1 block w-full"
                                />
                            </label>
                        </div>
                        <div>
                            <label className="block">
                                Directions:
                                <input
                                    type="text"
                                    name="directions"
                                    value={productData.informationRequest.directions}
                                    onChange={(e) => handleChange({ target: { name: 'directions', value: e.target.value } })}
                                    className="form-input mt-1 block w-full"
                                />
                            </label>
                        </div>
                        <div>
                            <label className="block">
                                Legal Disclaimer:
                                <input
                                    type="text"
                                    name="legalDisclaimer"
                                    value={productData.informationRequest.legalDisclaimer}
                                    onChange={(e) => handleChange({ target: { name: 'legalDisclaimer', value: e.target.value } })}
                                    className="form-input mt-1 block w-full"
                                />
                            </label>
                        </div>
                    </div>
                )}
                <div className="mb-4">
                    <label className="flex items-center">
                        <input
                            type="checkbox"
                            className="form-checkbox"
                            checked={showNutrionAdd}
                            onChange={handleCheckboxChangeNutrionInformation}
                        />
                        <span className="ml-2">Show Optional 2</span>
                    </label>
                </div>
                {showNutrionAdd && (
                    <div>
                        {/* Optional fields */}
                        <div>
                            <label className="block">
                                Calories:
                                <input
                                    type="number"
                                    name="calories"
                                    value={productData.nutritionInformationRequest.calories}
                                    onChange={(e) => handleChange({ target: { name: 'calories', value: e.target.value } })}
                                    className="form-input mt-1 block w-full"
                                />
                            </label>
                        </div>
                        <div>
                            <label className="block">
                                Protein:
                                <input
                                    type="number"
                                    name="protein"
                                    value={productData.nutritionInformationRequest.protein}
                                    onChange={(e) => handleChange({ target: { name: 'protein', value: e.target.value } })}
                                    className="form-input mt-1 block w-full"
                                />
                            </label>
                        </div>
                        <div>
                            <label className="block">
                                Sodium:
                                <input
                                    type="number"
                                    name="sodium"
                                    value={productData.nutritionInformationRequest.sodium}
                                    onChange={(e) => handleChange({ target: { name: 'sodium', value: e.target.value } })}
                                    className="form-input mt-1 block w-full"
                                />
                            </label>
                        </div>
                        <div>
                            <label className="block">
                                Fiber:
                                <input
                                    type="number"
                                    name="fiber"
                                    value={productData.nutritionInformationRequest.fiber}
                                    onChange={(e) => handleChange({ target: { name: 'fiber', value: e.target.value } })}
                                    className="form-input mt-1 block w-full"
                                />
                            </label>
                        </div>
                        <div>
                            <label className="block">
                                Sugars:
                                <input
                                    type="number"
                                    name="sugars"
                                    value={productData.nutritionInformationRequest.sugars}
                                    onChange={(e) => handleChange({ target: { name: 'sugars', value: e.target.value } })}
                                    className="form-input mt-1 block w-full"
                                />
                            </label>
                        </div>
                    </div>
                )}


                <button
                    type="submit"
                    className="bg-blue-500 hover:bg-blue-600 text-white font-semibold py-2 px-4 rounded"
                >
                    Add Product
                </button>
            </form>
        </div>
    );
}

export default AddProductForm;