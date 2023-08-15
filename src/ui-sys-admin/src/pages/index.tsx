import Head from 'next/head'
import Image from 'next/image'
import style from 'styles/Home.module.css'
import type { InferGetServerSidePropsType, GetServerSideProps, NextPageContext } from 'next';
import { useSession, signIn, signOut } from 'next-auth/react';


export async function getServerSideProps(context: NextPageContext) {

  return {
    props: { message: `Next.js is awesome` }, // will be passed to the page component as props
  }
}
const translations = {
  en: {
    title: 'PPM - System Administration',
    login: 'Login',
    signedInMessage: 'Signed in as',
    signedOutMessage: 'Not signed in',
  },
  fr: {
    title: 'PPM - Administration du système',
    login: 'Connexion',
    signedInMessage: 'Connecté en tant que',
    signedOutMessage: 'Non connecté'
  },
  es: {
    title: 'PPM - Administración del sistema',
    login: 'Iniciar sesión',
    signedInMessage: 'Conectado como',
    signedOutMessage: 'No conectado'
  }
}
// TODO derive lang from browser and pass in as prop
export default function Home() {
  const { data: session, status } = useSession();
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
      <div>
        {/* TODO Dynamic translations */}
        <h1 className={style.title}>{text.title}</h1>
        <div className="card" >
          <Image src="/images/logo.jpg" alt="PPM Logo" width={250} height={75} />
          <>
            {session && (
              <div>
                <p>{text.signedInMessage} {session?.user?.email}</p>
                <button className='button' onClick={() => signOut()}>Sign out</button>
              </div>
            )}
            {!session && (
              <div>
                <p>{text.signedOutMessage}</p>
                <button className='button' onClick={() => signIn()}>Sign in</button>
              </div>
            )}
          </>
        </div>
      </div>
    </>
  )
}
