import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { Filter } from '../../models/filter.model';
import { debounceTime, distinctUntilChanged } from 'rxjs';

@Component({
  selector: 'gd-filter',
  templateUrl: './filter.component.html',
  styleUrl: './filter.component.css',
})
export class FilterComponent implements OnInit {
  filterForm: FormGroup = this.formBuilder.group({
    searchParam: '',
    sortProp: '',
    sortByDescending: '',
    selectProps: '',
    pageNumber: 1,
  });
  selectedProperties: string[] = [];

  @Input() properties!: string[];
  @Output() applyFilter = new EventEmitter();

  constructor(private formBuilder: FormBuilder) {}
  ngOnInit(): void {
    this.filterForm.valueChanges
      .pipe(
        debounceTime(1000),
        distinctUntilChanged((a, b) => JSON.stringify(a) === JSON.stringify(b))
      )
      .subscribe(() => this.applyFilter.emit(this.filterForm.value as Filter));
  }

  updateSelectProperties(): void {
    this.filterForm
      .get('selectProps')
      ?.setValue(this.selectedProperties.join(','));
  }
}
