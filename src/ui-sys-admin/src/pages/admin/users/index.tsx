import { useEffect, useMemo, useState } from 'react';
import { User } from 'types';
import { getMockUsers } from 'utils';
import style from 'styles/CustomersPage.module.css';
import { AgGridReact } from 'ag-grid-react';
import 'ag-grid-community/styles/ag-grid.css';
import 'ag-grid-community/styles/ag-theme-material.css';
import Link from 'next/link';
import { ColDef, ValueGetterParams } from 'ag-grid-community';

interface ColDefUser extends User {
  viewDetails?: string;
  edit?: string;
}

type ColDefExtended = ColDef<ColDefUser>;

function getFullName(params: ValueGetterParams<ColDefUser>) {
  console.log('params', params);
  if (!params?.data?.name) {
    return '';
  }
  return `${params.data.name.given} ${params.data.name.family}`;
}

const initColumnDefs: ColDefExtended[] = [
  { field: 'name', headerName: 'Name', valueGetter: getFullName, filter: true, sortable: true },
  { field: 'phoneNumber', headerName: 'Phone Number', filter: true },
  { field: 'email', headerName: 'Email', filter: true, sortable: true },
  { field: 'customer', headerName: 'Customer', filter: true, sortable: true },
  { field: 'roles', headerName: 'Role', filter: true, sortable: true },
  { field: 'viewDetails', headerName: '', cellRenderer: ViewDetailsButton }
];

export default function Customers() {
  const [columnDefs, setColumnDefs] = useState<ColDefExtended[]>(initColumnDefs);
  const [rowData, setRowData] = useState<ColDefUser[]>([]);

  const getRowId = useMemo(() => {
    return (params: { data: ColDefUser }) => params.data.id;
  }, []);


  useEffect(() => {
    // TODO real data fetching
    const data = getMockUsers(145);

    setRowData(data);
  }, []);

  return (
    <div className={style.container}>
      <h1>Users</h1>
      <div className="ag-theme-material" style={{ height: 500 }} >
        <AgGridReact<ColDefUser>
          columnDefs={columnDefs}
          rowData={rowData}
          getRowId={getRowId}
          animateRows={true}
          pagination={true}
        />
      </div>
    </div>
  )
}

function ViewDetailsButton(params: { data: ColDefUser }) {
  return (
    <>
      <Link href={`/admin/users/${params.data.id}`}>
        <button className='button' >View</button>
      </Link>
      <Link href={`/admin/users/${params.data.id}/edit`}>
        <button className='button' >Edit</button>
      </Link>
    </>
  );
}
