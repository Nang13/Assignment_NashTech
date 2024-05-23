import React from 'react'

import { Typography , Image} from 'antd/es';

function Header() {
  return (
    <div className='h-15 flex justify-between items-center p-4 border-b border-custom-rgba'>
      <Image width={70} src='https://firebasestorage.googleapis.com/v0/b/ntassignment-518e1.appspot.com/o/OIP%20(4).jpg?alt=media&token=d30f5a2d-3598-430e-a7ad-28708271a927'></Image>
      <Typography.Title>Admin Dashboard</Typography.Title>
    </div>
  )
}

export default Header