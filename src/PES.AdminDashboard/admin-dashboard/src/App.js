import logo from './logo.svg';
import './App.css';
import SideMenu from './Components/SideMenu';
import Content from './Components/Content';
import Footer from './Components/Footer';
import Header from './Components/Header';
import {Space} from 'antd'

function App() {
  return (
   <div className='flex flex-col w-screen h-screen'>
      <Header/>
    <Space class="flex flex-1 justify-start items-start  bg-opacity-5">
      <SideMenu></SideMenu>
      <Content></Content>
    </Space>
    <Footer/>
   </div>
  );
}

export default App;
