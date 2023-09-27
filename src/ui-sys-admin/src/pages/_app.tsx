import { SessionProvider, useSession } from 'next-auth/react';
import 'styles/globals.css';
import { NextPage } from 'next';
import { Roboto } from 'next/font/google';
import type { AppProps } from 'next/app';
import Layout from 'components/Layout';


const ENVIRONMENT = process.env.ENVIRONMENT;

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


export default function App({
  Component,
  pageProps: { session, ...pageProps },
}: MyAppProps) {

  return (
    <SessionProvider session={session}>
      {/* If we don't care about a "landing page", we could wrap everything in the auth check */}
      {/* The first page would just be the generic signin from NextAuth */}
      {Component.bypassAuth ? (
        <Component {...pageProps} />
      ) : (
        <AuthRequired>
          <Component {...pageProps} />
        </AuthRequired>
      )}
    </SessionProvider>
  );
}

function AuthRequired({ children }: { children: React.ReactNode }) {
  // TODO This is just here for easier dev work. Remove to test auth
  const required = (ENVIRONMENT === 'production');
  const { status } = useSession({ required: true });
  if (status === 'loading') return (
    <div>
      <h1>Loading...</h1>
    </div>
  );
  return (
    <Layout>
      {children}
    </Layout>
  );
}