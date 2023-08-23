import { SessionProvider, useSession } from 'next-auth/react';
import 'styles/globals.css';
import { NextPage } from 'next';
import { Roboto } from 'next/font/google';
import type { AppProps } from 'next/app';

// We can use the auth flag to conditionally require authentication
// Can be extended to include roles, etc by converting to an object
export type NextPageWithAuthBypass<P = {}, IP = P> = NextPage<P, IP> & {
  bypassAuth?: boolean
}

export type NextPageWithAuthRequired<P = {}, IP = P> = NextPageWithAuthBypass<P, IP> & {
  bypassAuth: false,
  requiredRoles?: string[]
}

type MyAppProps = AppProps<any> & {
  Component: NextPageWithAuthBypass | NextPageWithAuthRequired
}

// Next Fonts. Self hosts the font automatically instead of using Google Fonts
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
        {/* If we don't care about a "landing page", we could wrap everything in the auth check */}
        {/* The first page would just be the generic signin from NextAuth */}
        {Component.bypassAuth ? (
          <Component {...pageProps} />
        ) : (
          <Auth>
            <Component {...pageProps} />
          </Auth>
        )}
      </main>
    </SessionProvider>
  );
}

function Auth({ children }: { children: React.ReactNode }) {
  const { status } = useSession({ required: true });
  if (status === 'loading') return (
    <div>
      <h1>Loading...</h1>
    </div>
  );
  return children;
}