import { useRouter } from "next/router";
import { useEffect, useState } from "react";
import { BillingInfo, Customer } from "types";
import { generateMockClinic } from "utils";
import { PaymentStatusIcon } from "components/PaymentStatusIcon";
import { MdSmartphone, MdOutlineEmail } from "react-icons/md";
import style from 'styles/TenantDisplayPage.module.css';

export default function CustomerDisplayPage() {
  const router = useRouter();
  const { customerId } = router.query;
  const [tenant, setTenant] = useState<Customer | null>(null);

  useEffect(() => {
    // TODO real data fetching
    const data = generateMockClinic();
    setTenant(data);
  }, []);
  if (!tenant) return <div>Loading...</div>;

  return (
    <div>
      <h2 >{tenant.name}</h2>
      <div className={style.wrapper}>
        <ContactCard tenant={tenant} />
        <div className="card">

          <p>Users: {tenant.associatedUsers.length}</p>
        </div>
        <div className="card">
          <h4>Billing Info</h4>
          <p>Payment Method: {tenant.billingInfo.billingMethod}</p>
          <p>Status: <PaymentStatusIcon status={tenant.billingInfo.paymentStatus} /></p>
        </div>
      </div>
    </div>
  )
}

function ContactCard({ tenant }: { tenant: Customer }) {
  return (
    <div className="card" >
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