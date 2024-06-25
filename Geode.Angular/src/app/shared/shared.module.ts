import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FilterComponent } from './components/filter/filter.component';
import { MatCheckbox } from '@angular/material/checkbox';
import { MatOption } from '@angular/material/core';
import { MatError, MatFormField, MatLabel } from '@angular/material/form-field';
import { MatInput } from '@angular/material/input';
import { MatSelect } from '@angular/material/select';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatProgressSpinner } from '@angular/material/progress-spinner';
import {
  MatExpansionPanel,
  MatExpansionPanelHeader,
  MatExpansionPanelTitle,
} from '@angular/material/expansion';
import { AgGridAngular } from 'ag-grid-angular';
import {
  MatSidenav,
  MatSidenavContainer,
  MatSidenavContent,
} from '@angular/material/sidenav';
import { MatListModule } from '@angular/material/list';
import { MatIcon } from '@angular/material/icon';
import { ScalingDirective } from './directives/scaling.directive';
import { RequiredPermissionsDirective } from './directives/required-permissions.directive';

@NgModule({
  declarations: [
    FilterComponent,
    ScalingDirective,
    RequiredPermissionsDirective,
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    MatFormField,
    MatSelect,
    MatLabel,
    MatOption,
    MatCheckbox,
    MatInput,
    MatProgressSpinner,
    MatExpansionPanel,
    MatExpansionPanelHeader,
    MatExpansionPanelTitle,
    MatError,
    MatSidenav,
    MatIcon,
    MatSidenavContainer,
    MatSidenavContent,
    MatListModule,
    AgGridAngular,
  ],
  exports: [
    FilterComponent,
    ScalingDirective,
    RequiredPermissionsDirective,
    FormsModule,
    ReactiveFormsModule,
    MatExpansionPanel,
    MatExpansionPanelHeader,
    MatExpansionPanelTitle,
    MatProgressSpinner,
    MatError,
    MatSidenav,
    MatIcon,
    MatSidenavContainer,
    MatSidenavContent,
    MatListModule,
    AgGridAngular,
  ],
})
export class SharedModule {}
