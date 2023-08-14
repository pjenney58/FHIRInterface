
import Link from 'next/link';

const navItems = [
  { name: 'User Management', path: '/admin/users' },
  { name: 'Billing Management', path: '/admin/billing' },
  { name: 'Customer Management', path: '/admin/customers' },
  { name: 'Deployment Management', path: '/admin/deployments' },
]

export default function Admin() {
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