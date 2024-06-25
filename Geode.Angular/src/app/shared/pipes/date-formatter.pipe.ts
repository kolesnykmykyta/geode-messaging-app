import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'dateFormatter',
  standalone: true,
})
export class DateFormatterPipe implements PipeTransform {
  transform(value: Date, countryCode: string): string {
    return value.toLocaleDateString(countryCode, {
      year: 'numeric',
      day: 'numeric',
      month: 'long',
    });
  }
}
