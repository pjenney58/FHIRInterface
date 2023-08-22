import Link from 'next/link';


const links = [
  { href: '/admin/users', label: 'Users' },
  { href: '/admin/billing', label: 'Billing' },
  { href: '/admin/customers', label: 'Customers' },
  { href: '/admin/deployments', label: 'Deployments' }
];

export default function Nav() {
  return (
    <nav>
      <ul>
        {links.map(({ href, label }) => (
          <li key={`${href}${label}`}>
            <Link href={href}>{label}</Link>
          </li>
        ))}
      </ul>
    </nav>
  );
}