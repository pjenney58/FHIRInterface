import Link from "next/link"

const links = [
  {
    name: 'Host Mangement',
    href: '/admin/deployments/hosts'
  },
  {
    name: 'Container Management',
    href: '/admin/deployments/containers'
  },
  {
    name: 'Performance Management',
    href: '/admin/deployments/performance'
  },
  {
    name: 'System Reporting',
    href: '/admin/deployments/reporting'
  },
  {
    name: 'Database Management',
    href: '/admin/deployments/database'
  },
  {
    name: 'Collector Management',
    href: '/admin/deployments/collectors'
  },
  {
    name: 'Retriever Management',
    href: '/admin/deployments/retrievers'
  },
  {
    name: 'Compliance Management',
    href: '/admin/deployments/compliance'
  }
]
export default function Deployments() {
  return (
    <div>
      <h1>Deployments</h1>
      <div className="card">
        <ul>
          {links.map((link) => (
            <li key={link.name}>
              <Link href={link.href}>
                {link.name}
              </Link>
            </li>
          ))}
        </ul>
      </div>
    </div>
  )
}