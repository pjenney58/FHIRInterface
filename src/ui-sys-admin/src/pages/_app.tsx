import { SessionProvider, useSession } from "next-auth/react"
import 'styles/globals.css';
import { NextPage } from 'next';
import { Roboto } from '@next/font/google';
import type { AppProps } from 'next/app';

// We can use the auth flag to conditionally require authentication
// Can be extended to include roles, etc by converting to an object
export type NextPageWithAuth<P = {}, IP = P> = NextPage<P, IP> & {
  bypassAuth?: boolean
}

type MyAppProps = AppProps<any> & {
  Component: NextPageWithAuth;
}


const roboto = Roboto({
  subsets: ['latin'],
  weight: ['400', '700']
});

export default function App({
  Component,
  pageProps: { session, ...pageProps },
}: MyAppProps) {
  const classNames = [roboto.className, 'main'].join(' ');
  return (
    <SessionProvider session={session}>
      <main className={classNames}>
        {Component.bypassAuth ? (
          <Component {...pageProps} />
        ) : (
          <Auth>
            <Component {...pageProps} />
          </Auth>
        )}
      </main>
    </SessionProvider>
  )
}

function Auth({ children }: { children: React.ReactNode }) {
  const { status } = useSession({ required: true });
  if (status === 'loading') return (
    <div>
      <h1>Loading...</h1>
    </div>
  )
  return children;
}