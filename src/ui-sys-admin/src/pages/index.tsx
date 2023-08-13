import Head from 'next/head'
import Image from 'next/image'
import style from 'styles/Home.module.css'
import { useForm, SubmitHandler } from "react-hook-form";


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
  const {register, handleSubmit, formState: {errors}} = useForm<Inputs>()
  const lang = 'en';
  const text = translations[lang];

  const onSubmit: SubmitHandler<Inputs> = data => console.log(data);

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
          <form onSubmit={handleSubmit(onSubmit)} >
            <div>
              <label htmlFor="username">Username
                <input {...register("username", {required: true})} />
              </label>
            </div>
            <div>
              <label htmlFor="password">Password
                <input {...register("password", {required: true})} type='password' />
              </label>
            </div>
            <button type="submit">Login</button>
          </form>
        </div>
      </main>
    </>
  )
}
