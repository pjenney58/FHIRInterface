

export default function CreateUserPage() {
    return (
        <div>
            <h1>Create User</h1>
            <form>
                <fieldset>
                    <div className="input-wrapper">
                        <label htmlFor="firstName">First Name</label>
                        <input className="input text-input" type="text" id="firstName" name="firstName" />
                    </div>
                    <div className="input-wrapper">
                        <label htmlFor="lastName">Last Name</label>
                        <input className="input text-input" type="text" id="lastName" name="lastName" />
                    </div>
                    <div className="input-wrapper">
                        <label htmlFor="email">Email</label>
                        <input className="input text-input" type="email" id="email" name="email" />
                    </div>
                    <div className="input-wrapper">
                        <label htmlFor="phoneNumber">Phone Number</label>
                        <input className="input text-input" type="tel" id="phoneNumber" name="phoneNumber" />
                    </div>

                </fieldset>
            </form>
        </div>
    )
}