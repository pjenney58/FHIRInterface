import { SessionProvider } from "next-auth/react"
import 'styles/globals.css';
import type { AppProps } from 'next/app';
import { Roboto } from '@next/font/google';

const roboto = Roboto({
  subsets: ['latin'],
  weight: ['400', '700']
});

export default function App({
  Component,
  pageProps: { session, ...pageProps },
}: AppProps) {
  const classNames = [roboto.className, 'main'].join(' ');
  return (
    <SessionProvider session={session}>
      <main className={classNames}>
        <Component {...pageProps} />
      </main>
    </SessionProvider>
  )
}
