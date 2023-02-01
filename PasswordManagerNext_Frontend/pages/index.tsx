import Head from 'next/head'
import Image from 'next/image'
import { Inter } from '@next/font/google'
import styles from '../styles/Home.module.css'
import Header from './components/header'
import TableView from './components/tableview'
import { NextPage } from 'next'
import { LoginForm } from './components/LoginForm'
import { RegisterForm } from './components/RegisterForm'

const inter = Inter({ subsets: ['latin'] })

const Home : NextPage = () => {
  return (
    <div className={styles.container}>
      <Head>
          <title>Password Manager</title>
          <meta name="description" content="Home page" />
      </Head>
      <Header title="Password Manager"/>
      <TableView />
      {/* <LoginForm/> */}
      {/* <RegisterForm/> */}
    </div>
  )
}

export default Home