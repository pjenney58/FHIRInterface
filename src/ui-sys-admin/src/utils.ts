import { faker } from '@faker-js/faker'
import { Address, Clinic, User } from 'types';

function generateRandomGuid(): string {
    return faker.string.uuid();
}

function generateRandomArrayOfGuids(min: number, max: number): string[] {
    const numUsers = faker.number.int({ min, max });
    const guids: string[] = [];

    for (let i = 0; i < numUsers; i++) {
        guids.push(generateRandomGuid());
    }

    return guids;
}

function generateRandomAddress() {
    return {
        street1: faker.location.streetAddress(),
        street2: faker.location.secondaryAddress(),
        city: faker.location.city(),
        state: faker.location.state(),
        zip: faker.location.zipCode()
    }
}

export function generateMockClinic(): Clinic {
    return {
        id: generateRandomGuid(),
        name: faker.company.name(),
        phoneNumber: faker.phone.number('###-###-####'),
        addresses: [
            {
                street1: faker.location.streetAddress(),
                street2: faker.location.secondaryAddress(),
                city: faker.location.city(),
                state: faker.location.state(),
                zip: faker.location.zipCode()
            }
        ],
        specialties: faker.word.words(2).split(' '),
        doctors: [faker.person.fullName(), faker.person.fullName()],
        website: faker.internet.url(),
        rating: faker.number.float({ min: 3, max: 5, precision: 0.1 }),
        mainContact: {
            name: {
                given: faker.person.firstName(),
                family: faker.person.lastName()
            },
            phoneNumber: faker.phone.number('###-###-####'),
            email: faker.internet.email()
        },
        billingInfo: {
            paymentStatus: faker.helpers.arrayElement(['Paid', 'Due', 'Overdue', 'Pending']),
            lastBillingDate: faker.date.recent().toISOString(),
            nextBillingDate: faker.date.soon().toISOString(),
            billingMethod: faker.helpers.arrayElement(['Credit Card', 'Bank Transfer', 'Check', 'ACH']),
            paymentMethod: faker.finance.creditCardNumber()
        },
        associatedUsers: generateRandomArrayOfGuids(2, 10)
    };
}

export function getMockClinics(numberOfClinics = 100): Clinic[] {
    const mockData: Clinic[] = [];

    for (let i = 0; i < numberOfClinics; i++) {
        mockData.push(generateMockClinic());
    }
    // console.log(JSON.stringify(mockData, null, 2));
    return mockData;
}

// 

export function generateRandomUser(): User {
    const name = {
        given: faker.person.firstName(),
        family: faker.person.lastName()
    }
    const email = faker.internet.email();
    const username = faker.internet.userName();
    const birthdate = faker.date.birthdate().toISOString();
    const address: Address = generateRandomAddress();
    const phoneNumber = faker.phone.number('###-###-####');
    const associatedCustomers = generateRandomArrayOfGuids(1, 3);
    const customer = faker.company.name();
    const roles = faker.helpers.arrayElements(['Admin', 'User', 'SuperUser'], 1);

    return {
        id: generateRandomGuid(),
        name,
        email,
        username,
        birthdate,
        address,
        phoneNumber,
        associatedCustomers,
        customer,
        roles
    };
}

export function getMockUsers(numberOfUsers = 100): User[] {

    const mockUserData: User[] = [];
    for (let i = 0; i < numberOfUsers; i++) {
        mockUserData.push(generateRandomUser());
    }
    return mockUserData;
}


export const stateSelectArray = [
    { value: "Alabama", label: "AL" },
    { value: "Alaska", label: "AK" },
    { value: "Arizona", label: "AZ" },
    { value: "Arkansas", label: "AR" },
    { value: "California", label: "CA" },
    { value: "Colorado", label: "CO" },
    { value: "Connecticut", label: "CT" },
    { value: "Delaware", label: "DE" },
    { value: "Florida", label: "FL" },
    { value: "Georgia", label: "GA" },
    { value: "Hawaii", label: "HI" },
    { value: "Idaho", label: "ID" },
    { value: "Illinois", label: "IL" },
    { value: "Indiana", label: "IN" },
    { value: "Iowa", label: "IA" },
    { value: "Kansas", label: "KS" },
    { value: "Kentucky", label: "KY" },
    { value: "Louisiana", label: "LA" },
    { value: "Maine", label: "ME" },
    { value: "Maryland", label: "MD" },
    { value: "Massachusetts", label: "MA" },
    { value: "Michigan", label: "MI" },
    { value: "Minnesota", label: "MN" },
    { value: "Mississippi", label: "MS" },
    { value: "Missouri", label: "MO" },
    { value: "Montana", label: "MT" },
    { value: "Nebraska", label: "NE" },
    { value: "Nevada", label: "NV" },
    { value: "New Hampshire", label: "NH" },
    { value: "New Jersey", label: "NJ" },
    { value: "New Mexico", label: "NM" },
    { value: "New York", label: "NY" },
    { value: "North Carolina", label: "NC" },
    { value: "North Dakota", label: "ND" },
    { value: "Ohio", label: "OH" },
    { value: "Oklahoma", label: "OK" },
    { value: "Oregon", label: "OR" },
    { value: "Pennsylvania", label: "PA" },
    { value: "Rhode Island", label: "RI" },
    { value: "South Carolina", label: "SC" },
    { value: "South Dakota", label: "SD" },
    { value: "Tennessee", label: "TN" },
    { value: "Texas", label: "TX" },
    { value: "Utah", label: "UT" },
    { value: "Vermont", label: "VT" },
    { value: "Virginia", label: "VA" },
    { value: "Washington", label: "WA" },
    { value: "West Virginia", label: "WV" },
    { value: "Wisconsin", label: "WI" },
    { value: "Wyoming", label: "WY" }
];
