import { HTMLInputTypeAttribute } from "react";
import { FieldValues, UseFormRegister } from "react-hook-form";

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

export type UserInputs = {
  firstName: string;
  middleName: string;
  lastName: string;
  email: string;
  phoneNumber: string;
  street1: string;
  street2: string;
  city: string;
  state: string;
  zip: string;
  country: string;
}

export type CustomerInputs = {
  name: string;
  phoneNumber: string;
  street1: string;
  street2: string;
  city: string;
  state: string;
  zip: string;
  country: string;
}
export interface InputField<T extends FieldValues> {
  label: string
  name: keyof T
  // The type for HTMLInputTypeAttribute has (string & {}) which kinda ruins the type safety. 
  type: HTMLInputTypeAttribute
  required: boolean
  component?: any
}

export interface InputProps<T extends FieldValues> extends InputField<T> {
  register: UseFormRegister<T>
  // control: Control<UserInputs> -- this is the type that react-hook-form wants, but it doesn't work with react-select.
  // "any" works, but it does bug me. Maybe this will be revisited later.
  control: any
}
