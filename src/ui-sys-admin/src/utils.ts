import {faker} from '@faker-js/faker'
import { Clinic } from 'types';

function generateRandomGuid(): string {
    return faker.string.uuid();
}

function generateRandomArrayOfGuids(): string[] {
    const numUsers = faker.number.int({ min: 2, max: 10 });
    const guids: string[] = [];

    for (let i = 0; i < numUsers; i++) {
        guids.push(generateRandomGuid());
    }

    return guids;
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
        associatedUsers: generateRandomArrayOfGuids()
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