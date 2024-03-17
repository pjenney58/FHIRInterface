import style from 'styles/PaymentStatusIcon.module.css';
import { BillingInfo } from 'types';

export function PaymentStatusIcon({ status }: { status: BillingInfo['paymentStatus'] }) {
    const className = [style.paymentStatus, style[status.toLowerCase()]].join(' ');
    return <span className={className}>{status}</span>;
}