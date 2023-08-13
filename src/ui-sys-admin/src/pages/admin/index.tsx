
// use getServerSidePros to check for auth
// if not logged in, redirect to login page
// if logged in, render admin page
import style from 'styles/Admin.module.css';
import Link from 'next/link';

const navItems = [
  { name: 'User Management', path: '/admin/users' },
  { name: 'Billing Management', path: '/admin/billing' },
  { name: 'Customer Management', path: '/admin/customers' },
  { name: 'Deployment Management', path: '/admin/deployments' },
]

export default function Admin() {
  return (
    <main className={style.main} >
      <h1>Admin</h1>
      <section>
        <nav>
          <ul>
            {navItems.map(item => (
              <li key={item.name}>
                <Link href={item.path} >
                  {item.name}
                </Link>
              </li>
            )
            )}
          </ul>
        </nav>
      </section>
    </main>
  )
}