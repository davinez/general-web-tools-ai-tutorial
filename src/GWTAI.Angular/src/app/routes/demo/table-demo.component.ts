
import { Component } from '@angular/core';
import { DynamicTableComponent } from '@shared/components/dynamic-table/dynamic-table.component';
import { ColumnDef, PaginatorConfig } from '@shared/components/dynamic-table/dynamic-table.interfaces';

interface User {
  id: number;
  name: string;
  email: string;
  editable: boolean;
}

@Component({
  selector: 'app-table-demo',
  standalone: true,
  imports: [DynamicTableComponent],
  template: `
    <app-dynamic-table
      [data]="data"
      [columns]="columns"
      [paginatorConfig]="paginatorConfig"
      (onUpdate)="handleUpdate($event)"
      (onDelete)="handleDelete($event)"
      (onCustomAction)="handleCustomAction($event)"
    ></app-dynamic-table>
  `,
})
export class TableDemoComponent {
  data: User[] = [
    { id: 1, name: 'John Doe', email: 'john.doe@example.com', editable: true },
    { id: 2, name: 'Jane Smith', email: 'jane.smith@example.com', editable: true },
    { id: 3, name: 'Peter Jones', email: 'peter.jones@example.com', editable: false },
  ];

  columns: ColumnDef<User>[] = [
    { key: 'id', label: 'ID' },
    { key: 'name', label: 'Name', editable: true },
    { key: 'email', label: 'Email', editable: true },
  ];

  paginatorConfig: PaginatorConfig = {
    length: this.data.length,
    pageSize: 5,
    pageSizeOptions: [5, 10, 20],
  };

  handleUpdate(item: User) {
    console.log('Update:', item);
  }

  handleDelete(item: User) {
    console.log('Delete:', item);
  }

  handleCustomAction(item: User) {
    console.log('Custom Action:', item);
  }
}
