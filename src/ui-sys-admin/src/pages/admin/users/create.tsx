import Head from 'next/head';
import style from 'styles/CreateUserPage.module.css';
import Select from 'react-select';
import { stateSelectArray } from 'utils';
import { useForm } from 'react-hook-form';
import { InputType } from 'zlib';
import { HTMLInputTypeAttribute } from 'react';

type UserInputs = {
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
type InputField = { label: string, name: keyof UserInputs, type: HTMLInputTypeAttribute, required: boolean }

const inputGroups: { groupName: string, fields: InputField[] }[] = [
    {
        groupName: 'Contact Information',
        fields: [
            {
                label: 'First Name',
                name: 'firstName',
                type: 'text',
                required: true
            },
            {
                label: 'Middle Name',
                name: 'middleName',
                type: 'text',
                required: false
            },
            {
                label: 'Last Name',
                name: 'lastName',
                type: 'text',
                required: true
            },
            {
                label: 'Email',
                name: 'email',
                type: 'email',
                required: true
            },
            {
                label: 'Phone Number',
                name: 'phoneNumber',
                type: 'tel',
                required: true
            }
        ]
    },
    {
        groupName: 'Address',
        fields: [
            {
                label: 'Street 1',
                name: 'street1',
                type: 'text',
                required: true
            },
            {
                label: 'Street 2',
                name: 'street2',
                type: 'text',
                required: false
            },
            {
                label: 'City',
                name: 'city',
                type: 'text',
                required: true
            },
            {
                label: 'State',
                name: 'state',
                type: 'text',
                required: true
            },
            {
                label: 'Zip',
                name: 'zip',
                type: 'text',
                required: true
            },
            {
                label: 'Country',
                name: 'country',
                type: 'text',
                required: true
            }
        ]
    }


]


export default function CreateUserPage() {
    const { register, handleSubmit, watch, formState: { errors } } = useForm<UserInputs>();
    const onSubmit = (data: UserInputs) => console.log(data);
    console.log(watch('firstName'));
    return (
        <>
            <Head>
                <title>Create User</title>
            </Head>
            <div>
                <h1>Create User</h1>
                <form onSubmit={handleSubmit(onSubmit)} >
                    {inputGroups.map((group, index) => {
                        return (
                            <fieldset className="card" key={index + group.groupName}>
                                <legend>{group.groupName}</legend>
                                {group.fields.map((field, index) => {
                                    return (
                                        <div className="input-wrapper" key={index}>
                                            <label htmlFor={field.name}>{field.label}</label>
                                            <input {...register(field.name)} type={field.type} required={field.required} />
                                        </div>
                                    )
                                })}
                            </fieldset>
                        )
                    })}
                    <div className={style.formButtons}>
                        <button className="button danger">Cancel</button>
                        <button type='submit' className="button primary">Save</button>
                    </div>
                </form>
            </div>
        </>
    )
}