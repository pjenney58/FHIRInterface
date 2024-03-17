import { signOut } from 'next-auth/react';
import Link from 'next/link';
import { useRouter } from 'next/router';
import style from 'styles/Nav.module.css';


const links = [
  { href: '/admin/users', label: 'Users' },
  { href: '/admin/billing', label: 'Billing' },
  { href: '/admin/customers', label: 'Customers' },
  { href: '/admin/deployments', label: 'Deployments' }
];

export default function Nav() {
  const router = useRouter();

  console.log(router.pathname);
  return (
    <nav className='main-nav'>
      <ul>
        {links.map(({ href, label }) => (
          <li key={`${href}${label}`} className={router.pathname === href ? 'isActive' : undefined}>
            <Link href={href}>{label}</Link>
          </li>
        ))}
        <li>
          <a onClick={() => signOut()} >Log Out</a>
        </li>
      </ul>
    </nav>
  );
}