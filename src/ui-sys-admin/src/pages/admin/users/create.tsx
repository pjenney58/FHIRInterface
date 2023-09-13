import style from 'styles/CreateUserPage.module.css';

export default function CreateUserPage() {
    return (
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
                    <div className="input-wrapper">
                        <label htmlFor="state">State</label>
                        <input type="text" id="state" name="state" />
                    </div>
                    <div className="input-wrapper">
                        <label htmlFor="zip">Zip</label>
                        <input type="text" id="zip" name="zip" />
                    </div>
                </fieldset>
                <div className={style.formButtons}>
                    <button className="button danger">Cancel</button>
                    <button className="button primary">Save</button>
                </div>
            </form>
        </div>
    )
}