import { useEffect, useMemo, useState } from 'react';
import { AgGridReact } from 'ag-grid-react';
import { ColDef, ValueGetterParams } from 'ag-grid-community';
import { usePrefersReducedMotion } from 'hooks';
import { ControlButtons } from 'components/Buttons';
import { Customer } from 'types';

import 'ag-grid-community/styles/ag-grid.css';
import 'ag-grid-community/styles/ag-theme-alpine.css';
import style from 'styles/CustomersPage.module.css';

import { getMockClinics } from 'utils';

export interface ColDefCustomer extends Customer {
  controls?: string;
}

type ColDefExtended = ColDef<ColDefCustomer>;

function getFullName(params: ValueGetterParams<ColDefCustomer>) {
  if (!params?.data?.mainContact) {
    return '';
  }
  return `${params.data.mainContact.name.given} ${params.data.mainContact.name.family}`;
}



export default function Customers() {
  // This seems to just default to off. Better for accessibility, but if this is true why use a hook?
  // could just turn animation off permanently. 
  const prefersReducedMotion = usePrefersReducedMotion();
  const initColumnDefs: ColDefExtended[] = [
    { field: 'name', headerName: 'Name', filter: true },
    { field: 'mainContact', headerName: 'Main Contact', valueGetter: getFullName, filter: true },
    { field: 'phoneNumber', headerName: 'Phone Number', filter: true },
    { field: 'mainContact.email', headerName: 'Email', filter: true, resizable: true },
    { field: 'billingInfo.paymentStatus', headerName: 'Payment Status', filter: true, sortable: true },
    // TODO DELETE HANDLER
    { field: 'controls', headerName: '', cellRenderer: ControlButtons, cellRendererParams: { handleDelete: () => alert('delete'), path: 'customers' }, width: 300 }
  ];
  const [columnDefs, setColumnDefs] = useState<ColDefExtended[]>(initColumnDefs);
  const [rowData, setRowData] = useState<ColDefCustomer[]>([]);


  const getRowId = useMemo(() => {
    return (params: { data: ColDefCustomer }) => params.data.id;
  }, []);


  useEffect(() => {
    // TODO real data fetching
    const data = getMockClinics(157);

    setRowData(data);
  }, []);

  return (
    <div className={style.container}>
      <h1>Customers</h1>
      <div className="ag-theme-alpine ag-grid-wrapper" >
        <AgGridReact<ColDefCustomer>
          columnDefs={columnDefs}
          colResizeDefault={'shift'}
          rowData={rowData}
          getRowId={getRowId}
          animateRows={prefersReducedMotion}
          pagination={true}
        />
      </div>
    </div>
  )
}

// Could abstract this to not repeat on Users and Billing.
// Generic doesn't have ID, gotta pass hrefs, at that point is it even worth it in the name of DRY?
