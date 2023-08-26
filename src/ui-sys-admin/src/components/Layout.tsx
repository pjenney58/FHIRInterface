import { Roboto } from "next/font/google";
import Nav from "./Nav"
import { useSession } from "next-auth/react";

// Next Fonts. Self hosts the font automatically instead of using Google Fonts
const roboto = Roboto({
  subsets: ['latin'],
  weight: ['400', '700']
});

type Props = {
  children: React.ReactNode
}

export default function Layout({ children }: Props) {
  return (
    <div className={roboto.className} >
      <Nav />
      <main className="main">
        {children}
      </main>
    </div>
  )
}