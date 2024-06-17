import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { IFilter } from '../../models/filter.model';

@Component({
  selector: 'gd-filter',
  templateUrl: './filter.component.html',
  styleUrl: './filter.component.css',
})
export class FilterComponent {
  filterForm: FormGroup = this.formBuilder.group({
    searchParam: null,
    sortProp: null,
    sortByDescending: null,
    selectProps: null,
    pageNumber: 1,
  });
  selectedProperties: string[] = [];

  @Input() properties!: string[];
  @Output() applyFilter = new EventEmitter();

  constructor(private formBuilder: FormBuilder) {}

  updateSelectProperties(): void {
    this.filterForm
      .get('selectProps')
      ?.setValue(this.selectedProperties.join(','));
  }

  applyButtonClicked(): void {
    this.applyFilter.emit(this.filterForm.value as IFilter);
  }
}
