import Head from 'next/head';
import style from 'styles/CreateEntityPages.module.css';
import { useForm } from 'react-hook-form';
import { UserInputs, InputField, CustomerInputs } from 'types';
import { StandardInput, StateSelect } from 'components/Inputs';

// Are all the hoops worth it to not be handwriting HTML? Maybe, if I'm not the one in here because of a bug.
export default function CreateCustomerPage() {
  const { register, control, handleSubmit, watch, formState: { errors } } = useForm<CustomerInputs>();
  const onSubmit = (data: CustomerInputs) => console.log(data);
  return (
    <>
      <Head>
        <title>Create Customer</title>
      </Head>
      <div>
        <h1>Create Customer</h1>
        <form className={style.form} onSubmit={handleSubmit(onSubmit)} >
          {inputGroups.map((group, index) => {
            return (
              <fieldset className="card" key={index + group.groupName}>
                <legend>{group.groupName}</legend>
                {group.fields.map((field, index) => <StandardInput key={index + field.label} {...field} register={register} control={control} />)}
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

const inputGroups: { groupName: string, fields: InputField<CustomerInputs>[] }[] = [
  {
    groupName: 'Customer Information',
    fields: [
      {
        name: 'name',
        type: 'text',
        required: true,
        label: 'Name'
      },
      {
        name: 'phoneNumber',
        type: 'tel',
        required: true,
        label: 'Phone Number'
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