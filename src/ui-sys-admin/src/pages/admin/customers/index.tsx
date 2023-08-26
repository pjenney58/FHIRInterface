import { TenantList } from 'components/TenantList';
import { useEffect, useState } from 'react';
import { Tenant } from 'types';
import { getMockClinics } from 'utils';

export default function Customers() {
  const [tenants, setTenants] = useState<Tenant[]>([])

  useEffect(() => {
    // TODO real data fetching
    const data = getMockClinics();
    console.log(data);
    setTenants(data);
  }, [])

  return (
    <div>
      <h1>Customers</h1>
      {tenants && <TenantList tenants={tenants} />}
    </div>
  )
}


      {/* <ul>
        <li>List Tenants -- multiple places, component</li>
        <li>Delete Tenant -- button on list and in the edit</li>
        <li>Edit Tenant -- full view</li>
        <li>View Tenant details? Full page, or just accordian?</li>
        <li><Link href='customers/create-tenant'>Create Tenants workflow -- separate page</Link></li>
      </ul> */}