import Head from 'next/head';
import Image from 'next/image';
import style from 'styles/Home.module.css';
import type { NextPageContext } from 'next';
import { useSession, signIn } from 'next-auth/react';
import { NextPageWithAuthBypass } from './_app';
import { useRouter } from 'next/router';
import { useEffect } from 'react';

export async function getServerSideProps(context: NextPageContext) {
  // Use this for translations 

  return {
    props: { message: 'Next.js is awesome' }, // will be passed to the page component as props
  };
}
// Fake translations for now
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
};
// TODO derive lang from browser and pass in as prop
const Home: NextPageWithAuthBypass = () => {
  const { data: session, status } = useSession();
  const lang = 'en';
  const text = translations[lang];
  const router = useRouter();

  useEffect(() => {
    if (status === 'authenticated' && session) {
      router.push('/admin');
    }
  }, [status, session, router]);
  if (status === 'loading') return (<div>Loading...</div>);

  return (
    <>
      <Head>
        <title>{text.title}</title>
        <meta name="description" content="PPM - SysAdmin" />
        <meta name="viewport" content="width=device-width, initial-scale=1" />
        <link rel="icon" href="/favicon.ico" />
      </Head>
      <div className={style.main} >
        <h1>{text.title}</h1>
        <section className={style.card}>
          <div>
            <Image src="/images/logo.jpg" alt="PPM Logo" width={250} height={60} />
          </div>
          {!session && <button className='button primary' onClick={() => signIn()}>Sign in</button>}
        </section>
      </div>
    </>
  );
};
Home.bypassAuth = true;
export default Home;
