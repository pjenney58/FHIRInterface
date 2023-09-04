
export type Customer = Clinic

export interface Address {
  street1: string;
  street2: string;
  city: string;
  state: string;
  zip: string;
}

export interface Name {
  given: string;
  family: string;
}

export interface Contact {
  name: Name;
  phoneNumber: string;
  email: string;
}

export interface BillingInfo {
  paymentStatus: 'Paid' | 'Due' | 'Overdue' | 'Pending';
  lastBillingDate: string;
  nextBillingDate: string;
  billingMethod: 'Credit Card' | 'Bank Transfer' | 'Check' | 'ACH';
  paymentMethod: string;
}

export interface Clinic {
  id: string;
  name: string;
  phoneNumber: string;
  addresses: Address[];
  specialties: string[];
  doctors: string[];
  website: string;
  rating: number;
  mainContact: Contact;
  billingInfo: BillingInfo;
  associatedUsers: string[];
}