import Link from "next/link";
import { useRouter } from "next/router";
import { ColDefCustomer } from "pages/admin/customers";
import { ColDefUser } from "pages/admin/users";
import style from 'styles/Buttons.module.css';

export interface ControlButtonsProps {
  // Better way to do this with generics? Using generics makes id possibly undefined
  data: ColDefCustomer | ColDefUser;
  handleDelete: (id: string) => void;
}

export function ControlButtons(params: ControlButtonsProps) {
  const router = useRouter();
  const baseURL = router.pathname;
  return (
    <div className={style.controlsWrapper}>
      <Link href={`${baseURL}/${params.data.id}`}>
        <button className='button' >View</button>
      </Link>
      <Link href={`${baseURL}/${params.data.id}/edit`}>
        <button className='button' >Edit</button>
      </Link>
      <button className={`${style.delete} button`} onClick={() => params.handleDelete(params.data.id)}>Delete</button>
    </div>
  );
}

