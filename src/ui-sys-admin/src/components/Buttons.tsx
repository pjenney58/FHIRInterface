import Link from "next/link";
import { useRouter } from "next/router";
import { ColDefCustomer } from "pages/admin/customers";
import { ColDefUser } from "pages/admin/users";

export interface ControlButtonsProps {
    // Better way to do this with generics? Using generics makes id possibly undefined
    data: ColDefCustomer | ColDefUser;
    handleDelete: () => void;
}

export function ControlButtons(params: ControlButtonsProps) {
    const router = useRouter();
    const baseURL = router.pathname;
    return (
        <div>
            <Link href={`${baseURL}/${params.data.id}`}>
                <button className='button' >View</button>
            </Link>
            <Link href={`${baseURL}/${params.data.id}/edit`}>
                <button className='button' >Edit</button>
            </Link>
            <button className='button' onClick={() => params.handleDelete()}>Delete</button>
        </div>
    );
}