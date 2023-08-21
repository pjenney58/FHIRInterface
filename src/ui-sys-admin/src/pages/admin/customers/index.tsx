import Link from 'next/link';

export default function Customers() {
  return (
    <div>
      <h1>Customers</h1>
      <ul>
        <li>List Tenants -- multiple places, component</li>
        <li>Delete Tenant -- button on list and in the edit</li>
        <li>Edit Tenant -- full view</li>
        <li>View Tenant details? Full page, or just accordian?</li>
        <li><Link href='customers/create-tenant'>Create Tenants workflow -- separate page</Link></li>
      </ul>
    </div>
  )
}