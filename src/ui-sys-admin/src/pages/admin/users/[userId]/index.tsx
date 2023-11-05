import { User } from 'types';
import { useRouter } from "next/router";
import { useEffect, useState } from "react";
import { generateRandomUser, getMockUsers } from 'utils';

export default function UserDisplayPage() {
    const router = useRouter();
    const { userId } = router.query;
    const [user, setUser] = useState<User | null>(null);

    useEffect(() => {
        // TODO real data fetching
        const data = generateRandomUser();
        setUser(data);
    }, []);

    return (
        <div>
            <h2 >{user?.name.given} {user?.name.family}</h2>
        </div>
    )
}