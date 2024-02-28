import { useRouter } from "next/router";


export default function UserEditPage() {
    const router = useRouter();
    const { userId } = router.query;
    return (
        <div>
            <h1>Edit User</h1>
            <div>
                <p>Editing user with id: {userId}</p>
                <p>Will we need a full page or make edit buttons in the view page that are tied to user roles?</p>
            </div>
        </div>
    )

}