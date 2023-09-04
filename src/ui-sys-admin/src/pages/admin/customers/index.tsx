import { useEffect, useMemo, useState } from 'react';
import { Customer } from 'types';
import { getMockClinics } from 'utils';
import style from 'styles/CustomersPage.module.css';
import { AgGridReact } from 'ag-grid-react';
import 'ag-grid-community/styles/ag-grid.css';
import 'ag-grid-community/styles/ag-theme-alpine.css';
import Link from 'next/link';
import { useRouter } from 'next/router';
import { ColDef } from 'ag-grid-community';

interface ColDefCustomer extends Customer {
  viewDetails?: string;
}

type ColDefExtended = ColDef<ColDefCustomer>;


const initColumnDefs: ColDefExtended[] = [
  { field: 'name', headerName: 'Name', width: 200, filter: true },
  { field: 'phoneNumber', headerName: 'Phone Number', width: 200, filter: true },
  { field: 'billingInfo.paymentStatus', headerName: 'Payment Status', width: 200 },
  { field: 'viewDetails', headerName: 'View Details', width: 200, cellRenderer: ViewDetailsButton },
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
  }, [])

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
      <button className='button'>View Details</button>
    </Link>
  );
}