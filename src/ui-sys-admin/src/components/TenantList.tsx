import Link from "next/link";
import { BillingInfo, Tenant } from "types";
import { MdSmartphone, MdOutlineEmail } from "react-icons/md";
import style from 'styles/TenantList.module.css';

type TenantListProps = {
  tenants: Tenant[];
};

export function TenantList({ tenants }: TenantListProps) {
  //TODO This is abstracted for adding pagination later
  return (
    <ul className={style.tenantList}>
      {tenants.map(tenant => (
        <TenantListItem key={tenant.id} tenant={tenant} />
      ))}
    </ul>
  );
}

function TenantListItem({ tenant }: { tenant: Tenant }) {
  return (
    <Link href={`/admin/customers/${tenant.id}`}>
      <li className={style.tenantListItem}>
        <div>
          <h4>{tenant.name}</h4>
        </div>
        <div>
          <PaymentStatus status={tenant.billingInfo.paymentStatus} />
        </div>
      </li>
    </Link>
  );
}
// todo dry
function PaymentStatus({ status }: { status: BillingInfo['paymentStatus'] }) {
  const className = [style.paymentStatus, style[status.toLowerCase()]].join(' ');
  return <span className={className}>{status}</span>;
}