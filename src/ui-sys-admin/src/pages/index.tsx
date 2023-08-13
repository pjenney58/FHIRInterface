import Head from 'next/head'
import Image from 'next/image'
import style from 'styles/Home.module.css'
import { useSession, signIn, signOut } from 'next-auth/react'


type Inputs = {
  username: string,
  password: string
}

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
  const {data: session} = useSession();
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
      <main className={style.main}>
        {/* TODO Dynamic translations */}
        <h1 className={style.title}>{text.title}</h1>
        <div className={style.card} >
          <h2>{text.login}</h2>
          <>
            {session && (
              <div>
                Signed in as {session?.user?.email} <br />
                <button onClick={() => signOut()}>Sign out</button>
              </div>
            )}
            {!session && (
              <div>
                Not signed in <br />
                <button onClick={() => signIn()}>Sign in</button>
              </div>
            )}
          </>
        </div>
      </main>
    </>
  )
}
