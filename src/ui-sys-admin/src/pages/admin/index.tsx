import Nav from 'components/Nav';
import { useSession } from 'next-auth/react';
import Link from 'next/link';

const navItems = [
  { name: 'User Management', path: '/admin/users' },
  { name: 'Billing Management', path: '/admin/billing' },
  { name: 'Customer Management', path: '/admin/customers' },
  { name: 'Deployment Management', path: '/admin/deployments' },
]

export default function Admin() {
  const { data: session, status } = useSession({ required: true });
  if (status === 'loading') return (
    <div>
      <h1>Loading...</h1>
    </div>
  )
  return (
    <div>
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
    </div>
  )
}