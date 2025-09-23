import { Component, EventEmitter, Input, Output, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatTableModule, MatTableDataSource } from '@angular/material/table';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatTooltipModule } from '@angular/material/tooltip';
import { ColumnDef, PaginatorConfig } from './dynamic-table.interfaces';

@Component({
  selector: 'app-dynamic-table',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatTableModule,
    MatPaginatorModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatIconModule,
    MatTooltipModule,
  ],
  templateUrl: './dynamic-table.component.html',
  styleUrls: ['./dynamic-table.component.scss'],
})
export class DynamicTableComponent<T extends { [key: string]: any }> {
  @Input() set data(data: T[]) {
    this.dataSource.data = data;
  }
  @Input() columns: ColumnDef<T>[] = [];
  @Input() paginatorConfig: PaginatorConfig = { length: 0, pageSize: 10, pageSizeOptions: [5, 10, 25, 100], idKey: 'id' };
  @Input() showActions = true;
  @Input() showCustomAction = true;

  @Output() onUpdate = new EventEmitter<T>();
  @Output() onDelete = new EventEmitter<T>();
  @Output() onCustomAction = new EventEmitter<T>();

  @ViewChild(MatPaginator) paginator!: MatPaginator;

  dataSource = new MatTableDataSource<T>([]);
  editingRow: T | null = null;

  get displayedColumns(): string[] {
    const columns = [...this.columns.map(c => String(c.key))];
    if (this.showActions) {
      columns.push('actions');
    }
    if (this.showCustomAction) {
      columns.push('customAction');
    }
    return columns;
  }

  ngAfterViewInit() {
    this.dataSource.paginator = this.paginator;
  }

  startEdit(row: T): void {
    this.editingRow = { ...row };
  }

  cancelEdit(): void {
    this.editingRow = null;
  }

  saveUpdate(): void {
    if (this.editingRow) {
      this.onUpdate.emit(this.editingRow);
      this.editingRow = null;
    }
  }

  delete(row: T): void {
    this.onDelete.emit(row);
  }

  customAction(row: T): void {
    this.onCustomAction.emit(row);
  }

  trackByFn(index: number, item: T): any {
    return item[(this.paginatorConfig ?? { idKey: 'id' }).idKey as keyof T] || index;
  }
}
