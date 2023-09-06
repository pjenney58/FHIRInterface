
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


export interface User {
  id: string;
  name: {
    given: string;
    family: string;
  };
  email: string;
  username: string;
  birthdate: string;
  address: Address;
  phoneNumber: string;
  associatedCustomers: string[]
  customer: string;
  roles: string[];
}

export interface Address {
  street1: string;
  street2: string;
  city: string;
  state: string;
  zip: string;
}