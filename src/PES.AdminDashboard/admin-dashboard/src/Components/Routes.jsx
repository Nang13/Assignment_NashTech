import {  Route, Routes } from "react-router-dom";
import Dashboard from "../Pages/Dashboard";
import User from "../Pages/Users/Index";
import Category from "../Pages/Categories/Index";
import Product from "../Pages/Products/Index";
import CategoryDetail from "../Pages/Categories/Detail";
import ProductDetail from "../Pages/Products/Detail";
import UpdateProduct from "../Pages/Products/Update";
import AddNewProduct from "../Pages/Products/Add";
function AppRoutes() {
    return (
        <Routes>
            <Route path="/" element={<Dashboard />}></Route>
            <Route path="/user" element={<User />}></Route>
            <Route path="/category" element={<Category />}></Route>
            <Route path="/product" element={<Product />}></Route>
            <Route path="/category_detail/:id"  element={<CategoryDetail />}></Route>
            <Route path="/product_update/:id"  element={<UpdateProduct />}></Route>
            <Route path="/update/:id"  element={<ProductDetail />}></Route>
            <Route path="/add_product/"  element={<AddNewProduct />}></Route>
        </Routes>
    )
}

export default AppRoutes;