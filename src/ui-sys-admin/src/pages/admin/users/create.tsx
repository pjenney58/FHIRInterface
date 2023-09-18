import Head from 'next/head';
import style from 'styles/CreateUserPage.module.css';
import Select from 'react-select';
import { Controller, UseFormRegister, useForm } from 'react-hook-form';
import { HTMLInputTypeAttribute } from 'react';
import { stateSelectArray } from 'utils';

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
interface InputField {
    label: string
    name: keyof UserInputs
    type: HTMLInputTypeAttribute
    required: boolean
    component?: any
}

interface InputProps extends InputField {
    register: UseFormRegister<UserInputs>
    // control: Control<UserInputs> -- this is the type that react-hook-form wants, but it doesn't work with react-select.
    // "any" works, but it does bug me. Maybe this will be revisited later.
    control: any
}

// Are all the hoops worth it to not be handwriting HTML? Maybe, if I'm not the one in here because of a bug.
export default function CreateUserPage() {
    const { register, control, handleSubmit, watch, formState: { errors } } = useForm<UserInputs>();
    const onSubmit = (data: UserInputs) => console.log(data);
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
                                {group.fields.map((field, index) => <CustomInput key={index + field.label} {...field} register={register} control={control} />)}
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

function CustomInput(props: InputProps) {
    const { label, name, type, required, register, component: Component } = props;
    return (
        <div className="input-wrapper">
            <label htmlFor={name}>{label}</label>
            {Component ? <Component {...props} /> : <input {...register(name, { required })} />}
        </div>
    )
}

function StateSelect({ control, name }: InputProps) {
    return (

        <Controller
            name={name}
            control={control}
            render={({ field }) => (
                <Select
                    {...field}
                    options={stateSelectArray}
                />
            )
            }
        />

    )
}


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
                required: true,
                component: StateSelect
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
