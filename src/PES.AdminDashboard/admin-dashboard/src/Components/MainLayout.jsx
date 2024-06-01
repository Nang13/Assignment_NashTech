import React from 'react';
import { Outlet } from 'react-router-dom';
import Header from './Header';
import Footer from './Footer';
import SideMenu from './SideMenu';

const MainLayout = () => {
  return (
    <div className="flex flex-col w-screen h-screen">
      <Header />
      <div className="flex flex-1 justify-start items-start bg-opacity-5">
        <SideMenu />
        <div className="flex-1">
          <Outlet />
        </div>
      </div>
      <Footer />
    </div>
  );
};

export default MainLayout;