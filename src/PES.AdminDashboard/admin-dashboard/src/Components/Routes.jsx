import { Navigate, Route, Routes } from "react-router-dom";
import Dashboard from "../Pages/Dashboard";
import User from "../Pages/Users/Index";
import Category from "../Pages/Categories/Index";
import Product from "../Pages/Products/Index";
import CategoryDetail from "../Pages/Categories/Detail";
import ProductDetail from "../Pages/Products/Detail";
import UpdateProduct from "../Pages/Products/Update";
import AddNewProduct from "../Pages/Products/Add";
import PrivateRoute from "../PrivateRoute";
import Login from "../Pages/Auth/Login";
import Header from './Header';
import { Space } from 'antd'
import { BrowserRouter as Router } from 'react-router-dom';
import SideMenu from "./SideMenu";
import Content from "./Content";
import Footer from "./Footer";
import MainLayout from "./MainLayout";
import Orders from "../Pages/Users/Orders";
import OrderDetail from "../Pages/Users/OrderDetail";
import Order from "../Pages/Order/Index";
function AppRoutes() {
    return (
        <Routes>
          <Route path="/" element={<Login />} />
          <Route element={<PrivateRoute />}>
            <Route path="/" element={<MainLayout />}>
              <Route path="dashboard" element={<Dashboard />} />
              <Route path="" element={<Dashboard />} />
              <Route path="user" element={<User />} />
              <Route path="category" element={<Category />} />
              <Route path="/product" element={<Product />} />
              <Route path="category_detail/:id" element={<CategoryDetail />} />
              <Route path="product_update/:id" element={<UpdateProduct />} />
              <Route path="product_detail/:id" element={<ProductDetail />} />
              <Route path="add_product" element={<AddNewProduct />} />
              <Route path="/orders/:userId" element={<Orders />} />
              <Route path="/order/:orderId" element={<OrderDetail />} />
              <Route path="/order" element={<Order />} />
            </Route>
          </Route>
          <Route path="*" element={<Navigate to="/login" />} />
        </Routes>
        // <Routes>

        //     <Navigate from="/" to="/login" />
        // </Routes>
    )
}

export default AppRoutes;
