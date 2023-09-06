import { useEffect, useMemo, useState } from 'react';
import { Customer } from 'types';
import { getMockClinics } from 'utils';
import style from 'styles/CustomersPage.module.css';
import { AgGridReact } from 'ag-grid-react';
import 'ag-grid-community/styles/ag-grid.css';
import 'ag-grid-community/styles/ag-theme-alpine.css';
import Link from 'next/link';
import { useRouter } from 'next/router';
import { ColDef, ValueGetterParams } from 'ag-grid-community';

interface ColDefCustomer extends Customer {
  viewDetails?: string;
  edit?: string;
  delete?: string
}

type ColDefExtended = ColDef<ColDefCustomer>;

function getFullName(params: ValueGetterParams<ColDefCustomer>) {
  console.log('params', params);
  if (!params?.data?.mainContact) {
    return '';
  }
  return `${params.data.mainContact.name.given} ${params.data.mainContact.name.family}`;
}



export default function Customers() {
  const initColumnDefs: ColDefExtended[] = [
    { field: 'name', headerName: 'Name', filter: true },
    { field: 'mainContact', headerName: 'Main Contact', valueGetter: getFullName, filter: true },
    { field: 'phoneNumber', headerName: 'Phone Number', filter: true },
    { field: 'mainContact.email', headerName: 'Email', filter: true },
    { field: 'billingInfo.paymentStatus', headerName: 'Payment Status', width: 200 },
    { field: 'viewDetails', headerName: '', cellRenderer: ViewDetailsButton },
    { field: 'delete', headerName: '', cellRenderer: DeleteButton }
  ];
  const [columnDefs, setColumnDefs] = useState<ColDefExtended[]>(initColumnDefs);
  const [rowData, setRowData] = useState<ColDefCustomer[]>([]);


  const getRowId = useMemo(() => {
    return (params: { data: ColDefCustomer }) => params.data.id;
  }, []);


  useEffect(() => {
    // TODO real data fetching
    const data = getMockClinics(2013);

    setRowData(data);
  }, []);

  return (
    <div className={style.container}>
      <h1>Customers</h1>
      <div className="ag-theme-alpine" style={{ width: '100%', height: 500 }} >
        <AgGridReact<ColDefCustomer>
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

// todo move both to separate file


function ViewDetailsButton(params: { data: ColDefCustomer }) {
  return (
    <>
      <Link href={`/admin/customers/${params.data.id}`}>
        <button className='button' >View</button>
      </Link>
      <Link href={`/admin/customers/${params.data.id}/edit`}>
        <button className='button' >Edit</button>
      </Link>
    </>
  );
}

function DeleteButton(params: { data: ColDefCustomer }) {
  // TODO pass in a callback to handle delete
  // TODO add a confirmation dialog
  function handleDelete() {
    alert('Do you want to delete this customer?');
    console.log('delete', params.data.id);
  }

  return (
    <>
      <button className='button' onClick={handleDelete}>Delete</button>
    </>
  );
}