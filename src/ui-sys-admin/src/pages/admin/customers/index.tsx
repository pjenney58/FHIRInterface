import { useEffect, useMemo, useState } from 'react';
import { Customer } from 'types';
import { getMockClinics } from 'utils';
import style from 'styles/CustomersPage.module.css';
import { AgGridReact } from 'ag-grid-react';
import 'ag-grid-community/styles/ag-grid.css';
import 'ag-grid-community/styles/ag-theme-alpine.css';
import Link from 'next/link';
import { useRouter } from 'next/router';
import { ColDef, ValueFormatterParams, ValueGetterParams } from 'ag-grid-community';
import { MdOutlinePageview } from 'react-icons/md';

interface ColDefCustomer extends Customer {
  viewDetails?: string;
}

type ColDefExtended = ColDef<ColDefCustomer>;

function getFullName(params: ValueGetterParams<ColDefCustomer>) {
  console.log('params', params);
  if (!params?.data?.mainContact) {
    return '';
  }
  return `${params.data.mainContact.name.given} ${params.data.mainContact.name.family}`;
}

const initColumnDefs: ColDefExtended[] = [
  { field: 'name', headerName: 'Name', width: 200, filter: true },
  { field: 'mainContact', headerName: 'Main Contact', width: 200, valueGetter: getFullName, filter: true },
  { field: 'phoneNumber', headerName: 'Phone Number', width: 200, filter: true },
  { field: 'mainContact.email', headerName: 'Email', width: 200, filter: true },
  { field: 'billingInfo.paymentStatus', headerName: 'Payment Status', width: 200 },
  { field: 'viewDetails', headerName: '', width: 200, cellRenderer: ViewDetailsButton },
]

export default function Customers() {
  const [columnDefs, setColumnDefs] = useState<ColDefExtended[]>(initColumnDefs);
  const [rowData, setRowData] = useState<ColDefCustomer[]>([]);
  const router = useRouter();

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

function ViewDetailsButton(params: { data: ColDefCustomer }) {
  return (
    <Link href={`/admin/customers/${params.data.id}`}>
      <button className='button'><MdOutlinePageview /></button>
    </Link>
  );
}