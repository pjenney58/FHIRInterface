import { TenantList } from 'components/TenantList';
import { useEffect, useMemo, useState } from 'react';
import { Tenant } from 'types';
import { getMockClinics } from 'utils';
import style from 'styles/CustomersPage.module.css';
import { AgGridReact } from 'ag-grid-react';
import 'ag-grid-community/styles/ag-grid.css';
import 'ag-grid-community/styles/ag-theme-alpine.css';
import Link from 'next/link';
import { useRouter } from 'next/router';
import { ColDef } from 'ag-grid-community';

interface ColDefTenant extends Tenant {
  viewDetails?: string;
}

type CustomColDef = ColDef<ColDefTenant>;


const initColumnDefs: CustomColDef[] = [
  { field: 'name', headerName: 'Name', width: 200, filter: true },
  { field: 'phoneNumber', headerName: 'Phone Number', width: 200, filter: true },
  { field: 'billingInfo.paymentStatus', headerName: 'Payment Status', width: 200 },
  { field: 'viewDetails', headerName: 'View Details', width: 200, cellRenderer: ViewDetailsButton },
]

export default function Customers() {
  const [columnDefs, setColumnDefs] = useState<CustomColDef[]>(initColumnDefs);
  const [rowData, setRowData] = useState<ColDefTenant[]>([]);
  const router = useRouter();

  const getRowId = useMemo(() => {
    return (params: { data: ColDefTenant }) => params.data.id;
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
        <AgGridReact<ColDefTenant>
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

function ViewDetailsButton(params: { data: ColDefTenant }) {
  return (
    <Link href={`/admin/customers/${params.data.id}`}>
      <button className='button'>View Details</button>
    </Link>
  );
}