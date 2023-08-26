import Link from "next/link";
import { BillingInfo, Tenant } from "types";
import { MdSmartphone, MdOutlineEmail } from "react-icons/md";
import style from 'styles/TenantList.module.css';

type TenantListProps = {
  tenants: Tenant[];
};

export function TenantList({tenants}: TenantListProps) {
  //TODO This is abstracted for adding pagination later
  return (
    <ul className={style.tenantList}>
      {tenants.map(tenant => (
        <TenantListItem key={tenant.id} tenant={tenant} />
      ))}
    </ul>
  );
}

function TenantListItem({tenant}: {tenant: Tenant}) {
  return (
    <li className={style.tenantListItem}>
      <h4>{tenant.name}</h4>
      <div className={style.itemContent}>
        <div>
          <ContactCard tenant={tenant} />
          <p>Users: {tenant.associatedUsers.length}</p>
        </div>
        <div>
          <PaymentStatus status={tenant.billingInfo.paymentStatus} />
          <Link href={`/admin/customers/${tenant.id}`}>View Details</Link>
        </div>
      </div>
    </li>
  );
}

function PaymentStatus({status}: {status: BillingInfo['paymentStatus']}) {
  const className = [style.paymentStatus, style[status.toLowerCase()]].join(' ');
  return <span className={className}>{status}</span>;
}

function ContactCard({tenant}: {tenant: Tenant}) {
  return (
    <div className={style.contactCard} >
      <h5>Primary Contact</h5>
      <p>{tenant.mainContact.name.given} {tenant.mainContact.name.family}</p>
      <div className={style.contactLine}>
        <MdSmartphone /><p>{tenant.phoneNumber}</p>
      </div>
      <div className={style.contactLine}>
        <MdOutlineEmail /><p>{tenant.mainContact.email}</p>
      </div>
    </div>
  );
}