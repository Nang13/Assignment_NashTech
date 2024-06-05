import React from 'react'

import { Typography , Image} from 'antd/es';

function Header() {
  return (
    <div className="h-16 flex justify-between items-center p-4 border-b border-green-300 bg-green-50 shadow-md">
    <Image
      width={70}
      height={70}
      src="https://firebasestorage.googleapis.com/v0/b/ntassignment-518e1.appspot.com/o/OIP%20(4).jpg?alt=media&token=d30f5a2d-3598-430e-a7ad-28708271a927"
      alt="Organic Store Logo"
      className="rounded-full"
    />
    <Typography.Title level={3} className="text-green-700 font-bold">
      Admin Dashboard
    </Typography.Title>
  </div>
  )
}

export default Header