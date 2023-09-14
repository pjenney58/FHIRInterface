import Head from 'next/head';
import style from 'styles/CreateUserPage.module.css';
import Select from 'react-select';
import { stateSelectArray } from 'utils';

export default function CreateUserPage() {
    return (
        <>
            <Head>
                <title>Create User</title>
            </Head>
            <div>
                <h1>Create User</h1>
                <form>
                    <fieldset className="card">
                        <legend>Contact Information</legend>
                        <div className="input-wrapper">
                            <label htmlFor="firstName">First Name</label>
                            <input type="text" id="firstName" name="firstName" />
                        </div>
                        {/* middle name input */}
                        <div className="input-wrapper">
                            <label htmlFor="middleName">Middle Name</label>
                            <input type="text" id="middleName" name="middleName" />
                        </div>
                        <div className="input-wrapper">
                            <label htmlFor="lastName">Last Name</label>
                            <input type="text" id="lastName" name="lastName" />
                        </div>
                        <div className="input-wrapper">
                            <label htmlFor="email">Email</label>
                            <input type="email" id="email" name="email" />
                        </div>
                        <div className="input-wrapper">
                            <label htmlFor="phoneNumber">Phone Number</label>
                            <input type="tel" id="phoneNumber" name="phoneNumber" />
                        </div>
                    </fieldset>
                    <fieldset className="card">
                        <legend>Address</legend>
                        <div className="input-wrapper">
                            <label htmlFor="street1">Street 1</label>
                            <input type="text" id="street1" name="street1" />
                        </div>
                        <div className="input-wrapper">
                            <label htmlFor="street2">Street 2</label>
                            <input type="text" id="street2" name="street2" />
                        </div>
                        <div className="input-wrapper">
                            <label htmlFor="city">City</label>
                            <input type="text" id="city" name="city" />
                        </div>
                        <div className='city-state-container' >
                            <div className="input-wrapper">
                                <label htmlFor="state">State</label>
                                <Select options={stateSelectArray} id="state" name="state" />
                            </div>
                            <div className="input-wrapper">
                                <label htmlFor="zip">Zip</label>
                                <input type="text" id="zip" name="zip" />
                            </div>
                        </div>
                    </fieldset>
                    <div className={style.formButtons}>
                        <button className="button danger">Cancel</button>
                        <button type='submit' className="button primary">Save</button>
                    </div>
                </form>
            </div>
        </>
    )
}