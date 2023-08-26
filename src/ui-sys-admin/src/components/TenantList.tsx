import Link from "next/link";
import { Tenant } from "types";

type TenantListProps = {
  tenants: Tenant[];
};

export function TenantList({tenants}: TenantListProps) {
  return (
    <ul>
      {tenants.map(tenant => (
        <li key={tenant.id}>
          <Link href={`/admin/customers/${tenant.id}`}>{tenant.name}</Link>
        </li>
      ))}
    </ul>
  );
}
