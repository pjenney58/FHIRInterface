import { Roboto } from "next/font/google";
import Nav from "./Nav"

// Next Fonts. Self hosts the font automatically instead of using Google Fonts
const roboto = Roboto({
  subsets: ['latin'],
  weight: ['400', '700']
});

type Props = {
  children: React.ReactNode
  session?: any
}

export default function Layout({ children, session }: Props) {
  return (
    <div className={roboto.className} >
      {session && <Nav />}
      <main className="main">
        {children}
      </main>
    </div>
  )
}