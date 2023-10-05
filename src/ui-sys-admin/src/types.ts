import { UseFormRegister, Path, RegisterOptions, Control, FieldValues } from "react-hook-form";

// NOTE These are all still in flux and may be changed. Some are wonky just because I'm generating fake data and haven't landed on names yet.
// Need to work with Pete to concretly define entities and their relationships.

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
  billingMethod: 'Email' | 'Mail' | 'Print' | 'Other';
  // redundant of billing? More likely some sub fields, but I'm guessing we don't want to actually handle financial data.
  paymentMethod: 'Credit Card' | 'Bank Transfer' | 'Check' | 'ACH' | 'Other';
}

export interface Customer {
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
  country: string;
}
// TODO Better sync UserInputs and User, Address etc
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


export type InputCommonProps<T> = {
  label: string
  name: keyof T
  required: boolean
}

export type InputProps<T> = InputCommonProps<T> & {
  type: 'text' | 'email' | 'tel'
  component?: never
} | InputCommonProps<T> & {
  type?: never
  component: React.ComponentType<any>
}

export interface FormInputGroup<T> {
  groupName: string,
  fields: InputProps<T>[]
}

export type FormInputProps<TFormValues extends FieldValues> = {
  name: Path<TFormValues>
  register: UseFormRegister<TFormValues>,
  rules?: RegisterOptions,
  control?: Control<TFormValues>,

} & Omit<InputProps<TFormValues>, 'name'>
