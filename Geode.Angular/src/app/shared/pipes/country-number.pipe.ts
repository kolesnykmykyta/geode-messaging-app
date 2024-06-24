import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'countryNumber',
  standalone: true,
})
export class CountryNumberPipe implements PipeTransform {
  transform(value: number, countryCode: string): string {
    return value.toLocaleString(countryCode);
  }
}
