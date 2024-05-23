import { Typography } from 'antd/es'
import React from 'react'

function Footer() {
  return (
    <div className='h-15 flex justify-between items-center border-t-2 border-b border-custom-rgba'>
      <Typography.Link href='tel:+123456789'>0989957451</Typography.Link>
      <Typography.Link href='https://www.youtube.com/' target={'_blank'}>Privace Policy</Typography.Link>
      <Typography.Link href='https://www.youtube.com/' target={'_blank'}>Term of Use</Typography.Link>
    </div>
  )
}

export default Footer