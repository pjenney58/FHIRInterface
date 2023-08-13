import Head from 'next/head'
import Image from 'next/image'
import { Inter } from 'next/font/google'
import styles from '@/styles/Home.module.css'

const inter = Inter({ subsets: ['latin'] })

const translations = {
  en: {
    title: 'PPM - System Administration',
    login: 'Login'
  },
  fr: {
    title: 'PPM - Administration du système',
    login: 'Connexion'
  },
  es: {
    title: 'PPM - Administración del sistema',
    login: 'Iniciar sesión'
  }
}
// TODO derive lang from browser and pass in as prop
export default function Home() {
  const lang = 'en';
  const text = translations[lang];
  return (
    <>
      <Head>
        <title>{text.title}</title>
        <meta name="description" content="PPM - SysAdmin" />
        <meta name="viewport" content="width=device-width, initial-scale=1" />
        <link rel="icon" href="/favicon.ico" />
      </Head>
      <main className={`${styles.main} ${inter.className}`}>
        {/* TODO Dynamic translations */}
        <h1 className={styles.title}>{text.title}</h1>
        <div className={styles.card} >
          <h2>{text.login}</h2>
          <form>
            <div>
              <label htmlFor="username">Username</label>
              <input type="text" id="username" name="username" />
            </div>
            <div>
              <label htmlFor="password">Password</label>
              <input type="password" id="password" name="password" />
            </div>
            <button type="submit">Login</button>
          </form>
        </div>
      </main>
    </>
  )
}
