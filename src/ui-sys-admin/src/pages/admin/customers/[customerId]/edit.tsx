import { useRouter } from "next/router";

export default function EditCustomer() {
    const router = useRouter();
    const { customerId } = router.query;
    // TODO: fetch customer data
    // TODO: populate form with customer data
    // TODO: save customer data
    // TODO: cancel
    // TODO: validation
    // TODO: delete customer
    // TODO: Permissions/roles
    return (
        <div>
            <h1>Edit Customer</h1>
            <p>Customer ID: {customerId}</p>
            <ul>
                <li>Form</li>
                <li>Save</li>
                <li>Cancel</li>
                <li>Validation</li>
            </ul>
            <div>
                {/* form with name, address, contact info, billing */}
                <form>
                    <div>
                        <label htmlFor="name">Name</label>
                        <input type="text" id="name" name="name" />
                    </div>
                    <div>
                        <label htmlFor="address">Address</label>
                        <input type="text" id="address" name="address" />
                    </div>
                    <div>
                        <label htmlFor="city">City</label>
                        <input type="text" id="city" name="city" />
                    </div>
                    <div>
                        <label htmlFor="state">State</label>
                        <input type="text" id="state" name="state" />
                    </div>
                    <div>
                        <label htmlFor="zip">Zip</label>
                        <input type="text" id="zip" name="zip" />
                    </div>
                    <div>
                        <label htmlFor="country">Country</label>
                        <input type="text" id="country" name="country" />
                    </div>
                    <div>
                        <label htmlFor="phone">Phone</label>
                        <input type="text" id="phone" name="phone" />
                    </div>
                    <div>
                        <label htmlFor="email">Email</label>
                        <input type="text" id="email" name="email" />
                    </div>
                </form>
            </div>
        </div>
    );
}